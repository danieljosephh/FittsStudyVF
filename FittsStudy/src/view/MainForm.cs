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
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.IO;
using System.IO.Ports;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using WobbrockLib;
using WobbrockLib.Extensions;
using WobbrockLib.Types;



namespace FittsStudy
{
    /// <summary>
    /// The main form that presents the UI and owns the data for the FittsStudy.exe application.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region Constants

        private const int MnomeAnimationTime = 30; // millisecond updates for border animation 
        private const double MinDblClickDist = 4.0; // minimum distance two clicks must be apart (filters double-clicks)
        private const int WaitBeforeClickMs = 2500; // milliseconds to wait before first click in metronome

        
        /*************** EDIT START ***************/

        // Optitrack data to cursor mapping: (to be set manually)

        // **remember that min's > max's (just flip the f'n signs)
        private const float minX = -70.0f;  // what optitrack sends at furthest WEST point
        private const float maxX = -525.0f;  // what optitrack sends at furthest EAST point
        private const float minZ = 325.0f;  // what optitrack sends at furthest NORTH point
        private const float maxZ = 190.0f;  // what optitrack sends at furthest SOUTH point
        private float slopeX, slopeZ, bX, bZ;       // constants to define mapping from OptiTrack to Windows Forms

        private int actuationTimeThresh;           // time threshold for motor actuation (units: ms)
        private int actuationDistanceThreshOuter;       // outer distance threshold for motor actuation (units: pixels)
        private int actuationDistanceThreshInner;       // inner distance threshold for motor actuation (units: pixels)

        /*************** EDIT END ****************/

        #endregion

      
        #region Fields

        OptionsForm.Options _o; // the options we obtain from the options dialog 

        private long _tLastMnomeTick; // timestamp of last metronome tick in milliseconds
        private long _tMnomeInterval; // duration between metronome ticks in milliseconds

        private SessionData _sdata; // the whole session (one test); holds conditions in order
        private ConditionData _cdata; // the current condition; retrieved from the session
        private TrialData _tdata; // the current trial; retrieved from the condition

        private byte[] _sndMnome; // metronome tick sound
        private uint _timerID; // timer identifier

        private string _fileNoExt; // full path and filename without extension
        private XmlTextWriter _writer; // XML writer -- uses _fileNoExt.xml



        /********** EDIT START ****************/
        bool useShoe = true;       // boolean defining whether the shoe or the conventional mouse will be used.
        //bool useShoe = false;       // boolean defining whether the shoe or the conventional mouse will be used.


        // haptic feedback
        SoundPlayer simpleSound = new SoundPlayer(@"C:\\Users\\horod_000\\Music\\100_hz_sawtooth_wave.wav");
        bool isVibrating = false;

        // variable friction
        SerialPort sp = new SerialPort("COM5", 2000000);        // define the serial port (2 Mbps #easy)
        
        // Optitrack motion tracking
        OptiTrackManager OptiObject = new OptiTrackManager();   // make an OptiTrack Manager


        private long lastTime;                              // Timestamp of the last time "MouseMove" was called (units: ms)    
        private double velocity, xVel, yVel;                // for actuation timing analysis
        
        private PointF[] distracterCentres = new PointF[4]; // gets the distracter coordinates for each trial
        private double[] distance = new double[4];          // this array holds the 4 distances to each distracter (in the 1D case, they will ALWAYS be constant offsets of eachother)
                                                            // the order of the distracters in the array is as follows: 0-far side, 1-target, 2-near side, 3-farthest

        private double[] directionalDerivative = new double[4];     // describes whether cursor is travelling TOWARDS or AWAY from the distracter or target
        private double[] oldDistance = new double[4];               // previous frame's distance from the cursor to the distracter or target
        private int _minIndex = 3;                              // index of the distracter / target closest to the mouse cursor (default to 3, because this value is only changed if the "Distracters" checkbox is checked.
        private bool isActuated;                            // describes whether the motor is extended or not
        private bool mouseOn = false;                       // a boolean switch that allows the painter to be called (it appears that if the Cursor.Pos is always being given an input the painter won't be called)

        private float[] xVelocityBuffer, yVelocityBuffer;       // holds the current and previous 15 x/y-coordinates
        List<long> theTimes;                                    // holds the actuation times
        List<string> csvFile;                                   // holds the experiment data

        // actuation time measures
        long actuateTime = 0;       // time between motor actuation and target entry
        bool isPrinted = false;     // boolean for actuateTime

        
        /********** EDIT END ****************/

        #endregion

        #region Constructor, Load, and Closing

        /// <summary>
        /// Constructs the main form for this application. The main form is a full-screen window in which
        /// the Fitts' law study trials take place.
        /// </summary>
        public MainForm()
        {

            // Consider this analogous to the Start() function of Unity.

            /*************** EDIT START ****************/

            if (useShoe)
            {
                // Optitrack
                OptiObject.Start();

                // Arduino
                sp.Open();
                sp.ReadTimeout = 1;
                sp.Write("0");        // resets the Arduino, not fully necessary, but a good precaution.
                Console.WriteLine("Successfully connected to Arduino!");
            }

            isActuated = false;
            csvFile = new List<string>();   // make a new list for: A | W | MT | IDe | TPavg | err%
            theTimes = new List<long>();    // initialize, just in case of variable-friction


            // Optitrack data to cursor mapping done here:
            // get the screen dimensions
            //System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.Bounds;

            slopeX = (workingRectangle.Size.Width) / (maxX - minX);
            slopeZ = (workingRectangle.Size.Height) / (maxZ - minZ);

            bX = -minX * slopeX;
            bZ = -minZ * slopeZ;

            xVelocityBuffer = new float[16];
            yVelocityBuffer = new float[16];

            /*************** EDIT END ****************/

            InitializeComponent();
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.UserPaint | 
                ControlStyles.UserMouse |
                ControlStyles.StandardClick, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            
            _tLastMnomeTick = -1L;
            _tMnomeInterval = -1L;

            _o = new OptionsForm.Options(); // defaults

        }

