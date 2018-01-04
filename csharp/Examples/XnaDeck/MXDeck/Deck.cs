#region File Description
//-----------------------------------------------------------------------------
// SkinningSample.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Web;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;
using MXDeck.Engine;
using MXP;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using ButtonState=Microsoft.Xna.Framework.Input.ButtonState;
using Keys=Microsoft.Xna.Framework.Input.Keys;

#endregion

namespace MXDeck
{
    /// <summary>
    /// Sample game showing how to display skinned character animation.
    /// </summary>
    public class Deck : Microsoft.Xna.Framework.Game
    {
        #region Fields

        DeckEngine engine;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        KeyboardState currentKeyboardState = new KeyboardState();
        GamePadState currentGamePadState = new GamePadState();
        MouseState currentMouseState = new MouseState();

        Model characterModel;        
        Model sphereModel;
        Model bubbleModel;
        Model discModel;
        
        Matrix[] Transforms;

        AnimationPlayer animationPlayer;

        float cameraArc = -10.0f;
        float cameraRotation = 180;
        float initialCameraDistance = 250;
        float cameraDistance = 250;

        int lastMouseX=0;
        int lastMouseY=0;


        float lightRotation = 0;
        Vector3 lightDirection = new Vector3(0, 1, 0);
        Color lightColor = new Color(255, 255, 255);
        Color ambientLightColor = new Color(50, 50, 50);

        public Guid AvatarId = Guid.NewGuid();
        public String AvatarName = "Unnamed";
        public String AvatarTypeName = "Human";
        public Guid AvatarTypeId = Guid.NewGuid();
        public Vector3 AvatarLocation=new Vector3(0,0,0);
        public Quaternion AvatarOrientation=Quaternion.CreateFromAxisAngle(new Vector3(0,1,0),0);
        public float AvatarBoundingSphereRadius = 1.5f;

        public DeckObject ReceivedAvatarObject = null;

        public bool avatarControlled = false;
        public bool lastAvatarControlled = true;
        
        Vector3 forwardDirection = new Vector3(0, 0, 1);
        Vector3 rightDirection = new Vector3(1, 0, 0);

        Dictionary<Keys, DateTime> keypressTimes = new Dictionary<Keys, DateTime>();
        StringBuilder textInputBuffer=new StringBuilder();
        bool keyboardTextInputMode = false;
        string letterKeys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public List<string> ConsoleLines = new List<string>();

        #endregion

        #region Initialization


