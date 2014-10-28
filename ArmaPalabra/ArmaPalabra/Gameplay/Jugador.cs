using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Graphics;


using System.IO;

using LPGames.KinectToolbox;


namespace ArmaPalabra.Gameplay
{
    enum TipoMano
    {
        Izquierda = 0,
        Derecha = 1

    }

    public enum Genero
    {
        Masculino = 0,
        Femenino = 1
    }

    


    public delegate bool Jugada(Jugador jugador);


    #region Clase Mano
    public class Mano
    {

        public Microsoft.Xna.Framework.Rectangle frame;
        public ObjetoSimple objetoSostenido { get; set; }

        public Mano(int X, int Y, int width, int height)
        {
            frame = new Microsoft.Xna.Framework.Rectangle(X, Y, width, height);
            objetoSostenido = null;

        }

        public void actualizarPosicion(int X, int Y)
        {
            frame = new Microsoft.Xna.Framework.Rectangle(X, Y, frame.Width, frame.Height);

            if (objetoSostenido != null)
            {
                objetoSostenido.setPosicion(X, Y);
            }
        }

    }
    #endregion


    public class Jugador : DrawableGameComponent
    {

        #region Atributos de la clase

        private static bool skeletonDrawn = true;

        private static Skeleton[] skeletonData;

        private Skeleton skeleton;

        public Mano[] manos {get; set;}

        private Microsoft.Xna.Framework.Vector2 jointOrigin;

        private Microsoft.Xna.Framework.Vector2 headOrigin;

        private Microsoft.Xna.Framework.Vector2 manoIzqOrigin;

        private Microsoft.Xna.Framework.Vector2 manoDerOrigin;

        public Genero generoJugador;

        private Texture2D jointClothTexture;

        private Texture2D jointTexture;

        private Texture2D headTexture;

        private Texture2D manoIzqTexture;

        private Texture2D manoDerTexture;

        private Texture2D algodonTexture;

        private Microsoft.Xna.Framework.Vector2 boneOrigin;

        private Texture2D boneTexture;

        private Jugada jugadaPrincipal;

        private SpriteFont font;

        public bool accionRealizada { get; set; }

        private bool initialized;

        private bool contentLoaded;
        
        public static bool juntoManos = false;

        public bool hayJugador = false;

        private float longitudProporcion;

        #endregion

        #region Constructor e inicializador
        public Jugador(Game game,Jugada jugada,Genero genero)
            : base(game)
        {
            manos = new Mano[2];
            jugadaPrincipal = jugada;
            accionRealizada = false;
            contentLoaded = false;
            generoJugador = genero;
            longitudProporcion = 1.0f;
         //   this.LoadGestureDetector(Path.Combine(Environment.CurrentDirectory, @"AppResources\GestureResource\circleKB.save"), "Circle");
          
            this.LoadContent();
        }


        public override void Initialize()
        {
            base.Initialize();
            this.initialized = true;
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            font = Game.Content.Load<SpriteFont>("Segoe16");
            this.jointTexture = Game.Content.Load<Texture2D>("Joint");
            
            this.headTexture = Game.Content.Load<Texture2D>((generoJugador == Genero.Masculino)?"cabeza":"cabeza2");
            this.manoIzqTexture = Game.Content.Load<Texture2D>("mano-izq");
            this.manoDerTexture = Game.Content.Load<Texture2D>("mano-der");
            this.algodonTexture = Game.Content.Load<Texture2D>("clothTexture");
            this.jointClothTexture = Game.Content.Load<Texture2D>("JointCloth");
            this.jointOrigin = new Microsoft.Xna.Framework.Vector2(this.jointTexture.Width / 2, this.jointTexture.Height / 2);
            this.headOrigin = new Microsoft.Xna.Framework.Vector2((float)this.headTexture.Width/2,2*(float)this.headTexture.Height/3);
            for (int i = 0; i < manos.Length; i++)
			{
                //fuera de pantalla
                manos[i] = new Mano(-100,-100*i, manoIzqTexture.Width,manoIzqTexture.Height);
			}
            
            this.boneTexture = Game.Content.Load<Texture2D>("Bone");
            this.boneOrigin = new Microsoft.Xna.Framework.Vector2(0.0f, 0.0f);
            this.manoIzqOrigin = new Microsoft.Xna.Framework.Vector2(0.45f*this.manoIzqTexture.Width,0);
            this.manoDerOrigin = new Microsoft.Xna.Framework.Vector2(0.2f*this.manoDerTexture.Width, 0);
           contentLoaded = true;
        }

