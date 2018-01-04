using System;
using System.Windows.Forms;
using CloudDeck.Sound;
using System.Threading;
using System.IO;
using MXP.Cloud;
using log4net.Config;
using CloudDeck.Asset;
using MXP.Util;
using CloudDeck.Model;
using CloudDeck.Controls;

namespace CloudDeck
{
    public static class DeckProgram
    {

        #region Application Component Fields
        
        public static SplashForm SplashForm;
        public static MainForm MainForm;
        public static LoginForm LoginForm;
        public static LoadingForm LoadingForm;
        public static HudForm HudForm;

        public static DeckRudder DeckRudder;
        public static DeckRenderer DeckRenderer;
        public static DeckEngine DeckEngine;
        public static DeckScene DeckScene;
        public static DeckSelection DeckSelection;
        public static DeckDaemon DeckDaemon;
        public static CloudView CloudView;
        public static AssetManager AssetManager;

        #endregion

        #region Application Main Method

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));

            BasicConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LogUtil.Info("Phase 0 - Startup begin.");
            
            SplashForm = new SplashForm();
            SplashForm.Visible = true;
            Application.DoEvents();

            DeckPlayer.Play("boom");

            AssetManager = new AssetManager(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/BubbleCloud/Cache");
            CloudView = new CloudView(100, DeckConstants.ProgramName, DeckConstants.ProgramMajorVersion, DeckConstants.ProgramMinorVersion);
            DeckEngine = new DeckEngine();
            DeckRudder = new DeckRudder();
            DeckRenderer = new DeckRenderer();

            DeckScene = new DeckScene();
            DeckSelection = new DeckSelection();
            DeckDaemon = new DeckDaemon();

            MainForm = new MainForm();
            HudForm = new HudForm();
            LoadingForm = new LoadingForm();
            LoginForm = new LoginForm();

            MainForm.AddOwnedForm(SplashForm);
            MainForm.AddOwnedForm(HudForm);
            MainForm.AddOwnedForm(LoadingForm);
            MainForm.AddOwnedForm(LoginForm);

            AssetManager.Startup();

            LogUtil.Info("Phase 0 - Startup end.");
            LogUtil.Info("Phase 1 - Initialization begin.");            
            
            MainLoop();

            LogUtil.Info("Shutdown begin.");
            while (CloudView.IsConnecting || CloudView.IsConnected)
            {
                CloudView.Disconnect();
                while (CloudView.IsConnected)
                {
                    CloudView.Process();
                    Thread.Sleep(10);
                }
            } 
            AssetManager.Shutdown();
            DeckRenderer.Shutdown();
            LogUtil.Info("Shutdown done.");

        }

        #endregion

        #region Application Custom Event Loop

        private static bool isShutdownRequested = false;

        private static void MainLoop()
        {
            MainForm.Closing += MainForm_Closing;
            MainForm.Visible = true;
            while(!isShutdownRequested)
            {
                MainProcess();
            }
        }

        private static void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isShutdownRequested=true;
        }

        #endregion

        #region Application Processing

        /// <summary>
        /// The main loop is called each time the application is idle.
        /// </summary>
        /// <remarks>
        /// That way the application is single threaded and therefore safe against deadlock and race conditions.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MainProcess()
        {
            try
            {
                long startTime = DateTime.Now.Ticks;
                long phaseStartTime = startTime;

                // PHASE LOGIC

                // PHASE 1 - Initialization phase. Show splash form.
                if (MainForm.Visible && SplashForm.Visible)
                {
                    // End phase
                    LogUtil.Info("Phase 1 - Initialization end.");
                    MainForm.Activate();
                    SplashForm.Hide();
                    DeckPlayer.Play("intro");
                }

                // PHASE 2 - Login phase. Show login form.
                if (MainForm.Visible && !CloudView.IsConnected && !CloudView.IsConnecting && !LoginForm.Visible)
                {
                    // Start phase
                    LogUtil.Info("Phase 2 - Login begin.");
                    LoginForm.Visible = true;
                    DeckRenderer.StartDemo();
                }
                if (MainForm.Visible && (CloudView.IsConnected || CloudView.IsConnecting) && LoginForm.Visible)
                {
                    // End phase
                    LogUtil.Info("Phase 2 - Login end.");
                    LoginForm.Visible = false;
                    MainForm.Activate();
                    DeckRenderer.StopDemo();
                }

                // PHASE 3 - Loading phase. Show loading form.
                if ((CloudView.IsConnecting || DeckEngine.IsSynchronizing) && !LoadingForm.Visible)
                {
                    // Start phase
                    LogUtil.Info("Phase 3 - Loading begin.");
                    LoadingForm.Visible = true;
                }
                if (!(CloudView.IsConnecting || DeckEngine.IsSynchronizing) && LoadingForm.Visible)
                {
                    // End phase
                    LogUtil.Info("Phase 3 - Loading end.");
                    LoadingForm.Visible = false;
                }

                // PHASE 4 - In world phase. Show hud form.
                if (CloudView.IsConnected && !DeckEngine.IsSynchronizing && !HudForm.Visible)
                {
                    // Start phase
                    LogUtil.Info("Phase 4 - In world begin.");
                    HudForm.Visible = true;
                    DeckProgram.MainForm.Focus();
                }
                if ((!CloudView.IsConnected || DeckEngine.IsSynchronizing) && HudForm.Visible)
                {
                    // End phase
                    LogUtil.Info("Phase 4 - In world end.");
                    HudForm.Visible = false;
                }

                long phaseLogicTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                // Processing
                DeckRenderer.Process();

                long deckRendererProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                // Processing
                MainForm.Process();

                long mainFormProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                DeckRudder.Process();

                long deckRudderProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                CloudView.Process();

                long cloudViewProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                AssetManager.Process();

                long assetManagerProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                // Engine needs processing calls only when connected.
                if (CloudView.IsConnected)
                {
                    DeckEngine.Process();
                }

                long deckSceneProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                if (CloudView.IsConnected)
                {
                    DeckScene.Process();
                }

                long deckEngineProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                // Loading form needs processing only when engine is synchronizing.
                if (DeckEngine.IsSynchronizing)
                {
                    LoadingForm.Process();
                }

                long loadingFormProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                Application.DoEvents();

                long doEventsProcessTime = DateTime.Now.Ticks - phaseStartTime;
                phaseStartTime = DateTime.Now.Ticks;

                long totalProcessTime = DateTime.Now.Ticks - startTime;

                /*LogUtil.Debug("phases: " + (phaseLogicTime/10000) +
                    " renderer: " + (deckRendererProcessTime/10000) +
                    " form: " + (mainFormProcessTime/10000) +
                    " rudder: " + (deckRudderProcessTime/10000)+
                    " view: " + (cloudViewProcessTime/10000)+
                    " assets: " + (assetManagerProcessTime/10000)+
                    " engine: " + (deckEngineProcessTime/10000)+
                    " scene: " + (deckSceneProcessTime/10000)+
                    " doevents: "+(doEventsProcessTime/10000)+
                    " total: " + (totalProcessTime / 10000)
                    );*/
            }
            catch (Exception ex)
            {
                LogUtil.Error("Error in CloudDeck ApplicationIdle loop: " + ex.ToString());
            }

        }

        #endregion

    }
}