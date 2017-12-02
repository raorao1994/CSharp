namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;

    public class RollingFileAppender : FileAppender
    {
        private static readonly Type declaringType = typeof(RollingFileAppender);
        private string m_baseFileName;
        private int m_countDirection = -1;
        private int m_curSizeRollBackups = 0;
        private string m_datePattern = ".yyyy-MM-dd";
        private IDateTime m_dateTime = null;
        private long m_maxFileSize = 0xa00000L;
        private int m_maxSizeRollBackups = 0;
        private DateTime m_nextCheck = DateTime.MaxValue;
        private DateTime m_now;
        private bool m_preserveLogFileNameExtension = false;
        private bool m_rollDate = true;
        private RollingMode m_rollingStyle = RollingMode.Composite;
        private RollPoint m_rollPoint;
        private bool m_rollSize = true;
        private string m_scheduledFilename = null;
        private bool m_staticLogFileName = true;
        private static readonly DateTime s_date1970 = new DateTime(0x7b2, 1, 1);

        public override void ActivateOptions()
        {
            if (this.m_dateTime == null)
            {
                this.m_dateTime = new LocalDateTime();
            }
            if (this.m_rollDate && (this.m_datePattern != null))
            {
                this.m_now = this.m_dateTime.Now;
                this.m_rollPoint = this.ComputeCheckPeriod(this.m_datePattern);
                if (this.m_rollPoint == RollPoint.InvalidRollPoint)
                {
                    throw new ArgumentException("Invalid RollPoint, unable to parse [" + this.m_datePattern + "]");
                }
                this.m_nextCheck = this.NextCheckDate(this.m_now, this.m_rollPoint);
            }
            else if (this.m_rollDate)
            {
                this.ErrorHandler.Error("Either DatePattern or rollingStyle options are not set for [" + base.Name + "].");
            }
            if (base.SecurityContext == null)
            {
                base.SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
            using (base.SecurityContext.Impersonate(this))
            {
                base.File = FileAppender.ConvertToFullPath(base.File.Trim());
                this.m_baseFileName = base.File;
            }
            if ((this.m_rollDate && (this.File != null)) && (this.m_scheduledFilename == null))
            {
                this.m_scheduledFilename = this.CombinePath(this.File, this.m_now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo));
            }
            this.ExistingInit();
            base.ActivateOptions();
        }

        protected virtual void AdjustFileBeforeAppend()
        {
            if (this.m_rollDate)
            {
                DateTime now = this.m_dateTime.Now;
                if (now >= this.m_nextCheck)
                {
                    this.m_now = now;
                    this.m_nextCheck = this.NextCheckDate(this.m_now, this.m_rollPoint);
                    this.RollOverTime(true);
                }
            }
            if (this.m_rollSize && ((this.File != null) && (((CountingQuietTextWriter) base.QuietWriter).Count >= this.m_maxFileSize)))
            {
                this.RollOverSize();
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            this.AdjustFileBeforeAppend();
            base.Append(loggingEvent);
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            this.AdjustFileBeforeAppend();
            base.Append(loggingEvents);
        }

        private string CombinePath(string path1, string path2)
        {
            string extension = Path.GetExtension(path1);
            if (this.m_preserveLogFileNameExtension && (extension.Length > 0))
            {
                return Path.Combine(Path.GetDirectoryName(path1), Path.GetFileNameWithoutExtension(path1) + path2 + extension);
            }
            return (path1 + path2);
        }

        private RollPoint ComputeCheckPeriod(string datePattern)
        {
            string str = s_date1970.ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
            for (int i = 0; i <= 5; i++)
            {
                string str2 = this.NextCheckDate(s_date1970, (RollPoint) i).ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
                LogLog.Debug(declaringType, string.Concat(new object[] { "Type = [", i, "], r0 = [", str, "], r1 = [", str2, "]" }));
                if (!(((str == null) || (str2 == null)) || str.Equals(str2)))
                {
                    return (RollPoint) i;
                }
            }
            return RollPoint.InvalidRollPoint;
        }

        protected void DeleteFile(string fileName)
        {
            if (this.FileExists(fileName))
            {
                IDisposable disposable;
                string path = fileName;
                string destFileName = string.Concat(new object[] { fileName, ".", Environment.TickCount, ".DeletePending" });
                try
                {
                    using (disposable = base.SecurityContext.Impersonate(this))
                    {
                        File.Move(fileName, destFileName);
                    }
                    path = destFileName;
                }
                catch (Exception exception)
                {
                    LogLog.Debug(declaringType, "Exception while moving file to be deleted [" + fileName + "] -> [" + destFileName + "]", exception);
                }
                try
                {
                    using (disposable = base.SecurityContext.Impersonate(this))
                    {
                        File.Delete(path);
                    }
                    LogLog.Debug(declaringType, "Deleted file [" + fileName + "]");
                }
                catch (Exception exception2)
                {
                    if (path == fileName)
                    {
                        this.ErrorHandler.Error("Exception while deleting file [" + path + "]", exception2, ErrorCode.GenericFailure);
                    }
                    else
                    {
                        LogLog.Debug(declaringType, "Exception while deleting temp file [" + path + "]", exception2);
                    }
                }
            }
        }

        private void DetermineCurSizeRollBackups()
        {
            this.m_curSizeRollBackups = 0;
            string path = null;
            string baseFile = null;
            using (base.SecurityContext.Impersonate(this))
            {
                path = Path.GetFullPath(this.m_baseFileName);
                baseFile = Path.GetFileName(path);
            }
            ArrayList existingFiles = this.GetExistingFiles(path);
            this.InitializeRollBackups(baseFile, existingFiles);
            LogLog.Debug(declaringType, "curSizeRollBackups starts at [" + this.m_curSizeRollBackups + "]");
        }

        protected void ExistingInit()
        {
            this.DetermineCurSizeRollBackups();
            this.RollOverIfDateBoundaryCrossing();
            if (!base.AppendToFile)
            {
                bool flag = false;
                string nextOutputFileName = this.GetNextOutputFileName(this.m_baseFileName);
                using (base.SecurityContext.Impersonate(this))
                {
                    flag = File.Exists(nextOutputFileName);
                }
                if (flag)
                {
                    if (this.m_maxSizeRollBackups == 0)
                    {
                        LogLog.Debug(declaringType, "Output file [" + nextOutputFileName + "] already exists. MaxSizeRollBackups is 0; cannot roll. Overwriting existing file.");
                    }
                    else
                    {
                        LogLog.Debug(declaringType, "Output file [" + nextOutputFileName + "] already exists. Not appending to file. Rolling existing file out of the way.");
                        this.RollOverRenameFiles(nextOutputFileName);
                    }
                }
            }
        }

        protected bool FileExists(string path)
        {
            using (base.SecurityContext.Impersonate(this))
            {
                return File.Exists(path);
            }
        }

        private int GetBackUpIndex(string curFileName)
        {
            int val = -1;
            string path = curFileName;
            if (this.m_preserveLogFileNameExtension)
            {
                path = Path.GetFileNameWithoutExtension(path);
            }
            int num2 = path.LastIndexOf(".");
            if (num2 > 0)
            {
                SystemInfo.TryParse(path.Substring(num2 + 1), out val);
            }
            return val;
        }

        private ArrayList GetExistingFiles(string baseFilePath)
        {
            ArrayList list = new ArrayList();
            string path = null;
            using (base.SecurityContext.Impersonate(this))
            {
                string fullPath = Path.GetFullPath(baseFilePath);
                path = Path.GetDirectoryName(fullPath);
                if (Directory.Exists(path))
                {
                    string fileName = Path.GetFileName(fullPath);
                    string[] files = Directory.GetFiles(path, this.GetWildcardPatternForFile(fileName));
                    if (files != null)
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            string str4 = Path.GetFileName(files[i]);
                            if (str4.StartsWith(Path.GetFileNameWithoutExtension(fileName)))
                            {
                                list.Add(str4);
                            }
                        }
                    }
                }
            }
            LogLog.Debug(declaringType, "Searched for existing files in [" + path + "]");
            return list;
        }

        protected string GetNextOutputFileName(string fileName)
        {
            if (!this.m_staticLogFileName)
            {
                fileName = fileName.Trim();
                if (this.m_rollDate)
                {
                    fileName = this.CombinePath(fileName, this.m_now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo));
                }
                if (this.m_countDirection >= 0)
                {
                    fileName = this.CombinePath(fileName, "." + this.m_curSizeRollBackups);
                }
            }
            return fileName;
        }

        private string GetWildcardPatternForFile(string baseFileName)
        {
            if (this.m_preserveLogFileNameExtension)
            {
                return (Path.GetFileNameWithoutExtension(baseFileName) + ".*" + Path.GetExtension(baseFileName));
            }
            return (baseFileName + '*');
        }

        private void InitializeFromOneFile(string baseFile, string curFileName)
        {
            if (curFileName.StartsWith(Path.GetFileNameWithoutExtension(baseFile)) && !curFileName.Equals(baseFile))
            {
                if ((this.m_rollDate && !this.m_staticLogFileName) && !curFileName.StartsWith(this.CombinePath(baseFile, this.m_dateTime.Now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo))))
                {
                    LogLog.Debug(declaringType, "Ignoring file [" + curFileName + "] because it is from a different date period");
                }
                else
                {
                    try
                    {
                        int backUpIndex = this.GetBackUpIndex(curFileName);
                        if (backUpIndex > this.m_curSizeRollBackups)
                        {
                            if (0 != this.m_maxSizeRollBackups)
                            {
                                if (-1 == this.m_maxSizeRollBackups)
                                {
                                    this.m_curSizeRollBackups = backUpIndex;
                                }
                                else if (this.m_countDirection >= 0)
                                {
                                    this.m_curSizeRollBackups = backUpIndex;
                                }
                                else if (backUpIndex <= this.m_maxSizeRollBackups)
                                {
                                    this.m_curSizeRollBackups = backUpIndex;
                                }
                            }
                            LogLog.Debug(declaringType, string.Concat(new object[] { "File name [", curFileName, "] moves current count to [", this.m_curSizeRollBackups, "]" }));
                        }
                    }
                    catch (FormatException)
                    {
                        LogLog.Debug(declaringType, "Encountered a backup file not ending in .x [" + curFileName + "]");
                    }
                }
            }
        }

        private void InitializeRollBackups(string baseFile, ArrayList arrayFiles)
        {
            if (null != arrayFiles)
            {
                string str = baseFile.ToLower(CultureInfo.InvariantCulture);
                foreach (string str2 in arrayFiles)
                {
                    this.InitializeFromOneFile(str, str2.ToLower(CultureInfo.InvariantCulture));
                }
            }
        }

        protected DateTime NextCheckDate(DateTime currentDateTime, RollPoint rollPoint)
        {
            DateTime time = currentDateTime;
            switch (rollPoint)
            {
                case RollPoint.TopOfMinute:
                    time = time.AddMilliseconds((double) -time.Millisecond);
                    return time.AddSeconds((double) -time.Second).AddMinutes(1.0);

                case RollPoint.TopOfHour:
                    time = time.AddMilliseconds((double) -time.Millisecond);
                    time = time.AddSeconds((double) -time.Second);
                    return time.AddMinutes((double) -time.Minute).AddHours(1.0);

                case RollPoint.HalfDay:
                    time = time.AddMilliseconds((double) -time.Millisecond);
                    time = time.AddSeconds((double) -time.Second);
                    time = time.AddMinutes((double) -time.Minute);
                    if (time.Hour >= 12)
                    {
                        return time.AddHours((double) -time.Hour).AddDays(1.0);
                    }
                    return time.AddHours((double) (12 - time.Hour));

                case RollPoint.TopOfDay:
                    time = time.AddMilliseconds((double) -time.Millisecond);
                    time = time.AddSeconds((double) -time.Second);
                    time = time.AddMinutes((double) -time.Minute);
                    return time.AddHours((double) -time.Hour).AddDays(1.0);

                case RollPoint.TopOfWeek:
                    time = time.AddMilliseconds((double) -time.Millisecond);
                    time = time.AddSeconds((double) -time.Second);
                    time = time.AddMinutes((double) -time.Minute);
                    time = time.AddHours((double) -time.Hour);
                    return time.AddDays((double) (7 - time.DayOfWeek));

                case RollPoint.TopOfMonth:
                    time = time.AddMilliseconds((double) -time.Millisecond);
                    time = time.AddSeconds((double) -time.Second);
                    time = time.AddMinutes((double) -time.Minute);
                    time = time.AddHours((double) -time.Hour);
                    return time.AddDays((double) (1 - time.Day)).AddMonths(1);
            }
            return time;
        }

        protected override void OpenFile(string fileName, bool append)
        {
            lock (this)
            {
                fileName = this.GetNextOutputFileName(fileName);
                long length = 0L;
                if (append)
                {
                    using (base.SecurityContext.Impersonate(this))
                    {
                        if (File.Exists(fileName))
                        {
                            length = new FileInfo(fileName).Length;
                        }
                    }
                }
                else if (LogLog.IsErrorEnabled && ((this.m_maxSizeRollBackups != 0) && this.FileExists(fileName)))
                {
                    LogLog.Error(declaringType, "RollingFileAppender: INTERNAL ERROR. Append is False but OutputFile [" + fileName + "] already exists.");
                }
                if (!this.m_staticLogFileName)
                {
                    this.m_scheduledFilename = fileName;
                }
                base.OpenFile(fileName, append);
                ((CountingQuietTextWriter) base.QuietWriter).Count = length;
            }
        }

        protected void RollFile(string fromFile, string toFile)
        {
            if (this.FileExists(fromFile))
            {
                this.DeleteFile(toFile);
                try
                {
                    LogLog.Debug(declaringType, "Moving [" + fromFile + "] -> [" + toFile + "]");
                    using (base.SecurityContext.Impersonate(this))
                    {
                        File.Move(fromFile, toFile);
                    }
                }
                catch (Exception exception)
                {
                    this.ErrorHandler.Error("Exception while rolling file [" + fromFile + "] -> [" + toFile + "]", exception, ErrorCode.GenericFailure);
                }
            }
            else
            {
                LogLog.Warn(declaringType, "Cannot RollFile [" + fromFile + "] -> [" + toFile + "]. Source does not exist");
            }
        }

        private void RollOverIfDateBoundaryCrossing()
        {
            if ((this.m_staticLogFileName && this.m_rollDate) && this.FileExists(this.m_baseFileName))
            {
                DateTime lastWriteTimeUtc;
                using (base.SecurityContext.Impersonate(this))
                {
                    if (this.DateTimeStrategy is UniversalDateTime)
                    {
                        lastWriteTimeUtc = File.GetLastWriteTimeUtc(this.m_baseFileName);
                    }
                    else
                    {
                        lastWriteTimeUtc = File.GetLastWriteTime(this.m_baseFileName);
                    }
                }
                LogLog.Debug(declaringType, "[" + lastWriteTimeUtc.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo) + "] vs. [" + this.m_now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo) + "]");
                if (!lastWriteTimeUtc.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo).Equals(this.m_now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo)))
                {
                    this.m_scheduledFilename = this.m_baseFileName + lastWriteTimeUtc.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo);
                    LogLog.Debug(declaringType, "Initial roll over to [" + this.m_scheduledFilename + "]");
                    this.RollOverTime(false);
                    LogLog.Debug(declaringType, "curSizeRollBackups after rollOver at [" + this.m_curSizeRollBackups + "]");
                }
            }
        }

        protected void RollOverRenameFiles(string baseFileName)
        {
            if (this.m_maxSizeRollBackups != 0)
            {
                if (this.m_countDirection < 0)
                {
                    if (this.m_curSizeRollBackups == this.m_maxSizeRollBackups)
                    {
                        this.DeleteFile(this.CombinePath(baseFileName, "." + this.m_maxSizeRollBackups));
                        this.m_curSizeRollBackups--;
                    }
                    for (int i = this.m_curSizeRollBackups; i >= 1; i--)
                    {
                        this.RollFile(this.CombinePath(baseFileName, "." + i), this.CombinePath(baseFileName, "." + (i + 1)));
                    }
                    this.m_curSizeRollBackups++;
                    this.RollFile(baseFileName, this.CombinePath(baseFileName, ".1"));
                }
                else
                {
                    if ((this.m_curSizeRollBackups >= this.m_maxSizeRollBackups) && (this.m_maxSizeRollBackups > 0))
                    {
                        int num2 = this.m_curSizeRollBackups - this.m_maxSizeRollBackups;
                        if (this.m_staticLogFileName)
                        {
                            num2++;
                        }
                        string str = baseFileName;
                        if (!this.m_staticLogFileName)
                        {
                            int length = str.LastIndexOf(".");
                            if (length >= 0)
                            {
                                str = str.Substring(0, length);
                            }
                        }
                        this.DeleteFile(this.CombinePath(str, "." + num2));
                    }
                    if (this.m_staticLogFileName)
                    {
                        this.m_curSizeRollBackups++;
                        this.RollFile(baseFileName, this.CombinePath(baseFileName, "." + this.m_curSizeRollBackups));
                    }
                }
            }
        }

        protected void RollOverSize()
        {
            base.CloseFile();
            LogLog.Debug(declaringType, "rolling over count [" + ((CountingQuietTextWriter) base.QuietWriter).Count + "]");
            LogLog.Debug(declaringType, "maxSizeRollBackups [" + this.m_maxSizeRollBackups + "]");
            LogLog.Debug(declaringType, "curSizeRollBackups [" + this.m_curSizeRollBackups + "]");
            LogLog.Debug(declaringType, "countDirection [" + this.m_countDirection + "]");
            this.RollOverRenameFiles(this.File);
            if (!(this.m_staticLogFileName || (this.m_countDirection < 0)))
            {
                this.m_curSizeRollBackups++;
            }
            this.SafeOpenFile(this.m_baseFileName, false);
        }

        protected void RollOverTime(bool fileIsOpen)
        {
            if (this.m_staticLogFileName)
            {
                if (this.m_datePattern == null)
                {
                    this.ErrorHandler.Error("Missing DatePattern option in rollOver().");
                    return;
                }
                string str = this.m_now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo);
                if (this.m_scheduledFilename.Equals(this.CombinePath(this.File, str)))
                {
                    this.ErrorHandler.Error("Compare " + this.m_scheduledFilename + " : " + this.CombinePath(this.File, str));
                    return;
                }
                if (fileIsOpen)
                {
                    base.CloseFile();
                }
                for (int i = 1; i <= this.m_curSizeRollBackups; i++)
                {
                    string fromFile = this.CombinePath(this.File, "." + i);
                    string toFile = this.CombinePath(this.m_scheduledFilename, "." + i);
                    this.RollFile(fromFile, toFile);
                }
                this.RollFile(this.File, this.m_scheduledFilename);
            }
            this.m_curSizeRollBackups = 0;
            this.m_scheduledFilename = this.CombinePath(this.File, this.m_now.ToString(this.m_datePattern, DateTimeFormatInfo.InvariantInfo));
            if (fileIsOpen)
            {
                this.SafeOpenFile(this.m_baseFileName, false);
            }
        }

        protected override void SetQWForFiles(TextWriter writer)
        {
            base.QuietWriter = new CountingQuietTextWriter(writer, this.ErrorHandler);
        }

        public int CountDirection
        {
            get => 
                this.m_countDirection;
            set
            {
                this.m_countDirection = value;
            }
        }

        public string DatePattern
        {
            get => 
                this.m_datePattern;
            set
            {
                this.m_datePattern = value;
            }
        }

        public IDateTime DateTimeStrategy
        {
            get => 
                this.m_dateTime;
            set
            {
                this.m_dateTime = value;
            }
        }

        public long MaxFileSize
        {
            get => 
                this.m_maxFileSize;
            set
            {
                this.m_maxFileSize = value;
            }
        }

        public string MaximumFileSize
        {
            get => 
                this.m_maxFileSize.ToString(NumberFormatInfo.InvariantInfo);
            set
            {
                this.m_maxFileSize = OptionConverter.ToFileSize(value, this.m_maxFileSize + 1L);
            }
        }

        public int MaxSizeRollBackups
        {
            get => 
                this.m_maxSizeRollBackups;
            set
            {
                this.m_maxSizeRollBackups = value;
            }
        }

        public bool PreserveLogFileNameExtension
        {
            get => 
                this.m_preserveLogFileNameExtension;
            set
            {
                this.m_preserveLogFileNameExtension = value;
            }
        }

        public RollingMode RollingStyle
        {
            get => 
                this.m_rollingStyle;
            set
            {
                this.m_rollingStyle = value;
                switch (this.m_rollingStyle)
                {
                    case RollingMode.Once:
                        this.m_rollDate = false;
                        this.m_rollSize = false;
                        base.AppendToFile = false;
                        break;

                    case RollingMode.Size:
                        this.m_rollDate = false;
                        this.m_rollSize = true;
                        break;

                    case RollingMode.Date:
                        this.m_rollDate = true;
                        this.m_rollSize = false;
                        break;

                    case RollingMode.Composite:
                        this.m_rollDate = true;
                        this.m_rollSize = true;
                        break;
                }
            }
        }

        public bool StaticLogFileName
        {
            get => 
                this.m_staticLogFileName;
            set
            {
                this.m_staticLogFileName = value;
            }
        }

        public interface IDateTime
        {
            DateTime Now { get; }
        }

        private class LocalDateTime : RollingFileAppender.IDateTime
        {
            public DateTime Now =>
                DateTime.Now;
        }

        public enum RollingMode
        {
            Once,
            Size,
            Date,
            Composite
        }

        protected enum RollPoint
        {
            HalfDay = 2,
            InvalidRollPoint = -1,
            TopOfDay = 3,
            TopOfHour = 1,
            TopOfMinute = 0,
            TopOfMonth = 5,
            TopOfWeek = 4
        }

        private class UniversalDateTime : RollingFileAppender.IDateTime
        {
            public DateTime Now =>
                DateTime.UtcNow;
        }
    }
}

