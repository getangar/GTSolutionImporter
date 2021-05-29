namespace Tangari.XrmToolBoxExtensions.SolutionImporter
{
    partial class GTSolutionImporterControlPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GTSolutionImporterControlPlugin));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.btnGetOrgs = new System.Windows.Forms.Button();
            this.lstOrgs = new System.Windows.Forms.ListBox();
            this.gtpSelectOrgs = new System.Windows.Forms.GroupBox();
            this.grpSelectSolution = new System.Windows.Forms.GroupBox();
            this.lblSolution = new System.Windows.Forms.Label();
            this.btnSelectSolution = new System.Windows.Forms.Button();
            this.txtSolutionPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpImportSolution = new System.Windows.Forms.GroupBox();
            this.lblImportSolution = new System.Windows.Forms.Label();
            this.btnImportSolution = new System.Windows.Forms.Button();
            this.grpImportStatus = new System.Windows.Forms.GroupBox();
            this.lstImportStatus = new System.Windows.Forms.ListBox();
            this.toolStripMenu.SuspendLayout();
            this.gtpSelectOrgs.SuspendLayout();
            this.grpSelectSolution.SuspendLayout();
            this.grpImportSolution.SuspendLayout();
            this.grpImportStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator1,
            this.tsbCancel,
            this.toolStripSeparator2,
            this.tsbClear,
            this.tsbAbout});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(856, 25);
            this.toolStripMenu.TabIndex = 0;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(23, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Enabled = false;
            this.tsbCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsbCancel.Image")));
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(63, 22);
            this.tsbCancel.Text = "Cancel";
            this.tsbCancel.ToolTipText = "Cancel the current request";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClear
            // 
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(54, 22);
            this.tsbClear.Text = "Clear";
            this.tsbClear.ToolTipText = "Clear the interface";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(44, 22);
            this.tsbAbout.Text = "About";
            this.tsbAbout.ToolTipText = "About";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // btnGetOrgs
            // 
            this.btnGetOrgs.Location = new System.Drawing.Point(6, 19);
            this.btnGetOrgs.Name = "btnGetOrgs";
            this.btnGetOrgs.Size = new System.Drawing.Size(108, 23);
            this.btnGetOrgs.TabIndex = 0;
            this.btnGetOrgs.Text = "Get Organizations";
            this.btnGetOrgs.UseVisualStyleBackColor = true;
            this.btnGetOrgs.Click += new System.EventHandler(this.btnGetOrgs_Click);
            // 
            // lstOrgs
            // 
            this.lstOrgs.FormattingEnabled = true;
            this.lstOrgs.Location = new System.Drawing.Point(6, 53);
            this.lstOrgs.Name = "lstOrgs";
            this.lstOrgs.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstOrgs.Size = new System.Drawing.Size(314, 173);
            this.lstOrgs.TabIndex = 1;
            // 
            // gtpSelectOrgs
            // 
            this.gtpSelectOrgs.Controls.Add(this.btnGetOrgs);
            this.gtpSelectOrgs.Controls.Add(this.lstOrgs);
            this.gtpSelectOrgs.Location = new System.Drawing.Point(13, 33);
            this.gtpSelectOrgs.Name = "gtpSelectOrgs";
            this.gtpSelectOrgs.Size = new System.Drawing.Size(325, 235);
            this.gtpSelectOrgs.TabIndex = 2;
            this.gtpSelectOrgs.TabStop = false;
            this.gtpSelectOrgs.Text = "1. Select Organizations";
            // 
            // grpSelectSolution
            // 
            this.grpSelectSolution.Controls.Add(this.lblSolution);
            this.grpSelectSolution.Controls.Add(this.btnSelectSolution);
            this.grpSelectSolution.Controls.Add(this.txtSolutionPath);
            this.grpSelectSolution.Controls.Add(this.label1);
            this.grpSelectSolution.Location = new System.Drawing.Point(13, 274);
            this.grpSelectSolution.Name = "grpSelectSolution";
            this.grpSelectSolution.Size = new System.Drawing.Size(325, 108);
            this.grpSelectSolution.TabIndex = 3;
            this.grpSelectSolution.TabStop = false;
            this.grpSelectSolution.Text = "2. Select Solution";
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(6, 79);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(106, 13);
            this.lblSolution.TabIndex = 3;
            this.lblSolution.Text = "Solution not selected";
            // 
            // btnSelectSolution
            // 
            this.btnSelectSolution.Location = new System.Drawing.Point(295, 30);
            this.btnSelectSolution.Name = "btnSelectSolution";
            this.btnSelectSolution.Size = new System.Drawing.Size(24, 23);
            this.btnSelectSolution.TabIndex = 2;
            this.btnSelectSolution.Text = "...";
            this.btnSelectSolution.UseVisualStyleBackColor = true;
            this.btnSelectSolution.Click += new System.EventHandler(this.btnSelectSolution_Click);
            // 
            // txtSolutionPath
            // 
            this.txtSolutionPath.Location = new System.Drawing.Point(9, 32);
            this.txtSolutionPath.Name = "txtSolutionPath";
            this.txtSolutionPath.Size = new System.Drawing.Size(280, 20);
            this.txtSolutionPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Solution File";
            // 
            // grpImportSolution
            // 
            this.grpImportSolution.Controls.Add(this.lblImportSolution);
            this.grpImportSolution.Controls.Add(this.btnImportSolution);
            this.grpImportSolution.Location = new System.Drawing.Point(13, 388);
            this.grpImportSolution.Name = "grpImportSolution";
            this.grpImportSolution.Size = new System.Drawing.Size(325, 86);
            this.grpImportSolution.TabIndex = 4;
            this.grpImportSolution.TabStop = false;
            this.grpImportSolution.Text = "3. Import Solution";
            // 
            // lblImportSolution
            // 
            this.lblImportSolution.AutoSize = true;
            this.lblImportSolution.Location = new System.Drawing.Point(6, 61);
            this.lblImportSolution.Name = "lblImportSolution";
            this.lblImportSolution.Size = new System.Drawing.Size(159, 13);
            this.lblImportSolution.TabIndex = 2;
            this.lblImportSolution.Text = "No Solution have been imported";
            // 
            // btnImportSolution
            // 
            this.btnImportSolution.Location = new System.Drawing.Point(9, 19);
            this.btnImportSolution.Name = "btnImportSolution";
            this.btnImportSolution.Size = new System.Drawing.Size(90, 23);
            this.btnImportSolution.TabIndex = 1;
            this.btnImportSolution.Text = "Import Solution";
            this.btnImportSolution.UseVisualStyleBackColor = true;
            this.btnImportSolution.Click += new System.EventHandler(this.btnImportSolution_Click);
            // 
            // grpImportStatus
            // 
            this.grpImportStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpImportStatus.Controls.Add(this.lstImportStatus);
            this.grpImportStatus.Location = new System.Drawing.Point(344, 33);
            this.grpImportStatus.Name = "grpImportStatus";
            this.grpImportStatus.Size = new System.Drawing.Size(499, 500);
            this.grpImportStatus.TabIndex = 5;
            this.grpImportStatus.TabStop = false;
            this.grpImportStatus.Text = "Import Status";
            // 
            // lstImportStatus
            // 
            this.lstImportStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstImportStatus.BackColor = System.Drawing.SystemColors.Window;
            this.lstImportStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstImportStatus.FormattingEnabled = true;
            this.lstImportStatus.Location = new System.Drawing.Point(6, 19);
            this.lstImportStatus.Name = "lstImportStatus";
            this.lstImportStatus.Size = new System.Drawing.Size(487, 459);
            this.lstImportStatus.TabIndex = 0;
            this.lstImportStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstImportStatus_DrawItem);
            // 
            // GTSolutionImporterControlPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpImportStatus);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.grpImportSolution);
            this.Controls.Add(this.grpSelectSolution);
            this.Controls.Add(this.gtpSelectOrgs);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GTSolutionImporterControlPlugin";
            this.Size = new System.Drawing.Size(856, 536);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.gtpSelectOrgs.ResumeLayout(false);
            this.grpSelectSolution.ResumeLayout(false);
            this.grpSelectSolution.PerformLayout();
            this.grpImportSolution.ResumeLayout(false);
            this.grpImportSolution.PerformLayout();
            this.grpImportStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Button btnGetOrgs;
        private System.Windows.Forms.ListBox lstOrgs;
        private System.Windows.Forms.GroupBox gtpSelectOrgs;
        private System.Windows.Forms.GroupBox grpSelectSolution;
        private System.Windows.Forms.Button btnSelectSolution;
        private System.Windows.Forms.TextBox txtSolutionPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.GroupBox grpImportSolution;
        private System.Windows.Forms.Button btnImportSolution;
        private System.Windows.Forms.Label lblImportSolution;

        // ToolStrip
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.GroupBox grpImportStatus;
        private System.Windows.Forms.ListBox lstImportStatus;
    }
}
