namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.Reflection;

    public class CyclicBuffer
    {
        private LoggingEvent[] m_events;
        private int m_first;
        private int m_last;
        private int m_maxSize;
        private int m_numElems;

        public CyclicBuffer(int maxSize)
        {
            if (maxSize < 1)
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("maxSize", maxSize, "Parameter: maxSize, Value: [" + maxSize + "] out of range. Non zero positive integer required");
            }
            this.m_maxSize = maxSize;
            this.m_events = new LoggingEvent[maxSize];
            this.m_first = 0;
            this.m_last = 0;
            this.m_numElems = 0;
        }

        public LoggingEvent Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            lock (this)
            {
                LoggingEvent event2 = this.m_events[this.m_last];
                this.m_events[this.m_last] = loggingEvent;
                if (++this.m_last == this.m_maxSize)
                {
                    this.m_last = 0;
                }
                if (this.m_numElems < this.m_maxSize)
                {
                    this.m_numElems++;
                }
                else if (++this.m_first == this.m_maxSize)
                {
                    this.m_first = 0;
                }
                if (this.m_numElems < this.m_maxSize)
                {
                    return null;
                }
                return event2;
            }
        }

        public void Clear()
        {
            lock (this)
            {
                Array.Clear(this.m_events, 0, this.m_events.Length);
                this.m_first = 0;
                this.m_last = 0;
                this.m_numElems = 0;
            }
        }

        public LoggingEvent[] PopAll()
        {
            lock (this)
            {
                LoggingEvent[] destinationArray = new LoggingEvent[this.m_numElems];
                if (this.m_numElems > 0)
                {
                    if (this.m_first < this.m_last)
                    {
                        Array.Copy(this.m_events, this.m_first, destinationArray, 0, this.m_numElems);
                    }
                    else
                    {
                        Array.Copy(this.m_events, this.m_first, destinationArray, 0, this.m_maxSize - this.m_first);
                        Array.Copy(this.m_events, 0, destinationArray, this.m_maxSize - this.m_first, this.m_last);
                    }
                }
                this.Clear();
                return destinationArray;
            }
        }

        public LoggingEvent PopOldest()
        {
            lock (this)
            {
                LoggingEvent event2 = null;
                if (this.m_numElems > 0)
                {
                    this.m_numElems--;
                    event2 = this.m_events[this.m_first];
                    this.m_events[this.m_first] = null;
                    if (++this.m_first == this.m_maxSize)
                    {
                        this.m_first = 0;
                    }
                }
                return event2;
            }
        }

        public LoggingEvent this[int i]
        {
            get
            {
                lock (this)
                {
                    if ((i < 0) || (i >= this.m_numElems))
                    {
                        return null;
                    }
                    return this.m_events[(this.m_first + i) % this.m_maxSize];
                }
            }
        }

        public int Length
        {
            get
            {
                lock (this)
                {
                    return this.m_numElems;
                }
            }
        }

        public int MaxSize
        {
            get
            {
                lock (this)
                {
                    return this.m_maxSize;
                }
            }
        }
    }
}

