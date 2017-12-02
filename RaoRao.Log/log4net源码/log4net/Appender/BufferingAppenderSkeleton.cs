namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Collections;

    public abstract class BufferingAppenderSkeleton : AppenderSkeleton
    {
        private const int DEFAULT_BUFFER_SIZE = 0x200;
        private int m_bufferSize;
        private CyclicBuffer m_cb;
        private ITriggeringEventEvaluator m_evaluator;
        private readonly bool m_eventMustBeFixed;
        private FixFlags m_fixFlags;
        private bool m_lossy;
        private ITriggeringEventEvaluator m_lossyEvaluator;

        protected BufferingAppenderSkeleton() : this(true)
        {
        }

        protected BufferingAppenderSkeleton(bool eventMustBeFixed)
        {
            this.m_bufferSize = 0x200;
            this.m_lossy = false;
            this.m_fixFlags = FixFlags.All;
            this.m_eventMustBeFixed = eventMustBeFixed;
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.m_lossy && (this.m_evaluator == null))
            {
                this.ErrorHandler.Error("Appender [" + base.Name + "] is Lossy but has no Evaluator. The buffer will never be sent!");
            }
            if (this.m_bufferSize > 1)
            {
                this.m_cb = new CyclicBuffer(this.m_bufferSize);
            }
            else
            {
                this.m_cb = null;
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if ((this.m_cb == null) || (this.m_bufferSize <= 1))
            {
                if ((!this.m_lossy || ((this.m_evaluator != null) && this.m_evaluator.IsTriggeringEvent(loggingEvent))) || ((this.m_lossyEvaluator != null) && this.m_lossyEvaluator.IsTriggeringEvent(loggingEvent)))
                {
                    if (this.m_eventMustBeFixed)
                    {
                        loggingEvent.Fix = this.Fix;
                    }
                    this.SendBuffer(new LoggingEvent[] { loggingEvent });
                }
            }
            else
            {
                loggingEvent.Fix = this.Fix;
                LoggingEvent firstLoggingEvent = this.m_cb.Append(loggingEvent);
                if (firstLoggingEvent != null)
                {
                    if (!this.m_lossy)
                    {
                        this.SendFromBuffer(firstLoggingEvent, this.m_cb);
                    }
                    else
                    {
                        if (!((this.m_lossyEvaluator != null) && this.m_lossyEvaluator.IsTriggeringEvent(firstLoggingEvent)))
                        {
                            firstLoggingEvent = null;
                        }
                        if ((this.m_evaluator != null) && this.m_evaluator.IsTriggeringEvent(loggingEvent))
                        {
                            this.SendFromBuffer(firstLoggingEvent, this.m_cb);
                        }
                        else if (firstLoggingEvent != null)
                        {
                            this.SendBuffer(new LoggingEvent[] { firstLoggingEvent });
                        }
                    }
                }
                else if ((this.m_evaluator != null) && this.m_evaluator.IsTriggeringEvent(loggingEvent))
                {
                    this.SendFromBuffer(null, this.m_cb);
                }
            }
        }

        public virtual void Flush()
        {
            this.Flush(false);
        }

        public virtual void Flush(bool flushLossyBuffer)
        {
            lock (this)
            {
                if ((this.m_cb != null) && (this.m_cb.Length > 0))
                {
                    if (this.m_lossy)
                    {
                        if (flushLossyBuffer)
                        {
                            if (this.m_lossyEvaluator != null)
                            {
                                LoggingEvent[] eventArray = this.m_cb.PopAll();
                                ArrayList list = new ArrayList(eventArray.Length);
                                foreach (LoggingEvent event2 in eventArray)
                                {
                                    if (this.m_lossyEvaluator.IsTriggeringEvent(event2))
                                    {
                                        list.Add(event2);
                                    }
                                }
                                if (list.Count > 0)
                                {
                                    this.SendBuffer((LoggingEvent[]) list.ToArray(typeof(LoggingEvent)));
                                }
                            }
                            else
                            {
                                this.m_cb.Clear();
                            }
                        }
                    }
                    else
                    {
                        this.SendFromBuffer(null, this.m_cb);
                    }
                }
            }
        }

        protected override void OnClose()
        {
            this.Flush(true);
        }

        protected abstract void SendBuffer(LoggingEvent[] events);
        protected virtual void SendFromBuffer(LoggingEvent firstLoggingEvent, CyclicBuffer buffer)
        {
            LoggingEvent[] events = buffer.PopAll();
            if (firstLoggingEvent == null)
            {
                this.SendBuffer(events);
            }
            else if (events.Length == 0)
            {
                this.SendBuffer(new LoggingEvent[] { firstLoggingEvent });
            }
            else
            {
                LoggingEvent[] destinationArray = new LoggingEvent[events.Length + 1];
                Array.Copy(events, 0, destinationArray, 1, events.Length);
                destinationArray[0] = firstLoggingEvent;
                this.SendBuffer(destinationArray);
            }
        }

        public int BufferSize
        {
            get => 
                this.m_bufferSize;
            set
            {
                this.m_bufferSize = value;
            }
        }

        public ITriggeringEventEvaluator Evaluator
        {
            get => 
                this.m_evaluator;
            set
            {
                this.m_evaluator = value;
            }
        }

        public virtual FixFlags Fix
        {
            get => 
                this.m_fixFlags;
            set
            {
                this.m_fixFlags = value;
            }
        }

        public bool Lossy
        {
            get => 
                this.m_lossy;
            set
            {
                this.m_lossy = value;
            }
        }

        public ITriggeringEventEvaluator LossyEvaluator
        {
            get => 
                this.m_lossyEvaluator;
            set
            {
                this.m_lossyEvaluator = value;
            }
        }

        [Obsolete("Use Fix property")]
        public virtual bool OnlyFixPartialEventData
        {
            get => 
                (this.Fix == FixFlags.Partial);
            set
            {
                if (value)
                {
                    this.Fix = FixFlags.Partial;
                }
                else
                {
                    this.Fix = FixFlags.All;
                }
            }
        }
    }
}

