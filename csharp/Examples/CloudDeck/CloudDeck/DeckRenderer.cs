using System;
using System.Windows.Forms;
using Horde3DNET;
using Horde3DNET.Utils;
using System.Collections.Generic;
using CloudMath;
using CloudDeck.Model;
using MXP.Util;
using System.Drawing;

namespace CloudDeck
{
    /// <summary>
    /// DeckRenderer is handles details of Horde3D rendering and switching between demo mode and 
    /// in world mode.
    /// </summary>
    public class DeckRenderer
    {

        #region Fields

        public float CurrentFramesPerSecond;

        public bool IsFreeze;
        public bool IsDebugViewMode;
        public bool IsWireframeMode;
        public int StatisticsMode = 0;
        public float AnimationBlendingWeight;

        public int CameraHid;
        public int HdrPipeHid;
        public int ForwardPipeHid;
        public int LightHid;

        private float m_animationTime;
        private float m_timer;
        private string m_framesPerSecondText;

        private int m_fontMaterialHid;
        private int m_panelMaterialHid;
        private int m_logoMaterialHid;

        private int m_bubbleRes;
        private int m_atmosphereRes;

        private IDictionary<int, Guid> m_hordeIdObjectIdDictionary = new Dictionary<int, Guid>();
        private IDictionary<Guid, int> m_objectIdHordeIdDictionary = new Dictionary<Guid, int>();

        //private int m_atmosphereHid;
        private HordeDemo m_demo = new HordeDemo();

        // workaround
        private bool            m_initialized = false;
        
        #endregion 

        #region Properties

        public HordeDemo Demo
        {
            get
            {
                return m_demo;
            }
        }

        public int BubbleResource
        {
            get
            {
                return m_bubbleRes;
            }
        }
        /*
        public int AtmosphereHid
        {
            get
            {
                return m_atmosphereHid;
            }
        }
        */

        #endregion

        #region Constructor

        public DeckRenderer()
        {
            CurrentFramesPerSecond = 30; m_timer = 0;

            IsFreeze = false; IsDebugViewMode = false;
            m_animationTime = 0; AnimationBlendingWeight = 0.0f;
            m_framesPerSecondText = string.Empty;
            IsWireframeMode = false;
        }

        #endregion

        #region Startup and Shutdown

