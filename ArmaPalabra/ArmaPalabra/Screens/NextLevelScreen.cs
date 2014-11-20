using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArmaPalabra.Controls;
using ArmaPalabra.Gameplay;
using LPGames.KinectToolbox;

namespace ArmaPalabra.Screens
{
    class NextLevelScreen : DrawableGameComponent
    {
        Texture2D backgroundPausa;
        Texture2D botonSalirTxt2d;
        Texture2D botonIrInicioTxt2d;
        Texture2D botonSiguienteTxt2d;
        Texture2D tituloTexture;

        private const int NUMERO_BOTONES = 3;
        Button[] botones;

        public GameComponentCollection Components { get; private set; }

        public event EventHandler<ScreenEventArgs> salirScreenEventArgs;

        public NextLevelScreen(Game game)
            : base(game)
        {
            botones = new Button[NUMERO_BOTONES];
            this.Components = new GameComponentCollection();


        }


        public override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            backgroundPausa = Game.Content.Load<Texture2D>("FondoPausa");
            botonSalirTxt2d = Game.Content.Load<Texture2D>("botonSalir");
            botonIrInicioTxt2d = Game.Content.Load<Texture2D>("botonOtroElemento");
            botonSiguienteTxt2d = Game.Content.Load<Texture2D>("botonContinuar");
            tituloTexture = Game.Content.Load<Texture2D>("tituloPausa");
            this.AgregarComponentesVista();
            SharedKinectCursor.CurrentPosition = new Vector2(0, 0);
            SharedKinectCursor.Enabled = true;
            SharedKinectCursor.Visible = true;

        }

        public void AgregarComponentesVista()
        {
            botones[0] = new Button((this.Game.GraphicsDevice.Viewport.Width / 2 - 350), 380, botonSiguienteTxt2d, 300, 100, Game);
            botones[1] = new Button((this.Game.GraphicsDevice.Viewport.Width / 2 + 50), 380, botonIrInicioTxt2d, 300, 100, Game);
            //  botones[2] = new Button((this.Game.GraphicsDevice.Viewport.Width / 2 ), 500, botonSiguienteTxt2d, 300, 100, Game);
            botones[2] = new Button((this.Game.GraphicsDevice.Viewport.Width / 2) - 140, 500, botonSalirTxt2d, 300, 100, Game);
            for (int i = 0; i < NUMERO_BOTONES; i++)
            {
                botones[i].presionadoEventArgs += new EventHandler<ButtonEventArgs>(HomeScreen_presionadoEventArgs);
                botones[i].tag = i;

                Components.Add(botones[i]);
            }

            SharedGuideView.position = new Vector2(25, 10);

            Components.Add(SharedGuideView);
        }
        private void HomeScreen_presionadoEventArgs(object sender, ButtonEventArgs e)
        {
            if (e.estadoActual == EstadoBoton.Seleccionado)
            {

                salirScreenEventArgs(sender, new ScreenEventArgs(TipoScreen.NextLevel));
            }
        }


        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }

        public KinectCursor SharedKinectCursor
        {
            get
            {
                return (KinectCursor)this.Game.Services.GetService(typeof(KinectCursor));
            }
        }
        public VisualizadorGuia SharedGuideView
        {
            get
            {
                return (VisualizadorGuia)this.Game.Services.GetService(typeof(VisualizadorGuia));
            }
        }


        public override void Draw(GameTime gameTime)
        {

            SharedSpriteBatch.Begin();
            SharedSpriteBatch.Draw(backgroundPausa, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.FromNonPremultiplied(0, 0, 0, 50));
            this.SharedSpriteBatch.Draw(
                    this.tituloTexture,
                    new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 3 + this.tituloTexture.Height / 2),
                    null,
                    Color.White,
                    0,
                    new Vector2(this.tituloTexture.Width / 2, this.tituloTexture.Height / 2),
                    1,
                    SpriteEffects.None,
                    0);
            SharedSpriteBatch.End();

            foreach (IDrawable dwb in Components)
            {
                if (dwb.Visible)
                    dwb.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (IUpdateable udb in Components)
            {
                if (udb.Enabled)
                    udb.Update(gameTime);
            }

            base.Update(gameTime);

        }
    }
}
