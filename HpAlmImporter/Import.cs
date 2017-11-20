using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ServiceModel;

namespace Inflectra.SpiraTest.AddOns.QualityCenterImporter
{
	/// <summary>
	/// This is the code behind class for the utility that imports projects from
	/// HP Mercury Quality Center / TestDirector into Inflectra SpiraTest
	/// </summary>
	public class ImportForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtLogin;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtServer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnImport;
		protected MainForm mainForm;
		protected ProgressForm progressForm;
		protected SpiraImportExport.ImportExportClient spiraImportProxy;
		protected CookieContainer cookieContainer;
		private System.Windows.Forms.PictureBox pictureBox1;
        private ProgressBar progressBar1;
        private CheckBox chkPassword;
        private GroupBox groupBox2;
        private Label label5;
        private Label label4;
        private TextBox txtNewUserPassword;

		protected const string IMPORT_WEB_SERVICES_URL = "/Services/v4_0/ImportExport.svc";

		#region Properties
		
		public MainForm MainFormHandle
		{
			get
			{
				return this.mainForm;
			}
			set
			{
				this.mainForm = value;
			}
		}

        /// <summary>
        /// Returns the contents of the password text box
        /// </summary>
        public string Password
        {
            get
            {
                return this.txtPassword.Text;
            }
        }

        /// <summary>
        /// Returns the contents of the new user password text box
        /// </summary>
        public string NewUserPassword
        {
            get
            {
                return this.txtNewUserPassword.Text;
            }
        }

		public ProgressForm ProgressFormHandle
		{
			get
			{
				return this.progressForm;
			}
			set
			{
				this.progressForm = value;
			}
		}

		/// <summary>
		/// The current SpiraTest web services proxy class
		/// </summary>
		public SpiraImportExport.ImportExportClient SpiraImportProxy
		{
			get
			{
				return this.spiraImportProxy;
			}
			set
			{
				this.spiraImportProxy = value;
			}
		}

		#endregion

		public ImportForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Add any event handlers
			this.Closing += new CancelEventHandler(ImportForm_Closing);

