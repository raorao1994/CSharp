namespace log4net.Core
{
    using System;

    public class ExceptionEvaluator : ITriggeringEventEvaluator
    {
        private bool m_triggerOnSubclass;
        private Type m_type;

        public ExceptionEvaluator()
        {
        }

        public ExceptionEvaluator(Type exType, bool triggerOnSubClass)
        {
            if (exType == null)
            {
                throw new ArgumentNullException("exType");
            }
            this.m_type = exType;
            this.m_triggerOnSubclass = triggerOnSubClass;
        }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_triggerOnSubclass && (loggingEvent.ExceptionObject != null))
            {
                Type type = loggingEvent.ExceptionObject.GetType();
                return ((type == this.m_type) || type.IsSubclassOf(this.m_type));
            }
            return (!(this.m_triggerOnSubclass || (loggingEvent.ExceptionObject == null)) && (loggingEvent.ExceptionObject.GetType() == this.m_type));
        }

        public Type ExceptionType
        {
            get => 
                this.m_type;
            set
            {
                this.m_type = value;
            }
        }

        public bool TriggerOnSubclass
        {
            get => 
                this.m_triggerOnSubclass;
            set
            {
                this.m_triggerOnSubclass = value;
            }
        }
    }
}