        public bool Startup()
        {
            DeckProgram.DeckScene.ObjectAdd += OnObjectAdd;
            DeckProgram.DeckScene.ObjectUpdate += OnObjectUpdate;
            DeckProgram.DeckScene.ObjectRemove += OnObjectRemove;
            DeckProgram.DeckScene.BubbleAdd += OnBubbleAdd;
            DeckProgram.DeckScene.BubbleRemove += OnBubbleRemove;

	        // Initialize engine
            if (!h3d.init())
            {
                Horde3DUtils.dumpMessages();
                return false;
            }

	        // Set options
            h3d.setOption(h3d.H3DOptions.LoadTextures, 1);
            h3d.setOption(h3d.H3DOptions.TexCompression, 0);
            h3d.setOption(h3d.H3DOptions.FastAnimation, 0);
            h3d.setOption(h3d.H3DOptions.MaxAnisotropy, 8);
            h3d.setOption(h3d.H3DOptions.ShadowMapSize, 2048);
            h3d.setOption(h3d.H3DOptions.TrilinearFiltering, 1);
            h3d.setOption(h3d.H3DOptions.SampleCount, 8);

            // Add resources
            HdrPipeHid = h3d.addResource( (int) h3d.H3DResTypes.Pipeline, "pipelines/hdr.pipeline.xml", 0);
            ForwardPipeHid = h3d.addResource((int)h3d.H3DResTypes.Pipeline, "pipelines/forward.pipeline.xml", 0);


            m_fontMaterialHid = h3d.addResource((int)h3d.H3DResTypes.Material, "overlays/font.material.xml", 0);
            m_panelMaterialHid = h3d.addResource((int)h3d.H3DResTypes.Material, "overlays/panel.material.xml", 0);
            m_logoMaterialHid = h3d.addResource((int)h3d.H3DResTypes.Material, "overlays/logo.material.xml", 0);
            m_bubbleRes = h3d.addResource((int)h3d.H3DResTypes.SceneGraph, "models/bubble5/bubble5.scene.xml", 0);
            m_atmosphereRes = h3d.addResource((int)h3d.H3DResTypes.SceneGraph, "models/whiteatmosphere/whiteatmosphere.scene.xml", 0);

            // Overlays
            m_demo.Initialize();

            // Load resources
            Horde3DUtils.loadResourcesFromDisk( "content" );

	        // Add camera
            CameraHid = h3d.addCameraNode(h3d.H3DRootNode, "CameraHid", ForwardPipeHid);

            // Add light source
            LightHid = h3d.addLightNode(h3d.H3DRootNode, "Light1", 0, "LIGHTING", "SHADOWMAP");
            h3d.setNodeTransform(LightHid, 0, 1, 15, 30, 0, 0, 1, 1, 1);

	        h3d.setNodeTransform( LightHid, 0, 15, 10, -60, 0, 0, 1, 1, 1 );
            h3d.setNodeParamF(LightHid,(int) h3d.H3DLight.RadiusF,0, 30);
            h3d.setNodeParamF(LightHid, (int)h3d.H3DLight.FovF, 0, 60);
            h3d.setNodeParamI(LightHid, (int)h3d.H3DLight.ShadowMapCountI, 1);
            h3d.setNodeParamF(LightHid, (int)h3d.H3DLight.ShadowMapBiasF, 0, 0.01f);
            h3d.setNodeParamF(LightHid, (int)h3d.H3DLight.ColorF3, 0, 1.0f);
            h3d.setNodeParamF(LightHid, (int)h3d.H3DLight.ColorF3, 1, 0.8f);
            h3d.setNodeParamF(LightHid, (int)h3d.H3DLight.ColorF3, 2, 0.7f);
            
	        // Customize post processing effects
            int matRes = h3d.findResource((int)h3d.H3DResTypes.Material, "pipelines/postHDR.material.xml");
            // hdrParams: exposure, brightpass threshold, brightpass offset
            h3d.setMaterialUniform(matRes, "hdrParams", 2.5f, 0.5f, 0.08f, 0);

            m_initialized = true;
            
            //m_atmosphereHid = h3d.addNodes(h3d.H3DRootNode, m_atmosphereRes);
            //h3d.setNodeTransform(m_atmosphereHid, 0, 0, 0, 0, 180, 0, 1000f, 1000f, 1000f);
	        
            return true;
        }

        public void Shutdown()
        {
            // Release engine
            h3d.release();
        }

        #endregion

        #region Processing

        public void Process()
        {
	        CurrentFramesPerSecond = DeckProgram.MainForm.FramesPerSecond;
            m_timer += 1 / CurrentFramesPerSecond;

            h3d.setOption( h3d.H3DOptions.DebugViewMode, IsDebugViewMode ? 1.0f : 0.0f );
	        h3d.setOption( h3d.H3DOptions.WireframeMode, IsWireframeMode ? 1.0f : 0.0f );
        	
	        if( !IsFreeze )
	        {
		        m_animationTime += 1.0f / CurrentFramesPerSecond;

		        m_demo.Process(m_animationTime,AnimationBlendingWeight,CurrentFramesPerSecond);
	        }

            Matrix cameraTranslationMatrix = Matrix.CreateTranslation(DeckProgram.DeckRudder.CameraLocation.X, DeckProgram.DeckRudder.CameraLocation.Y, DeckProgram.DeckRudder.CameraLocation.Z);
            Matrix cameraTransformationMatrix = Matrix.Multiply(DeckProgram.DeckRudder.CameraOrientationMatrix, cameraTranslationMatrix);
            h3d.setNodeTransMat(CameraHid, cameraTransformationMatrix.ToArray(MatrixElementOrder.RowMajor));


            Matrix lightTranslationMatrix = Matrix.CreateTranslation(DeckProgram.DeckRudder.FlashlightLocation.X, DeckProgram.DeckRudder.FlashlightLocation.Y, DeckProgram.DeckRudder.FlashlightLocation.Z);
            Matrix lightTransformationMatrix = Matrix.Multiply(DeckProgram.DeckRudder.FlashlightOrientationMatrix, lightTranslationMatrix);
            h3d.setNodeTransMat(LightHid, lightTransformationMatrix.ToArray(MatrixElementOrder.RowMajor));
            // Set camera parameters
            //h3d.setNodeTransform(CameraHid, DeckProgram.DeckRudder.CX, DeckProgram.DeckRudder.CY, DeckProgram.DeckRudder.CZ
            //    , DeckProgram.DeckRudder.RX, DeckProgram.DeckRudder.RY, 0, 1, 1, 1); //horde3d 1.0

            if (StatisticsMode > 0)
            {
                Horde3DUtils.showFrameStats( m_fontMaterialHid, m_panelMaterialHid, StatisticsMode );		        

                string text = string.Format("Weight: {0:F2}", AnimationBlendingWeight);
                Horde3DUtils.showText(text, 0.03f, 0.24f, 0.026f, 1, 1, 1, m_fontMaterialHid, 5);
            }

            // Show logo
            /*h3d.showOverlay( 0.75f, 0.8f, 0, 1, 0.75f, 1, 0, 0,
	                      1, 1, 1, 0, 1, 0.8f, 1, 1,
	                      1, 1, 1, 1, m_logoMaterialHid, 7 );
*/

            // Render scene
            h3d.render( CameraHid );//horde3D 1.0

            h3d.finalizeFrame();

            //horde3D 1.0
            h3d.clearOverlays();

            // Write all messages to log file
            Horde3DUtils.dumpMessages();


        }

