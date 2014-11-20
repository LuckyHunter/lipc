using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using LPGames.KinectToolbox;

namespace ArmaPalabra.Controls 
{
    public enum EstadoBoton
    {
        Normal = 0,
        Seleccionado = 1,
        Resaltado = 2
    }
    public class ButtonEventArgs : EventArgs
    {
        public EstadoBoton estadoActual;


        public ButtonEventArgs(Button boton)
            : base()
        {
            estadoActual = boton.estado;
        }
    }

    public class Button : DrawableGameComponent
    {



        public EstadoBoton estado;
        public int tag { get; set; }
        private const double tiempoSeleccion = 2000.0;
        Color tintColor;
        Texture2D buttonTexture;
        public Rectangle frame;
        private Rectangle frameResaltado;
        private Texture2D buttonResaltadoTexture;
        private EstadoBoton ultimoEstado;
        DateTime inicioSeleccionTiempo;
        DateTime finSeleccionTiempo;
        Song aparecerSonido;
        public event EventHandler<ButtonEventArgs> presionadoEventArgs;

        public Button(int X, int Y, Texture2D buttonTxt2d, int width, int height, Game game)
            : base(game)
        {
            this.inicioSeleccionTiempo = finSeleccionTiempo = DateTime.Now;
            this.buttonTexture = buttonTxt2d;
            this.estado = EstadoBoton.Normal;
            this.frame = new Rectangle(X, Y, width, height);
            this.frameResaltado = new Rectangle(X, Y, 0, height);
            this.tintColor = Color.White;
            buttonResaltadoTexture = new Texture2D(this.Game.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);
            buttonResaltadoTexture.SetData(new[] { new Color(Color.Green.R, Color.Green.G, Color.Green.B, 0.2f) });
            aparecerSonido = this.Game.Content.Load<Song>("sonidos/aparecer");

        }

        public KinectCursor SharedKinectCursor
        {
            get
            {
                return (KinectCursor)this.Game.Services.GetService(typeof(KinectCursor));
            }
        }

        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }


        public override void Update(GameTime gameTime)
        {
            if (frame.Intersects(SharedKinectCursor.frame))
            {
                tintColor = Color.LightGray;
                finSeleccionTiempo = DateTime.Now;
                TimeSpan intervaloTiempo = finSeleccionTiempo - inicioSeleccionTiempo;
                MediaPlayer.IsRepeating = false;
                if (intervaloTiempo.TotalMilliseconds <= 17 && MediaPlayer.State != MediaState.Playing)
                {
                    //MediaPlayer.Play(aparecerSonido);
                }

                if (intervaloTiempo.TotalMilliseconds < tiempoSeleccion)
                {
                    ultimoEstado = estado;
                    estado = EstadoBoton.Resaltado;
                    float milisegundosTranscurridos = (float)intervaloTiempo.TotalMilliseconds;
                    float porcentajeTranscurrido = milisegundosTranscurridos / (float)tiempoSeleccion;
                    frameResaltado = new Rectangle(frameResaltado.X, frameResaltado.Y, (int)((float)frame.Width * porcentajeTranscurrido), frameResaltado.Height);
                }
                else if (intervaloTiempo.TotalMilliseconds >= tiempoSeleccion)
                {
                    ultimoEstado = estado;
                    estado = EstadoBoton.Seleccionado;
                    inicioSeleccionTiempo = finSeleccionTiempo = DateTime.Now;
                }
                presionadoEventArgs(this, new ButtonEventArgs(this));
            }
            else
            {
                if (MediaPlayer.State == MediaState.Playing && ultimoEstado == EstadoBoton.Resaltado)
                    MediaPlayer.Stop();

                inicioSeleccionTiempo = finSeleccionTiempo = DateTime.Now;
                tintColor = Color.White;
                ultimoEstado = estado;
                estado = EstadoBoton.Normal;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();

            if (estado == EstadoBoton.Normal || estado == EstadoBoton.Seleccionado)
            {
                if (buttonTexture != null)
                    SharedSpriteBatch.Draw(buttonTexture, frame, new Rectangle((int)(estado) * buttonTexture.Bounds.Width / 2, 0, 350, 130), tintColor);

            }
            else
            {
                if (buttonTexture != null)
                    SharedSpriteBatch.Draw(buttonTexture, frame, new Rectangle(0, 0, 350, 130), tintColor);
                SharedSpriteBatch.Draw(buttonResaltadoTexture, frameResaltado, Color.White);
            }
            SharedSpriteBatch.End();
            base.Draw(gameTime);
        }

        public void ReiniciarSeleccion()
        {
            inicioSeleccionTiempo = finSeleccionTiempo = DateTime.Now;
        }


    }
}