			//Set the initial state of any buttons
			this.btnLogin.Enabled = true;
			this.btnImport.Enabled = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPassword = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNewUserPassword = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(408, 331);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(96, 23);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Start Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(216, 331);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "SpiraTest | Import From HP QC/ALM 11.5+";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkPassword);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtLogin);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(24, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 176);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SpiraTest Configuration";
            // 
            // chkPassword
            // 
            this.chkPassword.AutoSize = true;
            this.chkPassword.Location = new System.Drawing.Point(112, 138);
            this.chkPassword.Name = "chkPassword";
            this.chkPassword.Size = new System.Drawing.Size(126, 17);
            this.chkPassword.TabIndex = 23;
            this.chkPassword.Text = "Remember Password";
            this.chkPassword.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "SpiraTest URL:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtServer
            // 
            this.txtServer.AcceptsReturn = true;
            this.txtServer.Location = new System.Drawing.Point(112, 32);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(320, 20);
            this.txtServer.TabIndex = 20;
            this.txtServer.Text = "http://myserver/SpiraTest";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(40, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "User Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(344, 144);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(88, 23);
            this.btnLogin.TabIndex = 15;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(112, 112);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(320, 20);
            this.txtPassword.TabIndex = 11;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(112, 80);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(320, 20);
            this.txtLogin.TabIndex = 10;
            this.txtLogin.Text = "administrator";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(312, 331);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(88, 23);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "< Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Inflectra.SpiraTest.AddOns.QualityCenterImporter.Properties.Resources.SpiraTest_Importer_Icon_Large;
            this.pictureBox1.Location = new System.Drawing.Point(472, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.OrangeRed;
            this.progressBar1.Location = new System.Drawing.Point(-4, 314);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(533, 10);
            this.progressBar1.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtNewUserPassword);
            this.groupBox2.Location = new System.Drawing.Point(24, 231);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(480, 74);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Options";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(132, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(329, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "This is the password for imported users. \'PleaseChange\' is the default";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 17);
            this.label4.TabIndex = 25;
            this.label4.Text = "New User Password:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNewUserPassword
            // 
            this.txtNewUserPassword.Location = new System.Drawing.Point(132, 23);
            this.txtNewUserPassword.Name = "txtNewUserPassword";
            this.txtNewUserPassword.PasswordChar = '*';
            this.txtNewUserPassword.Size = new System.Drawing.Size(300, 20);
            this.txtNewUserPassword.TabIndex = 24;
            this.txtNewUserPassword.Text = "PleaseChange";
            // 
            // ImportForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(526, 361);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ImportForm";
            this.Text = "SpiraTest Importer for HP ALM / QC 11.5+";
            this.Load += new System.EventHandler(this.ImportForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Closes the application
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			//Close the application
			this.MainFormHandle.Close();
		}

		/// <summary>
		/// Called if the form is closed
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void ImportForm_Closing(object sender, CancelEventArgs e)
		{
			//Disconnect/Logout if connected
			if (MainFormHandle.TdConnection != null)
			{
				if (MainFormHandle.TdConnection.Connected)
				{
					MainFormHandle.TdConnection.Disconnect();
				}

				if (MainFormHandle.TdConnection.LoggedIn)
				{
					MainFormHandle.TdConnection.Logout();
				}
			}
		}

		/// <summary>
		/// Logs into the specified project/domain combination
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			//Make sure that a login was entered
			if (this.txtLogin.Text.Trim() == "")
			{
				MessageBox.Show ("You need to enter a SpiraTest login", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			//Make sure that a server was entered
			if (this.txtServer.Text.Trim() == "")
			{
				MessageBox.Show ("You need to enter a SpiraTest web-server URL", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

            //Store the info in settings for later
            Properties.Settings.Default.SpiraUrl = this.txtServer.Text.Trim();
            Properties.Settings.Default.SpiraUserName = this.txtLogin.Text.Trim();
            if (chkPassword.Checked)
            {
                Properties.Settings.Default.SpiraPassword = this.txtPassword.Text;  //Don't trip in case it contains a space
            }
            else
            {
                Properties.Settings.Default.SpiraPassword = "";
            }
            Properties.Settings.Default.Save();

            //Default the Start Import and login button to disabled
            this.btnImport.Enabled = false;
            this.btnLogin.Enabled = false;
            this.progressBar1.Style = ProgressBarStyle.Marquee;

			try
			{
				//Instantiate the web-service proxy class and set the URL from the text box
				SpiraImportProxy = new SpiraImportExport.ImportExportClient();

                //Set the end-point and allow cookies
                SpiraImportProxy.Endpoint.Address = new EndpointAddress(Properties.Settings.Default.SpiraUrl + IMPORT_WEB_SERVICES_URL);
                BasicHttpBinding httpBinding = (BasicHttpBinding)SpiraImportProxy.Endpoint.Binding;
                ConfigureBinding(httpBinding, SpiraImportProxy.Endpoint.Address.Uri);

                //Authenticate asynchronously
                SpiraImportProxy.Connection_AuthenticateCompleted += new EventHandler<SpiraImportExport.Connection_AuthenticateCompletedEventArgs>(spiraImportExport_Connection_AuthenticateCompleted);
                SpiraImportProxy.Connection_AuthenticateAsync(Properties.Settings.Default.SpiraUserName, this.txtPassword.Text);
			}
            catch (TimeoutException exception)
            {
                // Handle the timeout exception.
                this.progressBar1.Style = ProgressBarStyle.Blocks;
                this.btnLogin.Enabled = true;
                SpiraImportProxy.Abort();
                MessageBox.Show("A timeout error occurred! (" + exception.Message + ")", "Timeout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CommunicationException exception)
            {
                // Handle the communication exception.
                this.progressBar1.Style = ProgressBarStyle.Blocks;
                this.btnLogin.Enabled = true;

                SpiraImportProxy.Abort();
                MessageBox.Show("A communication error occurred! (" + exception.Message + ")", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

        /// <summary>
        /// Called when the Authentication call is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void spiraImportExport_Connection_AuthenticateCompleted(object sender, SpiraImportExport.Connection_AuthenticateCompletedEventArgs e)
        {
            //Stop the progress bar
            this.progressBar1.Style = ProgressBarStyle.Blocks;
            this.btnLogin.Enabled = true;  //Enable the connection button

            //See if we completed successfully or not
            if (e.Error != null)
            {
                //Error occurred
                MessageBox.Show("Unable to connect to Server. The error message was: '" + e.Error.Message + "'", "Connect Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (e.Result)
            {
                //Display success
                this.btnImport.Enabled = true; 
                MessageBox.Show("Successfully connected to the Server!", "Connect Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Authentication failed
                MessageBox.Show("Unable to authenticate with Server. Please check the username and password and try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Configure the SOAP connection for HTTP or HTTPS depending on what was specified
        /// </summary>
        /// <param name="httpBinding"></param>
        /// <param name="uri"></param>
        /// <remarks>Allows self-signed certs to be used</remarks>
        public static void ConfigureBinding(BasicHttpBinding httpBinding, Uri uri)
        {
            //Handle SSL if necessary
            if (uri.Scheme == "https")
            {
                httpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

                //Allow self-signed certificates
                PermissiveCertificatePolicy.Enact("");
            }
            else
            {
                httpBinding.Security.Mode = BasicHttpSecurityMode.None;
            }
        }

		/// <summary>
		/// Called when the Back button is clicked. Switches to the first form
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnBack_Click(object sender, System.EventArgs e)
		{
			//Hide the current form
			this.Hide();

			//Show the second page in the import wizard
			MainFormHandle.Show();
		}

		/// <summary>
		/// Called when the Start Import button is clicked.
		/// Starts the actual migration from QualityCenter to SpiraTest
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnImport_Click(object sender, System.EventArgs e)
		{
			//Hide the current form
			this.Hide();

			//Show the progress page
			ProgressFormHandle.Show();

			//Kickoff the import
			ProgressFormHandle.StartImport();
		}

        /// <summary>
        /// Populates the fields when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportForm_Load(object sender, EventArgs e)
        {
            this.txtServer.Text = Properties.Settings.Default.SpiraUrl;
            this.txtLogin.Text = Properties.Settings.Default.SpiraUserName;
            if (String.IsNullOrEmpty(Properties.Settings.Default.SpiraPassword))
            {
                this.chkPassword.Checked = false;
                this.txtPassword.Text = "";
            }
            else
            {
                this.chkPassword.Checked = true;
                this.txtPassword.Text = Properties.Settings.Default.SpiraPassword;
            }
        }
	}
}
