using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using LPGames.KinectToolbox;

namespace ArmaPalabra.Gameplay
{
    public class VisualizadorGuia : DrawableGameComponent
    {
        Texture2D kinectRGBVideo;
        private static readonly int[] IntensityShiftByPlayerR = { 1, 2, 0, 2, 0, 0, 2, 0 };
        private static readonly int[] IntensityShiftByPlayerG = { 1, 2, 2, 0, 2, 0, 0, 1 };
        private static readonly int[] IntensityShiftByPlayerB = { 1, 0, 2, 2, 0, 2, 0, 2 };

        private Skeleton[] _skeletons;
        private int playerIndex;

        private const int RedIndex = 2;
        private const int GreenIndex = 1;
        private const int BlueIndex = 0;

        public Vector2 position { get; set; }
        private Texture2D colorTexture;
        private static bool calibrated;
        public bool enableBackground { get; set; }
        private bool contentLoaded;

        public VisualizadorGuia(Game game)
            : base(game)
        {
            
            contentLoaded = false;
            enableBackground = true;
            this.Initialize();
        }
        #region Atributos Compartidos
        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }

        private KinectDetector SharedKinectDetector
        {
            get
            {
                return (KinectDetector)this.Game.Services.GetService(typeof(KinectDetector));
            }
        }

        #endregion
        protected override void LoadContent()
        {
            this.colorTexture = Game.Content.Load<Texture2D>("guiaFondo");
            contentLoaded = true;
        }
        public override void Draw(GameTime gameTime)
        {
            

            SharedSpriteBatch.Begin();
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, 128, 96);
            if (enableBackground)
                SharedSpriteBatch.Draw(colorTexture, rectangle, Color.White);

            if (kinectRGBVideo!=null)
                SharedSpriteBatch.Draw(kinectRGBVideo, rectangle, Color.White);
            SharedSpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (null == SharedKinectDetector.Sensor ||   false == SharedKinectDetector.Sensor.IsRunning || KinectStatus.Connected != SharedKinectDetector.Sensor.Status)
            {
                return;
            }

            if (!contentLoaded)
            {
                return;
            }

            using (SkeletonFrame skeletonFrame = SharedKinectDetector.Sensor.SkeletonStream.OpenNextFrame(0))
            {
                if (skeletonFrame != null && skeletonFrame.SkeletonArrayLength > 0)
                {
                    if (_skeletons == null || _skeletons.Length != skeletonFrame.SkeletonArrayLength)
                    {
                        _skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(_skeletons);

                    // grab the tracked skeleton and set the playerIndex for use pulling
                    // the depth data out for the silhouette.
                    this.playerIndex = -1;
                    for (int i = 0; i < _skeletons.Length; i++)
                    {
                        if (_skeletons[i].TrackingState != SkeletonTrackingState.NotTracked)
                        {
                            this.playerIndex = i + 1;
                            if (_skeletons[i].TrackingState == SkeletonTrackingState.Tracked && _skeletons[i] == KinectHelper.GetPrimarySkeleton(_skeletons))
                            {
                                this.Game.Services.RemoveService(typeof(Skeleton));
                                this.Game.Services.AddService(typeof(Skeleton), _skeletons[i]);
                            }
                        }
                    }
                }
            }

            calibrateSensor();

            using (DepthImageFrame depthImageFrame = SharedKinectDetector.Sensor.DepthStream.OpenNextFrame(0))
            {
                if (depthImageFrame != null)
                {
                    short[] pixelsFromFrame = new short[depthImageFrame.PixelDataLength];

                    depthImageFrame.CopyPixelDataTo(pixelsFromFrame);
                    byte[] convertedPixels = ConvertDepthFrame(pixelsFromFrame, SharedKinectDetector.Sensor.DepthStream, 640 * 480 * 4);

                    Color[] color = new Color[depthImageFrame.Height * depthImageFrame.Width];
                    kinectRGBVideo = new Texture2D(SharedSpriteBatch.GraphicsDevice, depthImageFrame.Width, depthImageFrame.Height);

                    // Set convertedPixels from the DepthImageFrame to a the datasource for our Texture2D
                    kinectRGBVideo.SetData<byte>(convertedPixels);
                }
            }


        }


        private byte[] ConvertDepthFrame(short[] depthFrame, DepthImageStream depthStream, int depthFrame32Length)
        {
            int tooNearDepth = depthStream.TooNearDepth;
            int tooFarDepth = depthStream.TooFarDepth;
            int unknownDepth = depthStream.UnknownDepth;
            byte[] depthFrame32 = new byte[depthFrame32Length];

            for (int i16 = 0, i32 = 0; i16 < depthFrame.Length && i32 < depthFrame32.Length; i16++, i32 += 4)
            {
                int player = depthFrame[i16] & DepthImageFrame.PlayerIndexBitmask;
                int realDepth = depthFrame[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(~(realDepth >> 4));

                if (player == this.playerIndex)
                {
                    // tint the intensity by dividing by per-player values
                    depthFrame32[i32 + RedIndex] = (byte)(intensity >> IntensityShiftByPlayerR[player]);
                    depthFrame32[i32 + GreenIndex] = (byte)(intensity >> IntensityShiftByPlayerG[player]);
                    depthFrame32[i32 + BlueIndex] = (byte)(intensity >> IntensityShiftByPlayerB[player]);
                }

            }


            return depthFrame32;
        }


        public void calibrateSensor()
        {
            if (_skeletons == null)
                return;

            if (!calibrated)
            {
                Skeleton skeletonChoosen = null;
                foreach (Skeleton skeleton in _skeletons)
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        skeletonChoosen = skeleton;
                    }
                }

                if (skeletonChoosen != null)
                {
                    this.SharedKinectDetector.EvaluateSkeleton(skeletonChoosen);
                    calibrated = true;
                }

            }
        }
    }
}