        /// <summary>
        /// The form load event is used to load *.WAV file sounds from disk into memory for playback during trials.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3); // major.minor.build.revision
            _sndMnome = SoundWave.LoadSoundResource(typeof(MainForm), "rsc.tick.wav");
        }

        /// <summary>
        /// Handler for when the form is about to close. If there is an active condition, the closing is
        /// canceled. If not, the form is allowed to close.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (_sdata != null); // don't close the app if there is an active session
            if (e.Cancel)
            {
                MessageBox.Show(this, "A test session is underway. First stop the test, then exit the application.", "Test Underway", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                /************* EDIT START *****************/
                if (useShoe)
                    sp.Write("4");  // moves the motor back home (slight overshoot but fuck it, 2 weeks)
                /************* EDIT END *****************/

                Win32.KillTimer(this.Handle, _timerID);
            }
        }

        #endregion

        #region Paint Handler

        /// <summary>
        /// The main form's paint event handler. It paints the current target on the screen.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            if (_sdata == null) // no session is yet underway
            {
                string msg = "Type Ctrl+N to begin a new test session.";
                SizeF sz = e.Graphics.MeasureString(msg, mnsMain.Font);
                e.Graphics.DrawString(msg, mnsMain.Font, Brushes.Black, Width / 2f - sz.Width / 2f, Height / 2f);
            }
            else // a session is underway
            {
                // Draw all of the targets in the condition light gray.
                foreach (Region rgn in _cdata.TargetRegions)
                {
                    e.Graphics.FillRegion(Brushes.LightGray, rgn);
                    rgn.Dispose();
                }


                /************** EDIT START **************/

                // Draw all of the DISTRACTER regions in the condition black.
                if(_o.isDistracters)
                {
                    foreach (Region rgn in _tdata.DistracterRegion)
                    {
                    
                        if (_tdata.Number > 1)  // for some reason, need to use a different graphics object to continuously re-paint
                        {
                            Graphics g = Graphics.FromHwnd(this.Handle);
                            g.FillRegion(Brushes.Black, rgn);
                            g.Dispose();
                        }
                        else if (_tdata.Number > 0) // don't need to paint distracters for the first trial...
                        {
                            e.Graphics.FillRegion(Brushes.Black, rgn);
                        }

                        //else // works for trials 0 and 1
                        //{
                        //    e.Graphics.FillRegion(Brushes.Black, rgn);
                        //}

                        rgn.Dispose();
                    }
                }
                    

                /************** EDIT END ************/

                
                // Draw the current target blue.
                Region trgn = _tdata.TargetRegion;
                e.Graphics.FillRegion(Brushes.DeepSkyBlue, trgn);
                trgn.Dispose();

                // Draw the animated fill for metronome trials.
                if (_cdata.MT != -1.0)
                {
                    Region[] rgns = _tdata.GetAnimatedRegions(TimeEx.NowMs - _tLastMnomeTick);
                    foreach (Region rgn in rgns)
                    {
                        if (_tdata.IsStartAreaTrial && TimeEx.NowMs - _cdata.TAppeared < WaitBeforeClickMs)
                            e.Graphics.FillRegion(Brushes.LightGray, rgn);
                        else
                            e.Graphics.FillRegion(Brushes.Blue, rgn);
                        rgn.Dispose();
                    }
                }

                // Draw the start label beside the first target to indicate the start of each condition.
                if (_tdata.IsStartAreaTrial)
                {   
                    SizeF sz = e.Graphics.MeasureString("Start Here >", mnsMain.Font);
                    RectangleF r = _tdata.TargetBounds;
                    e.Graphics.FillRectangle(Brushes.White, r.X - sz.Width, r.Y + r.Height / 2 - sz.Height / 2, sz.Width, sz.Height);
                    e.Graphics.DrawRectangle(Pens.Black, r.X - sz.Width, r.Y + r.Height / 2 - sz.Height / 2, sz.Width, sz.Height);
                    e.Graphics.DrawString("Start Here", mnsMain.Font, Brushes.Black, r.X - sz.Width, r.Y + r.Height / 2 - sz.Height / 2 + 1);
                    Font wdings = new Font("Wingdings", mnsMain.Font.Size + 2f);
                    SizeF szw = e.Graphics.MeasureString("à", wdings); // == (char) 224
                    e.Graphics.DrawString("à", wdings, Brushes.Black, r.X - szw.Width + 2, r.Y + r.Height / 2 - sz.Height / 2 + 1);
                    wdings.Dispose();
                }


                /************* EDIT START *****************/

                mouseOn = true;     // keep this for mouse debugging, I think it's necessary

                if (useShoe)
                    Cursor.Position = Point.Round(new PointF((OptiObject.getPosition(0).X * 1000.0f * slopeX + bX), (OptiObject.getPosition(0).Y * 1000.0f * slopeZ + bZ)));

                /************* EDIT END *****************/

            }
        }

        #endregion

        #region File Menu

        /// <summary>
        /// The handler for just before the File menu opens. It is used to enable and disable 
        /// menu items.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuFile_DropDownOpening(object sender, EventArgs e)
        {
            mniViewLog.Enabled = (_sdata == null);
            mniAnalyze.Enabled = (_sdata == null);
            mniExit.Enabled = (_sdata == null);
        }

        /// <summary>
        /// Handler for the File > Exit menu item. Closes the form.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handler for the File > View Log menu item. This allows the user to view a
        /// FittsStudy log within the application itself in both Web and plain text
        /// formats.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniViewLog_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                LogForm frm = new LogForm();
                frm.Filename = ofd.FileName;
                frm.Show(this);
            }

            ofd.Dispose();
        }

        /// <summary>
        /// Menu handler for the View Model menu item. Opens a log file and reads it in, and
        /// then opens the Model form and displays the proper model for the study.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniViewModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string msg;
                SessionData fs = new SessionData();
                XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileName));
                if (fs.ReadFromXml(reader))
                {
                    ModelForm mform = new ModelForm(fs, Path.GetFileNameWithoutExtension(ofd.FileName) + ".model.txt");
                    mform.ShowDialog(this);
                }
                else
                {
                    msg = String.Format("Unable to read data from {0}.", Path.GetFileName(ofd.FileName));
                    MessageBox.Show(this, msg, "Invalid Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ofd.Dispose();
        }

        /// <summary>
        /// Menu handler for the Analyze menu item. Opens and parses an XML log file written
        /// previously by the FittsStudy application. Then it builds up its internal data
        /// structures and writes them to a comma-separated TXT file that can be pasted into
        /// a spreadsheet (e.g., Excel or JMP).
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniAnalyze_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log(s)";
            ofd.Multiselect = true;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string msg;
                int successes = 0;
                for (int i = 0; i < ofd.FileNames.Length; i++)
                {
                    SessionData fs = new SessionData();
                    XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileNames[i]));
                    if (fs.ReadFromXml(reader))
                    {
                        string fileNoExt = Path.GetFileNameWithoutExtension(ofd.FileNames[i]);
                        if (fs.WriteResultsToTxt(new StreamWriter(fileNoExt + ".txt", false, Encoding.UTF8)))
                        {
                            successes++;
                        }
                        else
                        {
                            msg = String.Format("Unable to write results for {0}.", Path.GetFileName(ofd.FileNames[i]));
                            MessageBox.Show(this, msg, "Unwritable Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        msg = String.Format("Unable to read data from {0}.", Path.GetFileName(ofd.FileNames[i]));
                        MessageBox.Show(this, msg, "Invalid Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                msg = String.Format("{0} of {1} log(s) parsed and analyzed successfully,\r\nand written to the same directory as the log file(s).", successes, ofd.FileNames.Length);
                MessageBox.Show(this, msg, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ofd.Dispose();
        }

        /// <summary>
        /// Menu handler to open a log file and graph all the trials that occurred in that log. This
        /// includes a graphical depiction of each trial, and distance, velocity, acceleration, and 
        /// jerk submovement profiles.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniGraph_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string msg;
                SessionData fs = new SessionData();
                XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileName));
                if (fs.ReadFromXml(reader))
                {
                    GraphForm gform = new GraphForm(fs);
                    gform.ShowDialog(this);
                }
                else
                {
                    msg = String.Format("Unable to read data from {0}.", Path.GetFileName(ofd.FileName));
                    MessageBox.Show(this, msg, "Invalid Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ofd.Dispose();
        }

        #endregion

        #region Tools Menu

        /// <summary>
        /// The handler for just before the Tools menu opens. It is used to enable
        /// and disable menu items.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuTools_DropDownOpening(object sender, EventArgs e)
        {
            mniNew.Enabled = (_sdata == null);
            mniStop.Enabled = (_sdata != null);
        }

        /// <summary>
        /// Menu item handler that starts a new Fitts study session. This handler opens the experiment
        /// options dialog and parameterizes the test with the information obtained.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>A new Fitts' session <i>_fs</i> is begun in this handler. The first condition from that 
        /// session (index 0) is obtained. The XML log file for the session is also created.</remarks>
        private void mniNew_Click(object sender, EventArgs e)
        {
            OptionsForm dlg = new OptionsForm(_o);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _o = dlg.Settings; // update our options with those from the dialog

                // create the session instance. from that, we will get the first condition and its trial.
                _sdata = new SessionData(_o);
                _cdata = _sdata[0, 0]; // first overall condition
                _tdata = _cdata[0]; // first trial is special start-area trial at index 0

                // create the XML log for output and write the log header
                _fileNoExt = String.Format("{0}\\{1}__{2}", _o.Directory, _sdata.FilenameBase, Environment.TickCount);
                _writer = new XmlTextWriter(_fileNoExt + ".xml", Encoding.UTF8);
                _writer.Formatting = Formatting.Indented;
                _sdata.WriteXmlHeader(_writer);

                // initialize the test state
                if (_cdata.MT != -1.0) // for metronome studies
                {
                    _tLastMnomeTick = TimeEx.NowMs; // ms
                    _cdata.TAppeared = TimeEx.NowMs; // now
                    _tMnomeInterval = _cdata.MT; // interval in ms for 1 metronome 'tick'
                    _timerID = Win32.SetTimer(this.Handle, 1u, MnomeAnimationTime, IntPtr.Zero);
                }
                UpdateStatusBar(); // update the status bar at the bottom of the test screen
                Invalidate(); // redraw to show the initial targets
            }
        }

        /// <summary>
        /// Menu item handler that stops a Fitts study session in progress. This menu item should not
        /// be used in a normal session, but may be used to abort a session underway and close the
        /// existing XML log file. No TXT analysis file will be written.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>This handler assumes that a Fitts condition is underway. The current trial is not
        /// added to the current condition before the test is stopped.</remarks>
        private void mniStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This will stop the session, truncating the log at this point. The log may need adjusting by hand before parsing.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Win32.KillTimer(this.Handle, _timerID);
                _cdata.WriteXmlHeader(_writer); // write out the current condition
                _sdata.WriteXmlFooter(_writer); // write the end of the XML log and closes the log
                ResetState();
                UpdateStatusBar();
                Invalidate();
            }
        }

        /// <summary>
        /// Menu item handler to open the mouse control panel, for convenience.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniMouse_Click(object sender, EventArgs e)
        {
            ControlPanel.OpenMouseControlPanel(2);
        }

        #endregion

        #region Help Menu

        /// <summary>
        /// The handler for just before the Help menu opens. It is used to enable
        /// and disable menu items.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuHelp_DropDownOpening(object sender, EventArgs e)
        {
            mniAbout.Enabled = (_sdata == null);
        }

        /// <summary>
        /// Displays the About box for this application.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniAbout_Click(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();
            dlg.ShowDialog(this);
        }

        #endregion

        #region Close Box "Menu"

        /// <summary>
        /// No titlebar or menu items are shown for the main form window so that the
        /// entire form is maximized client space. But we have a menu that shows the
        /// close box image and can be clicked as a button.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Add a handler to clicking on the main menu bar that closes the form when the 
        /// close box is just barely missed in the top-right corner of the form. A "real"
        /// close box on a Microsoft window has this functionality already, but we have
        /// to provide it ourselves because of our full-screen style where the close box
        /// is actually a menu.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnsMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (mnuClose.Bounds.Left <= e.X && e.Y <= mnuClose.Bounds.Bottom && !mnuClose.Bounds.Contains(e.X, e.Y))
            {
                Close();
            }
        }

        #endregion

        #region Keyboard, Mouse, and WndProc

        /// <summary>
        /// When 'Esc' is pressed (ASCII 27), the current condition is reset and begun again.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27) // 'Esc'
            {
                if (_cdata != null) // condition in progress
                {
                    Win32.KillTimer(this.Handle, _timerID);
                    _cdata.ClearTrialData(); // clear the performance data for the trials in this condition
                    _tdata = _cdata[0]; // get the special start-area trial
                    if (_cdata.MT != -1.0) // metronome trials
                    {
                        _tLastMnomeTick = TimeEx.NowMs; // ms
                        _cdata.TAppeared = TimeEx.NowMs; // now
                        _timerID = Win32.SetTimer(this.Handle, 1u, MnomeAnimationTime, IntPtr.Zero);
                    }
                    UpdateStatusBar();
                    Invalidate();
                }

            }

            
            // **ONLY NECESSARY FOR OPEN-LOOP CONTROL

            /************ EDIT START ***************/
            // send the brake pad home, reset the Arduino
            /*
            if (e.KeyChar == 114) // 'r'
            {
                sp.Write("4");
                sp.Write("0");
            }
             * /
            /************ EDIT END ***************/

        }

        /// <summary>
        /// When the mouse goes down, if there is an active condition, it represents the
        /// end of one trial and the beginning of the next. A time-stamped click-point is
        /// passed to the function that ends the previous trial and begins the next one.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>
        /// Double-clicks are filtered by ensuring that the distance between two clicks
        /// is greater than a minimum.
        /// </remarks>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (_cdata != null)
            {
                if (_cdata.MT == -1.0 || TimeEx.NowMs - _cdata.TAppeared > WaitBeforeClickMs)
                {
                    TimePointF pt = new TimePointF(e.X, e.Y, TimeEx.NowMs);
                    if (_tdata.IsStartAreaTrial) // first click to begin condition
                    {
                        NextTrial(pt);
                    }
                    else // 2nd+ click ends trial, starts next
                    {
                        if (GeotrigEx.Distance(_tdata.Start, pt) > MinDblClickDist) // filter double clicks
                        {
                            /************* EDIT START ***************/
                            if(_o.isVariableFriction)
                                actuateMotor(true);     // actuates the motor based on a mouse click (i.e., the motor retracts the brake pads)
                            /************* EDIT END ***************/
                            NextTrial(pt);
                        }
                    }
                }
                else DoError(new PointF(e.X, e.Y)); // clicking before start is enabled
            }
        }

        /// <summary>
        /// When there is an active trial and the mouse is moved, the movements are added
        /// to the trial for later logging. The movements are time-stamped points.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>
        /// Mouse movements matter only when there is a current trial. This is not the same as
        /// when there is a current condition, since the first click to start a the first trial
        /// in a condition exists after an active condition but before the first active trial.
        /// We don't want to log mouse movements before that first trial is begun with that
        /// initial click.
        /// </remarks>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            /***************** EDIT START *********************/

            // Immediately below is code to obtain the frame index, just assume fs = 100;
            //Console.WriteLine(OptiObject.getPosition(0).Time.ToString());
            
            // This executes once a trial is set
            if ((_tdata != null) && mouseOn)
            {
                
                if (useShoe)    // shoe control
                    Cursor.Position = new Point((int)(Math.Round((OptiObject.getPosition(0).X * 1000f * slopeX),0) + bX), (int)(Math.Round((OptiObject.getPosition(0).Y * 1000f * slopeZ),0) + bZ));
                
                xVelocityBuffer[0] = Cursor.Position.X;
                yVelocityBuffer[0] = Cursor.Position.Y;
                
            }
            
            // This executes once a trial has began
            if (_tdata != null && !_tdata.IsStartAreaTrial) // means a trial is in progress
            {

                // vibrates the shoe over targets depedent on the options form condition
                vibrateShoe();

                // this code prints the time difference (in ms) between the point 
                // at which the motor is actuated and the point the cursor reaches the target
                if(!isPrinted && isActuated)
                {
                    if (_tdata.TargetContains(Cursor.Position) || (_tdata.DistracterContains(Cursor.Position) > -1))    // if -1 is returned, it means that no distracter has the cursor inside
                    {
                        theTimes.Add(TimeEx.NowMs - actuateTime);                   // length of time between actuation start and target entry
                        isPrinted = true;
                    }
                    //else if(_tdata.TargetContains(Cursor.Position)) // tells if the mouse cursor is in the target.
                    //{
                    //    //theTimes.Add(-2);
                    //    theTimes.Add(TimeEx.NowMs - actuateTime);
                    //    isPrinted = true;
                    //}

                }
                
                // if VF is engaged, check velocity and actuate motor if necessary
                if(_o.isVariableFriction)
                {
                    velocityFilter();   // shifts the velocity buffer if there's been too much idling (as the function is only called when the mouse moves). I don't even know if this is necessary.
                                        // _minIndex is calculated at the end of this function


                    isOvershoot();      // retracts the motor if there's overshoot OR if you're off track
                    actuateMotor(false);    // this is the main actuation code, a FALSE argument means that it's extending  
                }

                // DON'T TOUCH THIS
                _tdata.Movement.AddMove(new TimePointF(e.X, e.Y, TimeEx.NowMs));    // only record moves when we are within a trial
                // KEEP IT INSIDE IT'S ORIGINAL 'IF' STATEMENT (THIS ONE)
                // THIS IS THE ONLY PIECE OF ORIGINAL CODE IN THIS FUNCTION
            }

            

            lastTime = TimeEx.NowMs;
            
            /************** EDIT END *********************/

        }


        /// <summary>   THIS FUNCTION IS WRITTEN BY YOU
        /// This function gives the velocity of the mouse and its distance relative to each distracter. 
        /// It filters the velocity based on the past 15 mocap frames, method courtesy of Pavel Holobrodko.
        /// </summary>
        private void velocityFilter()
        {
            // THERE NEEDS TO BE A CHECK HERE, EVEN IF THERE'S MOVEMENT THE FUNCTION ISN'T ALWAYS CALLED TOO FREQUENTLY
            // MAYBE USE THE FRAME INDEX

            // if it's been more than 10 ms, shift the buffer
            if( ((TimeEx.NowMs - lastTime) >= 10) )
            {
                // shifts the buffer if there hasn't been mouse movement in awhile to reflect the "true" velocity (hopefully)

                // determine how long it's been to know how much to shift the buffer
                int factor = (int)Math.Round(((TimeEx.NowMs - lastTime) / 10.0f));
                if (factor > 16) factor = 16;

                // copy+paste to create the tail of the new buffer
                Buffer.BlockCopy(xVelocityBuffer, 0, xVelocityBuffer, 4 * factor, 4 * (16 - factor));       // Buffer.BlockCopy(src, srcOffset, dest, destOffset, totalDataSize);
                Buffer.BlockCopy(yVelocityBuffer, 0, yVelocityBuffer, 4 * factor, 4 * (16 - factor));

                // populate the rest with the SECOND value in the buffer
                // if the function has not been called in a long time, the first value in the buffer is the NEW position, not the one it has been idle at
                for (int i = 1; i < factor; i++)
                {
                    xVelocityBuffer[i] = xVelocityBuffer[1];
                    yVelocityBuffer[i] = yVelocityBuffer[1];
                }

            }


            // 2D trial
            if (_tdata.Circular)
            {
                //Stopwatch s1 = new Stopwatch();       // performance testing

                // distance to target centre (target width accounted for in actuateMotor())
                // distance[3] = GeotrigEx.Distance(new TimePointF(_tdata.TargetCenter.X, _tdata.TargetCenter.Y, 0), new TimePointF(shoePos.X, shoePos.Y, 0));

                if(_o.isDistracters)
                {
                    for (int i = 0; i < distracterCentres.Length; i++)
                    {
                        distance[i] = GeotrigEx.Distance(new TimePointF(distracterCentres[i].X, distracterCentres[i].Y, 0), new TimePointF(Cursor.Position.X, Cursor.Position.Y, 0));

                        directionalDerivative[i] = distance[i] - oldDistance[i];
                        oldDistance[i] = distance[i];
                    }
                }

                

                distance[3] = GeotrigEx.Distance(new TimePointF(_tdata.TargetCenter.X, _tdata.TargetCenter.Y, 0), new TimePointF(Cursor.Position.X, Cursor.Position.Y, 0));
                directionalDerivative[3] = distance[3] - oldDistance[3];
                oldDistance[3] = distance[3];

                //Console.WriteLine("GeoTrig: " + distance.ToString() + "\t getDx: " + _tdata.GetDx(_tdata.Circular).ToString());

                xVel = (1 / (16384 * 10.0f)) * (xVelocityBuffer[0] + 13 * xVelocityBuffer[1] + 77 * xVelocityBuffer[2] + 273 * xVelocityBuffer[3] + 637 * xVelocityBuffer[4] + 1001 * xVelocityBuffer[5] + 1001 * xVelocityBuffer[6] + 429 * xVelocityBuffer[7] - 429 * xVelocityBuffer[8] - 1001 * xVelocityBuffer[9] - 1001 * xVelocityBuffer[10] - 637 * xVelocityBuffer[11] - 273 * xVelocityBuffer[12] - 77 * xVelocityBuffer[13] - 13 * xVelocityBuffer[14] - xVelocityBuffer[15]);
                yVel = (1 / (16384 * 10.0f)) * (yVelocityBuffer[0] + 13 * yVelocityBuffer[1] + 77 * yVelocityBuffer[2] + 273 * yVelocityBuffer[3] + 637 * yVelocityBuffer[4] + 1001 * yVelocityBuffer[5] + 1001 * yVelocityBuffer[6] + 429 * yVelocityBuffer[7] - 429 * yVelocityBuffer[8] - 1001 * yVelocityBuffer[9] - 1001 * yVelocityBuffer[10] - 637 * yVelocityBuffer[11] - 273 * yVelocityBuffer[12] - 77 * yVelocityBuffer[13] - 13 * yVelocityBuffer[14] - yVelocityBuffer[15]);

                velocity = Math.Sqrt(Math.Pow(xVel, 2) + Math.Pow(yVel, 2));

            }
            // 1D trial (only care about x-direction (horizontal) movement)
            else
            {
                // for 1D distracters, the array index is indicative of the order in which it will

                // this code right below only needs to be ran once per trial, so...
                
                // distracterCentres = _tdata.DistracterCenters;

                if(_o.isDistracters)
                {
                    for (int i = 0; i < distracterCentres.Length; i++)
                    {
                        distance[i] = Math.Abs(distracterCentres[i].X - Cursor.Position.X);

                        directionalDerivative[i] = distance[i] - oldDistance[i];
                        oldDistance[i] = distance[i];
                    }
                }

                distance[3] = Math.Abs(_tdata.TargetCenter.X - Cursor.Position.X);
                directionalDerivative[3] = distance[3] - oldDistance[3];
                oldDistance[3] = distance[3];


                velocity = Math.Abs((1 / (16384 * 10.0f)) * (xVelocityBuffer[0] + 13 * xVelocityBuffer[1] + 77 * xVelocityBuffer[2] + 273 * xVelocityBuffer[3] + 637 * xVelocityBuffer[4] + 1001 * xVelocityBuffer[5] + 1001 * xVelocityBuffer[6] + 429 * xVelocityBuffer[7] - 429 * xVelocityBuffer[8] - 1001 * xVelocityBuffer[9] - 1001 * xVelocityBuffer[10] - 637 * xVelocityBuffer[11] - 273 * xVelocityBuffer[12] - 77 * xVelocityBuffer[13] - 13 * xVelocityBuffer[14] - xVelocityBuffer[15]));

                //Console.WriteLine("Dfar: " + distance[0].ToString() + "\tD1: " + distance[1].ToString() + "\tD2: " + distance[2].ToString() + "\tDtarg: " + distance[3].ToString() + "\tvel: " + velocity.ToString());
            }

            // directional derivative, for overshoots and other mishaps
            // if this value is POSITIVE then we have an overshoot/off track mouse cursor.
            // directionalDerivative tells us whether we're moving TOWARDS or AWAY from the target.
            //directionalDerivative = distance[3] - oldDistance;
            //oldDistance = distance[3];
            if(_o.isDistracters)
                _minIndex = Array.IndexOf(distance, distance.Min());

            //Console.WriteLine("Closest target: " + _minIndex.ToString() + "\tDistance: " + distance[_minIndex].ToString());

            // shift the buffers down one byte
            Buffer.BlockCopy(xVelocityBuffer, 0, xVelocityBuffer, 4, 60);
            Buffer.BlockCopy(yVelocityBuffer, 0, yVelocityBuffer, 4, 60);

            return;
        }


        /// <summary>   THIS FUNCTION IS WRITTEN BY YOU
        /// 
        /// This is used to determine if there is overshoot or the user has gone sufficiently offtrack to
        /// warrant a brake pad retraction.
        /// 
        /// With distracters, this becomes slightly more complicated...
        /// 
        /// </summary>
        private void isOvershoot()
        {
            // IF the motor has been actuated.
            if (isActuated)
            {
                // We want to check its position/velocity relative to the distracters and target and then determine if we wish to retract
                // Currently, the "distance" variable is checked, this value is the distance to the target
                // Calculate 4 distances (to each distracter and the target).
                // Actuate based on these values. (Don't do anything fancy, just find the four distances for now.)
                // Literally the exact same thing you have now, but you will have 4 targets, not just 1.
                // For the 1D case, the distances will be constant offsets.
                // Run a loop checking all cases, if a retraction condition comes up, break the loop.
                // You will need to implement something similar for the extension code.


                // This 'if' statement originally said: if you have exited the inner distance threshold and are travelling away from the closest distracter/target --> retract


                // Now, this 'if' statement says:
                // IF the distance to the nearest target/distracter is greater than half the width of the target/distracter and you are travelling away from the distracter/target, RETRACT

                if ((((float)distance[_minIndex]) > ((_tdata.TargetBounds.Width / 2.0f)+5)) && (directionalDerivative[_minIndex] > 0))     // if the distance is greater than (target_width) / 2 + the offset
                {
                    actuateMotor(true);     // true means that the motor is being retracted due to overshoot (true is also sent when a click occurs)
                }



                // ORIGINAL CODE
                //if ((distance[i] > ((_tdata.TargetBounds.Width / 2.0f) + actuationDistanceThreshInner)) && (directionalDerivative > 0))     // if the distance is greater than (target_width) / 2 + the offset
                //{
                //    actuateMotor(true);     // true means that the motor is being retracted due to overshoot NOT click
                //    Console.WriteLine("RETRACT");
                //    break;
                //}
                    
            }

            return;
                
        }


        /// <summary>   THIS FUNCTION IS WRITTEN BY YOU
        /// Actuates the motor if the conditions are right.
        /// 
        /// "isFromClickOrOvershoot" tells us if the user clicked or overshot, which means that the pads must be retracted
        /// 
        /// NEED TO IMPLEMENT OVERSHOOT METHODS.
        /// </summary>
        public void actuateMotor(bool isFromClickOrOvershoot)
        {
            
                        
            // EXTEND
            if (!isActuated && !isFromClickOrOvershoot)
            {
                // Essentially this nested 'if' statement says:
                // "If you're moving at an appreciable velocity and you're close enough to the target, actuate."
                // OR
                // "If you're super close, we gon' rip anyways."

                if ((distance[_minIndex] < actuationDistanceThreshOuter) && (directionalDerivative[_minIndex] < 0))
                {
                    if (((distance[_minIndex] / velocity) <= actuationTimeThresh))// || ((distance - (_tdata.TargetBounds.Width / 2.0f)) < actuationDistanceThreshInner))
                    {
                        // actuation timestamp goes here
                        if (useShoe)
                            sp.Write("3");

                        actuateTime = TimeEx.NowMs;
                        isActuated = true;
                        theTimes.Add(99999 + _minIndex);    // speed + _minIndex
                        //Console.WriteLine("EXTEND - speed thresh\tTarget: " + _minIndex.ToString());
                        //break;
                    }
                    //else if ((distance[_minIndex] - (_tdata.TargetBounds.Width / 2.0f)) < actuationDistanceThreshInner) // used with original thresholds
                    else if ((distance[_minIndex]) < actuationDistanceThreshInner)
                    {
                        if (useShoe)
                            sp.Write("3");

                        actuateTime = TimeEx.NowMs;
                        isActuated = true;
                        theTimes.Add(55555 + _minIndex);    // distance + _minIndex
                        //Console.WriteLine("EXTEND - distance thresh\tTarget: " + _minIndex.ToString());
                        //break;
                    }

                }

            }
            // RETRACT
            else if (isActuated && isFromClickOrOvershoot)
            {
                if (useShoe)
                    sp.Write("2");


                // if 'isPrinted' is false
                if (!isPrinted)
                    theTimes.Add(0);
                
                //theTimes.Add(44444);                        // tells us it's a retraction
                theTimes.Add(TimeEx.NowMs - actuateTime);   // tells the time since actuating

                isActuated = false;
                //Console.WriteLine("RETRACT - isFromClickOrOvershoot");
                //break;

                // EDIT (want to print all actuation times now)
                isPrinted = false;
            }


        }

        // OLD CODE
            
            //// EXTEND
            //if (!isActuated && !isFromClickOrOvershoot)
            //{
            //    // Essentially this nested 'if' statement says:
            //    // "If you're moving at an appreciable velocity and you're close enough to the target, actuate."
            //    // OR
            //    // "If you're super close, we gon' rip anyways."

            //    if ((distance < actuationDistanceThreshOuter) && (directionalDerivative[i] < 0))
            //    {
            //        if (((distance / velocity) <= actuationTimeThresh))// || ((distance - (_tdata.TargetBounds.Width / 2.0f)) < actuationDistanceThreshInner))
            //        {
            //            // actuation timestamp goes here
            //            if (useShoe)
            //                sp.Write("3");


            //            actuateTime = TimeEx.NowMs;
            //            isActuated = true;
            //            theTimes.Add(99999);    // speed
            //            Console.WriteLine("EXTEND - speed thresh\tTarget: " + i.ToString());
            //            break;
            //        }
            //        else if ((distance - (_tdata.TargetBounds.Width / 2.0f)) < actuationDistanceThreshInner)
            //        {
            //            if (useShoe)
            //                sp.Write("3");

            //            actuateTime = TimeEx.NowMs;
            //            isActuated = true;
            //            theTimes.Add(55555);    // distance
            //            Console.WriteLine("EXTEND - distance thresh\tTarget: " + i.ToString());
            //            break;
            //        }

            //    }

            //}
            //// RETRACT
            //else if (isActuated && isFromClickOrOvershoot)
            //{
            //    if (useShoe)
            //    {
            //        sp.Write("2");
            //    }

            //    isActuated = false;
            //    Console.WriteLine("RETRACT - isFromClickOrOvershoot");
            //    break;
            //}




        /// <summary>   THIS FUNCTION IS WRITTEN BY YOU
        /// 
        /// This function is intended to set the time and distance thresholds for motor extension actuation
        /// based on the target size (and possibly velocity history, later).
        /// Function argument: target width
        /// 
        /// Essentially the thresholds are modified by looking at the time elapsed between actuation and target entry.
        /// The condition which initiated the actuation (velocity or distance) is noted as well.
        /// 
        /// THRESHOLDS MAY NEED TO BE MODIFIED BASED ON AMPLITUDE AS WELL.
        /// 
        /// </summary>
        private void setThresh()
        {
            if(_tdata.Circular)     // 2D case      DON'T FORGET TO CHANGE PREVIOUS VALUES BEFORE YOU MODIFY CURRENT ONES
            {
                //  ORIGINAL
                switch ((int)_tdata.TargetBounds.Width)
                {
                    case 20:
                        actuationDistanceThreshOuter = 95;  // prev: 120
                        actuationDistanceThreshInner = 13;  // prev: 10
                        actuationTimeThresh = 375;          // prev: 350
                        break;

                    case 60:
                        actuationDistanceThreshOuter = 110; // prev: 120
                        actuationDistanceThreshInner = 31;  // prev: 20
                        actuationTimeThresh = 400;          // prev: 375
                        break;

                    case 128:
                        actuationDistanceThreshOuter = 160; // prev: 200
                        actuationDistanceThreshInner = 62;  // prev: 55
                        actuationTimeThresh = 420;          // prev: 380
                        break;

                    default:
                        break;
                }
                
                // for 60

                //  DISTRACTER TEST
                //actuationDistanceThreshOuter = (int)(3 * _tdata.W); // prev: 200
                //actuationDistanceThreshInner = (int)(1.5 * _tdata.W);  // prev: 55

            }
            else   // 1D case                       DON'T FORGET TO CHANGE PREVIOUS VALUES BEFORE YOU MODIFY CURRENT ONES
            {

                // ORIGINAL
                switch ((int)_tdata.TargetBounds.Width)
                {
                    case 20:
                        actuationDistanceThreshOuter = 150; // prev: 150
                        actuationDistanceThreshInner = 26;  // prev: 32
                        actuationTimeThresh = 380;          // prev: 350
                        break;

                    case 60:
                        actuationDistanceThreshOuter = 200; // prev: 300
                        actuationDistanceThreshInner = 70;  // prev: 70
                        actuationTimeThresh = 450;          // prev: 400
                        break;

                    case 128:

                        if (_tdata.A == 300)
                        {
                            actuationDistanceThreshOuter = 172;
                        }
                        else
                        {
                            actuationDistanceThreshOuter = 300; // prev: 300
                        }

                        actuationDistanceThreshInner = 90;  // prev: 90
                        actuationTimeThresh = 450;          // prev: 400

                        break;

                    default:
                        break;
                }

                // for 60

                // DISTRACTER TEST
                //actuationDistanceThreshOuter = (int)(3 * _tdata.W); // prev: 200
                //actuationDistanceThreshInner = (int)(2 * _tdata.W);  // prev: 55

            }
            
            return;
        }




        private void vibrateShoe()
        {
            if (!_o.isHapticFeedback)
                return;


            if (_tdata.TargetContains(Cursor.Position) && !isVibrating)
            {
                simpleSound.Play();
                isVibrating = true;
            }
            else if (!_tdata.TargetContains(Cursor.Position) && isVibrating)
            {
                simpleSound.Stop();
                isVibrating = false;
            }

        }



        /// <summary>
        /// When using a metronome, the metronome is portrayed graphically around the targets. This
        /// graphical representation animates according to this repeating animation timer. When this 
        /// timer function fires, the animations are invalidated, causing Paint to trigger. The actual 
        /// animzaiton dimensions are determined in Paint based on the amount of time that has elapsed 
        /// since the last metronome tick. That values is set here when the required amount of time has passed.
        /// </summary>
        /// <param name="m">The Win32 MSG structure provided to this event dispatch.</param>
        protected override void WndProc(ref Message m)
        {
            // call the base window dispatch
            base.WndProc(ref m);

            // handle the timer message from our animation timer
            if (m.Msg == (int) Win32.WM.TIMER)
            {
                if (TimeEx.NowMs - _tLastMnomeTick >= _tMnomeInterval)
                {
                    // time interval has passed
                    _tLastMnomeTick = TimeEx.NowMs; // update
                    SoundWave.PlaySound(_sndMnome);
                }
                // invalidate the current target so that the metronome animation is portrayed.
                RectangleF r0 = _tdata.TargetBounds;
                Invalidate(GeotrigEx.RectangleF2Rectangle(r0));
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// This is the main function that begins a new trial. The beginning of a new trial may
        /// mark the beginning of a new condition, a new trial in a condition already underway,
        /// or the last trial of the current condition, which may lead to a new condition or to the 
        /// end of the entire test session.
        /// </summary>
        /// <param name="click">The time-stamped mouse click that marks the transition from the 
        /// last trial (if there is one) to the next trial.</param>
        private void NextTrial(TimePointF click)
        {
            if (_tdata.IsStartAreaTrial) // click was to begin the first actual trial in the current condition
            {
                
                if (!_tdata.TargetContains(click)) // click missed start target
                {
                    DoError(click);
                }
                else // start first actual trial
                {
                    _tdata = _cdata[1]; // trial number 1
                    _tdata.Start = click;
                }

                /************* EDIT START ***************/
                // runs on the first trial of every new condition

                if (_o.isVariableFriction)
                {
                    // set distance thresholds
                    if (useShoe)
                        sp.Write("1");      // sets the first extension, you should find a better spot for this...

                    setThresh();
                    theTimes.Add((long)(_tdata.TargetBounds.Width * 10000));
                    distracterCentres = _tdata.DistracterCenters;
                }

                /************* EDIT END ***************/

                Invalidate(); // paint whole form because of START label 
            }
            else if (_tdata.Number < _cdata.NumTrials) // go to next trial in condition
            {
                _tdata.End = click;
                _tdata.NormalizeTimes();
                if (_tdata.IsError)
                    DoError(click);

                /************** EDIT START ******************/
                // This paints the old distracters white
                if (_o.isDistracters)
                {
                    foreach (Region rgn in _tdata.DistracterRegion)
                    {
                        Graphics g = Graphics.FromHwnd(this.Handle);
                        g.FillRegion(Brushes.White, rgn);
                        g.Dispose();
                    }
                }
                /************** EDIT END ******************/

                RectangleF r0 = _tdata.TargetBounds;
                Invalidate(GeotrigEx.RectangleF2Rectangle(r0));
                _tdata = _cdata[_tdata.Number + 1];
                _tdata.Start = click;
                RectangleF r1 = _tdata.TargetBounds;
                Invalidate(GeotrigEx.RectangleF2Rectangle(r1));

                /************** EDIT START ******************/
                if(_o.isDistracters)
                    distracterCentres = _tdata.DistracterCenters;
                /************** EDIT END ******************/
            }
            else // end the condition and go to the next, or end the session if done
            {
                Win32.KillTimer(this.Handle, _timerID);
                _tdata.End = click; // time point end of previous trial
                _tdata.NormalizeTimes();
                if (_tdata.IsError)
                    DoError(click);


                /********** EDIT START **********/

                // stop the vibration, set the variable false...
                if (_o.isHapticFeedback)
                {
                    simpleSound.Stop();
                    isVibrating = false;
                }
                
                // runs at the end of a condition

                // pick the type of outliers to exclude
                // should probably be both spatial and temporal (see: "Towards a standard for pointing device evaluation: ..." Soukoreff and MacKenzie (2004) 
                ExcludeOutliersType ex = ExcludeOutliersType.Both;

                csvFile.Add(_cdata.A.ToString() + "," + _cdata.W.ToString() + "," + _cdata.GetMTe(ex).ToString() + "," + _cdata.GetIDe(_tdata.Circular).ToString() + "," + _cdata.GetTP(_tdata.Circular).ToString() + "," + _cdata.GetErrorRate(ex).ToString());
                
                /********** EDIT END **********/

                
                string rtf = String.Format("{{\\rtf1 {{\\colortbl;\\red215\\green0\\blue0;}} {{\\par\\par\\qc\\b\\cf0 {0} of {1} conditions in block {2} complete.  Errors on test trials: {3:f1}%. {{\\cf1  Cumulative: {4:f1}%.}} \\par}}}}", _cdata.Index + 1, _sdata.NumConditionsPerBlock, _cdata.Block, _cdata.GetErrorRate(ExcludeOutliersType.None) * 100.0, _sdata.GetErrorRate(ExcludeOutliersType.None) * 100.0);
                if (MessageBanner.ShowDialog(this, rtf, true) == DialogResult.OK)
                {
                    _cdata.WriteXmlHeader(_writer); // write out the condition and its trials to XML
                    if (_cdata.Index + 1 == _sdata.NumConditionsPerBlock) // we have run all conditions in this block
                    {
                        if (_cdata.Block + 1 == _sdata.NumBlocks) // we have run all blocks
                        {
                            _sdata.WriteXmlFooter(_writer); // writes the end of the XML log and closes the log
                            MessageBox.Show(this, "All conditions are done. Session complete!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ModelForm m = new ModelForm(_sdata, _fileNoExt + ".model.txt"); // show the modal model :)
                            m.ShowDialog(this);
                            

                            //  THIS IS WHERE DATA IS SAVED. WITHOUT THIS, YOU ARE WORTHLESS.

                            /************* EDIT START ***************/
                            // write path variable for the end of whole shebang
                            string writePath = _o.Directory + _sdata.Subject.ToString() + ".csv";

                            // Write the data to file
                            // separate data for: A | W | MT | IDe | TPavg | %err
                            File.WriteAllLines(writePath, csvFile.ToArray());
                            
                            
                            // prints actuation times at the end of the dealio
                            if(_o.isVariableFriction)
                            {
                                // Reset the List and the 'writePath' variable
                                csvFile = new List<string>();
                                writePath = _o.Directory + "\\ActuationTimes\\" + _sdata.Subject.ToString() + ".csv";

                                long[] theTimesArray = new long[theTimes.Count];
                                string[] theTimesStringArray = new string[theTimes.Count];
                                theTimesArray = theTimes.ToArray();

                                for (int i = 0; i < theTimes.Count; i++)
                                {
                                    // LEAVE THIS TO BE HANDLED BY MATLAB

                                    //if(theTimesArray[i] == 99999)
                                    //    theTimesStringArray[i] = "SPEED";
                                    //else if (theTimesArray[i] == 55555)
                                    //    theTimesStringArray[i] = "DISTANCE";
                                    //else if (theTimesArray[i] == 44444)
                                    //    theTimesStringArray[i] = "RETRACT";
                                    //else
                                    theTimesStringArray[i] = theTimesArray[i].ToString();


                                    File.WriteAllLines(writePath, theTimesStringArray);

                                    for (int j = 0; j < theTimes.Count; j++)
                                        Console.WriteLine(theTimesStringArray[j]);


                                    // in case we wanna giv'er again
                                    theTimes = new List<long>();

                                    // send the motor home
                                    if (useShoe)
                                        sp.Write("4");

                                }


                                /******* DATA ANALYSIS EDIT *********/

                                //List<TimePointF> test = new List<TimePointF>();
                                //TimePointF[] theTimesArray = new long[theTimes.Count];
                                //string[] theTimesStringArray = new string[theTimes.Count];
                                //theTimesArray = theTimes.ToArray();
                                //test.Add(new TimePointF(50, 90, 100));
                                //test.Add(new TimePointF(60, 100, 110));
                                //test.ToArray();

                                /******* DATA ANALYSIS EDIT END *********/

                                _minIndex = 3;  
                                
                            }
                            /************* EDIT END ***************/

                            ResetState();

                        }
                        else // still more blocks to run
                        {
                            MessageBox.Show(this, "Starting a new block of all conditions.", "Block Break", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _cdata = _sdata[_cdata.Block + 1, 0]; // first condition in next block
                            _tdata = _cdata[0]; // special start-area trial at index 0
                        }
                    }
                    else // still more conditions in this block to be run
                    {
                        _cdata = _sdata[_cdata.Block, _cdata.Index + 1]; // next condition in same block
                        _tdata = _cdata[0]; // special start-area trial at index 0
                    }
                }
                else // 'Esc' was pressed on the message banner -- redo the condition
                {
                    _cdata.ClearTrialData(); // clear the data from the set of trials
                    _tdata = _cdata[0]; // get special start-area trial at index 0
                }
                if (_cdata != null && _cdata.MT != -1.0) // start metronome if applicable
                {
                    _tLastMnomeTick = TimeEx.NowMs; // ms
                    _cdata.TAppeared = TimeEx.NowMs; // now
                    _tMnomeInterval = _cdata.MT;
                    _timerID = Win32.SetTimer(this.Handle, 1u, MnomeAnimationTime, IntPtr.Zero);
                }
                Invalidate(); // redraw all for end of a condition or session
            }

            /************* EDIT START ***************/
            isPrinted = false;
            mouseOn = false;
            /************* EDIT END ***************/

            // update the status bar for the new trial
            UpdateStatusBar();

        }

        /// <summary>
        /// This method notifies the user of errors through a system beep and drawing a red "X"
        /// around the click point.
        /// </summary>
        /// <param name="click">The click point.</param>
        /// <param name="ms">The number of milliseconds to show the red "X".</param>
        /// <remarks>This is the only place that drawing is done outside of the Paint handler.</remarks>
        private void DoError(PointF click)
        {
            SystemSounds.Beep.Play();
            Graphics g = Graphics.FromHwnd(this.Handle);
            Region rgn = _tdata.TargetRegion;
            g.FillRegion(Brushes.Red, rgn);
            rgn.Dispose();
            g.Dispose();
            Thread.Sleep(MnomeAnimationTime);
        }

        /// <summary>
        /// Resets the current test data and state for the Fitts study. Does not involve clearing
        /// any UI rendering, widgets, file names, or logging.
        /// </summary>
        private void ResetState()
        {
            Win32.KillTimer(this.Handle, _timerID);
            _tLastMnomeTick = -1L;
            _tMnomeInterval = -1L;

            _sdata = null;
            _cdata = null;
            _tdata = null;

            if (_writer != null && _writer.BaseStream != null)
            {
                _writer.Close();
                _writer = null;
            }
        }

        /// <summary>
        /// Update the status bar at the bottom of the main form after each trial. All of the fields
        /// are updated to the current values if there is an active condition. If not, then the
        /// fields are set to zeroes.
        /// </summary>
        private void UpdateStatusBar()
        {
            if (_sdata != null)
            {
                lblSubject.Text = String.Format("Subject: {0}", _sdata.Subject);
                lblLayout.Text = String.Format("Layout: {0}D", _sdata.Is2D ? 2 : 1);
                lblBlock.Text = String.Format("Block: {0}", _cdata.Block);
                lblCondition.Text = String.Format("Condition: {0}", _cdata.Index);
                lblTrial.Text = String.Format("Trial: {0}", _tdata.Number);
                lblA.Text = String.Format("A: {0} px", _cdata.A);
                lblW.Text = String.Format("W: {0} px", _cdata.W);
                lblID.Text = String.Format("ID: {0:f2} bits", _cdata.ID);
                lblMTPct.Text = String.Format("MT%: {0:f2}", _cdata.MTPct);
                lblMTPred.Text = String.Format("MTPred: {0} ms", _cdata.MTPred);
                lblMT.Text = String.Format("MT: {0} ms", _cdata.MT);
                lbl_a.Text = String.Format("a: {0} ms", _sdata.Intercept);
                lbl_b.Text = String.Format("b: {0} ms/bit", _sdata.Slope);
            }
            else
            {
                lblSubject.Text = "Subject: 0";
                lblLayout.Text = "Layout: 0";
                lblBlock.Text = "Block: 0";
                lblCondition.Text = "Condition: 0";
                lblTrial.Text = "Trial: 0";
                lblA.Text = "A: 0 px";
                lblW.Text = "W: 0 px";
                lblID.Text = "ID: 0 bits";
                lblMTPct.Text = "MT%: 0.00";
                lblMTPred.Text = "MTPred: 0 ms";
                lblMT.Text = "MT: 0 ms";
                lbl_a.Text = "a: 0 ms";
                lbl_b.Text = "b: 0 ms/bit";
            }
        }

        #endregion

        
    }
}