        public Deck(string serverAddress, int serverPort, Guid bubbleId, string location, string identityProviderUrl, string participantName, string participantPassphrase)
        {
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(Window.Handle);
            AssemblyName assemblyName = this.GetType().Assembly.GetName();
            this.Window.Title = "MX Deck " + assemblyName.Version+ " - Interactive Bubble Bouncer Demo";
            this.Window.AllowUserResizing = false;
            this.IsMouseVisible = true;

            engine = new DeckEngine(
                serverAddress,
                serverPort,
                bubbleId,
                location,
                identityProviderUrl,
                participantName,
                participantPassphrase,this);

            AvatarName = participantName;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.SynchronizeWithVerticalRetrace = true;
            //graphics.PreferMultiSampling = true;
            //graphics.IsFullScreen = true;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);


           
            //graphics.PreferredBackBufferWidth = 320;
            //graphics.PreferredBackBufferHeight = 240;
            graphics.PreferredBackBufferWidth = screen.Bounds.Width*5/6;
            graphics.PreferredBackBufferHeight = screen.Bounds.Height*5/6;

            //graphics.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;
            //graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            int quality = 0;
            GraphicsAdapter adapter = e.GraphicsDeviceInformation.Adapter;
            SurfaceFormat format = adapter.CurrentDisplayMode.Format;

            // Check for 16xAA
            if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format, false, MultiSampleType.SixteenSamples, out quality))
            {
                // even if a greater quality is returned, we only want quality 0
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleQuality = 0;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType =
                    MultiSampleType.SixteenSamples;
            }
            // Check for 8xAA
            else if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format, false, MultiSampleType.EightSamples, out quality))
            {
                // even if a greater quality is returned, we only want quality 0
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleQuality = 0;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType =
                    MultiSampleType.EightSamples;
            }
            // Check for 4xAA
            else if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format, false, MultiSampleType.FourSamples, out quality))
            {
                // even if a greater quality is returned, we only want quality 0
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleQuality = 0;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType =
                    MultiSampleType.FourSamples;
            }
            // Check for 2xAA
            else if (adapter.CheckDeviceMultiSampleType(DeviceType.Hardware, format, false, MultiSampleType.TwoSamples, out quality))
            {
                // even if a greater quality is returned, we only want quality 0
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleQuality = 0;
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleType =
                    MultiSampleType.TwoSamples;
            }

            //e.GraphicsDeviceInformation.PresentationParameters.BackBufferCount = 1;
            //e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
            //e.GraphicsDeviceInformation.PresentationParameters.FullScreenRefreshRateInHz = 60;
            
            return;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Courier New");

            // Load the model.
            characterModel = Content.Load<Model>("dude");
            sphereModel = Content.Load<Model>("football");
            bubbleModel = Content.Load<Model>("bubble");

            discModel = Content.Load<Model>("disc");

            Transforms = new Matrix[sphereModel.Bones.Count];
            sphereModel.CopyAbsoluteBoneTransformsTo(Transforms);

            // Look up our custom skinning information.
            SkinningData skinningData = characterModel.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips["Take 001"];

            animationPlayer.StartClip(clip);

            engine.Startup();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            engine.Shutdown();
        }

        #endregion

        #region Update and Draw

        DateTime lastTime = DateTime.Now;
        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            avatarControlled = false;
            HandleInput(gameTime);
            engine.Process((float)DateTime.Now.Subtract(lastTime).TotalSeconds, avatarControlled || lastAvatarControlled!=avatarControlled);
            lastAvatarControlled = avatarControlled;

            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.White);

            Matrix[] bones = animationPlayer.GetSkinTransforms();

            float aspectRatio = (float)device.Viewport.Width /
                                (float)device.Viewport.Height;

            // Compute camera matrices.
            Vector3 avatarLocation = new Vector3(0, 0, 0);
            if(ReceivedAvatarObject!=null)
            {
                avatarLocation = 0.5f * (AvatarLocation + 10 * ReceivedAvatarObject.SmoothedLocation);
            }

            Matrix view = Matrix.CreateTranslation(avatarLocation * -1f) * Matrix.CreateTranslation(0, -40, 0) *
                          Matrix.CreateRotationY(-MathHelper.ToRadians(cameraRotation)) *
                          Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                          Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                              new Vector3(0, 0, 0), Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    aspectRatio,
                                                                    1,
                                                                    10000);

            // Render the  avatar where it would be with zero network lag.
            /*animationPlayer.Update(TimeSpan.Zero, true, Matrix.CreateRotationY(MathHelper.ToRadians(180 + cameraRotation)) * Matrix.CreateTranslation(AvatarLocation));
            foreach (ModelMesh mesh in characterModel.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["Bones"].SetValue(bones);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }
                
                mesh.Draw();
            }*/

            if (ReceivedAvatarObject != null)
            {
                // Render the  avatar in middle between zero lag and received location
                animationPlayer.Update(TimeSpan.Zero, true, Matrix.CreateRotationY(MathHelper.ToRadians(180 + cameraRotation)) * Matrix.CreateTranslation(avatarLocation));
                foreach (ModelMesh mesh in characterModel.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.Parameters["Bones"].SetValue(bones);
                        effect.Parameters["View"].SetValue(view);
                        effect.Parameters["Projection"].SetValue(projection);
                    }

                    mesh.Draw();
                }
            }


            device.RenderState.FillMode = FillMode.WireFrame;
            device.RenderState.CullMode = CullMode.None;

            {
                foreach (ModelMesh mesh in bubbleModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.View = view;
                        effect.Projection = projection;
                        //obj.BoundingSphereRadius
                        //obj.Location[0], obj.Location[1], obj.Location[2]
                        float scale = this.engine.Bubble.BubbleRange * 10f;

                        effect.EnableDefaultLighting();
                        effect.EnableDefaultLighting();
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.SpecularColor = new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f);
                        effect.DirectionalLight0.DiffuseColor = new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f);
                        effect.DirectionalLight0.Direction = -lightDirection;
                        effect.DirectionalLight1.Enabled = false;
                        effect.DirectionalLight2.Enabled = false;
                        effect.AmbientLightColor = new Vector3(0.7f, 0.7f, 0.7f);


                        effect.World = Matrix.CreateScale(scale);
                    }

                    mesh.Draw();
                }
            }

            {
                foreach (ModelMesh mesh in discModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.View = view;
                        effect.Projection = projection;
                        //obj.BoundingSphereRadius
                        //obj.Location[0], obj.Location[1], obj.Location[2]
                        float scale = this.engine.Bubble.BubbleRange * 10f;
                        //float scale = 1000f;
                        //effect.EnableDefaultLighting();

                        effect.World = Matrix.CreateScale(scale*0.9f);
                    }

                    mesh.Draw();
                }
            }

            device.RenderState.FillMode = FillMode.Solid;
            device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

            foreach (DeckObject obj in engine.Bubble.Objects.Values)
            {
                if (obj.ObjectId == AvatarId)
                {
                    continue;
                }
                Vector3 loc = obj.SmoothedLocation;
                loc = Vector3.Multiply(loc, 10f);
                Quaternion ori = obj.SmoothedOrientation;

                if (obj.TypeName == "Human")
                {
                    animationPlayer.Update(TimeSpan.Zero, true, Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), (float)Math.PI) * Matrix.CreateFromQuaternion(ori) * Matrix.CreateTranslation(loc));
                    foreach (ModelMesh mesh in characterModel.Meshes)
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            effect.Parameters["Bones"].SetValue(bones);
                            effect.Parameters["View"].SetValue(view);
                            effect.Parameters["Projection"].SetValue(projection);
                            effect.Parameters["Light1Direction"].SetValue(lightDirection);
                            effect.Parameters["Light1Color"].SetValue(new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f));
                            effect.Parameters["AmbientColor"].SetValue(new Vector3(ambientLightColor.R / 256f, ambientLightColor.G / 256f, ambientLightColor.B / 256f));
                        }
                        mesh.Draw();
                    }
                }
                else
                {

                    foreach (ModelMesh mesh in sphereModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.View = view;
                            effect.Projection = projection;
                            effect.EnableDefaultLighting();
                            effect.DirectionalLight0.Enabled = true;
                            effect.DirectionalLight0.SpecularColor = new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f);
                            effect.DirectionalLight0.DiffuseColor = new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f);
                            effect.DirectionalLight0.Direction = lightDirection;
                            effect.DirectionalLight1.Enabled = false;
                            effect.DirectionalLight2.Enabled = false;

                            effect.AmbientLightColor = new Vector3(ambientLightColor.R / 256f, ambientLightColor.G / 256f, ambientLightColor.B / 256f);

                            //obj.BoundingSphereRadius
                            //obj.Location[0], obj.Location[1], obj.Location[2]
                            float scale = obj.BoundingSphereRadius / 10f;
                            //effect.LightingEnabled = true;

                            effect.World = Transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Matrix.CreateTranslation(loc);
                        }

                        mesh.Draw();
                    }
                }
            }


            {
                foreach (ModelMesh mesh in bubbleModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.View = view;
                        effect.Projection = projection;
                        //obj.BoundingSphereRadius
                        //obj.Location[0], obj.Location[1], obj.Location[2]
                        float scale = this.engine.Bubble.BubbleRange * 10f;
                        //float scale = 1000f;
                        effect.EnableDefaultLighting();
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.SpecularColor = new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f);
                        effect.DirectionalLight0.DiffuseColor = new Vector3(lightColor.R / 256f, lightColor.G / 256f, lightColor.B / 256f);
                        effect.DirectionalLight0.Direction = -lightDirection;
                        effect.DirectionalLight1.Enabled = false;
                        effect.DirectionalLight2.Enabled = false;

                        effect.AmbientLightColor = new Vector3(0.7f, 0.7f, 0.7f);

                        effect.World = Matrix.CreateScale(scale);
                    }

                    mesh.Draw();
                }
            }


            spriteBatch.Begin();

            currentTextRenderHeight = 0;
            if (showStatistics)
            {
                RenderString("Cloud Url: " + engine.Bubble.CloudUrl);
                RenderString("Bubble Url: mxp://" + engine.serverAddress + ":" + engine.serverPort + "/" +
                             engine.Bubble.BubbleId + "/" + engine.Location);
                RenderString("Server Program Name: " + engine.Bubble.ServerProgramName);
                RenderString("Server Program Version: " + engine.Bubble.ServerProgramMajorVersion + "." +
                             engine.Bubble.ServerProgramMinorVersion);
                RenderString("Server Protocol Version: " + engine.Bubble.ServerProtocolMajorVersion + "." +
                             engine.Bubble.ServerProtocolMinorVersion+" (Source Revision: "+engine.Bubble.ServerProtocolSourceRevision+")");
                RenderString("Client Protocol Version: " + MxpConstants.ProtocolMajorVersion + "." +
                             MxpConstants.ProtocolMinorVersion+" (Source Revision: "+MxpConstants.ProtocolSourceRevision+")");
                RenderString("Session Status: " + engine.Client.SessionState);
                RenderString("Session Id: " + engine.Client.SessionId);
                RenderString("Transmitter Alive: " + engine.Client.IsTransmitterAlive);
                RenderString("Camera Rotation: " + (int) cameraRotation + " Arc: " + (int) cameraArc + " Distance: " +
                             (int) cameraDistance);
                RenderString("Receive Rate (kbits/s) : " + (int) (engine.Client.ReceiveRate*10.0/1000.0));
                RenderString("Send Rate (kbits/s) : " + (int) (engine.Client.SendRate*10.0/1000.0));
                RenderString("Sent (kbytes) : " + (int) (engine.Client.BytesSent*10.0/1024.0));
                RenderString("Received (kbytes) : " + (int) (engine.Client.BytesReceived/1024.0));

                RenderString("Bubble Name: " + engine.Bubble.BubbleName);
                RenderString("Bubble Range: " + engine.Bubble.BubbleRange);
                RenderString("Bubble Perception: " + engine.Bubble.BubblePerceptionRange);
                //RenderString("Location: x=" + location.X + " y=" + location.Y + " z=" + location.Z);
                RenderString("Object Count: " + engine.Bubble.Objects.Count);
            }

            if (keyboardTextInputMode)
            {
                RenderString("Chat: " + textInputBuffer.ToString());
            }

            while (ConsoleLines.Count > 10)
            {
                ConsoleLines.RemoveAt(0);
            }

            foreach (string consoleLine in ConsoleLines)
            {
                RenderString(consoleLine);
            }

            Vector2 FontOrigin = new Vector2(0, 0);
            foreach (DeckObject obj in engine.Bubble.Objects.Values)
            {
                Vector3 loc = obj.SmoothedLocation;
                loc = Vector3.Multiply(loc, 10f);

                if (obj.TypeName == "Human")
                {
                    Vector3 projectedCoordinate = graphics.GraphicsDevice.Viewport.Project(loc, projection, view, Matrix.Identity);
                    spriteBatch.DrawString(font, obj.ObjectName, new Vector2(projectedCoordinate.X, projectedCoordinate.Y), Color.Black, 0, FontOrigin, 0.8f, SpriteEffects.None, 0.5f);
                }
            }

            spriteBatch.End();

            device.RenderState.DepthBufferEnable = true;

            base.Draw(gameTime);
        }

        private int currentTextRenderHeight = 20;
        private bool showStatistics;

        private void RenderString(String str)
        {
            //font.MeasureString( str ).Y
            Vector2 FontOrigin = new Vector2(0,0);
            spriteBatch.DrawString(font, str,new Vector2(10,currentTextRenderHeight), Color.Black,0,FontOrigin,0.8f,SpriteEffects.None,0.5f);
            currentTextRenderHeight += 30;
        }

        
        #endregion

        #region Handle Input


        /// <summary>
        /// Handles input for quitting the game.
        /// </summary>
        private void HandleInput(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentMouseState = Mouse.GetState();

            float timeMillis = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            lightRotation += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            lightDirection = Vector3.Transform(new Vector3(0, 1, 0), Matrix.CreateRotationX(MathHelper.ToRadians(lightRotation)));

            AvatarOrientation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), (float)Math.PI * cameraRotation / 180f);
            forwardDirection = Vector3.Transform(new Vector3(0, 0, 1), Matrix.CreateFromQuaternion(AvatarOrientation));
            rightDirection = Vector3.Transform(new Vector3(1, 0, 0), Matrix.CreateFromQuaternion(AvatarOrientation));

            Keys[] pressedKeys = currentKeyboardState.GetPressedKeys();

            List<Keys> keystrokes = new List<Keys>();
            foreach (Keys key in keypressTimes.Keys)
            {
                if (DateTime.Now.Subtract(keypressTimes[key]).TotalMilliseconds > 300)
                {
                    keystrokes.Add(key);
                }
                bool isPressed = false;
                foreach (Keys pressedKey in pressedKeys)
                {
                    if (key == pressedKey)
                    {
                        isPressed = true;
                    }
                }
                if (!isPressed && !keystrokes.Contains(key))
                {
                    keystrokes.Add(key);
                }
            }

            foreach (Keys key in keystrokes)
            {
                keypressTimes.Remove(key);

                if (key == Keys.Enter)
                {
                    if (keyboardTextInputMode)
                    {
                        engine.SendChatLine(textInputBuffer.ToString());
                        textInputBuffer = new StringBuilder();
                    }
                    keyboardTextInputMode = !keyboardTextInputMode;
                }

                if( key == Keys.F10 )
                {
                    showStatistics = !showStatistics;
                }

                String keyString = key.ToString();
                if (keyboardTextInputMode)
                {
                    if (letterKeys.Contains(keyString))
                    {
                        textInputBuffer.Append(keyString.ToLower());
                    }
                    if (key == Keys.Space)
                    {
                        textInputBuffer.Append(" ");
                    }
                    if (key == Keys.Back)
                    {
                        if (textInputBuffer.Length > 0)
                        {
                            textInputBuffer.Remove(textInputBuffer.Length - 1, 1);
                        }
                    }
                }
            }

            foreach (Keys key in pressedKeys)
            {
                if (!keypressTimes.ContainsKey(key))
                {
                    keypressTimes.Add(key, DateTime.Now);
                }
            }

            if (!keyboardTextInputMode)
            {
                // Check for input to rotate the camera up and down around the model.
                if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                    currentKeyboardState.IsKeyDown(Keys.W))
                {
                    //cameraArc += time * 0.1f;
                    AvatarLocation += forwardDirection * 0.001f * timeMillis * 45f;
                    avatarControlled = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                    currentKeyboardState.IsKeyDown(Keys.S))
                {
                    //cameraArc -= time * 0.1f;
                    AvatarLocation -= forwardDirection * 0.001f * timeMillis * 45f;
                    avatarControlled = true;
                }

                // Check for input to rotate the camera around the model.
                if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                    currentKeyboardState.IsKeyDown(Keys.D))
                {
                    //cameraRotation -= timeMillis * 0.1f;
                    AvatarLocation -= rightDirection * 0.001f * timeMillis * 45f;
                    avatarControlled = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                    currentKeyboardState.IsKeyDown(Keys.A))
                {
                    //cameraRotation += timeMillis * 0.1f;
                    AvatarLocation += rightDirection * 0.001f * timeMillis * 45f;
                    avatarControlled = true;
                }

                cameraArc += currentGamePadState.ThumbSticks.Right.Y * timeMillis * 0.25f;

                // Limit the arc movement.
                if (cameraArc > 90.0f)
                    cameraArc = 90.0f;
                else if (cameraArc < -90.0f)
                    cameraArc = -90.0f;


                cameraRotation += currentGamePadState.ThumbSticks.Right.X * timeMillis * 0.25f;

                // Check for input to zoom camera in and out.
                if (currentKeyboardState.IsKeyDown(Keys.Z))
                    cameraDistance += timeMillis * 1f;

                if (currentKeyboardState.IsKeyDown(Keys.X))
                    cameraDistance -= timeMillis * 1f;

                cameraDistance += currentGamePadState.Triggers.Left * timeMillis * 0.5f;
                cameraDistance -= currentGamePadState.Triggers.Right * timeMillis * 0.5f;

                // Limit the camera distance.
                if (cameraDistance > 5000.0f)
                    cameraDistance = 5000.0f;
                else if (cameraDistance < 10.0f)
                    cameraDistance = 10.0f;

                if (currentGamePadState.Buttons.RightStick == ButtonState.Pressed ||
                    currentKeyboardState.IsKeyDown(Keys.R))
                {
                    cameraArc = 0;
                    cameraRotation = 0;
                    cameraDistance = 100;
                }

                int mouseLookReferenceX = Window.ClientBounds.Width / 2;
                int mouseLookReferenceY = Window.ClientBounds.Height / 2;
                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    if (IsMouseVisible)
                    {
                        lastMouseX = currentMouseState.X;
                        lastMouseY = currentMouseState.Y;
                    }
                    else
                    {
                        int dx = currentMouseState.X - mouseLookReferenceX;
                        int dy = currentMouseState.Y - mouseLookReferenceY;

                        cameraArc -= dy * 0.1f;
                        cameraRotation -= dx * 0.1f;
                    }

                    Mouse.SetPosition(mouseLookReferenceX, mouseLookReferenceY);

                    IsMouseVisible = false;
                }
                else
                {
                    if (!IsMouseVisible)
                    {
                        Mouse.SetPosition(lastMouseX, lastMouseY);
                    }

                    IsMouseVisible = true;
                }

                cameraDistance = initialCameraDistance - currentMouseState.ScrollWheelValue;

                // Check for exit.
                if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                    currentGamePadState.Buttons.Back == ButtonState.Pressed)
                {
                    Exit();
                }

            }

        }

        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    class Program
    {
        private string m_serverAddress = ConfigurationSettings.AppSettings["ServerAddress"];
        private int m_serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);
        private Guid m_bubbleId = new Guid(ConfigurationSettings.AppSettings["BubbleId"]);
        private string m_location = ConfigurationSettings.AppSettings["Location"];
        private string m_identityProviderUrl = ConfigurationSettings.AppSettings["IdentityProviderUrl"];
        private string m_participantName = ConfigurationSettings.AppSettings["ParticipantName"];
        private string m_participantPassphrase = ConfigurationSettings.AppSettings["ParticipantPassphrase"];

        static void Main()
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            if (ProcessArguments())
            {
                using (Deck game = new Deck(m_serverAddress, m_serverPort, m_bubbleId, m_location, m_identityProviderUrl, m_participantName, m_participantPassphrase))
                {
                    game.Run();
                }
            }
        }

        private string GetAppDir()
        {
            return Application.StartupPath;
        }

        private string GetAppExe()
        {
            return Path.GetFileName(Application.ExecutablePath);
        }

        protected void RegisterMoniker(string moniker)
        {
            RegistryKey regHKLM = Registry.ClassesRoot.CreateSubKey(moniker);

            regHKLM.SetValue("", "URL:" + moniker);
            regHKLM.SetValue("URL Protocol", "");
            RegistryKey regHKLMicon = regHKLM.CreateSubKey("DefaultIcon");
            string ex = Path.Combine(GetAppDir(), GetAppExe());
            regHKLMicon.SetValue("", ex);
            RegistryKey regShell = regHKLM.CreateSubKey("shell");
            RegistryKey regopen = regShell.CreateSubKey("open");
            RegistryKey regcom = regopen.CreateSubKey("command");
            regcom.SetValue("", ex + " \"%1\"");
        }

        void Register()
        {
            RegisterMoniker("mxp");
        }

        protected bool ProcessArguments()
        {
            bool launch = true;
            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg.ToLower() == "-register")
                {
                    Register();
                }

                launch = ProcessArgument(arg, launch);
            }

            return launch;
        }

        private bool ProcessArgument(string arg, bool launch)
        {           
            if (arg.ToLower().StartsWith("mxp://", StringComparison.InvariantCultureIgnoreCase))
            {
                Uri uri = new Uri(arg);

                m_serverAddress = uri.Host;
                m_serverPort = uri.Port;

                string[] authorization = uri.UserInfo.Split(':');
                if (authorization.Length > 0)
                {
                    m_participantName = HttpUtility.UrlDecode(authorization[0]);
                }

                if (authorization.Length > 1)
                {
                    m_participantPassphrase = HttpUtility.UrlDecode(authorization[1]);
                }


                string bubbleId = uri.Segments[1].Trim('/');
                string location = uri.Segments[2].Trim('/');

                m_bubbleId = new Guid( bubbleId );
                m_location = location;

                launch = true;
            }

            return launch;
        }
    }

    #endregion
}
