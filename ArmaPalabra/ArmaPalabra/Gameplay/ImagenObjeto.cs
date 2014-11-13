using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ArmaPalabra.GamePlay
{
    class ImagenObjeto : DrawableGameComponent
    {
       Texture2D objeto;

       public ImagenObjeto(Game game)
            : base(game)
        {
            this.Initialize();
        }

        public override void Initialize()
        {         
            switch (VariablesGlobales.tipoObjeto)
            {
                case 1:
                    objeto = Game.Content.Load<Texture2D>("animales/img_" + VariablesGlobales.palabraActual);
                    break;

                case 2:
                    objeto = Game.Content.Load<Texture2D>("comidas/img_" + VariablesGlobales.palabraActual);
                    break;

                case 3:
                    objeto = Game.Content.Load<Texture2D>("objetos/img_" + VariablesGlobales.palabraActual);
                    break;

            }            
           base.Initialize();           
        }

        public override void Draw(GameTime gameTime)
        {
            SharedSpriteBatch.Begin();
            SharedSpriteBatch.Draw(objeto, new Rectangle(200, 200,450, 450),Color.White);
            SharedSpriteBatch.End();
            base.Draw(gameTime);
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
