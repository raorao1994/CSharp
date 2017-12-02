namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    public class AdoNetAppender : BufferingAppenderSkeleton
    {
        private static readonly Type declaringType = typeof(AdoNetAppender);
        private string m_appSettingsKey;
        private string m_commandText;
        private System.Data.CommandType m_commandType = System.Data.CommandType.Text;
        private string m_connectionString;
        private string m_connectionStringName;
        private string m_connectionType = "System.Data.OleDb.OleDbConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        private IDbCommand m_dbCommand;
        private IDbConnection m_dbConnection;
        protected ArrayList m_parameters = new ArrayList();
        private bool m_reconnectOnError = false;
        private log4net.Core.SecurityContext m_securityContext;
        protected bool m_usePreparedCommand;
        private bool m_useTransactions = true;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_usePreparedCommand = (this.m_commandText != null) && (this.m_commandText.Length > 0);
            if (this.m_securityContext == null)
            {
                this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
            this.InitializeDatabaseConnection();
            this.InitializeDatabaseCommand();
        }

        public void AddParameter(AdoNetAppenderParameter parameter)
        {
            this.m_parameters.Add(parameter);
        }

        protected virtual IDbConnection CreateConnection(Type connectionType, string connectionString)
        {
            IDbConnection connection = (IDbConnection) Activator.CreateInstance(connectionType);
            connection.ConnectionString = connectionString;
            return connection;
        }

        private void DiposeConnection()
        {
            if (this.m_dbConnection != null)
            {
                try
                {
                    this.m_dbConnection.Close();
                }
                catch (Exception exception)
                {
                    LogLog.Warn(declaringType, "Exception while disposing cached connection object", exception);
                }
                this.m_dbConnection = null;
            }
        }

        private void DisposeCommand(bool ignoreException)
        {
            if (this.m_dbCommand != null)
            {
                try
                {
                    this.m_dbCommand.Dispose();
                }
                catch (Exception exception)
                {
                    if (!ignoreException)
                    {
                        LogLog.Warn(declaringType, "Exception while disposing cached command object", exception);
                    }
                }
                this.m_dbCommand = null;
            }
        }

        protected virtual string GetLogStatement(LoggingEvent logEvent)
        {
            if (this.Layout == null)
            {
                this.ErrorHandler.Error("AdoNetAppender: No Layout specified.");
                return "";
            }
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            this.Layout.Format(writer, logEvent);
            return writer.ToString();
        }

        private void InitializeDatabaseCommand()
        {
            if ((this.m_dbConnection != null) && this.m_usePreparedCommand)
            {
                Exception exception;
                try
                {
                    this.DisposeCommand(false);
                    this.m_dbCommand = this.m_dbConnection.CreateCommand();
                    this.m_dbCommand.CommandText = this.m_commandText;
                    this.m_dbCommand.CommandType = this.m_commandType;
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    this.ErrorHandler.Error("Could not create database command [" + this.m_commandText + "]", exception);
                    this.DisposeCommand(true);
                }
                if (this.m_dbCommand != null)
                {
                    try
                    {
                        foreach (AdoNetAppenderParameter parameter in this.m_parameters)
                        {
                            try
                            {
                                parameter.Prepare(this.m_dbCommand);
                            }
                            catch (Exception exception2)
                            {
                                exception = exception2;
                                this.ErrorHandler.Error("Could not add database command parameter [" + parameter.ParameterName + "]", exception);
                                throw;
                            }
                        }
                    }
                    catch
                    {
                        this.DisposeCommand(true);
                    }
                }
                if (this.m_dbCommand != null)
                {
                    try
                    {
                        this.m_dbCommand.Prepare();
                    }
                    catch (Exception exception3)
                    {
                        exception = exception3;
                        this.ErrorHandler.Error("Could not prepare database command [" + this.m_commandText + "]", exception);
                        this.DisposeCommand(true);
                    }
                }
            }
        }

        private void InitializeDatabaseConnection()
        {
            string connectionStringContext = "Unable to determine connection string context.";
            string connectionString = string.Empty;
            try
            {
                this.DisposeCommand(true);
                this.DiposeConnection();
                connectionString = this.ResolveConnectionString(out connectionStringContext);
                this.m_dbConnection = this.CreateConnection(this.ResolveConnectionType(), connectionString);
                using (this.SecurityContext.Impersonate(this))
                {
                    this.m_dbConnection.Open();
                }
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Could not open database connection [" + connectionString + "]. Connection string context [" + connectionStringContext + "].", exception);
                this.m_dbConnection = null;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            this.DisposeCommand(false);
            this.DiposeConnection();
        }

        protected virtual string ResolveConnectionString(out string connectionStringContext)
        {
            if ((this.m_connectionString != null) && (this.m_connectionString.Length > 0))
            {
                connectionStringContext = "ConnectionString";
                return this.m_connectionString;
            }
            if (!string.IsNullOrEmpty(this.m_connectionStringName))
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[this.m_connectionStringName];
                if (settings == null)
                {
                    throw new LogException("Unable to find [" + this.m_connectionStringName + "] ConfigurationManager.ConnectionStrings item");
                }
                connectionStringContext = "ConnectionStringName";
                return settings.ConnectionString;
            }
            if ((this.m_appSettingsKey != null) && (this.m_appSettingsKey.Length > 0))
            {
                connectionStringContext = "AppSettingsKey";
                string appSetting = SystemInfo.GetAppSetting(this.m_appSettingsKey);
                if ((appSetting == null) || (appSetting.Length == 0))
                {
                    throw new LogException("Unable to find [" + this.m_appSettingsKey + "] AppSettings key.");
                }
                return appSetting;
            }
            connectionStringContext = "Unable to resolve connection string from ConnectionString, ConnectionStrings, or AppSettings.";
            return string.Empty;
        }

        protected virtual Type ResolveConnectionType()
        {
            Type type;
            try
            {
                type = SystemInfo.GetTypeFromString(this.m_connectionType, true, false);
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Failed to load connection type [" + this.m_connectionType + "]", exception);
                throw;
            }
            return type;
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (this.m_reconnectOnError && ((this.m_dbConnection == null) || (this.m_dbConnection.State != ConnectionState.Open)))
            {
                LogLog.Debug(declaringType, "Attempting to reconnect to database. Current Connection State: " + ((this.m_dbConnection == null) ? SystemInfo.NullText : this.m_dbConnection.State.ToString()));
                this.InitializeDatabaseConnection();
                this.InitializeDatabaseCommand();
            }
            if ((this.m_dbConnection != null) && (this.m_dbConnection.State == ConnectionState.Open))
            {
                if (this.m_useTransactions)
                {
                    IDbTransaction dbTran = null;
                    try
                    {
                        dbTran = this.m_dbConnection.BeginTransaction();
                        this.SendBuffer(dbTran, events);
                        dbTran.Commit();
                    }
                    catch (Exception exception)
                    {
                        if (dbTran != null)
                        {
                            try
                            {
                                dbTran.Rollback();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        this.ErrorHandler.Error("Exception while writing to database", exception);
                    }
                }
                else
                {
                    this.SendBuffer(null, events);
                }
            }
        }

        protected virtual void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
        {
            if (this.m_usePreparedCommand)
            {
                if (this.m_dbCommand != null)
                {
                    if (dbTran != null)
                    {
                        this.m_dbCommand.Transaction = dbTran;
                    }
                    foreach (LoggingEvent event2 in events)
                    {
                        foreach (AdoNetAppenderParameter parameter in this.m_parameters)
                        {
                            parameter.FormatValue(this.m_dbCommand, event2);
                        }
                        this.m_dbCommand.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (IDbCommand command = this.m_dbConnection.CreateCommand())
                {
                    if (dbTran != null)
                    {
                        command.Transaction = dbTran;
                    }
                    foreach (LoggingEvent event2 in events)
                    {
                        string logStatement = this.GetLogStatement(event2);
                        LogLog.Debug(declaringType, "LogStatement [" + logStatement + "]");
                        command.CommandText = logStatement;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public string AppSettingsKey
        {
            get => 
                this.m_appSettingsKey;
            set
            {
                this.m_appSettingsKey = value;
            }
        }

        public string CommandText
        {
            get => 
                this.m_commandText;
            set
            {
                this.m_commandText = value;
            }
        }

        public System.Data.CommandType CommandType
        {
            get => 
                this.m_commandType;
            set
            {
                this.m_commandType = value;
            }
        }

        protected IDbConnection Connection
        {
            get => 
                this.m_dbConnection;
            set
            {
                this.m_dbConnection = value;
            }
        }

        public string ConnectionString
        {
            get => 
                this.m_connectionString;
            set
            {
                this.m_connectionString = value;
            }
        }

        public string ConnectionStringName
        {
            get => 
                this.m_connectionStringName;
            set
            {
                this.m_connectionStringName = value;
            }
        }

        public string ConnectionType
        {
            get => 
                this.m_connectionType;
            set
            {
                this.m_connectionType = value;
            }
        }

        public bool ReconnectOnError
        {
            get => 
                this.m_reconnectOnError;
            set
            {
                this.m_reconnectOnError = value;
            }
        }

        public log4net.Core.SecurityContext SecurityContext
        {
            get => 
                this.m_securityContext;
            set
            {
                this.m_securityContext = value;
            }
        }

        public bool UseTransactions
        {
            get => 
                this.m_useTransactions;
            set
            {
                this.m_useTransactions = value;
            }
        }
    }
}

