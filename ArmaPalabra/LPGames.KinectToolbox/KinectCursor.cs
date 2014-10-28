using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace LPGames.KinectToolbox
{
    public class KinectCursor : DrawableGameComponent
    {
        Texture2D cursorTexture;
        public Rectangle frame;
        public Vector2 currentPosition;
        Skeleton[] SkeletonData;
        private static bool cursorDibujado = true;

        public KinectCursor(Game game, Texture2D cursorTexture, int width, int height)
            : base(game)
        {
            frame = new Rectangle(0, 0, width, height);
            this.cursorTexture = cursorTexture;
            currentPosition = new Vector2(-1 * width, 0);

        }

        public KinectCursor(Game game, Texture2D cursorTexture)
            : base(game)
        {
            frame = new Rectangle(0, 0, cursorTexture.Width, cursorTexture.Height);
            this.cursorTexture = cursorTexture;
            currentPosition = Vector2.Zero;

        }

        public Vector2 CurrentPosition
        {
            set
            {
                currentPosition = value;
                frame = new Rectangle((int)currentPosition.X - frame.Width / 2, (int)currentPosition.Y - frame.Height / 2, frame.Width, frame.Height);
            }
        }

        private KinectDetector SharedKinectDetector
        {
            get
            {
                return (KinectDetector)this.Game.Services.GetService(typeof(KinectDetector));
            }
        }

        private SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }

        public override void Draw(GameTime gameTime)
        {




            SharedSpriteBatch.Begin();
            SharedSpriteBatch.Draw(
                this.cursorTexture,
                    currentPosition,
                    null,
                    Color.Violet,
                    0,
                    new Vector2(this.cursorTexture.Width / 2, this.cursorTexture.Height / 2),
                    (float)frame.Width / (float)this.cursorTexture.Width,
                    SpriteEffects.None,
                    0);
            SharedSpriteBatch.End();
            cursorDibujado = true;

            base.Draw(gameTime);
        }

        private Skeleton SharedSkeleton
        {
            get
            {
                return (Skeleton)this.Game.Services.GetService(typeof(Skeleton));
            }
        }

        public override void Update(GameTime gameTime)
        {

            if (null == this.SharedKinectDetector || null == this.SharedKinectDetector.Sensor || !this.SharedKinectDetector.Sensor.IsRunning || this.SharedKinectDetector.Sensor.Status != KinectStatus.Connected)
            {
                this.Visible = false;
                return;
            }
            if (cursorDibujado)
            {

                if (SharedSkeleton != null)
                {
                    Joint jointManoDerecha = SharedSkeleton.Joints[JointType.HandRight];
                    Joint jointColumna = SharedSkeleton.Joints[JointType.Spine];
                    Vector2 vectorManoDerecha = new Vector2(jointManoDerecha.Position.X,jointManoDerecha.Position.Y);
                    Vector2 vectorColumna = new Vector2(jointColumna.Position.X,jointColumna.Position.Y);
                    Vector2 normalManoDerecha = vectorManoDerecha - vectorColumna;
                    Vector2 ajustadaManoDerecha = KinectHelper.GetVectorScale(normalManoDerecha,this.Game.GraphicsDevice.Viewport.Width,this.Game.GraphicsDevice.Viewport.Height);
                    currentPosition.X = ajustadaManoDerecha.X;
                    currentPosition.Y = ajustadaManoDerecha.Y;

                    frame = new Rectangle((int)currentPosition.X - frame.Width / 2, (int)currentPosition.Y - frame.Height / 2, frame.Width, frame.Height);
                    this.Visible = true;

                }

            }

            base.Update(gameTime);
        }

    }    
}
