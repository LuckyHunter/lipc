using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using System.IO;

namespace LPGames.KinectToolbox
{

    public class KinectDetector : DrawableGameComponent
    {
        private readonly Dictionary<KinectStatus, string> statusDictionary = new Dictionary<KinectStatus, string>();

        public KinectSensor Sensor { get; private set; }

        public KinectStatus LastStatus { get; private set; }

        private Texture2D chooserBackground;

        private SpriteFont font;

        private int currentElevation;

        private bool isElevationSet;

        public KinectDetector(Game game)
            : base(game)
        {
            currentElevation = 0;
            isElevationSet = false;
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.DiscoverSensor();

            this.statusDictionary.Add(KinectStatus.Undefined, "No conectado, o en uso");
            this.statusDictionary.Add(KinectStatus.Connected, string.Empty);
            this.statusDictionary.Add(KinectStatus.DeviceNotGenuine, "Dispositivo no original");
            this.statusDictionary.Add(KinectStatus.DeviceNotSupported, "Dispositivo no soportado");
            this.statusDictionary.Add(KinectStatus.Disconnected, "Requerido");
            this.statusDictionary.Add(KinectStatus.Error, "Error");
            this.statusDictionary.Add(KinectStatus.Initializing, "Iniciando...");
            this.statusDictionary.Add(KinectStatus.InsufficientBandwidth, "Ancho de banda insuficiente");
            this.statusDictionary.Add(KinectStatus.NotPowered, "Verifique la conexión de energía");
            this.statusDictionary.Add(KinectStatus.NotReady, "Dispositivo aún no está preparado");

        }


        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }


        protected override void LoadContent()
        {
            base.LoadContent();

            this.chooserBackground = Game.Content.Load<Texture2D>("ChooserBackground");
            this.font = Game.Content.Load<SpriteFont>("Segoe16");
        }

        /// <summary>
        /// This method ensures that the KinectSensor is stopped before exiting.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            // Always stop the sensor when closing down
            if (null != this.Sensor)
            {
                this.Sensor.Stop();
            }
        }

        private void DiscoverSensor()
        {
            this.Sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (null != this.Sensor)
            {
                this.LastStatus = this.Sensor.Status;

                if (this.LastStatus == KinectStatus.Connected)
                {

                    try
                    {
                        this.Sensor.SkeletonStream.Enable();

                        try
                        {
                            this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                            var parameters = new TransformSmoothParameters
                            {

                                Smoothing = 0.5f,
                                Correction = 0.5f,
                                Prediction = 0.5f,
                                JitterRadius = 0.05f,
                                MaxDeviationRadius = 0.04f
                            };

                            this.Sensor.SkeletonStream.Enable(parameters);

                            this.Sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                            this.Sensor.Start();

                            this.Sensor.ElevationAngle = 0;
                        }
                        catch (IOException)
                        {
                            this.Sensor = null;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        this.Sensor = null;
                    }
                }
            }
            else
            {
                this.LastStatus = KinectStatus.Disconnected;
            }
        }


        private Vector2 SkeletonToColorMap(KinectSensor Sensor, SkeletonPoint point)
        {
            if ((null != Sensor) && (null != Sensor.ColorStream))
            {
                // This is used to map a skeleton point to the color image location
                var colorPt = Sensor.CoordinateMapper.MapSkeletonPointToColorPoint(point, this.Sensor.ColorStream.Format);
                return new Vector2(colorPt.X, colorPt.Y);
            }

            return Vector2.Zero;
        }

        public void EvaluateSkeleton(Skeleton skeleton)
        {
            JointCollection joints = skeleton.Joints;
            currentElevation = 0;
            foreach (Joint joint in joints)
            {
                Vector2 jointScreenLocation = this.SkeletonToColorMap(this.Sensor, joint.Position);
                switch (joint.JointType)
                {
                    case JointType.Head:
                        if (joint.TrackingState == JointTrackingState.Tracked)
                        {
                            if (jointScreenLocation.Y >= 140)
                            {
                                //System.Diagnostics.Debug.WriteLine("UBICACION: {0}", jointScreenLocation.Y);

                                currentElevation -= (int)(jointScreenLocation.Y / 20);
                            }
                        }


                        break;

                    case JointType.FootLeft:
                    case JointType.FootRight:

                        if (joint.TrackingState == JointTrackingState.Tracked)
                        {
                            if (jointScreenLocation.Y <= 240)
                            {
                                currentElevation += (int)((480 - jointScreenLocation.Y) / 10);
                            }
                        }
                        break;
                }
            }

            if (!isElevationSet)
            {
                this.Sensor.ElevationAngle = (currentElevation > Sensor.MaxElevationAngle) ? Sensor.MaxElevationAngle : (currentElevation < Sensor.MinElevationAngle) ? Sensor.MinElevationAngle : currentElevation;
                isElevationSet = true;
            }
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Status != KinectStatus.Connected)
            {
                e.Sensor.Stop();
            }

            this.LastStatus = e.Status;
        }

        public override void Draw(GameTime gameTime)
        {

            if (null == this.Sensor || this.LastStatus != KinectStatus.Connected)
            {
                this.SharedSpriteBatch.Begin();

                // Render the background
                this.SharedSpriteBatch.Draw(
                    this.chooserBackground,
                    new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2),
                    null,
                    Color.White,
                    0,
                    new Vector2(this.chooserBackground.Width / 2, this.chooserBackground.Height / 2),
                    1,
                    SpriteEffects.None,
                    0);


                string mensaje = this.statusDictionary[KinectStatus.Undefined];
                if (this.Sensor != null)
                {
                    mensaje = this.statusDictionary[this.LastStatus];
                }


                Vector2 size = this.font.MeasureString(mensaje);
                this.SharedSpriteBatch.DrawString(
                    this.font,
                    mensaje,
                    new Vector2((Game.GraphicsDevice.Viewport.Width - size.X) / 2, (Game.GraphicsDevice.Viewport.Height / 2) + size.Y),
                    Color.White);
                this.SharedSpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }

}
