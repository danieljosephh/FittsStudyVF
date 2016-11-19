/**
 * FittsStudy
 *
 *		Jacob O. Wobbrock, Ph.D.
 * 		The Information School
 *		University of Washington
 *		Mary Gates Hall, Box 352840
 *		Seattle, WA 98195-2840
 *		wobbrock@uw.edu
 *		
 * This software is distributed under the "New BSD License" agreement:
 * 
 * Copyright (c) 2007-2011, Jacob O. Wobbrock. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *    * Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *    * Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *    * Neither the name of the University of Washington nor the names of its 
 *      contributors may be used to endorse or promote products derived from 
 *      this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
 * IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Jacob O. Wobbrock
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
**/
namespace FittsStudy
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAnalyze = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mniViewModel = new System.Windows.Forms.ToolStripMenuItem();
            this.mniGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniMouse = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.staSettings = new System.Windows.Forms.StatusStrip();
            this.lblSubject = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLayout = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBlock = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCondition = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTrial = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblA = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblW = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblID = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMTPct = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMTPred = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMT = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_a = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_b = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnsMain.SuspendLayout();
            this.staSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsMain
            // 
            this.mnsMain.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.mnsMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuTools,
            this.mnuHelp,
            this.mnuClose});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Size = new System.Drawing.Size(1149, 27);
            this.mnsMain.TabIndex = 0;
            this.mnsMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mnsMain_MouseClick);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mnsMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAnalyze,
            this.mniViewLog,
            this.mniViewModel,
            this.mniGraph,
            this.mniSeparator0,
            this.mniExit});
            this.mnuFile.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(40, 23);
            this.mnuFile.Text = "&File";
            this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mnuFile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniAnalyze
            // 
            this.mniAnalyze.Name = "mniAnalyze";
            this.mniAnalyze.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mniAnalyze.Size = new System.Drawing.Size(226, 22);
            this.mniAnalyze.Text = "&Analyze Logs...";
            this.mniAnalyze.Click += new System.EventHandler(this.mniAnalyze_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniAnalyze.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniViewLog
            // 
            this.mniViewLog.Name = "mniViewLog";
            this.mniViewLog.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniViewLog.Size = new System.Drawing.Size(226, 22);
            this.mniViewLog.Text = "View &Log...";
            this.mniViewLog.Click += new System.EventHandler(this.mniViewLog_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniViewLog.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniViewModel
            // 
            this.mniViewModel.Name = "mniViewModel";
            this.mniViewModel.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mniViewModel.Size = new System.Drawing.Size(226, 22);
            this.mniViewModel.Text = "View &Model...";
            this.mniViewModel.Click += new System.EventHandler(this.mniViewModel_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniViewModel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniGraph
            // 
            this.mniGraph.Name = "mniGraph";
            this.mniGraph.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.mniGraph.Size = new System.Drawing.Size(226, 22);
            this.mniGraph.Text = "&Graph Trials...";
            this.mniGraph.Click += new System.EventHandler(this.mniGraph_Click);
            // 
            // mniSeparator0
            // 
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.Size = new System.Drawing.Size(223, 6);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniSeparator0.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniExit
            // 
            this.mniExit.Name = "mniExit";
            this.mniExit.Size = new System.Drawing.Size(226, 22);
            this.mniExit.Text = "E&xit";
            this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniExit.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNew,
            this.mniStop,
            this.mniSeparator1,
            this.mniMouse});
            this.mnuTools.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(53, 23);
            this.mnuTools.Text = "&Tools";
            this.mnuTools.DropDownOpening += new System.EventHandler(this.mnuTools_DropDownOpening);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mnuTools.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniNew
            // 
            this.mniNew.Name = "mniNew";
            this.mniNew.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mniNew.Size = new System.Drawing.Size(210, 22);
            this.mniNew.Text = "&New Test...";
            this.mniNew.Click += new System.EventHandler(this.mniNew_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniNew.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniStop
            // 
            this.mniStop.Name = "mniStop";
            this.mniStop.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mniStop.Size = new System.Drawing.Size(210, 22);
            this.mniStop.Text = "&Stop Test";
            this.mniStop.Click += new System.EventHandler(this.mniStop_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniStop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniSeparator1
            // 
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.Size = new System.Drawing.Size(207, 6);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniSeparator1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniMouse
            // 
            this.mniMouse.Name = "mniMouse";
            this.mniMouse.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mniMouse.Size = new System.Drawing.Size(210, 22);
            this.mniMouse.Text = "&Mouse Panel";
            this.mniMouse.Click += new System.EventHandler(this.mniMouse_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniMouse.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(48, 23);
            this.mnuHelp.Text = "&Help";
            this.mnuHelp.DropDownOpening += new System.EventHandler(this.mnuHelp_DropDownOpening);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mnuHelp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mniAbout
            // 
            this.mniAbout.Name = "mniAbout";
            this.mniAbout.Size = new System.Drawing.Size(186, 22);
            this.mniAbout.Text = "&About FittsStudy";
            this.mniAbout.Click += new System.EventHandler(this.mniAbout_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mniAbout.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // mnuClose
            // 
            this.mnuClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuClose.Image = ((System.Drawing.Image) (resources.GetObject("mnuClose.Image")));
            this.mnuClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.ShowShortcutKeys = false;
            this.mnuClose.Size = new System.Drawing.Size(31, 23);
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // this next line of code makes the foot mouse work at the top and bottom.
            this.mnuClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // staSettings
            // 
            this.staSettings.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.staSettings.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.staSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSubject,
            this.lblLayout,
            this.lblBlock,
            this.lblCondition,
            this.lblTrial,
            this.lblA,
            this.lblW,
            this.lblID,
            this.lblMTPct,
            this.lblMTPred,
            this.lblMT,
            this.lbl_a,
            this.lbl_b,
            this.lblVersion});
            this.staSettings.Location = new System.Drawing.Point(0, 378);
            this.staSettings.Name = "staSettings";
            this.staSettings.Size = new System.Drawing.Size(1149, 22);
            this.staSettings.SizingGrip = false;
            this.staSettings.TabIndex = 1;
            // this next line of code makes the foot mouse work at the top and bottom.
            this.staSettings.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);

            // 
            // lblSubject
            // 
            this.lblSubject.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSubject.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(74, 17);
            this.lblSubject.Text = "Subject: 0";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblSubject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblLayout
            // 
            this.lblLayout.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblLayout.Name = "lblLayout";
            this.lblLayout.Size = new System.Drawing.Size(70, 17);
            this.lblLayout.Text = "Layout: 0";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblLayout.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblBlock
            // 
            this.lblBlock.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(58, 17);
            this.lblBlock.Text = "Block: 0";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblBlock.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblCondition
            // 
            this.lblCondition.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCondition.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(85, 17);
            this.lblCondition.Text = "Condition: 0";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblCondition.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblTrial
            // 
            this.lblTrial.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTrial.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblTrial.Name = "lblTrial";
            this.lblTrial.Size = new System.Drawing.Size(52, 17);
            this.lblTrial.Text = "Trial: 0";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblTrial.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblA
            // 
            this.lblA.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblA.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(54, 17);
            this.lblA.Text = "A: 0 px";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblA.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblW
            // 
            this.lblW.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblW.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblW.Name = "lblW";
            this.lblW.Size = new System.Drawing.Size(57, 17);
            this.lblW.Text = "W: 0 px";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblW.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblID
            // 
            this.lblID.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblID.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(67, 17);
            this.lblID.Text = "ID: 0 bits";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblID.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblMTPct
            // 
            this.lblMTPct.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblMTPct.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblMTPct.Name = "lblMTPct";
            this.lblMTPct.Size = new System.Drawing.Size(57, 17);
            this.lblMTPct.Text = "MT%: 0";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblMTPct.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblMTPred
            // 
            this.lblMTPred.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblMTPred.Name = "lblMTPred";
            this.lblMTPred.Size = new System.Drawing.Size(95, 17);
            this.lblMTPred.Text = "MTPred: 0 ms";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblMTPred.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblMT
            // 
            this.lblMT.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblMT.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lblMT.Name = "lblMT";
            this.lblMT.Size = new System.Drawing.Size(65, 17);
            this.lblMT.Text = "MT: 0 ms";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblMT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lbl_a
            // 
            this.lbl_a.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_a.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lbl_a.Name = "lbl_a";
            this.lbl_a.Size = new System.Drawing.Size(55, 17);
            this.lbl_a.Text = "a: 0 ms";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lbl_a.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lbl_b
            // 
            this.lbl_b.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_b.Margin = new System.Windows.Forms.Padding(0, 3, 20, 2);
            this.lbl_b.Name = "lbl_b";
            this.lbl_b.Size = new System.Drawing.Size(80, 17);
            this.lbl_b.Text = "b: 0 ms/bit";
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lbl_b.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = false;
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(5, 17);
            this.lblVersion.Spring = true;
            this.lblVersion.Text = "ver";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // this next line of code makes the foot mouse work at the top and bottom.
            this.lblVersion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1149, 400);
            this.ControlBox = false;
            this.Controls.Add(this.staSettings);
            this.Controls.Add(this.mnsMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnsMain;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Fitts Study";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.staSettings.ResumeLayout(false);
            this.staSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.StatusStrip staSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripStatusLabel lblTrial;
        private System.Windows.Forms.ToolStripStatusLabel lblSubject;
        private System.Windows.Forms.ToolStripStatusLabel lblCondition;
        private System.Windows.Forms.ToolStripStatusLabel lblA;
        private System.Windows.Forms.ToolStripStatusLabel lblW;
        private System.Windows.Forms.ToolStripStatusLabel lblMTPct;
        private System.Windows.Forms.ToolStripStatusLabel lblMT;
        private System.Windows.Forms.ToolStripStatusLabel lblID;
        private System.Windows.Forms.ToolStripStatusLabel lbl_a;
        private System.Windows.Forms.ToolStripStatusLabel lbl_b;
        private System.Windows.Forms.ToolStripMenuItem mniAnalyze;
        private System.Windows.Forms.ToolStripMenuItem mniExit;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
        private System.Windows.Forms.ToolStripMenuItem mniNew;
        private System.Windows.Forms.ToolStripMenuItem mniStop;
        private System.Windows.Forms.ToolStripStatusLabel lblMTPred;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mniAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStripStatusLabel lblBlock;
        private System.Windows.Forms.ToolStripStatusLabel lblVersion;
        private System.Windows.Forms.ToolStripStatusLabel lblLayout;
        private System.Windows.Forms.ToolStripMenuItem mniGraph;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniMouse;
        private System.Windows.Forms.ToolStripMenuItem mniViewLog;
        private System.Windows.Forms.ToolStripMenuItem mniViewModel;
    }
}

