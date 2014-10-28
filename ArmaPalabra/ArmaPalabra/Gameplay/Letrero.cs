using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArmaPalabra.Gameplay
{
    public class Letrero : DrawableGameComponent
    {

        private SpriteFont fuente;
        private Texture2D fondo;
        public string frase { get; set; }
        public Vector2 posicion { get; set; }
        private DateTime tiempoIniciado;
        private bool animado;
        private bool aparece;
        private int contadorMillisegundo;


        public Letrero(Game game, SpriteFont spriteFont)
            : base(game)
        {
            animado = false;
            aparece = false;
            fuente = spriteFont;
            this.LoadContent();
            
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


        protected override void LoadContent()
        {
            base.LoadContent();
            fondo = this.Game.Content.Load<Texture2D>("letrero");
            posicion = new Vector2(this.Game.Window.ClientBounds.Width / 2, -1 * fondo.Height);
        }

        public void IniciarAnimacion()
        {
            this.animado = true;
            this.aparece = false;
       ;
            this.tiempoIniciado = DateTime.Now;
        }

        public override void Draw(GameTime gameTime)
        {
            if (animado)
            {
                SharedSpriteBatch.Begin();
                Vector2 dimensionFrase = fuente.MeasureString(frase);
                SharedSpriteBatch.Draw(fondo,new Rectangle((int)posicion.X-fondo.Width/2,(int)posicion.Y,fondo.Width,fondo.Height),Color.White);
                SharedSpriteBatch.DrawString(fuente, frase, posicion - new Vector2(dimensionFrase.X / 2, -0.5f*(fondo.Height-dimensionFrase.Y)), Color.White);
                SharedSpriteBatch.End();

            }
            base.Draw(gameTime);
        }



        public override void Update(GameTime gameTime)
        {
            if (animado)
            {
                
                    if (posicion.Y <0 && !aparece)
                    {
                        posicion += new Vector2(0, 1);

                    }
                    else if(posicion.Y == 0 && !aparece)
                    {
                        aparece = true;
                        tiempoIniciado = DateTime.Now;
                        
                    }
                    else if ((DateTime.Now - tiempoIniciado).TotalSeconds >=4.0 )
                    {
                        posicion += new Vector2(0, -1);

                        if (posicion.Y <= -1 * fondo.Height)
                        {
                            animado = false;
                        }
                    }
                  
                
            }

            base.Update(gameTime);
        }

    }
}
