namespace ProcessGuardService
{
    partial class ProcessGuard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CloudDaemonProcessGuard = new System.Diagnostics.Process();
            // 
            // CloudDaemonProcessGuard
            // 
            this.CloudDaemonProcessGuard.StartInfo.Domain = "";
            this.CloudDaemonProcessGuard.StartInfo.LoadUserProfile = false;
            this.CloudDaemonProcessGuard.StartInfo.Password = null;
            this.CloudDaemonProcessGuard.StartInfo.StandardErrorEncoding = null;
            this.CloudDaemonProcessGuard.StartInfo.StandardOutputEncoding = null;
            this.CloudDaemonProcessGuard.StartInfo.UserName = "";
            this.CloudDaemonProcessGuard.Exited += new System.EventHandler(this.CloudDaemonProcessGuard_Exited);
            // 
            // ProcessGuard
            // 
            this.ServiceName = "Cloud Daemon Process Guard";

        }

        #endregion

        private System.Diagnostics.Process CloudDaemonProcessGuard;
    }
}