        #endregion

        #region Event Handlers

        public void OnObjectAdd(DeckObject deckObject)
        {
            string modelUrl = deckObject.ModelUrl;

            int hordeId = h3d.addNodes(h3d.H3DRootNode, DeckProgram.AssetManager.GetAssetRendererId(deckObject.ModelUrl));
            deckObject.RenderId = hordeId;
            m_hordeIdObjectIdDictionary.Add(hordeId, deckObject.ObjectId);
            m_objectIdHordeIdDictionary.Add(deckObject.ObjectId, hordeId);
        }

        public void OnObjectUpdate(DeckObject deckObject)
        {
            int hordeId = m_objectIdHordeIdDictionary[deckObject.ObjectId];

            Matrix orientationMatrix;
            Quaternion orientation = deckObject.RenderOrientation;
            Common.Rotate(out orientationMatrix, ref orientation);
            Matrix translationMatrix = Matrix.CreateTranslation(deckObject.RenderLocation.X, deckObject.RenderLocation.Y, deckObject.RenderLocation.Z);
            Matrix scaleMatrix = Matrix.CreateScale(deckObject.RenderScale.X, deckObject.RenderScale.Y, deckObject.RenderScale.Z);
            Matrix transformationMatrix = Matrix.Multiply(scaleMatrix, Matrix.Multiply(Matrix.RotationY(Common.DegreesToRadians(180)) * orientationMatrix, translationMatrix));
            h3d.setNodeTransMat(hordeId, transformationMatrix.ToArray(MatrixElementOrder.RowMajor));

            if (deckObject.ObjectId == DeckProgram.DeckEngine.AvatarId)
            {
                DeckProgram.DeckRudder.AvatarCurrentLocation = deckObject.RenderLocation;
                DeckProgram.DeckRudder.AvatarCurrentOrientationMatrix = orientationMatrix;
            }
        }

        public void OnObjectRemove(DeckObject deckObject)
        {
            int hordeId = m_objectIdHordeIdDictionary[deckObject.ObjectId];
            h3d.removeNode(hordeId);
            m_objectIdHordeIdDictionary.Remove(deckObject.ObjectId);
            m_hordeIdObjectIdDictionary.Remove(hordeId);
        }

        public void OnBubbleAdd(DeckBubble deckBubble)
        {
            deckBubble.RenderId = h3d.addNodes(h3d.H3DRootNode, DeckProgram.DeckRenderer.BubbleResource);
            h3d.setNodeTransform(deckBubble.RenderId,
                    deckBubble.Center.X, deckBubble.Center.Y, deckBubble.Center.Z,
                    0, 0, 0,
                    deckBubble.Range, deckBubble.Range, deckBubble.Range); //horde3d 1.0
        }

