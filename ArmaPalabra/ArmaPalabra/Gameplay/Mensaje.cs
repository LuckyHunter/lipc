using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArmaPalabra.Gameplay
{
    public class Mensaje : DrawableGameComponent
    {
        private SpriteFont fuente;
        public string mensaje { get; set; }
        public Vector2 posicion { get; set; }
        private bool animado;
        private Color shadowColor;
        private DateTime tiempoIniciado;

        public Mensaje(Game game,SpriteFont spriteFont):base(game)
        {
            this.fuente = spriteFont;
            posicion = Vector2.Zero;
            shadowColor = new Color(0, 0, 0, 200);
            mensaje = "";
        }

        #region Atributos Compartidos
        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }
        #endregion

        public void IniciarAnimacion()
        {
            this.animado = true;
            this.tiempoIniciado = DateTime.Now;
        }

        public override void Draw(GameTime gameTime)
        {
            if (animado)
            {
                SharedSpriteBatch.Begin();
                SharedSpriteBatch.DrawString(fuente, mensaje, posicion, shadowColor);
                SharedSpriteBatch.DrawString(fuente, mensaje, posicion + new Vector2(2, 2), Color.Yellow);
                SharedSpriteBatch.End();

            }
            base.Draw(gameTime);
        }



        public override void Update(GameTime gameTime)
        {
            if (animado)
            {
                posicion += new Vector2(0,(float)Math.Sin( (double)gameTime.TotalGameTime.Seconds*(MathHelper.TwoPi/60.0)));

                if (this.mensaje.Equals("Ganaste"))
                {
                    if ((DateTime.Now - tiempoIniciado).TotalSeconds >= 10.0)
                    {
                        animado = false;
                    }
                }
                else
                {
                    if ((DateTime.Now - tiempoIniciado).TotalSeconds >= 3.0)
                    {
                        animado = false;
                    }
                }


            }

            base.Update(gameTime);
        }
        
    }
}
