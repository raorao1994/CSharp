namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public class LevelMatchFilter : FilterSkeleton
    {
        private bool m_acceptOnMatch = true;
        private Level m_levelToMatch;

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if ((this.m_levelToMatch != null) && (this.m_levelToMatch == loggingEvent.Level))
            {
                return (this.m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny);
            }
            return FilterDecision.Neutral;
        }

        public bool AcceptOnMatch
        {
            get => 
                this.m_acceptOnMatch;
            set
            {
                this.m_acceptOnMatch = value;
            }
        }

        public Level LevelToMatch
        {
            get => 
                this.m_levelToMatch;
            set
            {
                this.m_levelToMatch = value;
            }
        }
    }
}

