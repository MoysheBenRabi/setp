using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Horde3DNET;
using System.Threading;
using MXP.Util;
using Horde3DNET.Utils;
using CloudDeck.Model;

namespace CloudDeck
{
    public partial class MainForm : Form
    {
        #region Native Methods

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        #endregion


        #region Fields

        private bool m_isFullScreen = false;
        private bool m_isMouseLook = false;

        private Point m_previousMouseMovement;

        private float m_framesPerSecond = 30.0f;
        private Stopwatch m_fpsStopWatch = new Stopwatch();
        private Int64 m_fpsFrames = 0;

        private Point m_normalLocation=new Point();
        private Size m_normalSize=new Size(100,100);

        #endregion

        #region Properties

        public bool IsKeyboardSteeringEnabled
        {
            get
            {
                return true;
            }
        }

        public bool IsMouseSteeringEnabled
        {
            get
            {
                return m_isMouseLook;
            }
        }

        public float FramesPerSecond
        {
            get
            {
                return m_framesPerSecond;
            }
        }

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            m_normalLocation = new Point((Screen.PrimaryScreen.Bounds.Width-Size.Width)/2,
                (Screen.PrimaryScreen.Bounds.Height - Size.Height) / 2);
            m_normalSize=Size;

            // Fullscreen
            if (m_isFullScreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                Left = 0;
                Top = 0;
                Width = Screen.PrimaryScreen.Bounds.Width;
                Height = Screen.PrimaryScreen.Bounds.Height;
            }

            Text = DeckConstants.ProgramName + " - Version " + DeckConstants.ProgramMajorVersion + "." +
                        DeckConstants.ProgramMinorVersion + " (" + DeckConstants.ProgramSourceRevision + ")";
            MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
        }

        #endregion

        #region Initialization

