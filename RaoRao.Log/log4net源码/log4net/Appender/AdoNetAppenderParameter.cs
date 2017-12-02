namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Data;

    public class AdoNetAppenderParameter
    {
        private System.Data.DbType m_dbType;
        private bool m_inferType = true;
        private IRawLayout m_layout;
        private string m_parameterName;
        private byte m_precision = 0;
        private byte m_scale = 0;
        private int m_size = 0;

        public virtual void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
        {
            IDbDataParameter parameter = (IDbDataParameter) command.Parameters[this.m_parameterName];
            object obj2 = this.Layout.Format(loggingEvent);
            if (obj2 == null)
            {
                obj2 = DBNull.Value;
            }
            parameter.Value = obj2;
        }

        public virtual void Prepare(IDbCommand command)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = this.m_parameterName;
            if (!this.m_inferType)
            {
                parameter.DbType = this.m_dbType;
            }
            if (this.m_precision != 0)
            {
                parameter.Precision = this.m_precision;
            }
            if (this.m_scale != 0)
            {
                parameter.Scale = this.m_scale;
            }
            if (this.m_size != 0)
            {
                parameter.Size = this.m_size;
            }
            command.Parameters.Add(parameter);
        }

        public System.Data.DbType DbType
        {
            get => 
                this.m_dbType;
            set
            {
                this.m_dbType = value;
                this.m_inferType = false;
            }
        }

        public IRawLayout Layout
        {
            get => 
                this.m_layout;
            set
            {
                this.m_layout = value;
            }
        }

        public string ParameterName
        {
            get => 
                this.m_parameterName;
            set
            {
                this.m_parameterName = value;
            }
        }

        public byte Precision
        {
            get => 
                this.m_precision;
            set
            {
                this.m_precision = value;
            }
        }

        public byte Scale
        {
            get => 
                this.m_scale;
            set
            {
                this.m_scale = value;
            }
        }

        public int Size
        {
            get => 
                this.m_size;
            set
            {
                this.m_size = value;
            }
        }
    }
}

