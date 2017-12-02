namespace log4net
{
    using System;
    using System.Runtime.CompilerServices;

    public sealed class AssemblyInfo
    {
        public const bool ClientProfile = false;
        public const string TargetFramework = ".NET Framework";
        [DecimalConstant(1, 0, (uint) 0, (uint) 0, (uint) 40)]
        public static readonly decimal TargetFrameworkVersion = 4.0M;
        public const string Version = "1.2.11";

        public static string Info =>
            $"Apache log4net version {"1.2.11"} compiled for {".NET Framework"}{string.Empty} {4.0M}";
    }
}

