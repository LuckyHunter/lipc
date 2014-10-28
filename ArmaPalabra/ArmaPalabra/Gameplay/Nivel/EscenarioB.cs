using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ArmaPalabra.Gameplay.Niveles
{
    public class EscenarioB : DrawableGameComponent
    {
        private Texture2D backgroundGame;

        public EscenarioB(Game game)
            : base(game)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            backgroundGame = this.Game.Content.Load<Texture2D>("gamePBackground1");
           base.Initialize();
           
        }

        public override void Draw(GameTime gameTime)
        {

            SharedSpriteBatch.Begin();
            SharedSpriteBatch.Draw(backgroundGame, new Rectangle(0, 0, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height), Color.White);
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
