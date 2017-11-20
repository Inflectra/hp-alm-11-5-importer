using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using HP.ALM;
using HP.ALM.SiteAdmin;
using System.IO;

namespace Inflectra.SpiraTest.AddOns.QualityCenterImporter
{
	/// <summary>
	/// This is the code behind class for the utility that imports projects from
	/// HP Mercury Quality Center / TestDirector into Inflectra SpiraTest
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cboProject;
		private System.Windows.Forms.ComboBox cboDomain;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtLogin;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button btnAuthenticate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.Button btnNext;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected ImportForm importForm;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.CheckBox chkImportRequirements;
		public System.Windows.Forms.CheckBox chkImportTestCases;
		public System.Windows.Forms.CheckBox chkImportTestRuns;
		public System.Windows.Forms.CheckBox chkImportDefects;
        public System.Windows.Forms.CheckBox chkImportUsers;
        public CheckBox chkImportAttachments;
        public CheckBox chkImportTestSets;
        private CheckBox chkPassword;
		protected ProgressForm progressForm;

		#region Properties

        /// <summary>
        /// The current QualityCenter SiteAdmin connection object
        /// </summary>
        public HP.ALM.SiteAdmin.SAapi SaConnection
        {
            get;
            set;
        }

		/// <summary>
		/// The current QualityCenter OTA connection object
		/// </summary>
		public HP.ALM.TDConnection TdConnection
		{
            get;
            set;
        }

		#endregion

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Add any event handlers
			this.Closing += new CancelEventHandler(MainForm_Closing);
			this.cboDomain.SelectedIndexChanged += new EventHandler(cboDomain_SelectedIndexChanged);

			//Set the initial state of any buttons
			this.btnLogin.Enabled = false;
			this.btnNext.Enabled = false;

			//Create the other forms and set a handle to this form and the import form
			this.importForm = new ImportForm();
			this.progressForm = new ProgressForm();
			this.importForm.MainFormHandle = this;
			this.importForm.ProgressFormHandle = this.progressForm;
			this.progressForm.MainFormHandle = this;
			this.progressForm.ImportFormHandle = this.importForm;
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

