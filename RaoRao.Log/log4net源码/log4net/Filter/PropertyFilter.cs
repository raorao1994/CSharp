namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public class PropertyFilter : StringMatchFilter
    {
        private string m_key;

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_key != null)
            {
                object property = loggingEvent.LookupProperty(this.m_key);
                string input = loggingEvent.Repository.RendererMap.FindAndRender(property);
                if ((input == null) || ((base.m_stringToMatch == null) && (base.m_regexToMatch == null)))
                {
                    return FilterDecision.Neutral;
                }
                if (base.m_regexToMatch != null)
                {
                    if (!base.m_regexToMatch.Match(input).Success)
                    {
                        return FilterDecision.Neutral;
                    }
                    if (base.m_acceptOnMatch)
                    {
                        return FilterDecision.Accept;
                    }
                    return FilterDecision.Deny;
                }
                if (base.m_stringToMatch != null)
                {
                    if (input.IndexOf(base.m_stringToMatch) == -1)
                    {
                        return FilterDecision.Neutral;
                    }
                    if (base.m_acceptOnMatch)
                    {
                        return FilterDecision.Accept;
                    }
                    return FilterDecision.Deny;
                }
            }
            return FilterDecision.Neutral;
        }

        public string Key
        {
            get => 
                this.m_key;
            set
            {
                this.m_key = value;
            }
        }
    }
}

