namespace log4net.Filter
{
    using System;

    public enum FilterDecision
    {
        Accept = 1,
        Deny = -1,
        Neutral = 0
    }
}