        /// <summary>
        /// All application initialization is performed here, before the main loop thread is executed and the render panel is displayed for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenderForm_Load(object sender, EventArgs e)
        {
            LogUtil.Info("MainForm startup begin.");
            try
            {

				//renderPanel.MakeCurrent();					
                if (!Horde3DUtils.initOpenGL(GetDC(renderPanel.Handle).ToInt32()))
                    throw new Exception("Failed to initialize OpenGL");

                if (!DeckProgram.DeckRenderer.Startup())
                {
                    MessageBox.Show(
                        "Failed to init application.\nMake sure you have an OpenGL 2.0 compatible graphics card with the latest drivers installed!\nAlso verify if the pipeline config file exists.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    h3d.release();
                    Environment.Exit(0);
                }

				LogUtil.Debug("Initial panel size: "+renderPanel.ClientSize.Width+","+renderPanel.ClientSize.Height);

				
    			//ResizeChildren();

            }
            catch (Exception ex)
            {
                LogUtil.Error("Error in MainForm initialization: " + ex.ToString());
            }
            LogUtil.Info("MainForm startup end.");
        }

        #endregion

        #region Processing

        /// <summary>
        /// The main loop. This method is executed by the application event handler each time the application is idle.
        /// </summary>
        public void Process()
        {
            ProcessMouseMovement();
            CalculateFramesPerSecond();

            //renderPanel.MakeCurrent();
            //renderPanel.SwapBuffers();

            Thread.Sleep(10);

            if (DeckProgram.MainForm.Visible&&!DeckProgram.LoginForm.Visible)
            {
                Horde3DNET.Utils.Horde3DUtils.swapBuffers();
            }
			
            Invalidate();
        }


        /// <summary>
        /// Calculates the mouse movement based on the difference between the old and new position (delta).
        /// </summary>
        private void ProcessMouseMovement()
        {
            if (m_isMouseLook)
            {
                Point delta = new Point(Cursor.Position.X - m_previousMouseMovement.X, Cursor.Position.Y - m_previousMouseMovement.Y);
                DeckProgram.DeckRudder.ProcessMouse(delta.X, delta.Y);
                ResetMousePosition();
            }
        }

        /// <summary>
        /// The mouse cursor position is reset to the center of the application window.
        /// </summary>
        private void ResetMousePosition()
        {
            // calculate the center of the form window
            Point pos = Location;
            pos.X += Size.Width / 2;
            pos.Y += Size.Height / 2;

            // reset cursor position
            Cursor.Position = pos;
            m_previousMouseMovement = pos;
        }


        /// <summary>
        /// Calculates the actual frames per second (fps).
        /// </summary>
        private void CalculateFramesPerSecond()
        {
            // start stopwatch if it isn't running, yet
            if (!m_fpsStopWatch.IsRunning)
                m_fpsStopWatch.Start();

            // calculate frames per second (fps)                    
            ++m_fpsFrames;
            if (m_fpsFrames >= 3)
            {
                // calculate fps by deviding the frame count through the elapsed seconds
                m_framesPerSecond = m_fpsFrames * 1000.0f / m_fpsStopWatch.Elapsed.Milliseconds;
                m_fpsFrames = 0;      // reset number of frames
                m_fpsStopWatch.Reset();  // prevent a number overflow
                m_fpsStopWatch.Start();  // restart stopwatch because reset stops the time interval measurement
            }
        }

        #endregion

        #region Forms Events

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyData == Keys.F1)
            {
                if (!m_isFullScreen)
                {
                    m_isFullScreen = !m_isFullScreen;
                    FormBorderStyle = FormBorderStyle.None;
                    Left = 0;
                    Top = 0;
                    m_normalSize=Size;
                    m_normalLocation = Location;

                    Width = Screen.PrimaryScreen.Bounds.Width;
                    Height = Screen.PrimaryScreen.Bounds.Height;
                }
                else
                {
                    m_isFullScreen = !m_isFullScreen;
                    FormBorderStyle = FormBorderStyle.Sizable;
                    Left = 20;
                    Top = 20;
                    Location = m_normalLocation;
                    Size = m_normalSize;
                }
            }
            DeckProgram.DeckRudder.KeyDown(e.KeyData);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            DeckProgram.DeckRudder.KeyUp(e.KeyData);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!m_isMouseLook)
                {
                    Cursor.Hide();
                    ResetMousePosition();
                }
                m_isMouseLook = true;
            }

        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_isMouseLook = false;
                ResetMousePosition();
                Cursor.Show();
            }
        }

        void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            DeckProgram.DeckRudder.MouseWheel(e.Delta);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // stop stop watch process
            m_fpsStopWatch.Stop();

            // release Horde3D
            //Horde3DUtils.releaseOpenGL();
            LogUtil.Info("Exiting.");
        }

        private void OnMove(object sender, EventArgs e)
        {
            ResizeChildren();
        }

        private void OnResize(object sender, EventArgs e)
        {
            ResizeChildren();
        }

        private void ResizeChildren()
        {
            DeckProgram.DeckRenderer.ResizeViewport(renderPanel.ClientSize.Width, renderPanel.ClientSize.Height);
            if (DeckProgram.HudForm != null)
            {
                if (m_isFullScreen)
                {
                    DeckProgram.HudForm.Location = new Point(this.DesktopBounds.Location.X,
                                                             this.DesktopBounds.Location.Y);
                    DeckProgram.LoadingForm.Location = new Point(this.DesktopBounds.Location.X,
                                                                 this.DesktopBounds.Location.Y);
                    DeckProgram.LoginForm.Location = new Point(this.DesktopBounds.Location.X,
                                                                 this.DesktopBounds.Location.Y);
                }
                else
                {
                    DeckProgram.HudForm.Location =
                        new Point(this.DesktopBounds.Location.X + SystemInformation.FrameBorderSize.Width,
                                  this.DesktopBounds.Location.Y + SystemInformation.CaptionHeight +
                                  SystemInformation.FrameBorderSize.Height);
                    DeckProgram.LoadingForm.Location =
                        new Point(this.DesktopBounds.Location.X + SystemInformation.FrameBorderSize.Width,
                                  this.DesktopBounds.Location.Y + SystemInformation.CaptionHeight +
                                  SystemInformation.FrameBorderSize.Height);
                    DeckProgram.LoginForm.Location = 
                        new Point(this.DesktopBounds.Location.X + SystemInformation.FrameBorderSize.Width,
                                  this.DesktopBounds.Location.Y + SystemInformation.CaptionHeight +
                                  SystemInformation.FrameBorderSize.Height);
                }

                DeckProgram.HudForm.Size = new Size(renderPanel.Width, renderPanel.Height);
                DeckProgram.LoadingForm.Size = new Size(renderPanel.Width, renderPanel.Height);
                DeckProgram.LoginForm.Size = new Size(renderPanel.Width, renderPanel.Height);
            }
        }

        #endregion

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ResizeChildren();
        }

        private void renderPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys!=Keys.Control)
                {
                    DeckProgram.DeckSelection.ClearSelection();
                }
                DeckObject deckObject = DeckProgram.DeckRenderer.GetPointedObject();

                if (deckObject != null)
                {
                    if (DeckProgram.DeckSelection.GetSelection().Contains(deckObject))
                    {
                        DeckProgram.DeckSelection.DeselectObject(deckObject);
                    }
                    else
                    {
                        DeckProgram.DeckSelection.FocusObject(deckObject);
                    }
                }
            }
        }

    }
}