        #endregion 

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


        #region Renderizado del cuerpo
        public override void Draw(GameTime gameTime)
        {

            /*if (!contentLoaded)
            {
                this.LoadContent();
                return;
            }*/


            if (null == skeletonData)
            {
                return;
            }

            if (null == this.skeleton)
            {
                this.hayJugador = false;
                LiberarJugador();
                return;
            }


            if (false == this.initialized)
            {
                this.Initialize();
            }

            hayJugador = true;




            switch (skeleton.TrackingState)
            {
                case SkeletonTrackingState.Tracked:


                    Color jointColor = Color.FromNonPremultiplied(255, 217, 191, 255);
                    Microsoft.Xna.Framework.Vector2 diff = this.SkeletonToColorMap(skeleton.Joints[JointType.ShoulderCenter].Position) - this.SkeletonToColorMap(skeleton.Joints[JointType.Head].Position);
                    //System.Diagnostics.Debug.WriteLine("proporcion : {0}", diff.Length());

                    longitudProporcion = (diff.Length()) / 53.0f;

                    this.SharedSpriteBatch.Begin();


                    // Now draw the joints
                    foreach (Joint j in skeleton.Joints)
                    {
                        //if (j.JointType == JointType.ShoulderLeft) jointColor = Color.Brown;
                        Microsoft.Xna.Framework.Vector2 posicion = this.SkeletonToColorMap(j.Position);
                        if (j.JointType != JointType.ShoulderLeft && j.JointType != JointType.ShoulderRight)
                        {

                            this.SharedSpriteBatch.Draw(
                        this.jointTexture,
                        posicion,
                        null,
                        jointColor,
                        0.0f,
                        this.jointOrigin,
                        1.0f * longitudProporcion,
                        SpriteEffects.None,
                        0.0f);
                        }
                        /*
                        if (j.JointType == JointType.HandRight)
                        {
                            capturaDerechaGestureRecognizer.Add(j.Position, SharedKinectDetector.Sensor);

                        }
                        else if (j.JointType == JointType.HandLeft)
                        {
                            capturaIzquierdaGestureRecognizer.Add(j.Position, SharedKinectDetector.Sensor);
                        }*/
                    }

                    // Draw Bones

                    this.DrawBone(skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
                    this.DrawBone(skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
                    this.DrawBone(skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

                    this.DrawBone(skeleton.Joints, JointType.HipRight, JointType.KneeRight);
                    this.DrawBone(skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
                    this.DrawBone(skeleton.Joints, JointType.AnkleRight, JointType.FootRight);


                    this.DrawBone(skeleton.Joints, JointType.Head, JointType.ShoulderCenter);
                    //this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderLeft);
                    // this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderRight);
                    this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.Spine);
                    this.DrawBone(skeleton.Joints, JointType.Spine, JointType.HipCenter);
                    this.DrawBone(skeleton.Joints, JointType.HipCenter, JointType.HipLeft);
                    this.DrawBone(skeleton.Joints, JointType.HipCenter, JointType.HipRight);

                    // this.DrawBone(skeleton.Joints, JointType.ShoulderLeft, JointType.ElbowLeft);

                    //  this.DrawBone(skeleton.Joints, JointType.ShoulderRight, JointType.ElbowRight);


                    this.DrawPolo(skeleton.Joints);



                    this.DrawPantalon(skeleton.Joints);

                    this.DrawBone(skeleton.Joints, JointType.ElbowLeft, JointType.WristLeft);

                    this.SharedSpriteBatch.Draw(
                            this.jointTexture,
                            this.SkeletonToColorMap(skeleton.Joints[JointType.WristLeft].Position),
                            null,
                            jointColor,
                            0.0f,
                            this.jointOrigin,
                            1.0f * longitudProporcion,
                            SpriteEffects.None,
                            0.0f);
                    this.DrawHand(skeleton.Joints, JointType.WristLeft, JointType.HandLeft, TipoMano.Izquierda);


                    this.DrawBone(skeleton.Joints, JointType.ElbowRight, JointType.WristRight);
                    this.SharedSpriteBatch.Draw(
                            this.jointTexture,
                            this.SkeletonToColorMap(skeleton.Joints[JointType.WristRight].Position),
                            null,
                            jointColor,
                            0.0f,
                            this.jointOrigin,
                            1.0f * longitudProporcion,
                            SpriteEffects.None,
                            0.0f);

                    this.DrawHand(skeleton.Joints, JointType.WristRight, JointType.HandRight, TipoMano.Derecha);




                    this.SharedSpriteBatch.Draw(
                this.headTexture,
                this.SkeletonToColorMap(skeleton.Joints[JointType.Head].Position),
                null,
                Color.White,
                0.0f,
                this.headOrigin,
                1.0f * longitudProporcion,
                SpriteEffects.None,
                0.0f);

                    this.SharedSpriteBatch.End();
                    skeletonDrawn = true;

                    break;

                case SkeletonTrackingState.NotTracked:
                    hayJugador = false;
                    break;


                    
            }

            base.Draw(gameTime);
        }

        private Microsoft.Xna.Framework.Vector2 SkeletonToColorMap(SkeletonPoint point)
        {
            if ((null != SharedKinectDetector.Sensor) && (null != SharedKinectDetector.Sensor.ColorStream))
            {
                // This is used to map a skeleton point to the color image location
                var colorPt = SharedKinectDetector.Sensor.CoordinateMapper.MapSkeletonPointToColorPoint(point, SharedKinectDetector.Sensor.ColorStream.Format);
                int desplazamientoX = this.Game.GraphicsDevice.Viewport.Width / 2 - 320;
                int desplazamientoY = this.Game.GraphicsDevice.Viewport.Height / 2 - 240; 
                return new Microsoft.Xna.Framework.Vector2(colorPt.X + desplazamientoX, colorPt.Y+desplazamientoY);
            }

            return Microsoft.Xna.Framework.Vector2.Zero;
        }

        private void DrawPantalon(JointCollection joints)
        {
            Color pantalonColor = Color.FromNonPremultiplied(42, 82, 214, 255);

            this.SharedSpriteBatch.Draw(
                               this.jointClothTexture,
                               this.SkeletonToColorMap(joints[JointType.KneeLeft].Position) + new Microsoft.Xna.Framework.Vector2(13.0f * longitudProporcion, 0.0f),
                               null,
                               pantalonColor,
                               0.0f,


                               this.jointOrigin,
                               2.5f * longitudProporcion,
                               SpriteEffects.None,
                               0.0f);

            this.SharedSpriteBatch.Draw(
                               this.jointClothTexture,
                               this.SkeletonToColorMap(joints[JointType.KneeRight].Position) + new Microsoft.Xna.Framework.Vector2(13.0f * longitudProporcion, 0.0f),
                               null,
                               pantalonColor,
                               0.0f,
                               this.jointOrigin,
                               2.5f * longitudProporcion,
                               SpriteEffects.None,
                               0.0f);

            Microsoft.Xna.Framework.Vector2[] piernaIzqVertices = new Microsoft.Xna.Framework.Vector2[]{ 
           
              this.SkeletonToColorMap(joints[JointType.HipCenter].Position)+new Microsoft.Xna.Framework.Vector2(-25.0f*longitudProporcion,0.0f),
              this.SkeletonToColorMap(joints[JointType.KneeLeft].Position),
              this.SkeletonToColorMap(joints[JointType.AnkleLeft].Position)
                
            };

            this.DrawLines(piernaIzqVertices, pantalonColor, 5.0f * longitudProporcion);

            Microsoft.Xna.Framework.Vector2[] piernaDerVertices = new Microsoft.Xna.Framework.Vector2[]{ 
           
              this.SkeletonToColorMap(joints[JointType.HipCenter].Position),
              this.SkeletonToColorMap(joints[JointType.KneeRight].Position),
              this.SkeletonToColorMap(joints[JointType.AnkleRight].Position)
                
            };
            this.DrawLines(piernaDerVertices, pantalonColor, 5.0f * longitudProporcion);


            Microsoft.Xna.Framework.Vector2[] cinturaVertices = new Microsoft.Xna.Framework.Vector2[]
            { 
                this.SkeletonToColorMap(joints[JointType.HipLeft].Position)+new Microsoft.Xna.Framework.Vector2(-10.0f*longitudProporcion,0),
                this.SkeletonToColorMap(joints[JointType.HipRight].Position)+new Microsoft.Xna.Framework.Vector2(10.0f*longitudProporcion,0)
            };



            this.DrawLines(cinturaVertices, pantalonColor, 5.0f * longitudProporcion);

        }

        private void DrawPolo(JointCollection joints)
        {

            Color poloColor = (generoJugador == Genero.Masculino) ? Color.FromNonPremultiplied(148, 218, 10, 255) : Color.FromNonPremultiplied(255, 58, 182, 255);

            Microsoft.Xna.Framework.Vector2[] poloSubVertices = new Microsoft.Xna.Framework.Vector2[]{ 
            
              this.SkeletonToColorMap(joints[JointType.ShoulderCenter].Position)+new Microsoft.Xna.Framework.Vector2(0,26.0f*longitudProporcion),
              this.SkeletonToColorMap(joints[JointType.ShoulderLeft].Position)+new Microsoft.Xna.Framework.Vector2(31.0f*longitudProporcion,-6.0f*longitudProporcion),
                this.SkeletonToColorMap(joints[JointType.HipLeft].Position)+new Microsoft.Xna.Framework.Vector2(0,(this.SkeletonToColorMap(joints[JointType.ShoulderLeft].Position)-this.SkeletonToColorMap(joints[JointType.HipLeft].Position)).Y/2)+new Microsoft.Xna.Framework.Vector2(16.0f*longitudProporcion,0),
              this.SkeletonToColorMap(joints[JointType.HipLeft].Position)+new Microsoft.Xna.Framework.Vector2(22.0f*longitudProporcion,-22.0f*longitudProporcion),
              this.SkeletonToColorMap(joints[JointType.HipRight].Position)+new Microsoft.Xna.Framework.Vector2(-22.0f*longitudProporcion,-22.0f*longitudProporcion),
               this.SkeletonToColorMap(joints[JointType.HipRight].Position)+new Microsoft.Xna.Framework.Vector2(0,(this.SkeletonToColorMap(joints[JointType.ShoulderRight].Position)-this.SkeletonToColorMap(joints[JointType.HipRight].Position)).Y/2)+new Microsoft.Xna.Framework.Vector2(-16.0f*longitudProporcion,0),
            
              this.SkeletonToColorMap(joints[JointType.ShoulderRight].Position)+new Microsoft.Xna.Framework.Vector2(-31.0f*longitudProporcion,-6.0f*longitudProporcion),
               this.SkeletonToColorMap(joints[JointType.ShoulderCenter].Position)+new Microsoft.Xna.Framework.Vector2(0,31.0f*longitudProporcion)
                
            };

            this.DrawLines(poloSubVertices, poloColor, 4.0f * longitudProporcion);


            Microsoft.Xna.Framework.Vector2[] poloColumna = new Microsoft.Xna.Framework.Vector2[]{

                this.SkeletonToColorMap(joints[JointType.ShoulderCenter].Position)+new Microsoft.Xna.Framework.Vector2(0,25.0f*longitudProporcion),
                 this.SkeletonToColorMap(joints[JointType.HipCenter].Position)+new Microsoft.Xna.Framework.Vector2(-30.0f*longitudProporcion,0)
             };

            this.DrawLines(poloColumna, poloColor, 5.2f * longitudProporcion);

            Microsoft.Xna.Framework.Vector2[] poloVertices = new Microsoft.Xna.Framework.Vector2[]{ 
            
              this.SkeletonToColorMap(joints[JointType.ShoulderCenter].Position),
              this.SkeletonToColorMap(joints[JointType.ShoulderLeft].Position)+new Microsoft.Xna.Framework.Vector2(10.0f*longitudProporcion,-15.0f*longitudProporcion),
              // this.SkeletonToColorMap(joints[JointType.HipLeft].Position)+new Microsoft.Xna.Framework.Vector2(0,(this.SkeletonToColorMap(joints[JointType.ShoulderLeft].Position)-this.SkeletonToColorMap(joints[JointType.HipLeft].Position)).Y/2),
              this.SkeletonToColorMap(joints[JointType.HipLeft].Position),
              this.SkeletonToColorMap(joints[JointType.HipRight].Position),
            //   this.SkeletonToColorMap(joints[JointType.HipRight].Position)+new Microsoft.Xna.Framework.Vector2(0,(this.SkeletonToColorMap(joints[JointType.ShoulderRight].Position)-this.SkeletonToColorMap(joints[JointType.HipRight].Position)).Y/2),
              this.SkeletonToColorMap(joints[JointType.ShoulderRight].Position)+new Microsoft.Xna.Framework.Vector2(-10.0f*longitudProporcion,-15.0f*longitudProporcion),
               this.SkeletonToColorMap(joints[JointType.ShoulderCenter].Position)
                
            };
            this.DrawLines(poloVertices, poloColor, 7.0f * longitudProporcion);

            this.SharedSpriteBatch.Draw(
                               this.jointTexture,
                               this.SkeletonToColorMap(skeleton.Joints[JointType.ShoulderCenter].Position) + new Microsoft.Xna.Framework.Vector2(0.0f, -5.0f * longitudProporcion),
                               null,
                               Color.FromNonPremultiplied(255, 217, 191, 255),
                               0.0f,
                               this.jointOrigin,
                               1.0f * longitudProporcion,
                               SpriteEffects.None,
                               0.0f);



            this.SharedSpriteBatch.Draw(
                               this.jointClothTexture,
                               this.SkeletonToColorMap(joints[JointType.ShoulderLeft].Position) + new Microsoft.Xna.Framework.Vector2(10.0f * longitudProporcion, -15.0f * longitudProporcion),
                               null,
                               poloColor,
                               0.0f,
                               this.jointOrigin,
                               1.0f * longitudProporcion,
                               SpriteEffects.None,
                               0.0f);

            this.SharedSpriteBatch.Draw(
                               this.jointClothTexture,
                               this.SkeletonToColorMap(joints[JointType.ShoulderRight].Position) + new Microsoft.Xna.Framework.Vector2(-10.0f * longitudProporcion, -15.0f * longitudProporcion),
                               null,
                               poloColor,
                               0.0f,
                               this.jointOrigin,
                               1.0f * longitudProporcion,
                               SpriteEffects.None,
                               0.0f);
            /*this.SharedSpriteBatch.Draw(
                               this.jointClothTexture,
                               this.SkeletonToColorMap(joints[JointType.ShoulderCenter].Position),
                               null,
                               poloColor,
                               0.0f,
                               this.jointOrigin,
                               1.0f*longitudProporcion,
                               SpriteEffects.None,
                               0.0f);*/


            Microsoft.Xna.Framework.Vector2[] mangaIzqVertices = new Microsoft.Xna.Framework.Vector2[]{ 
            
               this.SkeletonToColorMap(joints[JointType.ShoulderLeft].Position)+new Microsoft.Xna.Framework.Vector2(10.0f*longitudProporcion,-15.0f*longitudProporcion),
               this.SkeletonToColorMap(joints[JointType.ElbowLeft].Position),
   
            };

            this.DrawLines(mangaIzqVertices, poloColor, 2.5f * longitudProporcion);


            Microsoft.Xna.Framework.Vector2[] mangaDerVertices = new Microsoft.Xna.Framework.Vector2[]{ 
            
               this.SkeletonToColorMap(joints[JointType.ShoulderRight].Position)+new Microsoft.Xna.Framework.Vector2(-10.0f*longitudProporcion,-15.0f*longitudProporcion),
                this.SkeletonToColorMap(joints[JointType.ElbowRight].Position),
   
            };

            this.DrawLines(mangaDerVertices, poloColor, 2.5f * longitudProporcion);




        }

        private void DrawLines(Microsoft.Xna.Framework.Vector2[] vertices, Color color, float escala)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                int firstIndex = i;

                int secondIndex = (i + 1 >= vertices.Length) ? 0 : i + 1;

                if (secondIndex == 0)
                    break;

                this.DrawLine(vertices[firstIndex], vertices[secondIndex], color, escala);
            }
        }


        private void DrawLine(Microsoft.Xna.Framework.Vector2 startVector, Microsoft.Xna.Framework.Vector2 endVector, Color color, float escala)
        {

            Microsoft.Xna.Framework.Vector2 tempDiff = endVector - startVector;
            float tempAngle = (float)Math.Atan2(tempDiff.Y, tempDiff.X) - Microsoft.Xna.Framework.MathHelper.PiOver2;
            int tempRadio = this.jointTexture.Height / 2;


            Microsoft.Xna.Framework.Vector2 start = new Microsoft.Xna.Framework.Vector2(startVector.X - tempRadio * (float)Math.Cos((double)tempAngle), startVector.Y - tempRadio * (float)Math.Sin((double)tempAngle));
            Microsoft.Xna.Framework.Vector2 end = new Microsoft.Xna.Framework.Vector2(endVector.X - tempRadio * (float)Math.Cos((double)tempAngle), endVector.Y - tempRadio * (float)Math.Sin((double)tempAngle));
            Microsoft.Xna.Framework.Vector2 diff = end - start;
            Microsoft.Xna.Framework.Vector2 scale = new Microsoft.Xna.Framework.Vector2(escala, (diff.Length() / this.algodonTexture.Height));
            //diff.Length() / this.boneTexture.Height
            float angle = (float)Math.Atan2(diff.Y, diff.X) - Microsoft.Xna.Framework.MathHelper.PiOver2;

            // Color color = Color.Blue;

            this.SharedSpriteBatch.Draw(this.algodonTexture, start, null, color, angle, this.boneOrigin, scale, SpriteEffects.None, 1.0f);


        }

        private void DrawBone(JointCollection joints, JointType startJoint, JointType endJoint)
        {


            Microsoft.Xna.Framework.Vector2 tempStart = this.SkeletonToColorMap(joints[startJoint].Position);
            Microsoft.Xna.Framework.Vector2 tempEnd = this.SkeletonToColorMap(joints[endJoint].Position);
            Microsoft.Xna.Framework.Vector2 tempDiff = tempEnd - tempStart;
            float tempAngle = (float)Math.Atan2(tempDiff.Y, tempDiff.X) - Microsoft.Xna.Framework.MathHelper.PiOver2;
            int tempRadio = this.jointTexture.Height / 2;


            Microsoft.Xna.Framework.Vector2 start = new Microsoft.Xna.Framework.Vector2(tempStart.X - tempRadio * (float)Math.Cos((double)tempAngle), tempStart.Y - tempRadio * (float)Math.Sin((double)tempAngle));
            Microsoft.Xna.Framework.Vector2 end = new Microsoft.Xna.Framework.Vector2(tempEnd.X - tempRadio * (float)Math.Cos((double)tempAngle), tempEnd.Y - tempRadio * (float)Math.Sin((double)tempAngle));
            Microsoft.Xna.Framework.Vector2 diff = end - start;
            Microsoft.Xna.Framework.Vector2 scale = new Microsoft.Xna.Framework.Vector2(4.0f * longitudProporcion, (diff.Length() / this.boneTexture.Height));
            //diff.Length() / this.boneTexture.Height
            float angle = (float)Math.Atan2(diff.Y, diff.X) - Microsoft.Xna.Framework.MathHelper.PiOver2;

            Color color = Color.FromNonPremultiplied(255, 217, 191, 255);
            if (joints[startJoint].TrackingState != JointTrackingState.Tracked ||
                joints[endJoint].TrackingState != JointTrackingState.Tracked)
            {
                color = Color.FromNonPremultiplied(255, 217, 191, 240);
            }

            this.SharedSpriteBatch.Draw(this.boneTexture, start, null, color, angle, this.boneOrigin, scale, SpriteEffects.None, 1.0f);
        }


        private void DrawHand(JointCollection joints, JointType startJoint, JointType endJoint, TipoMano tipoMano)
        {
            Microsoft.Xna.Framework.Vector2 tempStart = this.SkeletonToColorMap(joints[startJoint].Position);
            Microsoft.Xna.Framework.Vector2 tempEnd = this.SkeletonToColorMap(joints[endJoint].Position);
            Microsoft.Xna.Framework.Vector2 tempDiff = tempEnd - tempStart;
            float tempAngle = (float)Math.Atan2(tempDiff.Y, tempDiff.X) - Microsoft.Xna.Framework.MathHelper.PiOver2;
            int tempRadio = this.jointTexture.Height / 2;

            Microsoft.Xna.Framework.Vector2 start = new Microsoft.Xna.Framework.Vector2(tempStart.X - tempRadio * (float)Math.Cos((double)tempAngle), tempStart.Y - tempRadio * (float)Math.Sin((double)tempAngle));
            Microsoft.Xna.Framework.Vector2 end = new Microsoft.Xna.Framework.Vector2(tempEnd.X - tempRadio * (float)Math.Cos((double)tempAngle), tempEnd.Y - tempRadio * (float)Math.Sin((double)tempAngle));
            Microsoft.Xna.Framework.Vector2 diff = end - start;
            Microsoft.Xna.Framework.Vector2 scale = new Microsoft.Xna.Framework.Vector2(1.0f * longitudProporcion, 1.0f * longitudProporcion);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - Microsoft.Xna.Framework.MathHelper.PiOver2;

            Texture2D manoTextura = (tipoMano == TipoMano.Izquierda) ? manoIzqTexture : manoDerTexture;
            Microsoft.Xna.Framework.Vector2 manoOrigen = (tipoMano == TipoMano.Izquierda) ? manoIzqOrigin : manoDerOrigin;

            this.SharedSpriteBatch.Draw(manoTextura, start, null, Color.White, angle, manoOrigen, scale, SpriteEffects.None, 1.0f);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // If the sensor is not found, not running, or not connected, stop now
            if (null == SharedKinectDetector.Sensor ||
                false == SharedKinectDetector.Sensor.IsRunning ||
                KinectStatus.Connected != SharedKinectDetector.Sensor.Status)
            {
                return;
            }

            if (!contentLoaded)
            {
                return;
            }

            // If we have already drawn this skeleton, then we should retrieve a new frame
            // This prevents us from calling the next frame more than once per update
            if (skeletonDrawn)
            {
                using (var skeletonFrame = SharedKinectDetector.Sensor.SkeletonStream.OpenNextFrame(0))
                {
                    // Sometimes we get a null frame back if no data is ready
                    if (null == skeletonFrame)
                    {
                        return;
                    }

                    // Reallocate if necessary
                    if (null == skeletonData || skeletonData.Length != skeletonFrame.SkeletonArrayLength)
                    {
                        skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(skeletonData);

                    Skeleton tempSkeleton = (from s in skeletonData
                                             where s != null && s.TrackingState == SkeletonTrackingState.Tracked
                                             select s).FirstOrDefault();

                    if (tempSkeleton != null)
                    {
                            skeleton = tempSkeleton;
                    }
                    else
                    {
                        hayJugador = false;
                    }

                    if (skeleton != null)
                    {
                        //skeletonDrawn = false;
                        Microsoft.Xna.Framework.Vector2 posicionIzq = this.SkeletonToColorMap(skeleton.Joints[JointType.HandLeft].Position) + new Microsoft.Xna.Framework.Vector2(-1 * manos[(int)TipoMano.Izquierda].frame.Width / 2, -1 * manos[(int)TipoMano.Izquierda].frame.Height / 2);
                        manos[(int)TipoMano.Izquierda].actualizarPosicion((int)posicionIzq.X, (int)posicionIzq.Y);
                        Microsoft.Xna.Framework.Vector2 posicionDer = this.SkeletonToColorMap(skeleton.Joints[JointType.HandRight].Position) + new Microsoft.Xna.Framework.Vector2(-1 * manos[(int)TipoMano.Derecha].frame.Width / 2, -1 * manos[(int)TipoMano.Derecha].frame.Height / 2);
                        manos[(int)TipoMano.Derecha].actualizarPosicion((int)posicionDer.X, (int)posicionDer.Y);


                        if (jugadaPrincipal != null)
                        {
                            jugadaPrincipal(this);
                        }
                    }


                 

                }


            }
        }
        #endregion

        #region Reconocimiento de Gestos
    

        #endregion


        public void LiberarJugador()
        {
            for (int i = 0; i < manos.Length; i++)
            {
                if (manos[i].objetoSostenido != null)
                {
                   // manos[i].objetoSostenido.LiberarGlobo();
                    manos[i].objetoSostenido = null;
                }
            }
        }

        public bool LevantaManoIzquierda()
        {
            Microsoft.Xna.Framework.Vector2 manoIzquierda = SkeletonToColorMap(skeleton.Joints[JointType.HandLeft].Position);
            Microsoft.Xna.Framework.Vector2 codoIzquierda = SkeletonToColorMap(skeleton.Joints[JointType.ElbowLeft].Position);
            Microsoft.Xna.Framework.Vector2 hombroIzquierda = SkeletonToColorMap(skeleton.Joints[JointType.ShoulderLeft].Position);
            double componenteDenom1 = Math.Sqrt(Math.Pow(manoIzquierda.X,2.0)+Math.Pow(hombroIzquierda.X,2.0));
            double componenteDenom2 = Math.Sqrt(Math.Pow(manoIzquierda.Y,2.0)+Math.Pow(hombroIzquierda.Y,2.0));
            double componenteNum1 = (double)manoIzquierda.X * hombroIzquierda.X;
            double componenteNum2 = (double)manoIzquierda.Y * hombroIzquierda.Y;

            double anguloIzquierda = Math.Acos(( componenteNum1+componenteNum2 ) / (componenteDenom1 * componenteDenom2));

            
             
            if (manoIzquierda.Y < codoIzquierda.Y && codoIzquierda.Y<=hombroIzquierda.Y  /*&& anguloIzquierda>=Math.PI/2.0*/)
            {
                return true;
            }

            return false;
        }

        public bool LevantaManoDerecha()
        {
            Microsoft.Xna.Framework.Vector2 manoDerecha = SkeletonToColorMap(skeleton.Joints[JointType.HandRight].Position);
            Microsoft.Xna.Framework.Vector2 codoDerecha = SkeletonToColorMap(skeleton.Joints[JointType.ElbowRight].Position);
            Microsoft.Xna.Framework.Vector2 hombroDerecha = SkeletonToColorMap(skeleton.Joints[JointType.ShoulderRight].Position);


            double componenteDenom1 = Math.Sqrt(Math.Pow(manoDerecha.X, 2.0) + Math.Pow(hombroDerecha.X, 2.0));
            double componenteDenom2 = Math.Sqrt(Math.Pow(manoDerecha.Y, 2.0) + Math.Pow(hombroDerecha.Y, 2.0));
            double componenteNum1 = (double)manoDerecha.X * hombroDerecha.X;
            double componenteNum2 = (double)manoDerecha.Y * hombroDerecha.Y;

            double anguloIzquierda = Math.Acos((componenteNum1 + componenteNum2) / (componenteDenom1 * componenteDenom2));


            if (manoDerecha.Y < codoDerecha.Y && codoDerecha.Y <= hombroDerecha.Y /*&& anguloIzquierda >= Math.PI / 2.0*/)
            {
                return true;
            }

            return false;
        }

        public void LimpiarManos()
        {
            for (int i = 0; i < manos.Length; i++)
            {
                //manos[i].objetoSostenido.ReiniciarGlobo();
                if (manos[i].objetoSostenido != null)
                {
                    manos[i].objetoSostenido.Dispose();
                    manos[i].objetoSostenido = null;
                }
            }
        }
    }
}
