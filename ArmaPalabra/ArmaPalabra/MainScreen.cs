using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LPGames.KinectToolbox;
using Microsoft.Kinect;
using ArmaPalabra.Screens;
using ArmaPalabra.Gameplay;



namespace ArmaPalabra
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
   
    public class MainScreen : Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        KinectDetector kinectDetector;

        Texture2D cursorTexture;

        KinectCursor kinectCursor;

        VisualizadorGuia vistaGuia;

        //Screens
        HomeScreen homeScreen;
        GameBScreen gameScreen;
        PauseScreen pauseScreen;
        EndGameScreen endGameScreen;
        NextLevelScreen nextLevelScreen;
        ChangeObjectScreen changeObjectScreen;

        public SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Services.GetService(typeof(SpriteBatch));
            }
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            if (kinectDetector.Sensor !=null && kinectDetector.Sensor.Status == KinectStatus.Connected)
                 kinectDetector.Sensor.Stop();

            base.OnExiting(sender, args);
        }

        public MainScreen()
        {
             graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
             
            /*
            this.gameScreen = new GameBScreen(this);
            this.gameScreen.salirScreenEventArgs+= new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
            this.Components.Add(this.gameScreen);
            */
            
            this.homeScreen = new HomeScreen(this);
           
            this.homeScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
            this.Components.Add(this.homeScreen);

            vistaGuia = new VisualizadorGuia(this);

            this.Services.AddService(typeof(VisualizadorGuia), this.vistaGuia);
            this.kinectDetector = new KinectDetector(this);
            this.Services.AddService(typeof(KinectDetector), this.kinectDetector);
            this.Components.Add(this.kinectDetector);

            SesionPartida.Instancia.Ventana = (int)TipoScreen.Home;

        }

        public KinectDetector Control
        {
            get
            {
                return (KinectDetector)this.Services.GetService(typeof(KinectDetector));
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);
            this.cursorTexture = this.Content.Load<Texture2D>("handIcon");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.7f;

            kinectCursor = new KinectCursor(this, cursorTexture, 70, 70);
            this.Services.AddService(typeof(KinectCursor), kinectCursor);
            this.Components.Add(kinectCursor);

            VariablesGlobales.nivel = 1;
            VariablesGlobales.tipoObjeto = 1;
            VariablesGlobales.pronunciarPalabra = false;
                VariablesGlobales.sonidoAnimal = false;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                this.graphics.IsFullScreen = !this.graphics.IsFullScreen;
                //tryFullScreen = !tryFullScreen;
                //if (tryFullScreen)
                //{
                //    this.graphics.PreferredBackBufferWidth = 1280;
                //    this.graphics.PreferredBackBufferHeight = 720;
                //}
                //else{}
             
              
                graphics.ApplyChanges();
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void homeScreen_salirScreenEventArgs(object sender, ScreenEventArgs e)
        {
            switch (e.tipoScreen)
            {
                case TipoScreen.Home:
                    if (((Controls.Button)sender).tag == 0)
                    {
                        SesionPartida.Instancia.IniciarPartida();
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Game;
                        this.homeScreen.Enabled = false;
                        this.gameScreen = new GameBScreen(this);
                        gameScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.Components.Remove(this.kinectCursor);
                        this.Components.Add(this.gameScreen);                     
                    }
                    else if (((Controls.Button)sender).tag == 1)
                    {
                        this.Exit();
                    }
                    break;
                case TipoScreen.Game:
                    if (SesionPartida.Instancia.finPartida)
                    {
                        //System.Threading.Thread.Sleep(10000);
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.NextLevel;
                        //SesionPartida.Instancia.partidaActual.horaFin = DateTime.Now;
                        if(this.Components.Contains(pauseScreen))
                        this.Components.Remove(pauseScreen);

                        if(this.Components.Contains(gameScreen))
                        this.Components.Remove(gameScreen);

                        this.nextLevelScreen = new NextLevelScreen(this);
                        nextLevelScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.nextLevelScreen.Enabled = true;
                        this.nextLevelScreen.Visible = true;
                        this.kinectCursor.Visible = true;
                        this.kinectCursor.Enabled = true;
                        //this.pauseScreen.Enabled = false;
                      
                        //this.homeScreen.ReiniciarSeleccion();
                       // this.homeScreen.SincronizarInformacion();
                        this.homeScreen.Enabled = false;
                        this.homeScreen.Visible = false;
                        SesionPartida.Instancia.finPartida = false;
                        this.Components.Add(this.nextLevelScreen);
                        this.Components.Add(this.kinectCursor); 
                       // SesionPartida.Instancia.tiempoSegundo = 0;
                    }
                    else if(SesionPartida.Instancia.finNiveles)
                    {
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.EndGame;
                        this.endGameScreen = new EndGameScreen(this);
                        endGameScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.gameScreen.Enabled = false;
                        this.Components.Add(this.endGameScreen);
                        this.Components.Add(this.kinectCursor);

                    }
                    else
                    {
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Pausa;
                        this.pauseScreen = new PauseScreen(this);
                        pauseScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.gameScreen.Enabled = false;
                        this.Components.Add(this.pauseScreen);
                        this.Components.Add(this.kinectCursor);                      
                    }

                    break;

               case TipoScreen.Pausa :


                  
                    if (((Controls.Button)sender).tag == 0)
                    {
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Game;
                        this.Components.Remove(pauseScreen);
                        this.Components.Remove(this.kinectCursor);
                        this.gameScreen.ReanudarPartida();
                        this.gameScreen.Enabled = true;

                    }
                    else if (((Controls.Button)sender).tag == 1)
                    {
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Home;
                      //  SesionPartida.Instancia.partidaActual.horaFin = DateTime.Now;
                        this.Components.Remove(pauseScreen);
                        this.Components.Remove(gameScreen);
                        this.homeScreen.ReiniciarSeleccion();
                        this.homeScreen.Enabled = true;
                        this.homeScreen.Visible = true;
                        this.homeScreen.SincronizarInformacion();
                        SesionPartida.Instancia.tiempoSegundo = 0;
                    }
                    break;

               case TipoScreen.NextLevel:

                   if (((Controls.Button)sender).tag == 1)
                    {
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.ChangeObject;
                      //SesionPartida.Instancia.partidaActual.horaFin = DateTime.Now;
                        //-------------
               
                        
                         this.nextLevelScreen.Enabled = false;
                         this.Components.Remove(this.kinectCursor);
                         if (this.Components.Contains(nextLevelScreen))
                             this.Components.Remove(nextLevelScreen);

                         this.changeObjectScreen = new ChangeObjectScreen(this);
                         changeObjectScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);


                         this.Components.Add(this.changeObjectScreen);

                         this.Components.Add(this.kinectCursor);
                    }
                    else if (((Controls.Button)sender).tag == 0)
                    {
                        SesionPartida.Instancia.IniciarPartida();
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Game;               
                        this.nextLevelScreen.Enabled = false;
                        if (VariablesGlobales.nivel == 1)
                            VariablesGlobales.nivel = 2;
                        else if (VariablesGlobales.nivel == 2)
                            VariablesGlobales.nivel = 3;
                        else if (VariablesGlobales.nivel == 3)
                            VariablesGlobales.nivel = 1;
                        this.gameScreen = new GameBScreen(this);                 
                        gameScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.Components.Remove(this.kinectCursor);
                        if (this.Components.Contains(gameScreen))
                            this.Components.Remove(this.gameScreen);
                        this.Components.Add(this.gameScreen);                 
                    }
                    else if (((Controls.Button)sender).tag == 2)
                    {
                        this.Exit();
                    }
                    break;
                case TipoScreen.ChangeObject:

                    if (((Controls.Button)sender).tag == 0)//Animales
                    {

                        SesionPartida.Instancia.IniciarPartida();
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Game;
                        this.changeObjectScreen.Enabled = false;
                        if (VariablesGlobales.nivel == 1)
                            VariablesGlobales.nivel = 2;
                        else if (VariablesGlobales.nivel == 2)
                            VariablesGlobales.nivel = 3;
                        else if (VariablesGlobales.nivel == 3)
                            VariablesGlobales.nivel = 1;

                        VariablesGlobales.tipoObjeto = 1;

                        this.gameScreen = new GameBScreen(this);
                        gameScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.Components.Remove(this.kinectCursor);
                        if (this.Components.Contains(gameScreen))
                            this.Components.Remove(this.gameScreen);
                        this.Components.Add(this.gameScreen);


                    }
                    else if (((Controls.Button)sender).tag == 1)//Comidas
                    {

                        SesionPartida.Instancia.IniciarPartida();
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Game;
                        this.changeObjectScreen.Enabled = false;
                        if (VariablesGlobales.nivel == 1)
                            VariablesGlobales.nivel = 2;
                        else if (VariablesGlobales.nivel == 2)
                            VariablesGlobales.nivel = 3;
                        else if (VariablesGlobales.nivel == 3)
                            VariablesGlobales.nivel = 1;

                        VariablesGlobales.tipoObjeto = 2;

                        this.gameScreen = new GameBScreen(this);
                        gameScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.Components.Remove(this.kinectCursor);
                        if (this.Components.Contains(gameScreen))
                            this.Components.Remove(this.gameScreen);
                        this.Components.Add(this.gameScreen);


                    }
                    else if (((Controls.Button)sender).tag == 2)//Objetos
                    {

                        SesionPartida.Instancia.IniciarPartida();
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Game;
                        this.changeObjectScreen.Enabled = false;
                        if (VariablesGlobales.nivel == 1)
                            VariablesGlobales.nivel = 2;
                        else if (VariablesGlobales.nivel == 2)
                            VariablesGlobales.nivel = 3;
                        else if (VariablesGlobales.nivel == 3)
                            VariablesGlobales.nivel = 1;

                        VariablesGlobales.tipoObjeto = 3;

                        this.gameScreen = new GameBScreen(this);
                        gameScreen.salirScreenEventArgs += new EventHandler<ScreenEventArgs>(homeScreen_salirScreenEventArgs);
                        this.Components.Remove(this.kinectCursor);
                        if (this.Components.Contains(gameScreen))
                            this.Components.Remove(this.gameScreen);
                        this.Components.Add(this.gameScreen);


                    } 
















                    break;
                case TipoScreen.EndGame:

                    if (((Controls.Button)sender).tag == 0)
                    {
      
                        this.Exit();
                    }
                    else if (((Controls.Button)sender).tag == 1)
                    {
                        SesionPartida.Instancia.Ventana = (int)TipoScreen.Home;
                        //SesionPartida.Instancia.partidaActual.horaFin = DateTime.Now;
                        this.Components.Remove(endGameScreen);
                        this.Components.Remove(gameScreen);
                        this.homeScreen.ReiniciarSeleccion();
                        this.homeScreen.Enabled = true;
                        this.homeScreen.Visible = true;
                        this.homeScreen.SincronizarInformacion();
                        SesionPartida.Instancia.tiempoSegundo = 0;
                    }
                    break;

            }
        }
    }
}





