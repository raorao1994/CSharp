namespace log4net.Layout.Pattern
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text;

    internal class StackTraceDetailPatternConverter : StackTracePatternConverter
    {
        private static readonly Type declaringType = typeof(StackTracePatternConverter);

        internal override string GetMethodInformation(MethodBase method)
        {
            string str = "";
            try
            {
                string str2 = "";
                string[] methodParameterNames = this.GetMethodParameterNames(method);
                StringBuilder builder = new StringBuilder();
                if ((methodParameterNames != null) && (methodParameterNames.GetUpperBound(0) > 0))
                {
                    for (int i = 0; i <= methodParameterNames.GetUpperBound(0); i++)
                    {
                        builder.AppendFormat("{0}, ", methodParameterNames[i]);
                    }
                }
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 2, 2);
                    str2 = builder.ToString();
                }
                str = base.GetMethodInformation(method) + "(" + str2 + ")";
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method information.", exception);
            }
            return str;
        }

        private string[] GetMethodParameterNames(MethodBase methodBase)
        {
            ArrayList list = new ArrayList();
            try
            {
                ParameterInfo[] parameters = methodBase.GetParameters();
                int upperBound = parameters.GetUpperBound(0);
                for (int i = 0; i <= upperBound; i++)
                {
                    list.Add(parameters[i].ParameterType + " " + parameters[i].Name);
                }
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method parameters.", exception);
            }
            return (string[]) list.ToArray(typeof(string));
        }
    }
}