        /// <summary>
        /// Tries to reconnect QC if we get an error condition
        /// </summary>
        public void TryReconnect()
        {
            try
            {
                TdConnection.Disconnect();
            }
            catch (Exception)
            {
                //Fail quietly
            }
            string domainName = (string)this.cboDomain.SelectedValue;
            string projectName = (string)this.cboProject.SelectedValue;
            TdConnection.InitConnectionEx(this.txtServer.Text);
            TdConnection.Login(this.txtLogin.Text, this.txtPassword.Text);
            TdConnection.Connect(domainName, projectName);
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPassword = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.cboProject = new System.Windows.Forms.ComboBox();
            this.cboDomain = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkImportAttachments = new System.Windows.Forms.CheckBox();
            this.chkImportTestSets = new System.Windows.Forms.CheckBox();
            this.chkImportUsers = new System.Windows.Forms.CheckBox();
            this.chkImportDefects = new System.Windows.Forms.CheckBox();
            this.chkImportTestRuns = new System.Windows.Forms.CheckBox();
            this.chkImportTestCases = new System.Windows.Forms.CheckBox();
            this.chkImportRequirements = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(376, 64);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(96, 23);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next >";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 64);
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
            this.label1.Size = new System.Drawing.Size(440, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "SpiraTest | Import From HP QC/ALM 11.5+";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkPassword);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.btnAuthenticate);
            this.groupBox1.Controls.Add(this.cboProject);
            this.groupBox1.Controls.Add(this.cboDomain);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtLogin);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(24, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 208);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HP ALM Configuration";
            // 
            // chkPassword
            // 
            this.chkPassword.AutoSize = true;
            this.chkPassword.Location = new System.Drawing.Point(152, 176);
            this.chkPassword.Name = "chkPassword";
            this.chkPassword.Size = new System.Drawing.Size(126, 17);
            this.chkPassword.TabIndex = 22;
            this.chkPassword.Text = "Remember Password";
            this.chkPassword.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(24, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "Server:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(96, 24);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(336, 20);
            this.txtServer.TabIndex = 20;
            this.txtServer.Text = "http://myserver/qcbin";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "Project:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Domain:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(232, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "User Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(344, 176);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(88, 23);
            this.btnLogin.TabIndex = 15;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(344, 80);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(88, 23);
            this.btnAuthenticate.TabIndex = 14;
            this.btnAuthenticate.Text = "Authenticate";
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // cboProject
            // 
            this.cboProject.Location = new System.Drawing.Point(152, 152);
            this.cboProject.Name = "cboProject";
            this.cboProject.Size = new System.Drawing.Size(280, 21);
            this.cboProject.TabIndex = 13;
            // 
            // cboDomain
            // 
            this.cboDomain.Location = new System.Drawing.Point(152, 120);
            this.cboDomain.Name = "cboDomain";
            this.cboDomain.Size = new System.Drawing.Size(280, 21);
            this.cboDomain.TabIndex = 12;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(304, 56);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(128, 20);
            this.txtPassword.TabIndex = 11;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(96, 56);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(128, 20);
            this.txtLogin.TabIndex = 10;
            this.txtLogin.Text = "alex_alm";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Inflectra.SpiraTest.AddOns.QualityCenterImporter.Properties.Resources.SpiraTest_Importer_Icon_Large;
            this.pictureBox1.Location = new System.Drawing.Point(472, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkImportAttachments);
            this.groupBox2.Controls.Add(this.chkImportTestSets);
            this.groupBox2.Controls.Add(this.chkImportUsers);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.chkImportDefects);
            this.groupBox2.Controls.Add(this.chkImportTestRuns);
            this.groupBox2.Controls.Add(this.chkImportTestCases);
            this.groupBox2.Controls.Add(this.chkImportRequirements);
            this.groupBox2.Controls.Add(this.btnNext);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(24, 272);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(480, 96);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Options";
            // 
            // chkImportAttachments
            // 
            this.chkImportAttachments.AutoSize = true;
            this.chkImportAttachments.Location = new System.Drawing.Point(358, 27);
            this.chkImportAttachments.Name = "chkImportAttachments";
            this.chkImportAttachments.Size = new System.Drawing.Size(85, 17);
            this.chkImportAttachments.TabIndex = 8;
            this.chkImportAttachments.Text = "Attachments";
            this.chkImportAttachments.UseVisualStyleBackColor = true;
            // 
            // chkImportTestSets
            // 
            this.chkImportTestSets.AutoSize = true;
            this.chkImportTestSets.Location = new System.Drawing.Point(16, 75);
            this.chkImportTestSets.Name = "chkImportTestSets";
            this.chkImportTestSets.Size = new System.Drawing.Size(71, 17);
            this.chkImportTestSets.TabIndex = 7;
            this.chkImportTestSets.Text = "Test Sets";
            this.chkImportTestSets.UseVisualStyleBackColor = true;
            // 
            // chkImportUsers
            // 
            this.chkImportUsers.Checked = true;
            this.chkImportUsers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportUsers.Location = new System.Drawing.Point(168, 48);
            this.chkImportUsers.Name = "chkImportUsers";
            this.chkImportUsers.Size = new System.Drawing.Size(88, 24);
            this.chkImportUsers.TabIndex = 6;
            this.chkImportUsers.Text = "Users";
            // 
            // chkImportDefects
            // 
            this.chkImportDefects.Location = new System.Drawing.Point(273, 24);
            this.chkImportDefects.Name = "chkImportDefects";
            this.chkImportDefects.Size = new System.Drawing.Size(136, 24);
            this.chkImportDefects.TabIndex = 5;
            this.chkImportDefects.Text = "Defects";
            // 
            // chkImportTestRuns
            // 
            this.chkImportTestRuns.Location = new System.Drawing.Point(168, 24);
            this.chkImportTestRuns.Name = "chkImportTestRuns";
            this.chkImportTestRuns.Size = new System.Drawing.Size(184, 24);
            this.chkImportTestRuns.TabIndex = 4;
            this.chkImportTestRuns.Text = "Test Runs";
            // 
            // chkImportTestCases
            // 
            this.chkImportTestCases.Checked = true;
            this.chkImportTestCases.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportTestCases.Location = new System.Drawing.Point(16, 48);
            this.chkImportTestCases.Name = "chkImportTestCases";
            this.chkImportTestCases.Size = new System.Drawing.Size(184, 24);
            this.chkImportTestCases.TabIndex = 3;
            this.chkImportTestCases.Text = "Test Cases";
            this.chkImportTestCases.CheckedChanged += new System.EventHandler(this.chkImportTestCases_CheckedChanged);
            // 
            // chkImportRequirements
            // 
            this.chkImportRequirements.Checked = true;
            this.chkImportRequirements.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportRequirements.Location = new System.Drawing.Point(16, 24);
            this.chkImportRequirements.Name = "chkImportRequirements";
            this.chkImportRequirements.Size = new System.Drawing.Size(184, 24);
            this.chkImportRequirements.TabIndex = 2;
            this.chkImportRequirements.Text = "Requirements";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(526, 373);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SpiraTest Importer for HP ALM / QC 11.5+";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MiniDump.CreateMiniDump();
        }

		/// <summary>
		/// Authenticates the user from the providing server/login/password information
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnAuthenticate_Click(object sender, System.EventArgs e)
		{
			//Disable the login and next button
			this.btnLogin.Enabled = false;
			this.btnNext.Enabled = false;

			//Make sure that a login was entered
			if (this.txtLogin.Text.Trim() == "")
			{
				MessageBox.Show ("You need to enter a QualityCenter login", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			//Make sure that a server was entered
			if (this.txtServer.Text.Trim() == "")
			{
				MessageBox.Show ("You need to enter a QualityCenter server name", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

            try
            {
                //Instantiate the connection to QualityCenter
                TdConnection = new HP.ALM.TDConnection();

                try
                {
                    SaConnection = new SAapi();
                    try
                    {
                        //The OTA API
                        TdConnection.InitConnectionEx(this.txtServer.Text);
                        TdConnection.Login(this.txtLogin.Text, this.txtPassword.Text);

                        //The Site Admin API
                        SaConnection.Login(this.txtServer.Text, this.txtLogin.Text, this.txtPassword.Text);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error Logging in to QualityCenter: " + exception.Message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //Check to see if logged in and change the UI as appropriate
                    if (TdConnection.LoggedIn)
                    {
                        MessageBox.Show("You have logged into Quality Center Successfully", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("That login/password combination was not valid for the instance of QualityCenter", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //Now we need to populate the list of domains
                    HP.ALM.List domainsOleList = TdConnection.VisibleDomains;
                    ArrayList domainsList = new ArrayList();
                    //Convert into a standard bindable list source (OLEList index starts at 1)
                    for (int i = 1; i <= domainsOleList.Count; i++)
                    {
                        domainsList.Add(domainsOleList[i]);
                    }
                    this.cboDomain.DataSource = domainsList;

                    //Make sure we have at least one domain
                    if (domainsList.Count >= 1)
                    {
                        //Reload the projects list
                        ReloadProjects();
                    }
                }
                catch (COMException exception)
                {
                    MessageBox.Show("Unable to access the HP ALM/QC API. Please make sure you that you have downloaded and installed the HP client components for HP ALM Site Admin (" + exception.Message + ")", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (FileNotFoundException exception)
                {
                    MessageBox.Show("Unable to access the HP ALM/QC API. Please make sure you that you have downloaded and installed the HP client components for HP ALM Site Admin (" + exception.Message + ")", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch (COMException exception)
            {
                MessageBox.Show("Unable to access the HP ALM/QC API. Please make sure you that you have downloaded and installed the HP client components for HP ALM (" + exception.Message + ")", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            catch (FileNotFoundException exception)
            {
                MessageBox.Show("Unable to access the HP ALM/QC API. Please make sure you that you have downloaded and installed the HP client components for HP ALM (" + exception.Message + ")", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

		/// <summary>
		/// Closes the application
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			//Close the application
			this.Close();
		}

		/// <summary>
		/// Called if the form is closed
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			//Disconnect/Logout if connected
			if (TdConnection != null)
			{
				if (TdConnection.Connected)
				{
					TdConnection.Disconnect();
				}

				if (TdConnection.LoggedIn)
				{
					TdConnection.Logout();
				}
			}
		}

		/// <summary>
		/// Called when the domain list is changed. Need to refresh the list of projects
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void cboDomain_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Reload the projects list
			ReloadProjects();
		}

		/// <summary>
		/// Reloads the list of projects for the selected domain
		/// </summary>
		private void ReloadProjects()
		{
			//Now we need to populate the list of projects
			string domainName = (string) this.cboDomain.SelectedValue;
			HP.ALM.List projectsOleList = TdConnection.get_VisibleProjects(domainName);
			ArrayList projectsList = new ArrayList();
			//Convert into a standard bindable list source (OLEList index starts at 1)
			for (int i = 1; i <= projectsOleList.Count; i++)
			{
				projectsList.Add (projectsOleList[i]);
			}
			this.cboProject.DataSource = projectsList;

			//Enable the login button if we have at least one project
			if (projectsList.Count >= 1)
			{
				this.btnLogin.Enabled = true;
			}
		}

		/// <summary>
		/// Logs into the specified project/domain combination
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			string domainName = (string) this.cboDomain.SelectedValue;
			string projectName = (string) this.cboProject.SelectedValue;

			//Default the Next button to disabled
			this.btnNext.Enabled = false;

			try
			{
                //Connect to the project through OTA API
				TdConnection.Connect (domainName, projectName);
			}
			catch (Exception exception)
			{
				MessageBox.Show ("Error Logging in to QualityCenter: " + exception.Message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Check to see if connected and change the UI as appropriate
			if (TdConnection.Connected)
			{
				MessageBox.Show ("You have connected to '" + projectName + "' Successfully", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.btnNext.Enabled = true;
			}
			else
			{
				MessageBox.Show ("Unable to connect to '" + projectName + "'!", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        /// <summary>
        ///Makes sure that we remain connected to QC during import
        /// </summary>
        public void EnsureConnected()
        {
            //If not connected, then try to reconnect
            if (!this.TdConnection.Connected)
            {
                string domainName = (string)this.cboDomain.SelectedValue;
                string projectName = (string)this.cboProject.SelectedValue;
                TdConnection.InitConnectionEx(this.txtServer.Text);
                TdConnection.Login(this.txtLogin.Text, this.txtPassword.Text);
                TdConnection.Connect(domainName, projectName);
            }

            //Make sure project is connected
            if (!this.TdConnection.ProjectConnected)
            {
                string domainName = (string)this.cboDomain.SelectedValue;
                string projectName = (string)this.cboProject.SelectedValue;
                TdConnection.Connect(domainName, projectName);
            }
        }

		/// <summary>
		/// Called when the Next button is clicked. Switches to the second form
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void btnNext_Click(object sender, System.EventArgs e)
		{
            //Store the info in settings for later
            Properties.Settings.Default.QcUrl = this.txtServer.Text.Trim();
            Properties.Settings.Default.QcUserName = this.txtLogin.Text.Trim();
            if (chkPassword.Checked)
            {
                Properties.Settings.Default.QcPassword = this.txtPassword.Text;  //Don't trip in case it contains a space
            }
            else
            {
                Properties.Settings.Default.QcPassword = "";
            }
            Properties.Settings.Default.Requirements = this.chkImportRequirements.Checked;
            Properties.Settings.Default.TestCases = this.chkImportTestCases.Checked;
            Properties.Settings.Default.TestRuns = this.chkImportTestRuns.Checked;
            Properties.Settings.Default.TestSets = this.chkImportTestSets.Checked;
            Properties.Settings.Default.Defects = this.chkImportDefects.Checked;
            Properties.Settings.Default.Attachments = this.chkImportAttachments.Checked;
            Properties.Settings.Default.Users = this.chkImportUsers.Checked;
            Properties.Settings.Default.Save();

			//Hide the current form
			this.Hide();

			//Show the second page in the import wizard
			this.importForm.Show();
		}

		/// <summary>
		/// Change the active status of the test run import checkbox depending on this selection
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="e">The event arguments</param>
		private void chkImportTestCases_CheckedChanged(object sender, System.EventArgs e)
		{
			this.chkImportTestRuns.Enabled = this.chkImportTestCases.Checked;
			this.chkImportTestRuns.Checked = false;
        }

        /// <summary>
        /// Populates the fields when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.txtServer.Text = Properties.Settings.Default.QcUrl;
            this.txtLogin.Text = Properties.Settings.Default.QcUserName;
            if (String.IsNullOrEmpty(Properties.Settings.Default.QcPassword))
            {
                this.chkPassword.Checked = false;
                this.txtPassword.Text = "";
            }
            else
            {
                this.chkPassword.Checked = true;
                this.txtPassword.Text = Properties.Settings.Default.QcPassword;
            }

            this.chkImportRequirements.Checked = Properties.Settings.Default.Requirements;
            this.chkImportTestCases.Checked = Properties.Settings.Default.TestCases;
            this.chkImportTestRuns.Checked = Properties.Settings.Default.TestRuns;
            this.chkImportTestSets.Checked = Properties.Settings.Default.TestSets;
            this.chkImportDefects.Checked = Properties.Settings.Default.Defects;
            this.chkImportAttachments.Checked = Properties.Settings.Default.Attachments;
            this.chkImportUsers.Checked = Properties.Settings.Default.Users;
        }
	}
}
