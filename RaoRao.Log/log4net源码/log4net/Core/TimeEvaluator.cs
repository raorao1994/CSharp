namespace log4net.Core
{
    using System;

    public class TimeEvaluator : ITriggeringEventEvaluator
    {
        private const int DEFAULT_INTERVAL = 0;
        private int m_interval;
        private DateTime m_lasttime;

        public TimeEvaluator() : this(0)
        {
        }

        public TimeEvaluator(int interval)
        {
            this.m_interval = interval;
            this.m_lasttime = DateTime.Now;
        }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_interval == 0)
            {
                return false;
            }
            lock (this)
            {
                if (DateTime.Now.Subtract(this.m_lasttime).TotalSeconds > this.m_interval)
                {
                    this.m_lasttime = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        public int Interval
        {
            get => 
                this.m_interval;
            set
            {
                this.m_interval = value;
            }
        }
    }
}

