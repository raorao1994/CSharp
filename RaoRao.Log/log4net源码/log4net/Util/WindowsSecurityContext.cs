namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Principal;

    public class WindowsSecurityContext : log4net.Core.SecurityContext, IOptionHandler
    {
        private string m_domainName = Environment.MachineName;
        private WindowsIdentity m_identity;
        private ImpersonationMode m_impersonationMode = ImpersonationMode.User;
        private string m_password;
        private string m_userName;

        public void ActivateOptions()
        {
            if (this.m_impersonationMode == ImpersonationMode.User)
            {
                if (this.m_userName == null)
                {
                    throw new ArgumentNullException("m_userName");
                }
                if (this.m_domainName == null)
                {
                    throw new ArgumentNullException("m_domainName");
                }
                if (this.m_password == null)
                {
                    throw new ArgumentNullException("m_password");
                }
                this.m_identity = LogonUser(this.m_userName, this.m_domainName, this.m_password);
            }
        }

        [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);
        [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
        public override IDisposable Impersonate(object state)
        {
            if (this.m_impersonationMode == ImpersonationMode.User)
            {
                if (this.m_identity != null)
                {
                    return new DisposableImpersonationContext(this.m_identity.Impersonate());
                }
            }
            else if (this.m_impersonationMode == ImpersonationMode.Process)
            {
                return new DisposableImpersonationContext(WindowsIdentity.Impersonate(IntPtr.Zero));
            }
            return null;
        }

        [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private static WindowsIdentity LogonUser(string userName, string domainName, string password)
        {
            NativeError lastError;
            IntPtr zero = IntPtr.Zero;
            if (!LogonUser(userName, domainName, password, 2, 0, ref zero))
            {
                lastError = NativeError.GetLastError();
                throw new Exception("Failed to LogonUser [" + userName + "] in Domain [" + domainName + "]. Error: " + lastError.ToString());
            }
            IntPtr duplicateTokenHandle = IntPtr.Zero;
            if (!DuplicateToken(zero, 2, ref duplicateTokenHandle))
            {
                lastError = NativeError.GetLastError();
                if (zero != IntPtr.Zero)
                {
                    CloseHandle(zero);
                }
                throw new Exception("Failed to DuplicateToken after LogonUser. Error: " + lastError.ToString());
            }
            WindowsIdentity identity = new WindowsIdentity(duplicateTokenHandle);
            if (duplicateTokenHandle != IntPtr.Zero)
            {
                CloseHandle(duplicateTokenHandle);
            }
            if (zero != IntPtr.Zero)
            {
                CloseHandle(zero);
            }
            return identity;
        }

        [DllImport("advapi32.dll", SetLastError=true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        public ImpersonationMode Credentials
        {
            get => 
                this.m_impersonationMode;
            set
            {
                this.m_impersonationMode = value;
            }
        }

        public string DomainName
        {
            get => 
                this.m_domainName;
            set
            {
                this.m_domainName = value;
            }
        }

        public string Password
        {
            set
            {
                this.m_password = value;
            }
        }

        public string UserName
        {
            get => 
                this.m_userName;
            set
            {
                this.m_userName = value;
            }
        }

        private sealed class DisposableImpersonationContext : IDisposable
        {
            private readonly WindowsImpersonationContext m_impersonationContext;

            public DisposableImpersonationContext(WindowsImpersonationContext impersonationContext)
            {
                this.m_impersonationContext = impersonationContext;
            }

            public void Dispose()
            {
                this.m_impersonationContext.Undo();
            }
        }

        public enum ImpersonationMode
        {
            User,
            Process
        }
    }
}