        public void OnBubbleRemove(DeckBubble deckBubble)
        {
            h3d.removeNode(deckBubble.RenderId);
        }

        #endregion

        #region Public Interface

        public bool GetIntersectionPoint(int ignoreNodeId,ref Vector3 origin, ref Vector3 delta, out int intersectingNodeRenderId, out Vector3 intersection, out float distance)
        {
            int maxIntersections = 10;
            int intersectionCount = h3d.castRay(h3d.H3DRootNode, origin.X, origin.Y, origin.Z, delta.X, delta.Y, delta.Z, maxIntersections);

            intersection = Vector3.Zero;
            distance = 0;
            intersectingNodeRenderId = 0;

            if (intersectionCount == 0)
            {
                return false;
            }
            else
            {
                float distanceValue=0;
                float[] intersectionArray = new float[3];
                intersection = new Vector3();
                
                int index = 0;
                intersectingNodeRenderId = ignoreNodeId;
                while(ignoreNodeId==intersectingNodeRenderId || IsParent(ignoreNodeId, intersectingNodeRenderId))
                {
                    if (index == maxIntersections)
                    {
                        return false;
                    }
                    h3d.getCastRayResult(index, out intersectingNodeRenderId, out distanceValue, intersectionArray);
                    index++;
                }

                distance=distanceValue;
                intersection.X = intersectionArray[0];
                intersection.Y = intersectionArray[1];
                intersection.Z = intersectionArray[2];
                return true;
            }
        }

        private bool IsParent(int parentCandidateRenderId, int nodeRenderId)
        {
            int parentRenderId = h3d.getNodeParent(nodeRenderId);
            //LogUtil.Debug("Candidate: " + parentCandidateRenderId + " Parent: " + parentRenderId + " Child: " + nodeRenderId);
            if (parentCandidateRenderId == parentRenderId)
            {
                return true;
            }
            if (parentRenderId == h3d.H3DRootNode)
            {
                return false;
            }
            return IsParent(parentCandidateRenderId, parentRenderId);
        }

        public DeckObject GetPointedObject()
        {
            Point point=DeckProgram.MainForm.PointToClient(Cursor.Position);

            int nodeRenderId = Horde3DUtils.pickNode(CameraHid, point.X / ((float)DeckProgram.MainForm.ClientRectangle.Width), 1 - point.Y / ((float)DeckProgram.MainForm.ClientRectangle.Height));

            if (nodeRenderId == 0)
            {
                return null;
            }

            int nodeParentRenderId=nodeRenderId;
            
            while(nodeParentRenderId!=h3d.H3DRootNode)
            {
                nodeRenderId = nodeParentRenderId;
                nodeParentRenderId = h3d.getNodeParent(nodeParentRenderId);
            }

            if(this.m_hordeIdObjectIdDictionary.ContainsKey(nodeRenderId))
            {
                Guid objectId=m_hordeIdObjectIdDictionary[nodeRenderId];
                if (DeckProgram.DeckScene.ContainsObject(objectId))
                {
                    return DeckProgram.DeckScene.GetObject(objectId);
                }
            }

            return null;
        }

        public void StartDemo()
        {
            //if (m_atmosphere != 0)
            //{
            //    h3d.removeNode(m_atmosphere);
            //}
            //m_demo.Start();
        }

        public void StopDemo()
        {
            //m_demo.Stop();
            //m_atmosphere = h3d.addNodes(h3d.H3DRootNode, m_atmosphereRes);
            //h3d.setNodeTransform(m_atmosphere, 0, 0, 0, 0, 180, 0, 500f, 500f, 500f);
        }

        public void ResizeViewport(int width, int height)
        {
            if (!m_initialized) return;

			//Horde3DUtils.setOpenGLWindowSize(width,height);
			
            // Resize viewport
            h3d.setupViewport( 0, 0, width, height, true );

            // Set virtual camera parameters
            //depreceated h3d.setupCameraView(h3d.PrimeTimeCam, 45.0f, (float)width / height, 0.1f, 1000.0f);
            h3d.setupCameraView( CameraHid, 45.0f, (float)width / height, 0.1f, 5000.0f ); //horde3d 1.0
        }

        #endregion

    }
}
