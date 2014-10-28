using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArmaPalabra.Gameplay;
using ArmaPalabra.Controls;
using System.ComponentModel;
using LPGames.KinectToolbox;



namespace ArmaPalabra.Screens
{
    public enum TipoScreen
    {
        Ninguno = -1,
        Home = 0,
        Game = 1,
        Pausa = 2,
        Resultado =3,
        EndGame = 4,
        NextLevel = 5,
        ChangeObject = 6
    }

    public class ScreenEventArgs : EventArgs
    {
        public TipoScreen tipoScreen;


        public ScreenEventArgs(TipoScreen tipo)
            : base()
        {
            tipoScreen = tipo;
            
        }
    }


    public class HomeScreen : DrawableGameComponent
    {
        Texture2D backgroundMenu;
        Texture2D botonComenzarTxt2d;
        Texture2D botonSalirTxt2d;
        Texture2D tituloTexture;

       
        private const int NUMERO_BOTONES = 2;
        private const double RATIO_REBOTE = 0.0065;
        Button[] botones;

        SpriteFont fuenteSprite;
        Letrero letrero;
    
        public GameComponentCollection Components { get; private set; }

        public event EventHandler<ScreenEventArgs> salirScreenEventArgs;

        public HomeScreen(Game game):base(game)
        {
            this.Components = new GameComponentCollection();
            botones = new Button[NUMERO_BOTONES];
            
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

       public VisualizadorGuia SharedGuideView
       {
           get
           {
               return (VisualizadorGuia)this.Game.Services.GetService(typeof(VisualizadorGuia));
           }
       }
   


       public override void Initialize()
       {


       
           base.Initialize();
         

       }

       protected override void LoadContent()
       {
     

           base.LoadContent();
           fuenteSprite = this.Game.Content.Load<SpriteFont>("FuenteMensaje");
           backgroundMenu = Game.Content.Load<Texture2D>("imagenes/menu");
           botonComenzarTxt2d = Game.Content.Load<Texture2D>("botonComenzar");
           botonSalirTxt2d = Game.Content.Load<Texture2D>("botonSalir");
           //tituloTexture = Game.Content.Load<Texture2D>("tituloAtrapaAgrupa");
           tituloTexture = Game.Content.Load<Texture2D>("imagenes/logo");
           this.AgregarComponentesVista();

       }

       public void AgregarComponentesVista()
       {
           botones[0] = new Button((this.Game.GraphicsDevice.Viewport.Width / 2 + 50), 430, botonComenzarTxt2d, 300, 100, Game);
           botones[1] = new Button((this.Game.GraphicsDevice.Viewport.Width / 2 - 350), 430, botonSalirTxt2d, 300, 100, Game);

           for (int i = 0; i < NUMERO_BOTONES; i++)
           {
               botones[i].presionadoEventArgs +=new EventHandler<ButtonEventArgs>(HomeScreen_presionadoEventArgs);
               botones[i].tag = i;
               Components.Add(botones[i]);
           }

           letrero = new Letrero(this.Game, fuenteSprite);
           SharedGuideView.position = new Vector2(10, 10);
           Components.Add(SharedGuideView);
           Components.Add(letrero);

    
         
       }

       private void HomeScreen_presionadoEventArgs(object sender,ButtonEventArgs e)
       {
           if (e.estadoActual == EstadoBoton.Seleccionado)
           {
               this.Enabled = false;
               SharedKinectCursor.Enabled = false;
               SharedKinectCursor.Visible = false;
               salirScreenEventArgs(sender, new ScreenEventArgs(TipoScreen.Home));
           }
       }

       public override void Update(GameTime gameTime)
       {
           foreach (IUpdateable udb in Components)
           {

               if(udb.Enabled)
                 udb.Update(gameTime);
           }

               base.Update(gameTime);
    
       }

       public override void Draw(GameTime gameTime)
        {
            
            this.SharedSpriteBatch.Begin();
            this.SharedSpriteBatch.Draw(backgroundMenu, GraphicsDevice.Viewport.Bounds, Color.White);
            this.SharedSpriteBatch.Draw(
                    this.tituloTexture,
                    new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 100 + this.tituloTexture.Height / 2 + (int)(10 * Math.Cos(gameTime.TotalGameTime.TotalMilliseconds * RATIO_REBOTE))),
                    null,
                    Color.White,
                    0,
                //new Vector2(this.tituloTexture.Width / 2, this.tituloTexture.Height / 2),
                new Vector2(this.tituloTexture.Width / 2, this.tituloTexture.Height / 0.4f),
                //    new Vector2(((float)this.tituloTexture.Width) *0.0f ,((float)this.tituloTexture.Height) * 0.0f),
                    0.2f,
                    SpriteEffects.None,
                    0);

        //    this.SharedSpriteBatch.DrawString(fuenteSprite, string.Format("{0}x{1}", this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height), new Vector2(400, 5), Color.Black);
           this.SharedSpriteBatch.End();



            foreach (IDrawable dwb in Components)
            {

                 if(dwb.Visible)
                    dwb.Draw(gameTime);
            }

            if (gameTime.TotalGameTime.TotalMilliseconds <= 17)
            {
                //letrero.frase = (SesionPartida.Instancia.partidaActual.codigoAlumno == 0 || SesionPartida.Instancia.nombreCompletoJugador.Length == 0) ? "Ingresa desde la web" : "Bienvenid" + ((SesionPartida.Instancia.generoAlumno == Genero.Masculino) ? "o" : "a") + ", " + SesionPartida.Instancia.nombreCompletoJugador;
                //letrero.IniciarAnimacion();
            }


            base.Draw(gameTime);
 	           
        }

       public void ReiniciarSeleccion()
       {
           for (int i = 0; i < botones.Length; i++)
           {
               botones[i].ReiniciarSeleccion();
            }
           SharedKinectCursor.CurrentPosition = new Vector2(-100, -100);
     
       }


       public void SincronizarInformacion()
       {
           //letrero.frase = "Sincronizando Información...";
           //letrero.IniciarAnimacion();

           //int numeroBotones = botones.Length;
           //for (int i = 0; i < numeroBotones; i++)
           //{
           //    botones[i].Enabled = false;
           //}

           //BackgroundWorker bw = new BackgroundWorker();

           //bw.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
           //{
           //    SesionPartida.Instancia.EnviarPartida((x) =>
           //    {

           //        if (x.resultado > 0)
           //        {
           //            letrero.frase = "Completado!";
           //            letrero.IniciarAnimacion();

           //        }
           //        else
           //        {
           //            letrero.frase = "Falló el envio";
           //            letrero.IniciarAnimacion();
           //        }

           //        for (int i = 0; i < numeroBotones; i++)
           //        {
           //            botones[i].Enabled = true;
           //        }

           //    });
           //});

           //bw.RunWorkerAsync();

       }

     
     
      

      
    }
}
