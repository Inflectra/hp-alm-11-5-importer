using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;

using Inflectra.SpiraTest.AddOns.QualityCenterImporter.SpiraImportExport;
using HP.ALM;
using System.ServiceModel;
using System.Xml;

namespace Inflectra.SpiraTest.AddOns.QualityCenterImporter
{
    /// <summary>
    /// This is the code behind class for the utility that imports projects from
    /// HP Mercury Quality Center / TestDirector into Inflectra SpiraTest
    /// </summary>
    public class ProgressForm : System.Windows.Forms.Form
    {
        //Project role for new users
        private const int PROJECT_ROLE_ID = 5;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        protected MainForm mainForm;
        protected ImportForm importForm;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblProgress3;
        private System.Windows.Forms.Label lblProgress4;
        private System.Windows.Forms.Label lblProgress5;
        private System.Windows.Forms.Label lblProgress6;
        private System.Windows.Forms.Label lblProgress1;
        protected CookieContainer cookieContainer;
        protected Dictionary<int, int> requirementsMapping;
        protected Dictionary<int, int> testFolderMapping;
        protected Dictionary<int, int> testCaseMapping;
        protected Dictionary<int, int> testStepMapping;
        protected Dictionary<int, int> testSetFolderMapping;
        protected Dictionary<int, int> testSetMapping;
        protected Dictionary<int, int> testSetTestCaseMapping;
        protected Dictionary<string, int> usersMapping;
        protected Dictionary<int, int> testRunStepMapping;
        protected Dictionary<int, int> testRunStepToTestStepMapping;
        protected Dictionary<int, int> incidentMapping;
        protected Dictionary<int, int> qcBugToRunStepMapping;
        protected Dictionary<string, int> incidentPriorityMapping;
        protected Dictionary<string, int> incidentSeverityMapping;
        protected Dictionary<string, int> incidentStatusMapping;
        protected Dictionary<int, int> releaseMapping = new Dictionary<int,int>();
        protected Dictionary<int, int> iterationMapping = new Dictionary<int, int>();
        protected Dictionary<int, int> releaseFolderMapping = new Dictionary<int, int>();
        protected Dictionary<string, int> hostMapping = new Dictionary<string, int>();
        protected Dictionary<int, int> customValueMapping = new Dictionary<int, int>();
        protected Dictionary<string, int> customValueNameMapping = new Dictionary<string, int>();
        protected Dictionary<string, int> customPropertyMapping = new Dictionary<string, int>();
        protected Dictionary<string, int> customPropertyMappingType = new Dictionary<string, int>();
        private Label lblProgress7;
        private ProgressBar progressBar1;
        private Label lblProgress2;

        protected const int SPIRA_ARTIFACT_ID_REQUIREMENT = 1;

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

        public ImportForm ImportFormHandle
        {
            get
            {
                return this.importForm;
            }
            set
            {
                this.importForm = value;
            }
        }

        #endregion

