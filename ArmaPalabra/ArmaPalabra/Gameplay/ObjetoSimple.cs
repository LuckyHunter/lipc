using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArmaPalabra.Gameplay
{
    public class ObjetoSimple : DrawableGameComponent
    {
        private string nombreImagen;
        private int posX;
        private int posY;
        public Rectangle frame {get; set;}
        protected Texture2D texturaImagen;

        public ObjetoSimple(string nombreImagen,Game game, int posicionInicialX, int posicionInicialY):base(game)
        {
          this.nombreImagen = nombreImagen;
          this.posX = posicionInicialX;
          this.posY = posicionInicialY;
          this.frame = new Rectangle(frame.X, frame.Y, 0, 0);
          this.LoadContent();
        
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            this.texturaImagen = this.Game.Content.Load<Texture2D>(nombreImagen);
            //this.texturaImagen = this.Game.Content.Load<Texture2D>(
        }

        public override void Draw(GameTime gameTime)
        {
            if (texturaImagen == null)
                return;

            SharedSpriteBatch.Begin();

            SharedSpriteBatch.Draw(texturaImagen, frame, Color.White);
            //spriteBatch.Draw(texture, Vector2.Zero,
            //                 new Rectangle(currentFrame.X * frameSize.X,
            //                                currentFrame.Y * frameSize.Y,
            //                                frameSize.X,
            //                                frameSize.Y),
            //                Color.White, 0, Vector2.Zero,
            //                1, SpriteEffects.None, 0);

            SharedSpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public int PosX
        {
            get
            {
                return posX;
            }
            set
            {
                posX = value;
                frame = new Rectangle(posX, posY, frame.Width, frame.Height);
            }
        }

        public int PosY
        {
            get
            {
                return posY;
            }
            set
            {
                posY = value;
                frame = new Rectangle(posX, posY, frame.Width, frame.Height);
            }
        }

        public void setPosicion(int x, int y)
        {
            posX = x;
            posY = y;
            frame = new Rectangle(posX, posY, frame.Width, frame.Height);
        }

        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }




    }
}
