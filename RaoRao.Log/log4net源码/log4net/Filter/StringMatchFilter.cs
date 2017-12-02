namespace log4net.Filter
{
    using log4net.Core;
    using System;
    using System.Text.RegularExpressions;

    public class StringMatchFilter : FilterSkeleton
    {
        protected bool m_acceptOnMatch = true;
        protected Regex m_regexToMatch;
        protected string m_stringRegexToMatch;
        protected string m_stringToMatch;

        public override void ActivateOptions()
        {
            if (this.m_stringRegexToMatch != null)
            {
                this.m_regexToMatch = new Regex(this.m_stringRegexToMatch, RegexOptions.Compiled);
            }
        }

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            string renderedMessage = loggingEvent.RenderedMessage;
            if ((renderedMessage != null) && ((this.m_stringToMatch != null) || (this.m_regexToMatch != null)))
            {
                if (this.m_regexToMatch != null)
                {
                    if (!this.m_regexToMatch.Match(renderedMessage).Success)
                    {
                        return FilterDecision.Neutral;
                    }
                    if (this.m_acceptOnMatch)
                    {
                        return FilterDecision.Accept;
                    }
                    return FilterDecision.Deny;
                }
                if (this.m_stringToMatch != null)
                {
                    if (renderedMessage.IndexOf(this.m_stringToMatch) == -1)
                    {
                        return FilterDecision.Neutral;
                    }
                    if (this.m_acceptOnMatch)
                    {
                        return FilterDecision.Accept;
                    }
                    return FilterDecision.Deny;
                }
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

        public string RegexToMatch
        {
            get => 
                this.m_stringRegexToMatch;
            set
            {
                this.m_stringRegexToMatch = value;
            }
        }

        public string StringToMatch
        {
            get => 
                this.m_stringToMatch;
            set
            {
                this.m_stringToMatch = value;
            }
        }
    }
}

