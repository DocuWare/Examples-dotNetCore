using System;
using System.Linq;
using System.Runtime.InteropServices;
using DocuWare.Platform.ServerClient;
using Microsoft.Win32.SafeHandles;

namespace DocuWare.SDK.Samples.dotNetCore.Helpers {
    public class Authenticator : IDisposable {
        private bool _disposed = false;
        public ServiceConnection ServiceConnection;

        //Use ServiceConnection to use the whole functional range for connecting to DocuWare.
        public Authenticator(ServiceConnection serviceConnection) {
            this.ServiceConnection = serviceConnection;
        }

        //For examples most common method to get an connection to DocuWare.
        public Authenticator(string url, string username, string password) {
            ServiceConnection = ServiceConnection.Create(new Uri(url), username, password);
        }

        //Used if you have more than one organization.
        public Authenticator(string url, string username, string password, string organization) {
            ServiceConnection = ServiceConnection.Create(new Uri(url), username, password, organization);
        }

        public void Dispose() {
            Dispose(true);
        }

        public Organization Organization {
            get { return ServiceConnection?.Organizations.FirstOrDefault()?.GetOrganizationFromSelfRelation();}
        }

        protected virtual void Dispose(bool disposing) {
            //Only dispose one time
            if (_disposed) {
                return;
            }

            if (disposing) {
                //Logout and free sc
                if (ServiceConnection == null) {
                    return;
                }
                ServiceConnection.Disconnect();
                ServiceConnection = null;
            }

            _disposed = true;
        }
    }
}