        public ProgressForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Add any event handlers
            this.Closing += new CancelEventHandler(ProgressForm_Closing);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProgress2 = new System.Windows.Forms.Label();
            this.lblProgress7 = new System.Windows.Forms.Label();
            this.lblProgress6 = new System.Windows.Forms.Label();
            this.lblProgress5 = new System.Windows.Forms.Label();
            this.lblProgress4 = new System.Windows.Forms.Label();
            this.lblProgress3 = new System.Windows.Forms.Label();
            this.lblProgress1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(408, 286);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 23);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "Done";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 286);
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Inflectra.SpiraTest.AddOns.QualityCenterImporter.Properties.Resources.SpiraTest_Importer_Icon_Large;
            this.pictureBox1.Location = new System.Drawing.Point(472, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblProgress2);
            this.groupBox1.Controls.Add(this.lblProgress7);
            this.groupBox1.Controls.Add(this.lblProgress6);
            this.groupBox1.Controls.Add(this.lblProgress5);
            this.groupBox1.Controls.Add(this.lblProgress4);
            this.groupBox1.Controls.Add(this.lblProgress3);
            this.groupBox1.Controls.Add(this.lblProgress1);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(24, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 184);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import Progress";
            // 
            // lblProgress2
            // 
            this.lblProgress2.Location = new System.Drawing.Point(40, 44);
            this.lblProgress2.Name = "lblProgress2";
            this.lblProgress2.Size = new System.Drawing.Size(368, 16);
            this.lblProgress2.TabIndex = 27;
            this.lblProgress2.Text = ">>    Releases Imported";
            // 
            // lblProgress7
            // 
            this.lblProgress7.Location = new System.Drawing.Point(40, 160);
            this.lblProgress7.Name = "lblProgress7";
            this.lblProgress7.Size = new System.Drawing.Size(368, 16);
            this.lblProgress7.TabIndex = 26;
            this.lblProgress7.Text = ">>    Incidents Imported";
            // 
            // lblProgress6
            // 
            this.lblProgress6.Location = new System.Drawing.Point(40, 136);
            this.lblProgress6.Name = "lblProgress6";
            this.lblProgress6.Size = new System.Drawing.Size(368, 16);
            this.lblProgress6.TabIndex = 25;
            this.lblProgress6.Text = ">>    Test Runs Imported";
            // 
            // lblProgress5
            // 
            this.lblProgress5.Location = new System.Drawing.Point(40, 112);
            this.lblProgress5.Name = "lblProgress5";
            this.lblProgress5.Size = new System.Drawing.Size(368, 16);
            this.lblProgress5.TabIndex = 24;
            this.lblProgress5.Text = ">>    Test Sets Imported";
            // 
            // lblProgress4
            // 
            this.lblProgress4.Location = new System.Drawing.Point(40, 88);
            this.lblProgress4.Name = "lblProgress4";
            this.lblProgress4.Size = new System.Drawing.Size(368, 16);
            this.lblProgress4.TabIndex = 22;
            this.lblProgress4.Text = ">>    Test Cases Imported";
            // 
            // lblProgress3
            // 
            this.lblProgress3.Location = new System.Drawing.Point(40, 65);
            this.lblProgress3.Name = "lblProgress3";
            this.lblProgress3.Size = new System.Drawing.Size(368, 16);
            this.lblProgress3.TabIndex = 21;
            this.lblProgress3.Text = ">>    Requirements Imported";
            // 
            // lblProgress1
            // 
            this.lblProgress1.Location = new System.Drawing.Point(40, 24);
            this.lblProgress1.Name = "lblProgress1";
            this.lblProgress1.Size = new System.Drawing.Size(368, 16);
            this.lblProgress1.TabIndex = 26;
            this.lblProgress1.Text = ">>    Users Imported";
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.OrangeRed;
            this.progressBar1.Location = new System.Drawing.Point(-3, 268);
            this.progressBar1.Maximum = 7;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(533, 10);
            this.progressBar1.TabIndex = 28;
            // 
            // ProgressForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(526, 314);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExit);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.Text = "SpiraTest | Import From HP QC/ALM 11.5+";
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
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
            //Hide the current form
            this.Hide();

            //Return to the main form
            MainFormHandle.Show();
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The event arguments</param>
        private void btnExit_Click(object sender, System.EventArgs e)
        {
            //Close the application
            this.MainFormHandle.Close();
        }

        /// <summary>
        /// Called if the form is closed
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="e">The event arguments</param>
        private void ProgressForm_Closing(object sender, CancelEventArgs e)
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
        /// Starts the background thread for importing the data
        /// </summary>
        public void StartImport()
        {
            //Set the initial state of any buttons
            this.btnCancel.Enabled = true;
            this.btnExit.Enabled = false;

            //Clear the progress labels
            ProgressForm_OnProgressUpdate(0);

            //First change the cursor to an hourglass
            this.Cursor = Cursors.WaitCursor;

            //Start the background thread that performs the import
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ImportData));
        }

        /// <summary>
        /// Updates the form state when the import has finished
        /// </summary>
        public void ProgressForm_OnFinish()
        {
            //Change the cursor back to the default
            this.Cursor = Cursors.Default;

            //Enable the Exit button and disable cancel
            this.btnCancel.Enabled = false;
            this.btnExit.Enabled = true;

            //Display a message
            MessageBox.Show("SpiraTest Import Successful!", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Displays any errors raised by the import thread process
        /// </summary>
        /// <param name="exception">The exception raised</param>
        public void ProgressForm_OnError(Exception exception)
        {
            //Change the cursor back to the default
            this.Cursor = Cursors.Default;

            //Enable the Exit button and disable cancel
            this.btnCancel.Enabled = false;
            this.btnExit.Enabled = true;

            //Display the exception error message
            MessageBox.Show("SpiraTest Import Failed. Error: " + exception.Message, "Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Displays any errors raised by the import thread process
        /// </summary>
        /// <param name="exception">The exception raised</param>
        public void ProgressForm_OnError(string message)
        {
            //Change the cursor back to the default
            this.Cursor = Cursors.Default;

            //Enable the Exit button and disable cancel
            this.btnCancel.Enabled = false;
            this.btnExit.Enabled = true;

            //Display the exception error message
            MessageBox.Show(message, "Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Updates the progress display of the form
        /// </summary>
        /// <param name="progress">The progress state</param>
        public void ProgressForm_OnProgressUpdate(int progress)
        {
            //Make all the controls up to the specified one visible
            for (int i = 1; i <= 7; i++)
            {
                this.Controls.Find("lblProgress" + i.ToString(), true)[0].Visible = (i <= progress);
            }

            //Also update the progress bar
            this.progressBar1.Value = progress;
        }

        /// <summary>
        /// This method is responsible for actually importing the data
        /// </summary>
        /// <param name="stateInfo">State information handle</param>
        /// <remarks>This runs in background thread to avoid freezing the progress form</remarks>
        protected void ImportData(object stateInfo)
        {
            //First open up the textfile that we will log information to (used for debugging purposes)
            string debugFile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Spira_QCImport.log";
            StreamWriter streamWriter = File.CreateText(debugFile);

            try
            {
                streamWriter.WriteLine("Starting import at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

                //Create the mapping hashtables
                this.requirementsMapping = new Dictionary<int, int>();
                this.testCaseMapping = new Dictionary<int, int>();
                this.testFolderMapping = new Dictionary<int, int>();
                this.testStepMapping = new Dictionary<int, int>();
                this.usersMapping = new Dictionary<string, int>();
                this.testRunStepMapping = new Dictionary<int, int>();
                this.testRunStepToTestStepMapping = new Dictionary<int, int>();
                this.testSetFolderMapping = new Dictionary<int, int>();
                this.testSetMapping = new Dictionary<int, int>();
                this.testSetTestCaseMapping = new Dictionary<int, int>();
                this.incidentStatusMapping = new Dictionary<string, int>();
                this.incidentPriorityMapping = new Dictionary<string, int>();
                this.incidentSeverityMapping = new Dictionary<string, int>();
                this.incidentMapping = new Dictionary<int, int>();
                this.qcBugToRunStepMapping = new Dictionary<int, int>();

                //Connect to Spira
                streamWriter.WriteLine("Connecting to Spira...");
                bool success = ImportFormHandle.SpiraImportProxy.Connection_Authenticate2(Properties.Settings.Default.SpiraUserName, ImportFormHandle.Password, "HpAlmImporter");
                if (!success)
                {
                    string message = "Failed to authenticate with Spira using login: '" + Properties.Settings.Default.SpiraUserName + "' so terminating import!";
                    streamWriter.WriteLine(message);
                    streamWriter.Close();

                    //Display the exception message
                    ProgressForm_OnError(message);
                    return;
                }

                //1) Create a new project
                int newProjectId = ImportProject(streamWriter);

                //2) Get the users and import - if we don't want to import user, map all QC users to single SpiraId
                int userId = -1;
                string newPassword = ImportFormHandle.NewUserPassword;
                if (!this.MainFormHandle.chkImportUsers.Checked)
                {
                    RemoteUser remoteUser = new RemoteUser();
                    remoteUser.FirstName = "Quality";
                    remoteUser.LastName = "Center";
                    remoteUser.UserName = "qualitycenter";
                    remoteUser.EmailAddress = "qualitycenter@mycompany.com";
                    remoteUser.Active = true;
                    remoteUser.Approved = true;
                    remoteUser.Admin = false;
                    userId = ImportFormHandle.SpiraImportProxy.User_Create(remoteUser, newPassword, "What was my default email address?", remoteUser.EmailAddress, PROJECT_ROLE_ID).UserId.Value;
                }

                HP.ALM.IList usersList = MainFormHandle.TdConnection.UsersList;
                for (int i = 1; i <= usersList.Count; i++)
                {
                    //Extract the user data
                    string userName = (string)usersList[i];
                    string firstName = userName;
                    string lastName = userName;
                    string emailAddress = userName + "@mycompany.com";
                    bool isActive = true;

                    if (this.MainFormHandle.chkImportUsers.Checked)
                    {
                        //Use the SiteAdmin api to get the extended user information, if the user doesn't have permissions, log and carry on
                        try
                        {
                            string userXml = MainFormHandle.SaConnection.GetUser(userName);
                            //streamWriter.WriteLine("User XML: " + userXml);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(userXml);
                            XmlNode xmlNode = xmlDoc.SelectSingleNode("/GetUser/TDXItem/ACC_IS_ACTIVE");
                            if (xmlNode != null && !String.IsNullOrEmpty(xmlNode.InnerText))
                            {
                                isActive = (xmlNode.InnerText == "Y");
                            }
                            xmlNode = xmlDoc.SelectSingleNode("/GetUser/TDXItem/EMAIL");
                            if (xmlNode != null && !String.IsNullOrEmpty(xmlNode.InnerText))
                            {
                                emailAddress = MakeXmlSafe(xmlNode.InnerText);
                            }
                            xmlNode = xmlDoc.SelectSingleNode("/GetUser/TDXItem/FULL_NAME");
                            if (xmlNode != null && !String.IsNullOrEmpty(xmlNode.InnerText))
                            {
                                string fullName = MakeXmlSafe(xmlNode.InnerText);
                                string[] names = fullName.Split(' ');
                                if (names.Length > 1)
                                {
                                    firstName = names[0];
                                    lastName = names[1];
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            streamWriter.WriteLine("Unable to access the user through the SiteAdmin API, no user properties will be imported: " + exception.Message);
                        }

                        //We only have access to the username via the OTA API
                        //So we will populate a shell record into SpiraTest
                        //Default to observer role for all imports, for security reasons
                        RemoteUser remoteUser = new RemoteUser();
                        remoteUser.FirstName = firstName;
                        remoteUser.LastName = lastName;
                        remoteUser.UserName = userName;
                        remoteUser.EmailAddress = emailAddress;
                        remoteUser.Active = isActive;
                        remoteUser.Approved = true;
                        remoteUser.Admin = false;
                        userId = ImportFormHandle.SpiraImportProxy.User_Create(remoteUser, newPassword, "What was my default email address?", emailAddress, PROJECT_ROLE_ID).UserId.Value;
                    }

                    //Add the mapping to the hashtable for use later on
                    this.usersMapping.Add(userName, userId);
                }

                //**** Show that we've imported users ****
                if (this.MainFormHandle.chkImportUsers.Checked)
                {
                    streamWriter.WriteLine("Users Imported");
                    this.ProgressForm_OnProgressUpdate(1);
                }

                //3) Get the releases and import
                MainFormHandle.EnsureConnected();

                //In future may want to add a checkbox for releases, right now, always import them
                if (true)
                {
                    //First Release Folders (and their child releases)
                    HP.ALM.ReleaseFolderFactory relFolderFactory = (HP.ALM.ReleaseFolderFactory)MainFormHandle.TdConnection.ReleaseFolderFactory;
                    ImportReleaseFolders(streamWriter, relFolderFactory, null);

                    //Next any top-level releases
                    HP.ALM.ReleaseFactory releaseFactory = (HP.ALM.ReleaseFactory)MainFormHandle.TdConnection.ReleaseFactory;
                    ImportReleases(streamWriter, releaseFactory, null);

                    //**** Show that we've imported releases ****
                    streamWriter.WriteLine("Releases Imported");
                    this.ProgressForm_OnProgressUpdate(2);
                }

                //3) Get the requirements and import
                MainFormHandle.EnsureConnected();
                if (this.MainFormHandle.chkImportRequirements.Checked)
                {
                    HP.ALM.ReqFactory reqFactory = (HP.ALM.ReqFactory)MainFormHandle.TdConnection.ReqFactory;
                    ImportRequirements(streamWriter, reqFactory);

                    //**** Show that we've imported requirements ****
                    streamWriter.WriteLine("Requirements Imported");
                    this.ProgressForm_OnProgressUpdate(3);
                }

                //4) Get the test cases and import
                MainFormHandle.EnsureConnected();

                if (this.MainFormHandle.chkImportTestCases.Checked)
                {
                    //First we need to build out the folders from the 'subject tree'
                    HP.ALM.TreeManager treeManager = (HP.ALM.TreeManager)MainFormHandle.TdConnection.TreeManager;
                    HP.ALM.List subjectList;
                    try
                    {
                        subjectList = treeManager.get_RootList(1); //Get the Subject tree root list (code 1)
                    }
                    catch (Exception)
                    {
                        //If we get an exception, try to reconnect and re-execute
                        MainFormHandle.TryReconnect();
                        treeManager = (HP.ALM.TreeManager)MainFormHandle.TdConnection.TreeManager;
                        subjectList = treeManager.get_RootList(1); //Get the Subject tree root list (code 1)
                    }
                    ImportSubjectNode(treeManager, subjectList, null);

                    //Now we import the test cases themselves
                    HP.ALM.TestFactory testFactory = (HP.ALM.TestFactory)MainFormHandle.TdConnection.TestFactory;
                    HP.ALM.TDFilter hierFilter = (HP.ALM.TDFilter)testFactory.Filter;
                    hierFilter.set_Order("ts_path", 1);
                    HP.ALM.List testList;
                    try
                    {
                        testList = (HP.ALM.List)hierFilter.NewList();
                    }
                    catch (Exception)
                    {
                        //If we get an exception, try to reconnect and re-execute
                        MainFormHandle.TryReconnect();
                        testFactory = (HP.ALM.TestFactory)MainFormHandle.TdConnection.TestFactory;
                        hierFilter = (HP.ALM.TDFilter)testFactory.Filter;
                        testList = (HP.ALM.List)hierFilter.NewList();
                    }

                    //Now the first pass of importing the test cases, used to just import the test case itself
                    //since we may have linked test cases that link to 'later' test cases
                    foreach (HP.ALM.Test testObject in testList)
                    {
                        //Extract the test info
                        int testId = (int)testObject.ID;
                        streamWriter.WriteLine("Importing Test Case: " + testId.ToString());
                        string name = "Un-named";
                        bool objectExists = true;
                        try
                        {
                            if (testObject.Name != null)
                            {
                                name = MakeXmlSafe(testObject.Name);
                            }
                        }
                        catch (Exception)
                        {
                            //Sometimes the underlying object doesn't exist, even though an ID is available
                            streamWriter.WriteLine("Unable to import test case " + testId + " as its object is not accessible!");
                            objectExists = false;
                        }

                        if (objectExists)
                        {
                            string testCaseDescription = "";
                            if (testObject["ts_description"] != null)
                            {
                                testCaseDescription = MakeXmlSafe(testObject["ts_description"].ToString());
                            }
                            int subjectNodeId = -1;
                            if (testObject["ts_subject"] != null)
                            {
                                try
                                {
                                    HP.ALM.SubjectNode subjectNode = (HP.ALM.SubjectNode)testObject["ts_subject"];
                                    subjectNodeId = subjectNode.NodeID;
                                }
                                catch (Exception)
                                {
                                    //Fail quietly
                                }
                            }

                            //Locate the subject path and get the corresponding SpiraTest folder id
                            Nullable<int> folderId = null;
                            if (subjectNodeId != -1 && this.testFolderMapping.ContainsKey(subjectNodeId))
                            {
                                folderId = (int)this.testFolderMapping[subjectNodeId];
                            }

                            string authorName = "";
                            if (testObject["ts_responsible"] != null)
                            {
                                authorName = testObject["ts_responsible"].ToString();
                            }

                            //Lookup the author from the user mapping hashtable
                            Nullable<int> authorId = null;
                            if (authorName != null && authorName != "")
                            {
                                if (this.usersMapping.ContainsKey(authorName))
                                {
                                    authorId = (int)this.usersMapping[authorName];
                                }
                            }

                            //Now we need to see if this test has any parameters
                            HP.ALM.StepParams stepParams = null;
                            try
                            {
                                if (testObject.Params != null)
                                {
                                    stepParams = (HP.ALM.StepParams)testObject.Params;
                                }
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    //If there is an error try reconnecting
                                    MainFormHandle.TryReconnect();
                                    if (testObject.Params != null)
                                    {
                                        stepParams = (HP.ALM.StepParams)testObject.Params;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    //Just log a warning and move on
                                    streamWriter.WriteLine("*WARNING*: Unable to get test parameters for test case " + testId + " - continuing. Error message = '" + exception.Message + "'");
                                }
                            }

                            //Load the test case and capture the new id
                            Nullable<int> newTestCaseId = null;
                            RemoteTestCase remoteTestCase = new RemoteTestCase();
                            try
                            {
                                //Populate the test case
                                remoteTestCase.Name = name;
                                remoteTestCase.Description = MakeXmlSafe(testCaseDescription);
                                remoteTestCase.AuthorId = authorId;
                                remoteTestCase.Active = true;

                                List<RemoteArtifactCustomProperty> customProperties = new List<RemoteArtifactCustomProperty>();
                                ImportCustomField(streamWriter, testObject, "ts_user_01", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_02", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_03", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_04", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_05", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_06", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_07", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_08", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_09", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_10", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_11", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_12", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_13", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_14", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_15", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_16", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_17", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_18", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_19", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_20", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_21", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_22", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_23", customProperties);
                                ImportCustomField(streamWriter, testObject, "ts_user_24", customProperties);
                                remoteTestCase.CustomProperties = customProperties.ToArray();
                                newTestCaseId = ImportFormHandle.SpiraImportProxy.TestCase_Create(remoteTestCase, folderId).TestCaseId;
                            }
                            catch (FaultException<ServiceFaultMessage> exception)
                            {
                                //Need to get the underlying message fault
                                ServiceFaultMessage detail = exception.Detail;
                                if (detail.Type == "SessionNotAuthenticated")
                                {
                                    try
                                    {
                                        //Try reconnecting and then re-adding the test case
                                        ImportFormHandle.SpiraImportProxy.Connection_Authenticate(Properties.Settings.Default.SpiraUserName, ImportFormHandle.Password);
                                        ImportFormHandle.SpiraImportProxy.Connection_ConnectToProject(newProjectId);
                                        newTestCaseId = ImportFormHandle.SpiraImportProxy.TestCase_Create(remoteTestCase, folderId).TestCaseId;
                                    }
                                    catch (Exception exception2)
                                    {
                                        //If we have an error, log it and continue
                                        streamWriter.WriteLine("Error adding test case " + testId + " to SpiraTest (" + exception2.Message + ")");
                                    }
                                }
                                else
                                {
                                    //If we have an error, log it and continue
                                    streamWriter.WriteLine("Error adding test case " + testId + " to SpiraTest (" + exception.Message + ")");
                                }
                            }
                            catch (Exception exception)
                            {
                                //If we have an error, log it and continue
                                streamWriter.WriteLine("Error adding test case " + testId + " to SpiraTest (" + exception.Message + ")");
                            }

                            if (newTestCaseId.HasValue)
                            {
                                //Now load in any parameters
                                if (stepParams != null && stepParams.Count > 0)
                                {
                                    for (int i = 0; i < stepParams.Count; i++)
                                    {
                                        string paramName = "";
                                        if (!String.IsNullOrEmpty(stepParams.ParamName[i]))
                                        {
                                            paramName = MakeXmlSafe(stepParams.ParamName[i]);
                                        }
                                        string defaultValue = "";
                                        if (!String.IsNullOrEmpty(stepParams.ParamValue[i]))
                                        {
                                            defaultValue = MakeXmlSafe(stepParams.ParamValue[i]);
                                        }
                                        if (paramName != "")
                                        {
                                            try
                                            {
                                                //Need to replace any spaces in the parameter name with underscores
                                                RemoteTestCaseParameter remoteTestCaseParameter = new RemoteTestCaseParameter();
                                                remoteTestCaseParameter.TestCaseId = newTestCaseId.Value;
                                                remoteTestCaseParameter.Name = paramName.ToLowerInvariant().Replace(" ", "_");
                                                remoteTestCaseParameter.DefaultValue = defaultValue;
                                                ImportFormHandle.SpiraImportProxy.TestCase_AddParameter(remoteTestCaseParameter);
                                            }
                                            catch (FaultException)
                                            {
                                                //This is thrown if the parameter name already exists, just ignore
                                            }
                                        }
                                    }
                                }

                                //Add to the mapping dictionary
                                this.testCaseMapping.Add(testId, newTestCaseId.Value);

                                //Add attachments if requested
                                if (this.MainFormHandle.chkImportAttachments.Checked)
                                {
                                    try
                                    {
                                        ImportAttachments(streamWriter, "TEST", testId, testObject.Attachments, newTestCaseId.Value, Utils.ArtifactType.TestCase);
                                    }
                                    catch (Exception exception)
                                    {
                                        streamWriter.WriteLine("Warning: Unable to import attachments for test " + testId + " (" + exception.Message + ")");
                                    }
                                }
                                //Finally we need to load the requirements coverage for this test case
                                if (this.MainFormHandle.chkImportRequirements.Checked)
                                {
                                    try
                                    {
                                        HP.ALM.IList coverageList = testObject.GetCoverList();
                                        if (coverageList != null && coverageList.Count > 0)
                                        {
                                            streamWriter.WriteLine("Importing Requirements Coverage");
                                            foreach (HP.ALM.Req reqCover in coverageList)
                                            {
                                                //Extract the coverage info
                                                int reqId = (int)reqCover.ID;

                                                //Locate the requirement and get the corresponding SpiraTest requirement id
                                                int requirementId = -1;
                                                if (this.requirementsMapping.ContainsKey(reqId))
                                                {
                                                    requirementId = (int)this.requirementsMapping[reqId];
                                                }


                                                //Load the coverage entry assuming we have a valid requirements id and test case id
                                                if (requirementId != -1)
                                                {
                                                    RemoteRequirementTestCaseMapping remoteRequirementTestCaseMapping = new RemoteRequirementTestCaseMapping();
                                                    remoteRequirementTestCaseMapping.TestCaseId = newTestCaseId.Value;
                                                    remoteRequirementTestCaseMapping.RequirementId = requirementId;
                                                    ImportFormHandle.SpiraImportProxy.Requirement_AddTestCoverage(remoteRequirementTestCaseMapping);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        //If we can't get the test steps, proceed and log error
                                        streamWriter.WriteLine("*WARNING*: Unable to get Requirements Coverage for test case " + testId + " - continuing. Error message = '" + exception.Message + "'");
                                    }
                                }
                            }
                        }
                    }

                    //Next we make a second pass through to import the test steps (and associated data)
                    foreach (HP.ALM.Test testObject in testList)
                    {
                        //Extract the test info
                        int testId = (int)testObject.ID;
                        string name = "Un-named";
                        bool objectExists = true;
                        try
                        {
                            if (testObject.Name != null)
                            {
                                name = MakeXmlSafe(testObject.Name);
                            }
                        }
                        catch (Exception)
                        {
                            //Sometimes the underlying object doesn't exist, even though an ID is available
                            streamWriter.WriteLine("Unable to import steps for test case " + testId + " as its object is not accessible!");
                            objectExists = false;
                        }

                        if (objectExists)
                        {
                            //Make sure we have already imported the corresponding Spira test case
                            if (testCaseMapping.ContainsKey(testId))
                            {
                                int newTestCaseId = testCaseMapping[testId];
                                //Now get the test steps that belong to this test case
                                streamWriter.WriteLine("Getting Test Steps for test case " + testId);
                                HP.ALM.DesignStepFactory designStepFactory = (HP.ALM.DesignStepFactory)testObject.DesignStepFactory;
                                HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)designStepFactory.Filter;
                                tdFilter.set_Order("ds_step_order", 1);
                                try
                                {
                                    HP.ALM.List stepList = (HP.ALM.List)tdFilter.NewList();
                                    foreach (HP.ALM.DesignStep designStep in stepList)
                                    {
                                        //Extract the test step info
                                        int stepId = (int)designStep.ID;
                                        int linkedTestId = -1;
                                        streamWriter.WriteLine("Importing Test Step: " + stepId.ToString());

                                        //See if we have a 'real' test step or a linked test case
                                        if (designStep.LinkTestID != 0)
                                        {
                                            linkedTestId = designStep.LinkTestID;
                                        }
                                        //Get from the db field as a backup
                                        if (linkedTestId == -1)
                                        {
                                            if (designStep["ds_link_test"] != null)
                                            {
                                                if (designStep["ds_link_test"] is string)
                                                {
                                                    linkedTestId = Int32.Parse(designStep["ds_link_test"].ToString());
                                                }
                                                else if (designStep["ds_link_test"] is int)
                                                {
                                                    if ((int)designStep["ds_link_test"] != 0)
                                                    {
                                                        linkedTestId = (int)designStep["ds_link_test"];
                                                    }
                                                }
                                            }
                                        }

                                        if (linkedTestId != -1)
                                        {
                                            streamWriter.WriteLine("Design step is linked to test: " + linkedTestId.ToString());
                                        }

                                        name = MakeXmlSafe(designStep.StepName);
                                        string description = MakeXmlSafe(designStep.StepDescription);
                                        string expectedResult = MakeXmlSafe(designStep.StepExpectedResult);
                                        int position = designStep.Order;
                                        string sampleData = "";	//Not supported by QualityCenter

                                        //See if we have any parameters in the text that need to be converted into
                                        //SpiraTest format. 
                                        description = ReplaceParameterToken(description);
                                        expectedResult = ReplaceParameterToken(expectedResult);

                                        //Try and locate the linked test case
                                        int linkedTestCaseId = -1;
                                        if (this.testCaseMapping.ContainsKey(linkedTestId))
                                        {
                                            linkedTestCaseId = (int)this.testCaseMapping[linkedTestId];
                                            streamWriter.WriteLine("Test Step is linked to Test Case: TC" + linkedTestCaseId.ToString());
                                        }

                                        //Locate the test step and get the corresponding SpiraTest test case id
                                        if (this.testCaseMapping.ContainsKey(testId))
                                        {
                                            int testCaseId = (int)this.testCaseMapping[testId];

                                            //Load the test step or test link, and capture the new id
                                            if (linkedTestCaseId == -1)
                                            {
                                                try
                                                {
                                                    RemoteTestStep remoteTestStep = new RemoteTestStep();
                                                    remoteTestStep.TestCaseId = testCaseId;
                                                    remoteTestStep.Position = position;
                                                    remoteTestStep.Description = MakeXmlSafe(description);
                                                    remoteTestStep.ExpectedResult = expectedResult;
                                                    remoteTestStep.SampleData = sampleData;
                                                    Nullable<int> newTestStepId = ImportFormHandle.SpiraImportProxy.TestCase_AddStep(remoteTestStep, testCaseId).TestStepId;

                                                    //Add to the mapping hashtable
                                                    if (newTestStepId.HasValue)
                                                    {
                                                        this.testStepMapping.Add(stepId, newTestStepId.Value);

                                                        //Add attachments if requested
                                                        if (this.MainFormHandle.chkImportAttachments.Checked)
                                                        {
                                                            try
                                                            {
                                                                ImportAttachments(streamWriter, "DESSTEP", stepId, designStep.Attachments, newTestStepId.Value, Utils.ArtifactType.TestStep);
                                                            }
                                                            catch (Exception exception)
                                                            {
                                                                streamWriter.WriteLine("Warning: Unable to import attachments for design step " + stepId + " (" + exception.Message + ")");
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception exception)
                                                {
                                                    //If we have an error, log it and continue
                                                    streamWriter.WriteLine("Error adding test step " + stepId + " to SpiraTest (" + exception.Message + ")");
                                                }
                                            }
                                            else
                                            {
                                                //Now we need to see if this link test step has any parameters
                                                List<RemoteTestStepParameter> remoteTestStepParameters = new List<RemoteTestStepParameter>();
                                                if (designStep.LinkedParams != null)
                                                {
                                                    HP.ALM.StepParams linkedStepParams = (HP.ALM.StepParams)designStep.LinkedParams;
                                                    if (linkedStepParams.Count > 0)
                                                    {
                                                        for (int i = 0; i < linkedStepParams.Count; i++)
                                                        {
                                                            string paramName = "";
                                                            if (!String.IsNullOrEmpty(linkedStepParams.ParamName[i]))
                                                            {
                                                                paramName = MakeXmlSafe(linkedStepParams.ParamName[i]);
                                                            }
                                                            string paramValue = "";
                                                            if (!String.IsNullOrEmpty(linkedStepParams.ParamValue[i]))
                                                            {
                                                                paramValue = MakeXmlSafe(linkedStepParams.ParamValue[i]);
                                                            }
                                                            if (paramName != "" && paramValue != "")
                                                            {
                                                                //Need to replace any spaces in the parameter name with underscores
                                                                RemoteTestStepParameter remoteTestStepParameter = new SpiraImportExport.RemoteTestStepParameter();
                                                                remoteTestStepParameter.Name = paramName.ToLowerInvariant().Replace(" ", "_");
                                                                remoteTestStepParameter.Value = paramValue;
                                                                remoteTestStepParameters.Add(remoteTestStepParameter);
                                                            }
                                                        }
                                                    }
                                                }
                                                Nullable<int> newTestStepId = ImportFormHandle.SpiraImportProxy.TestCase_AddLink(testCaseId, position, linkedTestCaseId, remoteTestStepParameters.ToArray());

                                                //Add to the mapping hashtable
                                                if (newTestStepId.HasValue)
                                                {
                                                    this.testStepMapping.Add(stepId, newTestStepId.Value);

                                                    //Add attachments if requested
                                                    if (this.MainFormHandle.chkImportAttachments.Checked)
                                                    {
                                                        try
                                                        {
                                                            ImportAttachments(streamWriter, "DESSTEP", stepId, designStep.Attachments, newTestStepId.Value, Utils.ArtifactType.TestStep);
                                                        }
                                                        catch (Exception exception)
                                                        {
                                                            streamWriter.WriteLine("Warning: Unable to import attachments for design step " + stepId + " (" + exception.Message + ")");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    //If we can't get the test steps, proceed and log error
                                    streamWriter.WriteLine("*WARNING*: Unable to get Test Steps for test case " + testId + " - continuing. Error message = '" + exception.Message + "'");
                                }

                                //Finally we need to load the requirements coverage for this test case
                                if (this.MainFormHandle.chkImportRequirements.Checked)
                                {
                                    try
                                    {
                                        HP.ALM.IList coverageList = testObject.GetCoverList();
                                        if (coverageList != null && coverageList.Count > 0)
                                        {
                                            streamWriter.WriteLine("Importing Requirements Coverage");
                                            foreach (HP.ALM.Req reqCover in coverageList)
                                            {
                                                //Extract the coverage info
                                                int reqId = (int)reqCover.ID;

                                                //Locate the requirement and get the corresponding SpiraTest requirement id
                                                int requirementId = -1;
                                                if (this.requirementsMapping.ContainsKey(reqId))
                                                {
                                                    requirementId = (int)this.requirementsMapping[reqId];
                                                }


                                                //Load the coverage entry assuming we have a valid requirements id and test case id
                                                if (requirementId != -1)
                                                {
                                                    RemoteRequirementTestCaseMapping remoteRequirementTestCaseMapping = new RemoteRequirementTestCaseMapping();
                                                    remoteRequirementTestCaseMapping.TestCaseId = newTestCaseId;
                                                    remoteRequirementTestCaseMapping.RequirementId = requirementId;
                                                    ImportFormHandle.SpiraImportProxy.Requirement_AddTestCoverage(remoteRequirementTestCaseMapping);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        //If we can't get the test steps, proceed and log error
                                        streamWriter.WriteLine("*WARNING*: Unable to get Requirements Coverage for test case " + testId + " - continuing. Error message = '" + exception.Message + "'");
                                    }
                                }
                            }
                        }
                        else
                        {
                            streamWriter.WriteLine("*WARNING*: Ignoring test case " + testId + " as it does not exist in QC - continuing");
                        }
                    }

                    //**** Show that we've imported test cases, test steps and test coverage ****
                    streamWriter.WriteLine("Test Cases, Test Steps and Test Coverage Imported");
                    this.ProgressForm_OnProgressUpdate(4);
                }

                //6) Get the automation hosts, test sets and their mapped test cases
                MainFormHandle.EnsureConnected();
                if (this.MainFormHandle.chkImportTestSets.Checked && this.MainFormHandle.chkImportTestCases.Checked)
                {
                    //First we need to get the automation hosts
                    HP.ALM.HostFactory hostFactory = (HP.ALM.HostFactory)MainFormHandle.TdConnection.HostFactory;
                    ImportHosts(streamWriter, hostFactory);

                    //Next we need to build out the test set folders
                    HP.ALM.TestSetTreeManager testSetTreeManager = (HP.ALM.TestSetTreeManager)MainFormHandle.TdConnection.TestSetTreeManager;
                    HP.ALM.SysTreeNode testSetTreeNode = (HP.ALM.SysTreeNode)testSetTreeManager.Root; //Get the Test Set Folder Root
                    if (testSetTreeNode.Count > 0)
                    {
                        HP.ALM.IList folderList = testSetTreeNode.NewList();
                        ImportTestSetFolderNode(testSetTreeManager, folderList, null);
                    }

                    streamWriter.WriteLine("Importing Test Sets");
                    HP.ALM.TestSetFactory testSetFactory = (HP.ALM.TestSetFactory)MainFormHandle.TdConnection.TestSetFactory;
                    HP.ALM.TDFilter hierFilter = (HP.ALM.TDFilter)testSetFactory.Filter;
                    hierFilter.set_Order("cy_cycle_id", 1);
                    HP.ALM.List testSetList;
                    try
                    {
                        testSetList = (HP.ALM.List)hierFilter.NewList();
                    }
                    catch (Exception)
                    {
                        //If we get an exception, try to reconnect and re-execute
                        MainFormHandle.TryReconnect();
                        testSetFactory = (HP.ALM.TestSetFactory)MainFormHandle.TdConnection.TestSetFactory;
                        hierFilter = (HP.ALM.TDFilter)testSetFactory.Filter;
                        testSetList = (HP.ALM.List)hierFilter.NewList();
                    }

                    foreach (HP.ALM.TestSet testSetObject in testSetList)
                    {
                        //Extract the test set info
                        int cycleId = (int)testSetObject.ID;
                        string name = "Un-named";
                        if (testSetObject.Name != null)
                        {
                            name = MakeXmlSafe(testSetObject.Name);
                        }
                        string description = "";
                        if (testSetObject["cy_description"] != null)
                        {
                            description = MakeXmlSafe(testSetObject["cy_description"].ToString());
                        }
                        else if (testSetObject["cy_comment"] != null)
                        {
                            description = MakeXmlSafe(testSetObject["cy_comment"].ToString());
                        }

                        int folderNodeId = -1;
                        try
                        {
                            if (testSetObject.TestSetFolder != null)
                            {
                                HP.ALM.TestSetFolder testSetFolder = (HP.ALM.TestSetFolder)testSetObject.TestSetFolder;
                                folderNodeId = testSetFolder.NodeID;
                            }
                        }
                        catch (Exception)
                        {
                            //Fail quietly and just leave the folder unset
                        }

                        //Locate the test set folder path and get the corresponding SpiraTest test set folder id
                        Nullable<int> folderId = null;
                        if (this.testSetFolderMapping.ContainsKey(folderNodeId))
                        {
                            folderId = (int)this.testSetFolderMapping[folderNodeId];
                        }

                        //Check iterations
                        int? iterationId = null;
                        if (testSetObject["cy_assign_rcyc"] != null)
                        {
                            if (testSetObject["cy_assign_rcyc"] is Int32)
                            {
                                int releaseCycleId = testSetObject["cy_assign_rcyc"];
                                if (this.iterationMapping.ContainsKey(releaseCycleId))
                                {
                                    iterationId = this.iterationMapping[releaseCycleId];
                                }
                            }
                            else if (testSetObject["cy_assign_rcyc"] is String)
                            {
                                string releaseCycleString = testSetObject["cy_assign_rcyc"];
                                int releaseCycleId = Int32.Parse(releaseCycleString);
                                if (this.iterationMapping.ContainsKey(releaseCycleId))
                                {
                                    iterationId = this.iterationMapping[releaseCycleId];
                                }
                            }
                            else if (testSetObject["cy_assign_rcyc"] is ICycle)
                            {
                                ICycle cycle = (ICycle)testSetObject["cy_assign_rcyc"];
                                int releaseCycleId = cycle.ID;
                                if (this.iterationMapping.ContainsKey(releaseCycleId))
                                {
                                    iterationId = this.iterationMapping[releaseCycleId];
                                }
                            }
                            else
                            {
                                streamWriter.WriteLine("Unable to access cy_assign_rcyc, so leaving unset, type=" + testSetObject["cy_assign_rcyc"].GetType());
                            }
                        }

                        //QC stores the tester and planned-date info at the test case level only (not at the test set level)
                        //So we'll just leave them as unset.
                        string testSetStatus = testSetObject.Status;
                        int testSetStatusId = ConvertTestSetStatus(testSetStatus);

                        //Load the test set and capture the new id
                        Nullable<int> newTestSetId = null;
                        try
                        {
                            //Populate the test set
                            RemoteTestSet remoteTestSet = new RemoteTestSet();
                            remoteTestSet.Name = name;
                            remoteTestSet.Description = MakeXmlSafe(description);
                            remoteTestSet.TestSetStatusId = testSetStatusId;
                            remoteTestSet.ReleaseId = iterationId;

                            List<RemoteArtifactCustomProperty> customProperties = new List<RemoteArtifactCustomProperty>();
                            ImportCustomField(streamWriter, testSetObject, "cy_user_01", customProperties);
                            ImportCustomField(streamWriter, testSetObject, "cy_user_02", customProperties);
                            ImportCustomField(streamWriter, testSetObject, "cy_user_03", customProperties);
                            ImportCustomField(streamWriter, testSetObject, "cy_user_04", customProperties);
                            ImportCustomField(streamWriter, testSetObject, "cy_user_05", customProperties);
                            ImportCustomField(streamWriter, testSetObject, "cy_user_06", customProperties);
                            remoteTestSet.CustomProperties = customProperties.ToArray();

                            newTestSetId = ImportFormHandle.SpiraImportProxy.TestSet_Create(remoteTestSet, folderId).TestSetId.Value;
                        }
                        catch (Exception exception)
                        {
                            //If we have an error, log it and continue
                            streamWriter.WriteLine("Error adding test set " + cycleId + " to SpiraTest (" + exception.Message + ")");
                        }

                        if (newTestSetId.HasValue)
                        {
                            //Add to the mapping dictionary
                            this.testSetMapping.Add(cycleId, newTestSetId.Value);

                            //Add attachments if requested
                            if (this.MainFormHandle.chkImportAttachments.Checked)
                            {
                                try
                                {
                                    ImportAttachments(streamWriter, "CYCLE", cycleId, testSetObject.Attachments, newTestSetId.Value, Utils.ArtifactType.TestSet);
                                }
                                catch (Exception exception)
                                {
                                    streamWriter.WriteLine("Warning: Unable to import attachments for cycle " + cycleId + " (" + exception.Message + ")");
                                }
                            }

                            //Now we need to load the test case to test set mapping information
                            //for this test set
                            streamWriter.WriteLine("Importing Test Case mappings for test set " + cycleId);
                            HP.ALM.TSTestFactory testSetTestFactory = (HP.ALM.TSTestFactory)testSetObject.TSTestFactory;
                            HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)testSetTestFactory.Filter;
                            tdFilter.set_Order("tc_test_order", 1);

                            HP.ALM.List testSetTestList = (HP.ALM.List)tdFilter.NewList();
                            int index = 0;
                            Dictionary<int, int> testInstanceIndexMapping = new Dictionary<int, int>();
                            foreach (HP.ALM.TSTest testSetTest in testSetTestList)
                            {
                                //Extract the test set mapping info
                                int testId = -1;
                                if (testSetTest.TestId is Int32)
                                {
                                    testId = (int)testSetTest.TestId;
                                }
                                if (testSetTest.TestId is String)
                                {
                                    testId = Int32.Parse((string)testSetTest.TestId);
                                }
                                if (testId != -1)
                                {
                                    //See if we have any parameters for the this test set - test case instance
                                    List<RemoteTestSetTestCaseParameter> remoteTestSetTestCaseParameters = new List<RemoteTestSetTestCaseParameter>();
                                    if (testSetTest.Params != null)
                                    {
                                        HP.ALM.StepParams linkedStepParams = (HP.ALM.StepParams)testSetTest.Params;
                                        if (linkedStepParams.Count > 0)
                                        {
                                            streamWriter.WriteLine("Retrieving test set parameters for test case " + testId + " in test set " + cycleId);
                                            for (int i = 0; i < linkedStepParams.Count; i++)
                                            {
                                                string paramName = "";
                                                if (!String.IsNullOrEmpty(linkedStepParams.ParamName[i]))
                                                {
                                                    paramName = MakeXmlSafe(linkedStepParams.ParamName[i]);
                                                }
                                                string paramValue = "";
                                                if (!String.IsNullOrEmpty(linkedStepParams.ParamValue[i]))
                                                {
                                                    paramValue = MakeXmlSafe(linkedStepParams.ParamValue[i]);
                                                }
                                                if (paramName != "" && paramValue != "")
                                                {
                                                    RemoteTestSetTestCaseParameter remoteTestSetTestCaseParameter = new SpiraImportExport.RemoteTestSetTestCaseParameter();
                                                    remoteTestSetTestCaseParameter.Name = paramName.ToLowerInvariant();
                                                    remoteTestSetTestCaseParameter.Value = paramValue;
                                                    remoteTestSetTestCaseParameters.Add(remoteTestSetTestCaseParameter);
                                                }
                                            }
                                        }
                                    }
                                    streamWriter.WriteLine("Importing Test Case " + testId + " in test set " + cycleId);

                                    //Lookup the mappings from the dictionaries
                                    if (this.testCaseMapping.ContainsKey(testId) && this.testSetMapping.ContainsKey(cycleId))
                                    {
                                        int testSetId = this.testSetMapping[cycleId];
                                        int testCaseId = this.testCaseMapping[testId];

                                        //Load the test case to test set mapping
                                        RemoteTestSetTestCaseMapping remoteTestSetTestCaseMapping = new SpiraImportExport.RemoteTestSetTestCaseMapping();
                                        remoteTestSetTestCaseMapping.TestSetId = testSetId;
                                        remoteTestSetTestCaseMapping.TestCaseId = testCaseId;
                                        ImportFormHandle.SpiraImportProxy.TestSet_AddTestMapping(remoteTestSetTestCaseMapping, null, remoteTestSetTestCaseParameters.ToArray());

                                        int testInstanceID = 0;
                                        if (testSetTest["tc_testcycl_id"] is Int32)
                                        {
                                            testInstanceID = testSetTest["tc_testcycl_id"];
                                        }
                                        if (testSetTest["tc_testcycl_id"] is String)
                                        {
                                            string instanceValue = testSetTest["tc_testcycl_id"];
                                            testInstanceID = Int32.Parse(instanceValue);
                                        }
                                        streamWriter.WriteLine("Imported Test Case " + testId + " in test set " + cycleId + " with InstanceID=" + testInstanceID);

                                        //Add a mapping for the test set test case instance. Since it's not returned by the API :-(
                                        //we need to retrieve them at the end and rely on the ordering
                                        testInstanceIndexMapping.Add(index, testInstanceID);
                                        index++;
                                    }
                                }
                            }

                            //Now we need to remap the test set test case id against the instance id by means of the index
                            RemoteTestSetTestCaseMapping[] remoteTestSetTestCaseMappings = ImportFormHandle.SpiraImportProxy.TestSet_RetrieveTestCaseMapping(newTestSetId.Value);
                            for (int i = 0; i < remoteTestSetTestCaseMappings.Length; i++)
                            {
                                //Get the instance id from the index dictionary
                                if (testInstanceIndexMapping.ContainsKey(i))
                                {
                                    int instanceId = testInstanceIndexMapping[i];
                                    if (!this.testSetTestCaseMapping.ContainsKey(instanceId))
                                    {
                                        this.testSetTestCaseMapping.Add(instanceId, remoteTestSetTestCaseMappings[i].TestSetTestCaseId);
                                    }
                                }
                            }
                        }
                    }

                    //**** Show that we've imported test sets ****
                    streamWriter.WriteLine("Test Sets Imported");
                    this.ProgressForm_OnProgressUpdate(5);
                }

                //7) Get the test runs (including run steps) and import
                MainFormHandle.EnsureConnected();
                if (this.MainFormHandle.chkImportTestCases.Checked && this.MainFormHandle.chkImportTestRuns.Checked)
                {
                    streamWriter.WriteLine("Importing Test Runs");
                    HP.ALM.RunFactory runFactory = (HP.ALM.RunFactory)MainFormHandle.TdConnection.RunFactory;
                    HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)runFactory.Filter;
                    tdFilter.set_Order("rn_run_id", 1);
                    HP.ALM.List runList;
                    try
                    {
                        runList = (HP.ALM.List)tdFilter.NewList();
                    }
                    catch (Exception)
                    {
                        //If there is an error try reconnecting
                        MainFormHandle.TryReconnect();
                        runFactory = (HP.ALM.RunFactory)MainFormHandle.TdConnection.RunFactory;
                        tdFilter = (HP.ALM.TDFilter)runFactory.Filter;
                        runList = (HP.ALM.List)tdFilter.NewList();
                    }
                    if (runList == null)
                    {
                        streamWriter.WriteLine("A null run list was returned, so skipping test run import.");
                    }
                    else
                    {
                        foreach (HP.ALM.Run runObject in runList)
                        {
                            //Extract the test run info
                            int runId = (int)runObject.ID;
                            int testId = runObject.TestId;
                            string runStatus = runObject.Status;
                            string name = "No Name";
                            if (!String.IsNullOrEmpty(runObject.Name))
                            {
                                name = MakeXmlSafe(runObject.Name);
                            }
                            string tester = (string)runObject["rn_tester_name"];
                            DateTime execDate = DateTime.Now;
                            if (runObject["rn_execution_date"] != null)
                            {
                                if (runObject["rn_execution_date"] is DateTime)
                                {
                                    execDate = (DateTime)runObject["rn_execution_date"];
                                }
                                else if (runObject["rn_execution_date"] is string)
                                {
                                    if (!DateTime.TryParse((string)runObject["rn_execution_date"], out execDate))
                                    {
                                        execDate = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    execDate = DateTime.Now;
                                }
                            }
                            int duration = 0;
                            if (runObject["rn_duration"] != null)
                            {
                                duration = (int)runObject["rn_duration"];
                            }
                            int statusId = ConvertExecutionStatus(runStatus);

                            //Lookup the tester from the user mapping hashtable
                            Nullable<int> testerId = null;
                            if (!String.IsNullOrEmpty(tester) && this.usersMapping.ContainsKey(tester))
                            {
                                testerId = (int)this.usersMapping[tester];
                            }

                            //Check iterations
                            int? iterationId = null;
                            if (runObject["rn_assign_rcyc"] != null)
                            {
                                if (runObject["rn_assign_rcyc"] is Int32)
                                {
                                    int releaseCycleId = runObject["rn_assign_rcyc"];
                                    if (this.iterationMapping.ContainsKey(releaseCycleId))
                                    {
                                        iterationId = this.iterationMapping[releaseCycleId];
                                    }
                                }
                                else if (runObject["rn_assign_rcyc"] is String)
                                {
                                    string releaseCycleString = runObject["rn_assign_rcyc"];
                                    int releaseCycleId = Int32.Parse(releaseCycleString);
                                    if (this.iterationMapping.ContainsKey(releaseCycleId))
                                    {
                                        iterationId = this.iterationMapping[releaseCycleId];
                                    }
                                }
                                else if (runObject["rn_assign_rcyc"] is ICycle)
                                {
                                    ICycle cycle = (ICycle)runObject["rn_assign_rcyc"];
                                    int releaseCycleId = cycle.ID;
                                    if (this.iterationMapping.ContainsKey(releaseCycleId))
                                    {
                                        iterationId = this.iterationMapping[releaseCycleId];
                                    }
                                }
                                else
                                {
                                    streamWriter.WriteLine("Unable to access rn_assign_rcyc, so leaving unset, type=" + runObject["rn_assign_rcyc"].GetType());
                                }
                            }

                            //Locate the test cycle/set and get the corresponding test set id
                            Nullable<int> testSetId = null;
                            try
                            {
                                int cycleId = runObject.TestSetID;
                                if (this.testSetMapping.ContainsKey(cycleId))
                                {
                                    testSetId = this.testSetMapping[cycleId];
                                }
                            }
                            catch (Exception)
                            {
                                //Fail quietly
                            }

                            Nullable<int> testSetTestCaseId = null;
                            try
                            {
                                int testInstanceID = runObject.TestInstanceID;

                                //Make sure we have a real value, if not, try get from the db column
                                if (testInstanceID < 1)
                                {
                                    testInstanceID = (int)runObject["rn_testcycl_id"];
                                }

                                if (this.testSetTestCaseMapping.ContainsKey(testInstanceID))
                                {
                                    testSetTestCaseId = this.testSetTestCaseMapping[testInstanceID];
                                }

                                //If the test set id is captured but not the test instance id we need to log it
                                if (testSetId.HasValue && !testSetTestCaseId.HasValue)
                                {
                                    streamWriter.WriteLine("Unable to locate TestSetTestCaseId for TestSet TX" + testSetId.Value + " and Test Instance " + testInstanceID + " in Run " + runId);
                                }
                            }
                            catch (Exception)
                            {
                                //Try using the DB field directly
                                int testInstanceID = (int)runObject["rn_testcycl_id"];

                                if (this.testSetTestCaseMapping.ContainsKey(testInstanceID))
                                {
                                    testSetTestCaseId = this.testSetTestCaseMapping[testInstanceID];
                                }
                            }

                            //Locate the test and get the corresponding SpiraTest test case id
                            if (this.testCaseMapping.ContainsKey(testId))
                            {
                                int testCaseId = (int)this.testCaseMapping[testId];

                                //Now create a new test run shell from this test case
                                RemoteManualTestRun[] remoteTestRuns = ImportFormHandle.SpiraImportProxy.TestRun_CreateFromTestCases(new int[] { testCaseId }, null);
                                if (remoteTestRuns.Length > 0)
                                {
                                    //Update the test run information
                                    RemoteManualTestRun remoteTestRun = remoteTestRuns[0];
                                    remoteTestRun.ExecutionStatusId = statusId;
                                    remoteTestRun.StartDate = execDate;
                                    DateTime endDate = execDate.AddMinutes(duration);
                                    remoteTestRun.EndDate = endDate;
                                    remoteTestRun.Name = name;
                                    remoteTestRun.TestSetId = testSetId;
                                    remoteTestRun.TestSetTestCaseId = testSetTestCaseId;
                                    remoteTestRun.TesterId = testerId;
                                    remoteTestRun.ReleaseId = iterationId;

                                    //Load any custom properties
                                    try
                                    {
                                        List<RemoteArtifactCustomProperty> customProperties = new List<RemoteArtifactCustomProperty>();
                                        ImportCustomField(streamWriter, runObject, "rn_user_01", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_02", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_03", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_04", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_05", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_06", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_07", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_08", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_09", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_10", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_11", customProperties);
                                        ImportCustomField(streamWriter, runObject, "rn_user_12", customProperties);
                                        remoteTestRun.CustomProperties = customProperties.ToArray();
                                    }
                                    catch (Exception exception)
                                    {
                                        //If we have an error, log it and continue
                                        streamWriter.WriteLine("Error adding test run custom properties for Run " + runId + " to SpiraTest (" + exception.Message + ")");
                                    }

                                    //Now we need to get the test run steps that belong to this test run
                                    streamWriter.WriteLine("Importing Run Steps for Test Run: " + runId.ToString());
                                    HP.ALM.StepFactory stepFactory = (HP.ALM.StepFactory)runObject.StepFactory;
                                    HP.ALM.TDFilter tdFilter2 = (HP.ALM.TDFilter)stepFactory.Filter;
                                    tdFilter2.set_Order("st_step_order", 1);
                                    tdFilter2.set_Order("st_id", 2);
                                    try
                                    {
                                        HP.ALM.List runStepList = (HP.ALM.List)tdFilter2.NewList();
                                        foreach (HP.ALM.Step runStepObject in runStepList)
                                        {
                                            //Extract the test run step information
                                            int runStepId = (int)runStepObject.ID;
                                            streamWriter.WriteLine("Importing Test Run Step: " + runStepId.ToString());
                                            try
                                            {
                                                //Some steps may refer to deleted design steps - so need try...catch
                                                int designStepId = -1;
                                                try
                                                {
                                                    designStepId = runStepObject.DesignStepSource;
                                                }
                                                catch (Exception)
                                                {
                                                    //Try using the field name
                                                    try
                                                    {
                                                        if (runStepObject["st_desstep_id"] != null)
                                                        {
                                                            designStepId = (int)runStepObject["st_desstep_id"];
                                                        }
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        streamWriter.WriteLine("*WARNING*: Unable to access DesignStepSource for step " + runStepId + " due to QC API exception: " + exception.Message);
                                                    }
                                                }

                                                //Make sure we have a valid id
                                                if (designStepId < 1)
                                                {
                                                    //Try using the field name
                                                    try
                                                    {
                                                        if (runStepObject["st_desstep_id"] != null)
                                                        {
                                                            designStepId = (int)runStepObject["st_desstep_id"];
                                                        }
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        streamWriter.WriteLine("*WARNING*: Unable to access DesignStepSource for step " + runStepId + " due to QC API exception: " + exception.Message);
                                                    }
                                                }

                                                string runStepStatus = runStepObject.Status;
                                                string actualResult = "";
                                                if (runStepObject["st_actual"] != null)
                                                {
                                                    actualResult = MakeXmlSafe((string)runStepObject["st_actual"]);
                                                }
                                                int runStepStatusId = ConvertExecutionStatus(runStepStatus);

                                                //Also need to see if there are any bugs linked to this test run step in QC
                                                //as we'll need the information later when importing defects
                                                ILinkable linkable = (ILinkable)runStepObject;
                                                LinkFactory linkBugFactory = linkable.BugLinkFactory;
                                                List linkedBugList = linkBugFactory.NewList("");
                                                streamWriter.WriteLine("Found " + linkedBugList.Count + " Linked Defects against " + runStepId.ToString());
                                                foreach (Link linkedItem in linkedBugList)
                                                {
                                                    //Make sure that the child item is the right type
                                                    Bug linkedBug = (Bug)linkedItem.TargetEntity;
                                                    int bugId = linkedBug.ID;
                                                    //Spira only allows an incident to link to a single test run step
                                                    if (!qcBugToRunStepMapping.ContainsKey(bugId))
                                                    {
                                                        qcBugToRunStepMapping.Add(bugId, runStepId);
                                                    }
                                                }
                                                streamWriter.WriteLine("Mapped " + qcBugToRunStepMapping.Count + " Linked Defects against " + runStepId.ToString());

                                                //Locate the SpiraTest test step this relates to
                                                if (this.testStepMapping.ContainsKey(designStepId))
                                                {
                                                    int testStepId = (int)this.testStepMapping[designStepId];
                                                    streamWriter.WriteLine("Importing Test Run Step: " + runStepId.ToString() + " against SpiraTest test step " + testStepId.ToString());

                                                    //Update the test run step objects
                                                    for (int i = 0; i < remoteTestRun.TestRunSteps.Length; i++)
                                                    {
                                                        //Locate the matching test run step by test step id (not primary key)
                                                        //Since can have the same link twice, need to also make sure not already set
                                                        if (remoteTestRun.TestRunSteps[i].TestStepId == testStepId && !this.testRunStepToTestStepMapping.ContainsKey(i))
                                                        {
                                                            remoteTestRun.TestRunSteps[i].ActualResult = actualResult;
                                                            remoteTestRun.TestRunSteps[i].ExecutionStatusId = runStepStatusId;

                                                            //Add the mapping between the indexer in the dataset and the test run step (needed later)
                                                            this.testRunStepToTestStepMapping.Add(i, runStepId);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    streamWriter.WriteLine("*WARNING*: Unable to get find test step that maps to Test Run Step " + runStepId + " with Design Step ID=" + designStepId);
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                streamWriter.WriteLine("*WARNING*: Unable to get information for test run step " + runStepId + " - continuing. Error message = '" + exception.Message + "' (" + exception.StackTrace + ")");
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        //If we can't get the test run steps, proceed and log error
                                        streamWriter.WriteLine("*WARNING*: Unable to get Test Run Steps for test run " + runId + " - continuing. Error message = '" + exception.Message + "' (" + exception.StackTrace + ")");
                                    }

                                    //Finally save this test run and get the updated dataset back
                                    try
                                    {
                                        streamWriter.WriteLine("Saving test run " + runId + " in SpiraTest.");
                                        remoteTestRuns = ImportFormHandle.SpiraImportProxy.TestRun_Save(remoteTestRuns, endDate);
                                        if (remoteTestRuns.Length > 0 && remoteTestRun.TestRunSteps != null)
                                        {
                                            remoteTestRun = remoteTestRuns[0];

                                            //Now we need to iterate through the run to extract the generated test run steps
                                            //and populate the new mapping
                                            for (int i = 0; i < remoteTestRun.TestRunSteps.Length; i++)
                                            {
                                                int testRunStepId = remoteTestRun.TestRunSteps[i].TestRunStepId.Value;
                                                if (this.testRunStepToTestStepMapping.ContainsKey(i))
                                                {
                                                    //Get the QC run step from the indexer, then map to the SpiraTest run step id
                                                    int runStepId = (int)this.testRunStepToTestStepMapping[i];
                                                    if (!this.testRunStepMapping.ContainsKey(runStepId))
                                                    {
                                                        this.testRunStepMapping.Add(runStepId, testRunStepId);
                                                    }
                                                }
                                            }

                                            //Add attachments if requested
                                            if (this.MainFormHandle.chkImportAttachments.Checked)
                                            {
                                                try
                                                {
                                                    ImportAttachments(streamWriter, "RUN", runId, runObject.Attachments, remoteTestRun.TestRunId.Value, Utils.ArtifactType.TestRun);
                                                }
                                                catch (Exception exception)
                                                {
                                                    streamWriter.WriteLine("Warning: Unable to import attachments for run " + runId + " (" + exception.Message + ")");
                                                }
                                            }
                                        }
                                        //Now clear the temporary mapping
                                        this.testRunStepToTestStepMapping.Clear();
                                    }
                                    catch (Exception exception)
                                    {
                                        //If we can't save the test run, proceed and log error
                                        streamWriter.WriteLine("*WARNING*: Unable to save Test Run " + runId + " in SpiraTest - continuing. Error message = '" + exception.Message + "' (" + exception.StackTrace + ")");
                                    }
                                }
                            }
                        }
                    }

                    //**** Show that we've imported test runs ****
                    streamWriter.WriteLine("Test Runs Imported");
                    this.ProgressForm_OnProgressUpdate(6);
                }

                //8) Get the incidents and import
                MainFormHandle.EnsureConnected();
                if (this.MainFormHandle.chkImportDefects.Checked)
                {
                    streamWriter.WriteLine("Importing Defects");
                    HP.ALM.BugFactory bugFactory = (HP.ALM.BugFactory)MainFormHandle.TdConnection.BugFactory;
                    HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)bugFactory.Filter;
                    tdFilter.set_Order("bg_bug_id", 1);
                    HP.ALM.List bugList;
                    try
                    {
                        bugList = (HP.ALM.List)tdFilter.NewList();
                    }
                    catch (Exception)
                    {
                        //If there is an error try reconnecting
                        MainFormHandle.TryReconnect();
                        bugFactory = (HP.ALM.BugFactory)MainFormHandle.TdConnection.BugFactory;
                        tdFilter = (HP.ALM.TDFilter)bugFactory.Filter;
                        bugList = (HP.ALM.List)tdFilter.NewList();
                    }
                    if (bugList == null)
                    {
                        streamWriter.WriteLine("A null defect list was returned, so skipping defect import.");
                    }
                    else
                    {
                        foreach (HP.ALM.Bug bugObject in bugList)
                        {
                            bool objectExists = true;
                            int bugId = -1;
                            string name = "Unnamed Defect";
                            try
                            {
                                //Extract the incident/bug info
                                bugId = (int)bugObject.ID;
                                name = MakeXmlSafe(bugObject.Summary);
                            }
                            catch (Exception)
                            {
                                //Sometimes the underlying object doesn't exist, even though an ID is available
                                objectExists = false;
                                streamWriter.WriteLine("Skipping an empty defect record.");
                            }
                            if (objectExists)
                            {
                                streamWriter.WriteLine("Importing Defect: " + bugId.ToString());
                                string description = "Migrated from QualityCenter";
                                if (bugObject["bg_description"] != null)
                                {
                                    description = MakeXmlSafe(bugObject["bg_description"].ToString());
                                }
                                string resolution = "";
                                if (bugObject["bg_dev_comments"] != null)
                                {
                                    resolution = MakeXmlSafe(bugObject["bg_dev_comments"].ToString());
                                }
                                string bugSeverity = "";
                                if (bugObject["bg_severity"] != null)
                                {
                                    bugSeverity = bugObject["bg_severity"].ToString();
                                }
                                string bugPriority = bugObject.Priority;
                                string bugStatus = bugObject.Status;
                                string bugReproducibleYn = "N";
                                if (bugObject["bg_reproducible"] != null)
                                {
                                    bugReproducibleYn = bugObject["bg_reproducible"].ToString();
                                }

                                string openerName = bugObject.DetectedBy;
                                string ownerName = bugObject.AssignedTo;
                                Nullable<DateTime> closedDate = null;
                                if (bugObject["bg_closing_date"] != null)
                                {
                                    closedDate = DateTime.Parse(bugObject["bg_closing_date"].ToString());
                                }
                                Nullable<DateTime> creationDate = null;
                                if (bugObject["bg_detection_date"] != null)
                                {
                                    creationDate = DateTime.Parse(bugObject["bg_detection_date"].ToString());
                                }
                                //If we have a mapped test run step, lookup the SpiraTest id from the mapping hashtable
                                Nullable<int> testRunStepId = null;
                                if (this.MainFormHandle.chkImportTestRuns.Checked)
                                {
                                    testRunStepId = GetTestRunStepIdForBug(bugObject);
                                }

                                //Lookup the opener from the user mapping hashtable
                                Nullable<int> openerId = null;
                                if (this.usersMapping.ContainsKey(openerName))
                                {
                                    openerId = (int)this.usersMapping[openerName];
                                }
                                //Lookup the owner from the user mapping hashtable
                                Nullable<int> ownerId = null;
                                if (ownerName != null)
                                {
                                    if (this.usersMapping.ContainsKey(ownerName))
                                    {
                                        ownerId = (int)this.usersMapping[ownerName];
                                    }
                                }

                                //Convert the priority
                                Nullable<int> priorityId = ConvertPriority(bugPriority, ImportFormHandle.SpiraImportProxy);

                                //Convert the severity
                                Nullable<int> severityId = ConvertSeverity(bugSeverity, ImportFormHandle.SpiraImportProxy);

                                //Convert the status and type
                                Nullable<int> statusId = ConvertStatus(bugStatus, ImportFormHandle.SpiraImportProxy);

                                //Convert the various efforts from days to minutes
                                Nullable<int> estimatedEffort = null;
                                if (bugObject["bg_estimated_fix_time"] != null)
                                {
                                    //Assume 8 hour day
                                    estimatedEffort = ((int)bugObject["bg_estimated_fix_time"]) * 8 * 60;
                                }
                                Nullable<int> actualEffort = null;
                                if (bugObject["bg_actual_fix_time"] != null)
                                {
                                    //Assume 8 hour day
                                    actualEffort = ((int)bugObject["bg_actual_fix_time"]) * 8 * 60;
                                }

                                //See if we have a detected release specified
                                int? detectedReleaseId = null;
                                try
                                {
                                    if (bugObject["bg_detected_in_rel"] != null)
                                    {
                                        if (bugObject["bg_detected_in_rel"] is Int32)
                                        {
                                            int qcReleaseId = bugObject["bg_detected_in_rel"];
                                            if (this.releaseMapping.ContainsKey(qcReleaseId))
                                            {
                                                detectedReleaseId = this.releaseMapping[qcReleaseId];
                                            }
                                        }
                                        else if (bugObject["bg_detected_in_rel"] is String)
                                        {
                                            string qcReleaseString = bugObject["bg_detected_in_rel"];
                                            int qcReleaseId = Int32.Parse(qcReleaseString);
                                            if (this.releaseMapping.ContainsKey(qcReleaseId))
                                            {
                                                detectedReleaseId = this.releaseMapping[qcReleaseId];
                                            }
                                        }
                                        else if (bugObject["bg_detected_in_rel"] is IRelease)
                                        {
                                            IRelease release = bugObject["bg_detected_in_rel"];
                                            int qcReleaseId = release.ID;
                                            if (this.releaseMapping.ContainsKey(qcReleaseId))
                                            {
                                                detectedReleaseId = this.releaseMapping[qcReleaseId];
                                            }
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine("Unable to access Bug:DetectedRelease, so leaving unset, type=" + bugObject["bg_detected_in_rel"].GetType());
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    streamWriter.WriteLine("Unable to access Bug:DetectedRelease, so leaving unset - " + exception.Message);
                                }

                                //See if we have a resolved release specified
                                int? resolvedReleaseId = null;
                                try
                                {
                                    if (bugObject["bg_target_rel"] != null)
                                    {
                                        if (bugObject["bg_target_rel"] is Int32)
                                        {
                                            int qcReleaseId = bugObject["bg_target_rel"];
                                            if (this.releaseMapping.ContainsKey(qcReleaseId))
                                            {
                                                resolvedReleaseId = this.releaseMapping[qcReleaseId];
                                            }
                                        }
                                        else if (bugObject["bg_target_rel"] is String)
                                        {
                                            string qcReleaseString = bugObject["bg_target_rel"];
                                            int qcReleaseId = Int32.Parse(qcReleaseString);
                                            if (this.releaseMapping.ContainsKey(qcReleaseId))
                                            {
                                                resolvedReleaseId = this.releaseMapping[qcReleaseId];
                                            }
                                        }
                                        else if (bugObject["bg_target_rel"] is IRelease)
                                        {
                                            IRelease release = bugObject["bg_target_rel"];
                                            int qcReleaseId = release.ID;
                                            if (this.releaseMapping.ContainsKey(qcReleaseId))
                                            {
                                                resolvedReleaseId = this.releaseMapping[qcReleaseId];
                                            }
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine("Unable to access Bug:TargetRelease, so leaving unset, type=" + bugObject["bg_target_rel"].GetType());
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    streamWriter.WriteLine("Unable to access Bug:TargetRelease, so leaving unset - " + exception.Message);
                                }

                                try
                                {
                                    //Check detected iteration
                                    if (bugObject["bg_detected_in_rcyc"] != null)
                                    {
                                        if (bugObject["bg_detected_in_rcyc"] is Int32)
                                        {
                                            int releaseCycleId = bugObject["bg_detected_in_rcyc"];
                                            if (this.iterationMapping.ContainsKey(releaseCycleId))
                                            {
                                                detectedReleaseId = this.iterationMapping[releaseCycleId];
                                            }
                                        }
                                        else if (bugObject["bg_detected_in_rcyc"] is String)
                                        {
                                            string releaseCycleString = bugObject["bg_detected_in_rcyc"];
                                            int releaseCycleId = Int32.Parse(releaseCycleString);
                                            if (this.iterationMapping.ContainsKey(releaseCycleId))
                                            {
                                                detectedReleaseId = this.iterationMapping[releaseCycleId];
                                            }
                                        }
                                        else if (bugObject["bg_detected_in_rcyc"] is ICycle)
                                        {
                                            ICycle cycle = (ICycle)bugObject["bg_detected_in_rcyc"];
                                            int releaseCycleId = cycle.ID;
                                            if (this.iterationMapping.ContainsKey(releaseCycleId))
                                            {
                                                detectedReleaseId = this.iterationMapping[releaseCycleId];
                                            }
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine("Unable to access bg_detected_in_rcyc, so leaving unset, type=" + bugObject["bg_detected_in_rcyc"].GetType());
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    streamWriter.WriteLine("Unable to access Bug:DetectedRelCycle, so leaving unset - " + exception.Message);
                                }

                                try
                                {
                                    //Check target iteration
                                    if (bugObject["bg_target_rcyc"] != null)
                                    {
                                        if (bugObject["bg_target_rcyc"] is Int32)
                                        {
                                            int releaseCycleId = bugObject["bg_target_rcyc"];
                                            if (this.iterationMapping.ContainsKey(releaseCycleId))
                                            {
                                                resolvedReleaseId = this.iterationMapping[releaseCycleId];
                                            }
                                        }
                                        else if (bugObject["bg_target_rcyc"] is String)
                                        {
                                            string releaseCycleString = bugObject["bg_target_rcyc"];
                                            int releaseCycleId = Int32.Parse(releaseCycleString);
                                            if (this.iterationMapping.ContainsKey(releaseCycleId))
                                            {
                                                resolvedReleaseId = this.iterationMapping[releaseCycleId];
                                            }
                                        }
                                        else if (bugObject["bg_target_rcyc"] is ICycle)
                                        {
                                            ICycle cycle = (ICycle)bugObject["bg_target_rcyc"];
                                            int releaseCycleId = cycle.ID;
                                            if (this.iterationMapping.ContainsKey(releaseCycleId))
                                            {
                                                resolvedReleaseId = this.iterationMapping[releaseCycleId];
                                            }
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine("Unable to access bg_target_rcyc, so leaving unset, type=" + bugObject["bg_target_rcyc"].GetType());
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    streamWriter.WriteLine("Unable to access Bug:TargetRelCycle, so leaving unset - " + exception.Message);
                                }

                                //QualityCenter doesn't have the concept of a defect type, so pass NULL to use the project's default

                                //Load the incident and capture the new id
                                Nullable<int> newIncidentId = null;
                                try
                                {
                                    //Load the incident data
                                    RemoteIncident remoteIncident = new RemoteIncident();
                                    remoteIncident.PriorityId = priorityId;
                                    remoteIncident.SeverityId = severityId;
                                    remoteIncident.IncidentStatusId = statusId;
                                    remoteIncident.Name = name;
                                    remoteIncident.Description = MakeXmlSafe(description);
                                    remoteIncident.TestRunStepId = testRunStepId;
                                    remoteIncident.OpenerId = openerId;
                                    remoteIncident.OwnerId = ownerId;
                                    remoteIncident.ClosedDate = closedDate;
                                    remoteIncident.CreationDate = creationDate;
                                    remoteIncident.EstimatedEffort = estimatedEffort;
                                    remoteIncident.ActualEffort = actualEffort;
                                    remoteIncident.DetectedReleaseId = detectedReleaseId;
                                    remoteIncident.ResolvedReleaseId = resolvedReleaseId;

                                    //Load any custom properties
                                    List<RemoteArtifactCustomProperty> customProperties = new List<RemoteArtifactCustomProperty>();
                                    ImportCustomField(streamWriter, bugObject, "bg_user_01", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_02", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_03", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_04", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_05", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_06", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_07", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_08", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_09", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_10", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_11", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_12", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_13", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_14", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_15", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_16", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_17", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_18", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_19", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_20", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_21", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_22", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_23", customProperties);
                                    ImportCustomField(streamWriter, bugObject, "bg_user_24", customProperties);

                                    //See if we have the special HP ALM Bug ID mapped
                                    if (this.customPropertyMapping.ContainsKey(Utils.CUSTOM_FIELD_ALM_DEFECT_ID))
                                    {
                                        int propertyNumber = this.customPropertyMapping[Utils.CUSTOM_FIELD_ALM_DEFECT_ID];
                                        customProperties.Add(new RemoteArtifactCustomProperty() { PropertyNumber = propertyNumber, IntegerValue = bugId });
                                    }

                                    remoteIncident.CustomProperties = customProperties.ToArray();
                                    newIncidentId = ImportFormHandle.SpiraImportProxy.Incident_Create(remoteIncident).IncidentId;
                                }
                                catch (Exception exception)
                                {
                                    //If we have an error, log it and continue
                                    streamWriter.WriteLine("Error adding defect " + bugId + " to SpiraTest (" + exception.Message + ")");
                                }

                                if (newIncidentId.HasValue)
                                {
                                    //Now add the resolution
                                    RemoteComment remoteIncidentComment = new RemoteComment();
                                    remoteIncidentComment.ArtifactId = newIncidentId.Value;
                                    remoteIncidentComment.Text = MakeXmlSafe(resolution);
                                    remoteIncidentComment.CreationDate = (creationDate.HasValue) ? creationDate.Value : DateTime.Now;
                                    ImportFormHandle.SpiraImportProxy.Incident_AddComments(new RemoteComment[] { remoteIncidentComment });

                                    //Add attachments if requested
                                    if (this.MainFormHandle.chkImportAttachments.Checked)
                                    {
                                        try
                                        {
                                            ImportAttachments(streamWriter, "BUG", bugId, bugObject.Attachments, newIncidentId.Value, Utils.ArtifactType.Incident);
                                        }
                                        catch (Exception exception)
                                        {
                                            streamWriter.WriteLine("Warning: Unable to import attachments for bug " + bugId + " (" + exception.Message + ")");
                                        }
                                    }

                                    //Add to the incident mapping list
                                    incidentMapping.Add(bugId, newIncidentId.Value);
                                }
                            }
                        }
                    }

                    //**** Show that we've imported incidents ****
                    streamWriter.WriteLine("Defects/Incidents Imported");
                    this.ProgressForm_OnProgressUpdate(7);
                }

                //**** Mark the form as finished ****
                streamWriter.WriteLine("Import completed at: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                this.ProgressForm_OnFinish();

                //Close the debugging file
                streamWriter.Close();
            }
            catch (Exception exception)
            {
                //Log the error
                streamWriter.WriteLine("*ERROR* Occurred during Import: '" + exception.Message + "' at " + exception.Source + " (" + exception.StackTrace + ")");
                streamWriter.Close();

                //Display the exception message
                ProgressForm_OnError(exception);
            }
        }

        /// <summary>
        /// Imports a single custom field value
        /// </summary>
        /// <param name="qcObject"></param>
        /// <param name="remoteArtifact"></param>
        private void ImportCustomField(StreamWriter streamWriter, IBaseField qcObject, string qcFieldName, List<RemoteArtifactCustomProperty> artifactCustomProperties)
        {
            try
            {
                string qcFieldNameUpperCase = qcFieldName.ToUpperInvariant();
                if (qcObject[qcFieldName] != null && qcObject[qcFieldName] is String)
                {
                    string fieldValue = (string)qcObject[qcFieldName];

                    //Find the matching custom property/type
                    if (this.customPropertyMapping.ContainsKey(qcFieldNameUpperCase))
                    {
                        int propertyNumber = this.customPropertyMapping[qcFieldNameUpperCase];
                        if (this.customPropertyMappingType.ContainsKey(qcFieldNameUpperCase))
                        {
                            int customPropertyTypeId = this.customPropertyMappingType[qcFieldNameUpperCase];

                            //Handle each type separately
                            switch ((Utils.CustomPropertyTypeEnum)customPropertyTypeId)
                            {
                                case Utils.CustomPropertyTypeEnum.Text:
                                    {
                                        RemoteArtifactCustomProperty remoteArtifactCustomProperty = new RemoteArtifactCustomProperty();
                                        artifactCustomProperties.Add(remoteArtifactCustomProperty);
                                        remoteArtifactCustomProperty.PropertyNumber = propertyNumber;
                                        remoteArtifactCustomProperty.StringValue = fieldValue;
                                    }
                                    break;

                                case Utils.CustomPropertyTypeEnum.List:
                                    {
                                        //Get the custom property value id                                    
                                        if (this.customValueNameMapping.ContainsKey(fieldValue))
                                        {
                                            int customPropertyValueId = this.customValueNameMapping[fieldValue];
                                            RemoteArtifactCustomProperty remoteArtifactCustomProperty = new RemoteArtifactCustomProperty();
                                            artifactCustomProperties.Add(remoteArtifactCustomProperty);
                                            remoteArtifactCustomProperty.PropertyNumber = propertyNumber;
                                            remoteArtifactCustomProperty.IntegerValue = customPropertyValueId;
                                        }
                                        else
                                        {
                                            streamWriter.WriteLine(String.Format("Warning: Unable top find a matching custom property value for field value '{0}', so leaving custom property '{1}' unset.", fieldValue, qcFieldName));
                                        }
                                    }
                                    break;

                                case Utils.CustomPropertyTypeEnum.MultiList:
                                    {
                                        //Split the custom values (by semicolon)
                                        string[] values = fieldValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (values.Length > 0)
                                        {
                                            List<int> customPropertyValueIds = new List<int>();
                                            foreach (string customFieldValue in values)
                                            {
                                                //Get the custom property value id                                    
                                                if (this.customValueNameMapping.ContainsKey(customFieldValue))
                                                {
                                                    int customPropertyValueId = this.customValueNameMapping[customFieldValue];
                                                    customPropertyValueIds.Add(customPropertyValueId);
                                                }
                                                else
                                                {
                                                    streamWriter.WriteLine(String.Format("Warning: Unable top find a matching custom property value for field value '{0}', so leaving custom property '{1}' unset.", fieldValue, qcFieldName));
                                                }
                                            }
                                            if (customPropertyValueIds.Count > 0)
                                            {
                                                RemoteArtifactCustomProperty remoteArtifactCustomProperty = new RemoteArtifactCustomProperty();
                                                artifactCustomProperties.Add(remoteArtifactCustomProperty);
                                                remoteArtifactCustomProperty.PropertyNumber = propertyNumber;
                                                remoteArtifactCustomProperty.IntegerListValue = customPropertyValueIds.ToArray();
                                            }
                                        }
                                    }
                                    break;

                                case Utils.CustomPropertyTypeEnum.User:
                                    {
                                        //Get the user id for the login
                                        string userLogin = fieldValue;
                                        if (this.usersMapping.ContainsKey(userLogin))
                                        {
                                            int userId = this.usersMapping[userLogin];
                                            RemoteArtifactCustomProperty remoteArtifactCustomProperty = new RemoteArtifactCustomProperty();
                                            artifactCustomProperties.Add(remoteArtifactCustomProperty);
                                            remoteArtifactCustomProperty.PropertyNumber = propertyNumber;
                                            remoteArtifactCustomProperty.IntegerValue = userId;
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            streamWriter.WriteLine("Warning: Unable to find TYPE mapping for QC user field '" + qcFieldName + "' for HP ALM object " + qcObject.ID + " so ignoring this custom field and continuing.");
                        }
                    }
                    else
                    {
                        streamWriter.WriteLine("Warning: Unable to find NAME mapping for QC user field '" + qcFieldName + "' for HP ALM object " + qcObject.ID + " so ignoring this custom field and continuing.");
                    }
                }
            }
            catch (Exception exception)
            {
                //If we have an error, log it and continue
                streamWriter.WriteLine("Warning: Unable to access QC user field '" + qcFieldName + "' for HP ALM object " + qcObject.ID + " so ignoring this custom field and continuing (" + exception.Message + ")");
            }
        }

        private int ImportProject(StreamWriter streamWriter)
        {
            //Get the basic data from the OTA API
            MainFormHandle.EnsureConnected();
            string name = MainFormHandle.TdConnection.ProjectName;
            RemoteProject remoteProject = new RemoteProject();
            remoteProject.Name = name;
            remoteProject.Description = name;
            remoteProject.Active = true;

            //Use the SiteAdmin api to get the description, if the user doesn't have permissions, log and carry on
            try
            {
                //Get the project meta-data
                string projectXml = MainFormHandle.SaConnection.GetProject(MainFormHandle.TdConnection.DomainName, MainFormHandle.TdConnection.ProjectName);
                //streamWriter.WriteLine("Project XML: " + projectXml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(projectXml);
                XmlNode xmlDescNode = xmlDoc.SelectSingleNode("/TDXItem/DESCRIPTION");
                if (xmlDescNode != null)
                {
                    remoteProject.Description = MakeXmlSafe(xmlDescNode.InnerText);
                }
            }
            catch (Exception exception)
            {
                streamWriter.WriteLine("Unable to access the project through the SiteAdmin API, so project description will not be imported: " + exception.Message);
            }
            
            remoteProject = ImportFormHandle.SpiraImportProxy.Project_Create(remoteProject, null);
            streamWriter.WriteLine("New Project '" + name + "' Created");
            int projectId = remoteProject.ProjectId.Value;

            //Now we need to get the custom lists for the project
            Dictionary<int, int> customListMapping = new Dictionary<int, int>();
            Customization customization = MainFormHandle.TdConnection.Customization;

            CustomizationLists projectLists = customization.Lists;
            if (projectLists != null)
            {
                //the lists are a hierarchy (ROOT>LIST>VALUE), so get the root node
                for (int i = 1; i <= projectLists.Count; i++)
                {
                    CustomizationList projectList = projectLists.get_ListByCount(i);
                    string listName = projectList.Name;
                    bool isListDeleted = projectList.Deleted;

                    streamWriter.WriteLine(String.Format("Adding Project List: {0} to Spira", listName));

                    //Create the Spira custom list
                    RemoteCustomList remoteCustomList = new RemoteCustomList();
                    remoteCustomList.Name = listName;
                    remoteCustomList.Active = !isListDeleted;
                    int newCustomListId = ImportFormHandle.SpiraImportProxy.CustomProperty_AddCustomList(remoteCustomList).CustomPropertyListId.Value;
                    CustomizationListNode rootNode = projectList.RootNode;
                    if (!customListMapping.ContainsKey(rootNode.ID))
                    {
                        customListMapping.Add(rootNode.ID, newCustomListId);
                    }

                    //Get the children (values)
                    foreach (CustomizationListNode valueNode in rootNode.Children)
                    {
                        int valueNodeId = valueNode.ID;
                        string valueNodeName = valueNode.Name;
                        bool isValueDeleted = valueNode.Deleted;
                        string type = valueNode.Type;

                        //Create the Spira custom list values
                        RemoteCustomListValue remoteCustomListValue = new RemoteCustomListValue();
                        remoteCustomListValue.CustomPropertyListId = newCustomListId;
                        remoteCustomListValue.Name = valueNodeName;
                        //IsDeleted and IsActive fields may be added in future versions of the system
                        int newCustomListValueId = ImportFormHandle.SpiraImportProxy.CustomProperty_AddCustomListValue(remoteCustomListValue).CustomPropertyValueId.Value;

                        //Add to mapping (we map both name and ID)
                        streamWriter.WriteLine(String.Format("Adding Project List Value: {0} (ID={1}) to Spira, new Spira ID={2}", valueNodeName, valueNodeId, newCustomListValueId));
                        if (!this.customValueMapping.ContainsKey(valueNodeId))
                        {
                            this.customValueMapping.Add(valueNodeId, newCustomListValueId);
                        }
                        if (!this.customValueNameMapping.ContainsKey(valueNodeName))
                        {
                            this.customValueNameMapping.Add(valueNodeName, newCustomListValueId);
                        }
                    }
                }
            }

            //Now we need to get the custom fields defined in the project
            CustomizationFields customFieldDefinitions = (CustomizationFields)customization.Fields;
            if (customFieldDefinitions != null)
            {
                List customFields = customFieldDefinitions.get_Fields();
                Dictionary<int, int> artifactPropertyNumber = new Dictionary<int, int>();
                foreach (CustomizationField customField in customFields)
                {
                    //We only want custom fields
                    if (!customField.IsSystem && customField.IsActive)
                    {
                        string displayName = customField.UserLabel;
                        string fieldName = customField.ColumnName;
                        bool isMultiValue = customField.IsMultiValue;
                        bool isRequired = customField.IsRequired;   //We don't import options so not currently used
                        string tableName = customField.TableName;
                        string editType = customField.UserColumnType;

                        //streamWriter.WriteLine("Debug3: " + customField.RootId + "," + customField.ColumnType + "," + customField.UserColumnType + "," + customField.EditMask);

                        //See if we have a matching custom list
                        int? customPropertyListId = null;
                        if (customField.RootId != null)
                        {
                            int listRootId = customField.RootId;
                            if (customListMapping.ContainsKey(listRootId))
                            {
                                customPropertyListId = customListMapping[listRootId];
                            }
                        }

                        //Get the corresponding artifact id and custom property type
                        int? artifactTypeId = ConvertArtifactType(tableName);
                        int customPropertyTypeId = ConvertCustomPropertyType(customPropertyListId.HasValue, isMultiValue);

                        if (artifactTypeId.HasValue)
                        {
                            //Get the next available property number for this artifact type
                            int propertyNumber = 1;
                            if (artifactPropertyNumber.ContainsKey(artifactTypeId.Value))
                            {
                                propertyNumber = artifactPropertyNumber[artifactTypeId.Value] + 1;
                                artifactPropertyNumber[artifactTypeId.Value] = propertyNumber;
                            }
                            else
                            {
                                artifactPropertyNumber.Add(artifactTypeId.Value, propertyNumber);
                            }

                            //Add to Spira
                            RemoteCustomProperty remoteCustomProperty = new RemoteCustomProperty();
                            remoteCustomProperty.Name = displayName;
                            remoteCustomProperty.PropertyNumber = propertyNumber;
                            remoteCustomProperty.ProjectId = projectId;
                            remoteCustomProperty.CustomPropertyTypeId = customPropertyTypeId;
                            remoteCustomProperty.ArtifactTypeId = artifactTypeId.Value;
                            int customPropertyId = ImportFormHandle.SpiraImportProxy.CustomProperty_AddDefinition(remoteCustomProperty, customPropertyListId).CustomPropertyId.Value;

                            //Add to the mapping dictionaries
                            if (!this.customPropertyMapping.ContainsKey(fieldName))
                            {
                                this.customPropertyMapping.Add(fieldName, propertyNumber);
                            }
                            if (!this.customPropertyMappingType.ContainsKey(fieldName))
                            {
                                this.customPropertyMappingType.Add(fieldName, customPropertyTypeId);
                                streamWriter.WriteLine(String.Format("Adding Custom Property Mapping for: {0}=Custom_{1}:Type={2}:Caption={3}", fieldName, propertyNumber, customPropertyTypeId, displayName));
                            }
                        }
                        else
                        {
                            streamWriter.WriteLine(String.Format("WARNING: Unable to find artifact type for system table '{0}'", tableName));
                        }
                    }
                }

                //If we have at least one custom field type remaining for defects, add a special field to capture the legacy defect ID
                if (artifactPropertyNumber.ContainsKey((int)Utils.ArtifactType.Incident))
                {
                    int propertyNumber = artifactPropertyNumber[(int)Utils.ArtifactType.Incident];
                    if (propertyNumber < 30)
                    {
                        //Add to Spira
                        RemoteCustomProperty remoteCustomProperty = new RemoteCustomProperty();
                        remoteCustomProperty.Name = "ALM Defect #";
                        remoteCustomProperty.PropertyNumber = propertyNumber + 1;
                        remoteCustomProperty.ProjectId = projectId;
                        remoteCustomProperty.CustomPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.Integer;
                        remoteCustomProperty.ArtifactTypeId = (int)Utils.ArtifactType.Incident;
                        int customPropertyId = ImportFormHandle.SpiraImportProxy.CustomProperty_AddDefinition(remoteCustomProperty, null).CustomPropertyId.Value;

                        //Add to mapping
                        if (!this.customPropertyMapping.ContainsKey(Utils.CUSTOM_FIELD_ALM_DEFECT_ID))
                        {
                            this.customPropertyMapping.Add(Utils.CUSTOM_FIELD_ALM_DEFECT_ID, propertyNumber);
                        }
                    }
                }
            }

            return remoteProject.ProjectId.Value;
        }

        /// <summary>
        /// Imports a list of attachments for an artifact from QC extended storage
        /// </summary>
        /// <param name="streamWriter">The streamwriter used to log messages</param>
        /// <param name="entityType">The QC entity name</param>
        /// <param name="entityId">The QC entity ID</param>
        /// <param name="attachmentFactory">The QC attachment factory object</param>
        /// <param name="artifactId">The Spira artifact id</param>
        /// <param name="artifactTypeId">The Spira artifact type id</param>
        private void ImportAttachments(StreamWriter streamWriter, string entityType, int entityId, object attachmentFactory, int artifactId, Utils.ArtifactType artifactType)
        {
            //Make sure we have a populated attachment factory
            if (attachmentFactory == null)
            {
                return;
            }

            if (attachmentFactory is HP.ALM.AttachmentFactory)
            {
                try
                {
                    //Create a temporary folder to download the attachments to
                    string localSettings = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    if (!Directory.Exists(localSettings + "\\Inflectra"))
                    {
                        Directory.CreateDirectory(localSettings + "\\Inflectra");
                    }
                    if (!Directory.Exists(localSettings + "\\Inflectra\\QCImport"))
                    {
                        Directory.CreateDirectory(localSettings + "\\Inflectra\\QCImport");
                    }

                    //Now get the list of attachments and import
                    streamWriter.WriteLine("Retrieving Attachments for " + entityType + " '" + entityId + "'");
                    HP.ALM.List attachmentFactoryList = ((HP.ALM.AttachmentFactory)attachmentFactory).NewList("");
                    foreach (HP.ALM.Attachment attachment in attachmentFactoryList)
                    {
                        string displayFilename = (string)attachment.get_Name(1);
                        string filename = (string)attachment.get_Name(0);
                        streamWriter.WriteLine("Retrieving Attachment '" + filename + "'");

                        string description = "";
                        if (attachment.Description != null)
                        {
                            description = MakeXmlSafe(attachment.Description);
                        }

                        //Now we need to actually get the attachment binary data
                        HP.ALM.IExtendedStorage attachmentStorage = (HP.ALM.IExtendedStorage)attachment.AttachmentStorage;
                        attachmentStorage.ClientPath = localSettings + "\\Inflectra\\QCImport\\";
                        attachmentStorage.Load(filename, true);

                        //Now read the file back in and import
                        if (File.Exists(localSettings + "\\Inflectra\\QCImport\\" + filename))
                        {
                            FileStream fileStream = File.OpenRead(localSettings + "\\Inflectra\\QCImport\\" + filename);
                            int numBytes = (int)fileStream.Length;
                            byte[] attachmentData = new byte[numBytes];
                            fileStream.Read(attachmentData, 0, numBytes);
                            fileStream.Close();

                            //Import the attachment
                            RemoteDocument remoteDocument = new RemoteDocument();
                            remoteDocument.FilenameOrUrl = displayFilename;
                            remoteDocument.Description = MakeXmlSafe(description);
                            remoteDocument.ArtifactId = artifactId;
                            remoteDocument.ArtifactTypeId = (int)artifactType;
                            ImportFormHandle.SpiraImportProxy.Document_AddFile(remoteDocument, attachmentData);
                        }
                        else
                        {
                            streamWriter.WriteLine("Unable to import attachment '" + filename + "' - file data not readable from QualityCenter.");
                        }
                    }
                }
                catch (Exception exception)
                {
                    //Log and continue
                    streamWriter.WriteLine("Warning: Unable to import attachments for entity id '" + entityId + "' - continuing with import (" + exception.Message + ").");
                }
            }
        }

        /// <summary>
        /// Replaces the QualityCenter parameter token with the SpiraTest equivalent.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The output string</returns>
        /// <remarks>
        /// Quality center uses <<<Parameter Name>>> and SpiraTest uses
        ///	${parameter name} so we need to account for the potential
        ///	case difference as well. Also need to handle HTML encoded version as well
        /// </remarks>
        private string ReplaceParameterToken(string input)
        {
            string output = Regex.Replace(input, @"&lt;&lt;&lt;[a-zA-Z0-9 /:\-_\.]+&gt;&gt;&gt;", new MatchEvaluator(MakeParameterToken));
            output = Regex.Replace(output, @"\<\<\<[a-zA-Z0-9 /:\-_\.]+\>\>\>", new MatchEvaluator(MakeParameterToken2));
            return output;
        }

        private string MakeParameterToken(Match m)
        {
            // Get the matched string.
            string x = m.ToString();

            //Replace the <<< with ${ and >>> with }
            x = x.Replace("&lt;&lt;&lt;", "${");
            x = x.Replace("&gt;&gt;&gt;", "}");

            //Remove any spaces and make them underscores
            x = x.Replace(" ", "_");
            return x.ToLower();
        }
        private string MakeParameterToken2(Match m)
        {
            // Get the matched string.
            string x = m.ToString();

            //Replace the <<< with ${ and >>> with }
            x = x.Replace("<<<", "${");
            x = x.Replace(">>>", "}");

            //Remove any spaces and make them underscores
            x = x.Replace(" ", "_");
            return x.ToLower();
        }

        /// <summary>
        /// Converts a QualityCenter test set status for use in SpiraTest
        /// </summary>
        /// <param name="status">The TD execution status</param>
        /// <returns>The SpiraTest execution status</returns>
        protected int ConvertTestSetStatus(string status)
        {
            int statusId = 1; //Default to not started (no nulls allowed)
            switch (status)
            {
                case "Open":
                    //Not Started
                    statusId = 1;
                    break;

                case "Closed":
                    //Deferred
                    statusId = 5;
                    break;

            }
            return statusId;
        }

        /// <summary>
        /// Returns the Spira custom property type for the appropriate QC Edit Style (text if not one of the known ones)
        /// </summary>
        /// <param name="editType"></param>
        /// <returns></returns>
        private int ConvertCustomPropertyType(bool listProvided, bool isMultiValue)
        {
            int customPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.Text;
            /*  If they every allow you to access the SF_EDIT_TYPE field through the API, could get a better type relationshop
            switch (editType)
            {
                case "ListCombo":
                case "TreeCombo":
                    {
                        if (isMultiValue)
                        {
                            customPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.MultiList;
                        }
                        else
                        {
                            customPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.List;
                        }
                    }
                    break;

                case "UserCombo":
                    customPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.User;
                    break;
            }*/
            if (listProvided)
            {
                if (isMultiValue)
                {
                    customPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.MultiList;
                }
                else
                {
                    customPropertyTypeId = (int)Utils.CustomPropertyTypeEnum.List;
                }
            }

            return customPropertyTypeId;
        }

        /// <summary>
        /// Converts a QC artifact type into the corresponding Spira artifact type
        /// </summary>
        /// <param name="qcType"></param>
        /// <returns></returns>
        private int? ConvertArtifactType(string qcType)
        {
            int? artifactTypeId = null;
            switch (qcType)
            {
                case "REQ":
                    artifactTypeId = (int)Utils.ArtifactType.Requirement;
                    break;

                case "RELEASES":
                    artifactTypeId = (int)Utils.ArtifactType.Release;
                    break;

                case "TEST":
                    artifactTypeId = (int)Utils.ArtifactType.TestCase;
                    break;

                case "RUN":
                    artifactTypeId = (int)Utils.ArtifactType.TestRun;
                    break;

                case "BUG":
                    artifactTypeId = (int)Utils.ArtifactType.Incident;
                    break;

                case "CYCLE":
                    artifactTypeId = (int)Utils.ArtifactType.TestSet;
                    break;

                case "DESSTEP":
                    artifactTypeId = (int)Utils.ArtifactType.TestStep;
                    break;
            }

            return artifactTypeId;
        }

        /// <summary>
        /// Converts a QualityCenter execution status for use in SpiraTest
        /// </summary>
        /// <param name="status">The TD execution status</param>
        /// <returns>The SpiraTest execution status</returns>
        protected int ConvertExecutionStatus(string status)
        {
            int statusId = 3; //Default to not run (no nulls allowed)
            switch (status)
            {
                case "Failed":
                    //Failed
                    statusId = 1;
                    break;
                case "Passed":
                    //Passed
                    statusId = 2;
                    break;
                case "N/A":
                    //Not Run
                    statusId = 3;
                    break;
                case "No Run":
                    //Not Run
                    statusId = 3;
                    break;
                case "Not Completed":
                    //Not Run
                    statusId = 3;
                    break;
            }
            return statusId;
        }

        /// <summary>
        /// Makes a string safe for use in XML (e.g. web service)
        /// </summary>
        /// <param name="input">The input string (as object)</param>
        /// <returns>The output string</returns>
        protected string MakeXmlSafe(object input)
        {
            //Handle null reference case
            if (input == null)
            {
                return "";
            }

            //Handle the case where the object is not a string
            string inputString;
            if (input.GetType() == typeof(string))
            {
                inputString = (string)input;
            }
            else
            {
                inputString = input.ToString();
            }

            //Handle empty string case
            if (inputString == "")
            {
                return inputString;
            }

            string output = inputString.Replace("\x00", "");
            output = output.Replace("\x01", "");
            output = output.Replace("\x02", "");
            output = output.Replace("\x03", "");
            output = output.Replace("\x04", "");
            output = output.Replace("\x05", "");
            output = output.Replace("\x06", "");
            output = output.Replace("\x07", "");
            output = output.Replace("\x08", "");
            output = output.Replace("\x0B", "");
            output = output.Replace("\x0C", "");
            output = output.Replace("\x0E", "");
            output = output.Replace("\x0F", "");
            output = output.Replace("\x10", "");
            output = output.Replace("\x11", "");
            output = output.Replace("\x12", "");
            output = output.Replace("\x13", "");
            output = output.Replace("\x14", "");
            output = output.Replace("\x15", "");
            output = output.Replace("\x16", "");
            output = output.Replace("\x17", "");
            output = output.Replace("\x18", "");
            output = output.Replace("\x19", "");
            output = output.Replace("\x1A", "");
            output = output.Replace("\x1B", "");
            output = output.Replace("\x1C", "");
            output = output.Replace("\x1D", "");
            output = output.Replace("\x1E", "");
            output = output.Replace("\x1F", "");
            return output;
        }

        /// <summary>
        /// Converts a TestDirector priority for use in SpiraTest
        /// </summary>
        /// <param name="priority">The TD priority</param>
        /// <param name="spiraImport">Handle to the import API</param>
        /// <returns>The SpiraTest priority</returns>
        /// <remarks>Inserts the priority into SpiraTest upon first usage then maps for subsequent</remarks>
        protected Nullable<int> ConvertPriority(string priority, SpiraImportExport.ImportExportClient spiraImport)
        {
            //Handle NULL case
            if (priority == null || priority == "")
            {
                return null;
            }

            //First see if we have the priority in our mapping hashtable
            if (this.incidentPriorityMapping.ContainsKey(priority))
            {
                return (int)this.incidentPriorityMapping[priority];
            }
            else
            {
                //Add a new custom incident priority to SpiraTest - default to color=silver
                RemoteIncidentPriority remoteIncidentPriority = new RemoteIncidentPriority();
                remoteIncidentPriority.Name = priority;
                remoteIncidentPriority.Active = true;
                remoteIncidentPriority.Color = "eeeeee";
                Nullable<int> priorityId = spiraImport.Incident_AddPriority(remoteIncidentPriority).PriorityId;
                if (priorityId.HasValue)
                {
                    this.incidentPriorityMapping.Add(priority, priorityId.Value);
                }
                return priorityId;
            }
        }

        /// <summary>
        /// Converts a TestDirector severity for use in SpiraTest
        /// </summary>
        /// <param name="severity">The TD severity</param>
        /// <param name="spiraImport">Handle to the import API</param>
        /// <returns>The SpiraTest severity</returns>
        /// <remarks>Inserts the severity into SpiraTest upon first usage then maps for subsequent</remarks>
        protected Nullable<int> ConvertSeverity(string severity, SpiraImportExport.ImportExportClient spiraImport)
        {
            //Handle NULL case
            if (severity == null || severity == "")
            {
                return null;
            }

            //First see if we have the severity in our mapping hashtable
            if (this.incidentSeverityMapping.ContainsKey(severity))
            {
                return (int)this.incidentSeverityMapping[severity];
            }
            else
            {
                //Add a new custom incident severity to SpiraTest - default to color=silver
                RemoteIncidentSeverity remoteIncidentSeverity = new RemoteIncidentSeverity();
                remoteIncidentSeverity.Name = severity;
                remoteIncidentSeverity.Color = "eeeeee";
                remoteIncidentSeverity.Active = true;
                Nullable<int> severityId = spiraImport.Incident_AddSeverity(remoteIncidentSeverity).SeverityId;
                if (severityId.HasValue)
                {
                    this.incidentSeverityMapping.Add(severity, severityId.Value);
                }
                return severityId;
            }
        }

        /// <summary>
        /// Converts a TestDirector incident status for use in SpiraTest
        /// </summary>
        /// <param name="status">The TD status</param>
        /// <param name="spiraImport">Handle to the import API</param>
        /// <returns>The SpiraTest status</returns>
        /// <remarks>Inserts the status into SpiraTest upon first usage then maps for subsequent</remarks>
        protected Nullable<int> ConvertStatus(string status, SpiraImportExport.ImportExportClient spiraImport)
        {
            //Handle NULL case
            if (status == null || status == "")
            {
                return null;
            }

            //First see if we have the status in our mapping hashtable
            if (this.incidentStatusMapping.ContainsKey(status))
            {
                return (int)this.incidentStatusMapping[status];
            }
            else
            {
                //Add a new custom incident status to SpiraTest
                RemoteIncidentStatus remoteIncidentStatus = new RemoteIncidentStatus();
                remoteIncidentStatus.Name = status;
                remoteIncidentStatus.Active = true;
                Nullable<int> incidentStatusId = spiraImport.Incident_AddStatus(remoteIncidentStatus).IncidentStatusId;
                if (incidentStatusId.HasValue)
                {
                    this.incidentStatusMapping.Add(status, incidentStatusId.Value);
                }
                return incidentStatusId;
            }
        }

        /// <summary>
        /// Converts a HP ALM requirement status for use in SpiraTest
        /// </summary>
        /// <param name="reviewStatus">The HP review status</param>
        /// <returns>The SpiraTest importance</returns>
        protected int ConvertReqStatus(string reviewStatus)
        {
            int statusId = (int)Utils.RequirementStatusEnum.Requested;
            switch (reviewStatus)
            {
                case "Not Reviewed":
                    //Low
                    statusId = (int)Utils.RequirementStatusEnum.Requested;
                    break;
                case "Reviewed":
                    //Medium
                    statusId = (int)Utils.RequirementStatusEnum.Accepted;
                    break;
                case "Changed":
                    //High
                    statusId = (int)Utils.RequirementStatusEnum.UnderReview;
                    break;
            }
            return statusId;
        }

        /// <summary>
        /// Converts a TestDirector requirement importance for use in SpiraTest
        /// </summary>
        /// <param name="importance">The TD importance</param>
        /// <returns>The SpiraTest importance</returns>
        protected Nullable<int> ConvertImportance(string importance)
        {
            Nullable<int> importanceId = null;
            switch (importance)
            {
                case "1-Low":
                    //Low
                    importanceId = (int)Utils.RequirementImportanceEnum.Low;
                    break;
                case "2-Medium":
                    //Medium
                    importanceId = (int)Utils.RequirementImportanceEnum.Medium;
                    break;
                case "3-High":
                    //High
                    importanceId = (int)Utils.RequirementImportanceEnum.High;
                    break;
                case "4-Very High":
                    //Very High
                    importanceId = (int)Utils.RequirementImportanceEnum.Critical;
                    break;
                case "5-Urgent":
                    //Urgent
                    importanceId = (int)Utils.RequirementImportanceEnum.Critical;
                    break;
            }
            return importanceId;
        }

        /// <summary>
        /// Retrieves the SpiraTest test run step id from a linked Quality Center bug id
        /// </summary>
        /// <param name="bugObject">The QC bug object</param>
        /// <returns>The test run step of the test step that generated the bug</returns>
        protected Nullable<int> GetTestRunStepIdForBug(Bug bugObject)
        {
            Nullable<int> testRunStepId = null;

            //First get the qc test step id for this bug
            if (qcBugToRunStepMapping.ContainsKey(bugObject.ID))
            {
                int runStepId = qcBugToRunStepMapping[bugObject.ID];
                //Lookup the mapping to the Spira test run step ID
                if (this.testRunStepMapping.ContainsKey(runStepId))
                {
                    testRunStepId = (int)this.testRunStepMapping[runStepId];
                }
            }
            return testRunStepId;
        }

        /// <summary>
        /// Imports release folders into Spira as 'parent' releases
        /// </summary>
        /// <param name="streamWriter">The logfile writer</param>
        /// <param name="releaseFactory">The QC release folder factory object</param>
        /// <param name="parentReleaseId">The parent Spira release, or null for top-level</param>
        protected void ImportReleaseFolders(StreamWriter streamWriter, HP.ALM.ReleaseFolderFactory relFolderFactory, int? parentReleaseId)
        {
            HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)relFolderFactory.Filter;
            tdFilter.set_Order("rf_path", 1);
            HP.ALM.List relFolderList;
            try
            {
                relFolderList = (HP.ALM.List)tdFilter.NewList();
            }
            catch (Exception)
            {
                //If we get an exception, try to reconnect and re-execute
                MainFormHandle.TryReconnect();
                relFolderFactory = (HP.ALM.ReleaseFolderFactory)MainFormHandle.TdConnection.ReleaseFolderFactory;
                tdFilter = (HP.ALM.TDFilter)relFolderFactory.Filter;
                relFolderList = (HP.ALM.List)tdFilter.NewList();
            }

            //Import the release folders as top-level releases in Spira
            foreach (HP.ALM.ReleaseFolder relFolderObject in relFolderList)
            {
                //Extract the release folder info
                int releaseFolderId = (int)relFolderObject.ID;

                //The factory object returns all children, not just the direct ones, so make sure we only load a release once
                if (!releaseFolderMapping.ContainsKey(releaseFolderId))
                {
                    //Load the release and capture the ID
                    RemoteRelease remoteRelease = new RemoteRelease();
                    remoteRelease.Name = relFolderObject.Name;
                    remoteRelease.VersionNumber = relFolderObject.Name.SafeSubstring(10);
                    if (relFolderObject["rf_description"] != null)
                    {
                        remoteRelease.Description = MakeXmlSafe(relFolderObject["rf_description"]);
                    }
                    //Dummy dates for folders
                    remoteRelease.StartDate = DateTime.UtcNow;
                    remoteRelease.EndDate = DateTime.UtcNow.AddMonths(1);
                    remoteRelease.ResourceCount = 1;
                    remoteRelease.Active = true;
                    int newReleaseId = ImportFormHandle.SpiraImportProxy.Release_Create(remoteRelease, parentReleaseId).ReleaseId.Value;
                    streamWriter.WriteLine("Added release folder: '" + releaseFolderId);

                    //Add attachments if requested
                    if (this.MainFormHandle.chkImportAttachments.Checked)
                    {
                        try
                        {
                            ImportAttachments(streamWriter, "RELEASE_FOLDERS", releaseFolderId, relFolderObject.Attachments, newReleaseId, Utils.ArtifactType.Release);
                        }
                        catch (Exception exception)
                        {
                            streamWriter.WriteLine("Warning: Unable to import attachments for release folder " + releaseFolderId + " (" + exception.Message + ")");
                        }
                    }

                    //Add to the mapping hashtables
                    if (!this.releaseFolderMapping.ContainsKey(releaseFolderId))
                    {
                        this.releaseFolderMapping.Add(releaseFolderId, newReleaseId);
                    }

                    //Import any child release folders
                    ImportReleaseFolders(streamWriter, relFolderObject.ReleaseFolderFactory, newReleaseId);

                    //Now import the child releases
                    ImportReleases(streamWriter, relFolderObject.ReleaseFactory, newReleaseId);
                }
            }
        }

        /// <summary>
        /// Imports automation hosts into Spira
        /// </summary>
        /// <param name="streamWriter">The logfile writer</param>
        /// <param name="hostFactory">The QC host factory object</param>
        protected void ImportHosts(StreamWriter streamWriter, HP.ALM.HostFactory hostFactory)
        {
            HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)hostFactory.Filter;
            tdFilter.set_Order("ho_name", 1);
            HP.ALM.List hostList = (HP.ALM.List)tdFilter.NewList();

            //Import the hosts into Spira
            foreach (HP.ALM.Host hostObject in hostList)
            {
                //Extract the host info
                string hostToken = (string)hostObject.ID;

                //Load the automation host and capture the ID
                RemoteAutomationHost remoteAutomationHost = new RemoteAutomationHost();
                remoteAutomationHost.Name = hostObject.Name;
                remoteAutomationHost.Token = hostToken;
                remoteAutomationHost.Active = true;
                remoteAutomationHost.Description = MakeXmlSafe(hostObject.Description);
                int newAutomationHostId = ImportFormHandle.SpiraImportProxy.AutomationHost_Create(remoteAutomationHost).AutomationHostId.Value;
                streamWriter.WriteLine("Added host: '" + hostToken);

                //Add to the mapping hashtables
                if (!this.hostMapping.ContainsKey(hostToken))
                {
                    this.hostMapping.Add(hostToken, newAutomationHostId);
                }
            }
        }

        protected void ImportRequirements(StreamWriter streamWriter, HP.ALM.ReqFactory reqFactory)
        {
            HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)reqFactory.Filter;
            tdFilter.set_Order("rq_req_path", 1);
            HP.ALM.List reqList;
            try
            {
                reqList = (HP.ALM.List)tdFilter.NewList();
            }
            catch (Exception)
            {
                //If we get an exception, try to reconnect and re-execute
                MainFormHandle.TryReconnect();
                reqFactory = (HP.ALM.ReqFactory)MainFormHandle.TdConnection.ReqFactory;
                tdFilter = (HP.ALM.TDFilter)reqFactory.Filter;
                reqList = (HP.ALM.List)tdFilter.NewList();
            }

            string lastRequirementPath = "";
            foreach (HP.ALM.Req reqObject in reqList)
            {
                //Extract the requirement info
                int requirementId = (int)reqObject.ID;
                string requirementPath = "";
                try
                {
                    requirementPath = reqObject["rq_req_path"];
                }
                catch (Exception exception)
                {
                    //Folders sometimes don't have paths
                    streamWriter.WriteLine(String.Format("Warning: unable to access rq_req_path for requirement {0} so ignoring path! ({1})", requirementId, exception.Message));
                }
                streamWriter.WriteLine(String.Format("Importing Requirement: {0} with path '{1}'", requirementId, requirementPath));
                string requirementPriority = reqObject.Priority;
                string name = MakeXmlSafe(reqObject.Name);
                string requirementComments = "";
                if (!String.IsNullOrEmpty(reqObject.Comment))
                {
                    //TD comments will be used for SpiraTest's description field
                    requirementComments = MakeXmlSafe(reqObject.Comment);
                }
                string authorName = reqObject.Author;

                //Convert the priority (TD has 5 states, SpiraTest has 4)
                Nullable<int> importanceId = ConvertImportance(requirementPriority);

                //Convert the status
                string reqReviewStatus = reqObject.Reviewed;
                int statusId = ConvertReqStatus(reqReviewStatus);

                //Convert the path into a relative position
                int indentPosition = 0;
                if (lastRequirementPath != "")
                {
                    //Check to see if the current path is a different length to the last one
                    if (lastRequirementPath.Length != requirementPath.Length)
                    {
                        //Get the relative offset
                        indentPosition = (requirementPath.Length - lastRequirementPath.Length) / 3;
                    }
                }
                lastRequirementPath = requirementPath;

                //See if we're a folder (doesn't have all the fields)
                bool isFolder = false;
                try
                {
                    if (reqObject.Type != null && reqObject.Type.ToLowerInvariant() == "folder")
                    {
                        isFolder = true;
                    }
                }
                catch (Exception)
                {
                    //Folders sometimes throw an exception when you try and access the field
                    isFolder = true;
                }

                //See if we have a release specified
                int? releaseId = null;
                try
                {
                    if (!isFolder && reqObject["rq_target_rel"] != null)
                    {
                        int qcReleaseId = (int)reqObject["rq_target_rel"];
                        if (this.releaseMapping.ContainsKey(qcReleaseId))
                        {
                            releaseId = this.releaseMapping[qcReleaseId];
                            streamWriter.WriteLine("Found requirement release: RL" + releaseId.Value.ToString());
                            break;
                        }
                    }
                    else if (!isFolder && reqObject["rq_target_rel_varchar"] != null)
                    {
                        //They are semicolon-separated
                        string relList = (string)reqObject["rq_target_rel_varchar"];
                        //streamWriter.WriteLine("DEBUG1: " + relList);
                        string[] releases = relList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string qcRelease in releases)
                        {
                            int qcReleaseId;
                            if (Int32.TryParse(qcRelease, out qcReleaseId))
                            {
                                if (this.releaseMapping.ContainsKey(qcReleaseId))
                                {
                                    releaseId = this.releaseMapping[qcReleaseId];
                                    streamWriter.WriteLine("Found requirement release: RL" + releaseId.Value.ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Some types don't let you access this field
                    streamWriter.WriteLine(String.Format("Requirement {0} of type {1} does not allow access to the Release field so ignoring.", reqObject.ID, reqObject.Type));
                }

                //See if we have an iteration specified
                int? iterationId = null;
                try
                {
                    if (!isFolder && reqObject["rq_target_rcyc"] != null)
                    {
                        int qcCycleId = (int)reqObject["rq_target_rcyc"];
                        if (this.iterationMapping.ContainsKey(qcCycleId))
                        {
                            iterationId = this.iterationMapping[qcCycleId];
                            streamWriter.WriteLine("Found requirement iteration: RL" + iterationId.Value.ToString());
                            break;
                        }
                    }
                    else if (!isFolder && reqObject["rq_target_rcyc_varchar"] != null)
                    {
                        //They are semicolon-separated
                        string cycList = (string)reqObject["rq_target_rcyc_varchar"];
                        //streamWriter.WriteLine("DEBUG1: " + cycList);
                        string[] cycles = cycList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string qcCycle in cycles)
                        {
                            int qcCycleId;
                            if (Int32.TryParse(qcCycle, out qcCycleId))
                            {
                                if (this.iterationMapping.ContainsKey(qcCycleId))
                                {
                                    iterationId = this.iterationMapping[qcCycleId];
                                    streamWriter.WriteLine("Found requirement iteration: RL" + iterationId.Value.ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Some types don't let you access this field
                    streamWriter.WriteLine(String.Format("Requirement {0} of type {1} does not allow access to the Release field so ignoring.", reqObject.ID, reqObject.Type));
                }

                //If we have an iteration, use that instead of the release
                if (iterationId.HasValue)
                {
                    releaseId = iterationId.Value;
                }

                //Lookup the author from the user mapping hashtable
                Nullable<int> authorId = null;
                if (authorName != null && authorName != "")
                {
                    if (this.usersMapping.ContainsKey(authorName))
                    {
                        authorId = (int)this.usersMapping[authorName];
                    }
                }

                //Load the requirement and capture the new id
                int? newRequirementId = null;
                try
                {
                    //Populate the requirement
                    RemoteRequirement remoteRequirement = new RemoteRequirement();
                    remoteRequirement.StatusId = statusId;
                    remoteRequirement.ImportanceId = importanceId;
                    remoteRequirement.Name = name;
                    remoteRequirement.Description = MakeXmlSafe(requirementComments);
                    remoteRequirement.AuthorId = authorId;
                    remoteRequirement.ReleaseId = releaseId;

                    //Load any custom properties - QC stores them all as text
                    if (!isFolder)
                    {
                        try
                        {
                            List<RemoteArtifactCustomProperty> customProperties = new List<RemoteArtifactCustomProperty>();
                            ImportCustomField(streamWriter, reqObject, "rq_user_01", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_02", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_03", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_04", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_05", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_06", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_07", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_08", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_09", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_10", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_11", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_12", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_13", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_14", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_15", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_16", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_17", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_18", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_19", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_20", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_21", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_22", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_23", customProperties);
                            ImportCustomField(streamWriter, reqObject, "rq_user_24", customProperties);
                            remoteRequirement.CustomProperties = customProperties.ToArray();
                        }
                        catch (Exception exception)
                        {
                            //If we have an error, log it and continue
                            streamWriter.WriteLine("Warning: Unable to access QC user fields for requirement " + requirementId + " so ignoring custom fields and continuing (" + exception.Message + ")");
                        }
                    }
                    newRequirementId = ImportFormHandle.SpiraImportProxy.Requirement_Create1(remoteRequirement, indentPosition).RequirementId;
                }
                catch (Exception exception)
                {
                    //If we have an error, log it and continue
                    streamWriter.WriteLine("Error adding requirement " + requirementId + " to SpiraTest (" + exception.Message + ")");
                }
                if (newRequirementId.HasValue)
                {
                    //Add to the mapping hashtable
                    this.requirementsMapping.Add(requirementId, newRequirementId.Value);

                    //Add attachments if requested
                    if (this.MainFormHandle.chkImportAttachments.Checked)
                    {
                        try
                        {
                            ImportAttachments(streamWriter, "REQ", requirementId, reqObject.Attachments, newRequirementId.Value, Utils.ArtifactType.Requirement);
                        }
                        catch (Exception exception)
                        {
                            streamWriter.WriteLine("Warning: Unable to import attachments for requirement " + requirementId + " (" + exception.Message + ")");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Imports releases into Spira
        /// </summary>
        /// <param name="streamWriter">The logfile writer</param>
        /// <param name="releaseFactory">The QC release factory object</param>
        /// <param name="parentReleaseId">The parent Spira release, or null for top-level</param>
        protected void ImportReleases(StreamWriter streamWriter, HP.ALM.ReleaseFactory releaseFactory, int? parentReleaseId)
        {
            HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)releaseFactory.Filter;
            tdFilter.set_Order("rel_name", 1);
            HP.ALM.List releaseList = (HP.ALM.List)tdFilter.NewList();

            //Import the releases into Spira
            foreach (HP.ALM.Release releaseObject in releaseList)
            {
                //Extract the release info
                int releaseId = (int)releaseObject.ID;

                //The factory object returns all children, not just the direct ones, so make sure we only load a release once
                if (!releaseMapping.ContainsKey(releaseId))
                {
                    //Load the release and capture the ID
                    RemoteRelease remoteRelease = new RemoteRelease();
                    remoteRelease.Name = releaseObject.Name;
                    if (releaseObject["rel_description"] != null)
                    {
                        remoteRelease.Description = MakeXmlSafe(releaseObject["rel_description"]);
                    }
                    remoteRelease.VersionNumber = releaseObject.Name.SafeSubstring(10);
                    remoteRelease.StartDate = releaseObject.StartDate.ToUniversalTime();
                    remoteRelease.EndDate = releaseObject.EndDate.ToUniversalTime();
                    remoteRelease.ResourceCount = 1;
                    remoteRelease.Active = true;
                    int newReleaseId = ImportFormHandle.SpiraImportProxy.Release_Create(remoteRelease, parentReleaseId).ReleaseId.Value;
                    streamWriter.WriteLine("Added release: '" + releaseId);

                    //Add attachments if requested
                    if (this.MainFormHandle.chkImportAttachments.Checked)
                    {
                        try
                        {
                            ImportAttachments(streamWriter, "RELEASES", releaseId, releaseObject.Attachments, newReleaseId, Utils.ArtifactType.Release);
                        }
                        catch (Exception exception)
                        {
                            streamWriter.WriteLine("Warning: Unable to import attachments for release " + releaseId + " (" + exception.Message + ")");
                        }
                    }

                    //Add to the mapping hashtables
                    if (!this.releaseMapping.ContainsKey(releaseId))
                    {
                        this.releaseMapping.Add(releaseId, newReleaseId);
                    }

                    //Add any iterations (if there are any)
                    if (releaseObject.CycleFactory != null)
                    {
                        ImportIterations(streamWriter, releaseObject.CycleFactory, newReleaseId);
                    }
                }
            }
        }

        /// <summary>
        /// Imports releases into Spira
        /// </summary>
        /// <param name="streamWriter">The logfile writer</param>
        /// <param name="releaseFactory">The QC release factory object</param>
        /// <param name="parentReleaseId">The parent Spira release, or null for top-level</param>
        protected void ImportIterations(StreamWriter streamWriter, HP.ALM.CycleFactory cycleFactory, int? parentReleaseId)
        {
            HP.ALM.TDFilter tdFilter = (HP.ALM.TDFilter)cycleFactory.Filter;
            tdFilter.set_Order("rcyc_name", 1);
            HP.ALM.List iterationList = (HP.ALM.List)tdFilter.NewList();

            //Import the iterations into Spira
            foreach (HP.ALM.Cycle cycleObject in iterationList)
            {
                //Extract the iteration info
                int cycleId = (int)cycleObject.ID;

                //The factory object returns all children, not just the direct ones, so make sure we only load an iteration once
                if (!iterationMapping.ContainsKey(cycleId))
                {
                    //Load the iteration and capture the ID
                    RemoteRelease remoteRelease = new RemoteRelease();
                    remoteRelease.Name = cycleObject.Name;
                    if (cycleObject["rcyc_description"] != null)
                    {
                        remoteRelease.Description = MakeXmlSafe(cycleObject["rcyc_description"]);
                    }
                    remoteRelease.VersionNumber = cycleObject.Name.SafeSubstring(10);
                    remoteRelease.StartDate = cycleObject.StartDate.ToUniversalTime();
                    remoteRelease.EndDate = cycleObject.EndDate.ToUniversalTime();
                    remoteRelease.ResourceCount = 1;
                    remoteRelease.Active = true;
                    remoteRelease.Iteration = true;
                    int newIterationId = ImportFormHandle.SpiraImportProxy.Release_Create(remoteRelease, parentReleaseId).ReleaseId.Value;
                    streamWriter.WriteLine("Added iteration(cycle): '" + cycleId);

                    /*
                    //Add attachments if requested
                    if (this.MainFormHandle.chkImportAttachments.Checked)
                    {
                        try
                        {
                            ImportAttachments(streamWriter, "RELEASES", cycleId, releaseObject.Attachments, newReleaseId, Utils.ArtifactType.Release);
                        }
                        catch (Exception exception)
                        {
                            streamWriter.WriteLine("Warning: Unable to import attachments for release " + releaseId + " (" + exception.Message + ")");
                        }
                    }*/

                    //Add to the mapping hashtables
                    if (!this.iterationMapping.ContainsKey(cycleId))
                    {
                        this.iterationMapping.Add(cycleId, newIterationId);
                    }
                }
            }
        }

        /// <summary>
        /// Recursively imports a list of subjects into SpiraTest as test folders
        /// </summary>
        /// <param name="treeManager">Handle to the tree manager</param>
        /// <param name="subjectList">The subject list</param>
        /// <param name="parentFolderId">The SpiraTest parent test folder</param>
        protected void ImportSubjectNode(TreeManager treeManager, HP.ALM.IList subjectList, Nullable<int> parentFolderId)
        {
            for (int i = 1; i <= subjectList.Count; i++)
            {
                //See if we have a subject node or a subject name
                HP.ALM.SubjectNode subjectNode;
                if (subjectList[i].GetType() == typeof(string))
                {
                    //Extract the test folder info from the subject tree
                    string subjectName = (string)subjectList[i];
                    subjectNode = (HP.ALM.SubjectNode)treeManager[subjectName];
                }
                else
                {
                    subjectNode = (HP.ALM.SubjectNode)subjectList[i];
                }

                //Extract the subject node information
                int subjectId = subjectNode.NodeID;
                string name = MakeXmlSafe(subjectNode.Name);

                //Load the test folder and capture the new id
                RemoteTestCase remoteTestFolder = new RemoteTestCase();
                remoteTestFolder.Name = name;
                int newTestFolderId = ImportFormHandle.SpiraImportProxy.TestCase_CreateFolder(remoteTestFolder, parentFolderId).TestCaseId.Value;

                //Add to the mapping hashtable
                this.testFolderMapping.Add(subjectId, newTestFolderId);

                //Now get the children and load them under this node
                HP.ALM.IList childList = subjectNode.NewList();
                if (childList.Count > 0)
                {
                    ImportSubjectNode(treeManager, childList, newTestFolderId);
                }
            }
        }

        /// <summary>
        /// Recursively imports a list of test set folders into SpiraTest as test set folders
        /// </summary>
        /// <param name="treeManager">Handle to the tree manager</param>
        /// <param name="folderList">The subject list</param>
        /// <param name="parentFolderId">The SpiraTest parent test set folder</param>
        protected void ImportTestSetFolderNode(HP.ALM.TestSetTreeManager treeManager, HP.ALM.IList folderList, Nullable<int> parentFolderId)
        {
            for (int i = 1; i <= folderList.Count; i++)
            {
                //See if we have a test set foldernode or a test set folder name
                HP.ALM.TestSetFolder testSetFolderNode;
                testSetFolderNode = (HP.ALM.TestSetFolder)folderList[i];

                //Extract the subject node information
                int testSetFolderNodeId = testSetFolderNode.NodeID;
                string name = MakeXmlSafe(testSetFolderNode.Name);

                //Load the test set folder and capture the new id
                RemoteTestSet remoteTestSetFolder = new RemoteTestSet();
                remoteTestSetFolder.Name = name;
                int newTestSetFolderId = ImportFormHandle.SpiraImportProxy.TestSet_CreateFolder(remoteTestSetFolder, parentFolderId).TestSetId.Value;

                //Add to the mapping hashtable
                this.testSetFolderMapping.Add(testSetFolderNodeId, newTestSetFolderId);

                //Now get the children and load them under this node
                HP.ALM.IList childList = testSetFolderNode.NewList();
                if (childList.Count > 0)
                {
                    ImportTestSetFolderNode(treeManager, childList, newTestSetFolderId);
                }
            }
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {

        }
    }
}
