using System;
using System.Collections.Generic;
using System.Text;
using Horde3DNET;

namespace CloudDeck
{
    public class HordeDemo
    {
        // DeckEngine objects

        private int m_knight;
        private int m_particleSystem;
        private bool m_running = false;

        private int m_env;
        private int m_envRes;
        private int m_knightRes;
        private int m_knightAnim1Res;
        private int m_knightAnim2Res;
        private int m_particleSysRes;

        public int SphereResource
        {
            get
            {
                return m_envRes;
            }
        }

        public void Initialize()
        {
            // Environment
            m_envRes = h3d.addResource((int)h3d.H3DResTypes.SceneGraph, "models/sphere/sphere.scene.xml", 0);
            // Knight
            m_knightRes = h3d.addResource((int)h3d.H3DResTypes.SceneGraph, "models/knight/knight.scene.xml", 0);
            m_knightAnim1Res = h3d.addResource((int)h3d.H3DResTypes.Animation, "animations/knight_order.anim", 0);
            m_knightAnim2Res = h3d.addResource((int)h3d.H3DResTypes.Animation, "animations/knight_attack.anim", 0);
            // Particle system
            m_particleSysRes = h3d.addResource((int)h3d.H3DResTypes.SceneGraph, "particles/particleSys1/particleSys1.scene.xml", 0);

        }

        public void Start()
        {
            /////////////
            // Add scene nodes
            // Add environment
            m_env = h3d.addNodes(h3d.H3DRootNode, m_envRes);
            h3d.setNodeTransform(m_env, 0, -20, 0, 0, 0, 0, 20, 20, 20); //horde3d 1.0

            // Add knight
            m_knight = h3d.addNodes(h3d.H3DRootNode, m_knightRes);
            h3d.setNodeTransform(m_knight, 0, 0, 0, 0, 180, 0, 0.1f, 0.1f, 0.1f);
            h3d.setupModelAnimStage(m_knight, 0, m_knightAnim1Res, 0, string.Empty, false);
            h3d.setupModelAnimStage(m_knight, 1, m_knightAnim2Res, 0, string.Empty, false);

            //horde3d 1.0
            // Attach particle system to hand joint
            h3d.findNodes(m_knight, "Bip01_R_Hand", (int)h3d.H3DNodeTypes.Joint);
            int hand = h3d.getNodeFindResult(0);
            m_particleSystem = h3d.addNodes(hand, m_particleSysRes);
            h3d.setNodeTransform(m_particleSystem, 0, 40, 0, 90, 0, 0, 1, 1, 1);
            /////////
            m_running = true;
        }

        public void Stop()
        {
            m_running = false;
            h3d.removeNode(m_env);
            h3d.removeNode(m_knight);
        }

        public void Process(float animationTime,float animationBlendingWeight,float currentFramesPerSecond)
        {
            if (m_running)
            {
                // Do animation blending
                h3d.setModelAnimParams(m_knight, 0, animationTime*24.0f, animationBlendingWeight);
                h3d.setModelAnimParams(m_knight, 1, animationTime*24.0f, 1.0f - animationBlendingWeight);

                // Animate particle system                                
                int cnt = cnt = h3d.findNodes(m_particleSystem, "", (int)h3d.H3DNodeTypes.Emitter);
                for (int i = 0; i < cnt; ++i)
                    h3d.advanceEmitterTime(h3d.getNodeFindResult(i), 1.0f/currentFramesPerSecond);
            }
        }


    }
}
