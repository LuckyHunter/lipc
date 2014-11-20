using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArmaPalabra.Gameplay.Niveles;
using ArmaPalabra.Gameplay;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using LPGames.KinectToolbox;
using ArmaPalabra.GamePlay;


namespace ArmaPalabra.Screens
{

    public class GameBScreen : DrawableGameComponent
    {

        private int contadorPantallGrande = 0;

        EscenarioB escenarioActual;
        ImagenObjeto imgObj;

        Jugador jugador;
        SpriteFont fuenteSprite;
        SoundEffect burbujaSound;
        DateTime inicioValidarJugador;
        Letrero letreroNivel;
        Mensaje mensaje;
        ObjetosCapturables objetosCapturables;
        private int UltimoSegundo;
        private const int ACCION_AGRUPAR_OBJETO = 4;
        private const int TIEMPO_JUEGO = 20;

        private const int ACCION_MANO_DERECHA = 1;
        private const int ACCION_MANO_IZQUIERDA = 2;
        public static int contadorIntervalo = 0;
        public event EventHandler<ScreenEventArgs> salirScreenEventArgs;

        private string palabraactual = String.Empty;

        Song backgroundMusic;
        SoundEffect exitoSound;
        SoundEffect guardarSound;
        SoundEffect errorGuardarSound;
        SoundEffect sonidoAnimal;
        SoundEffect sonidoA;
        SoundEffect sonidoE;

        SoundEffect sonidoI;
        SoundEffect sonidoO;
        SoundEffect sonidoU;


        SoundEffect sonidoB;
        SoundEffect sonidoC;
        SoundEffect sonidoD;
        SoundEffect sonidoF;
        SoundEffect sonidoG;
        //SoundEffect sonidoH;
        SoundEffect sonidoJ;
        SoundEffect sonidoK;
        SoundEffect sonidoL;
        SoundEffect sonidoM;
        SoundEffect sonidoN;
        SoundEffect sonidoÑ;
        SoundEffect sonidoP;
        SoundEffect sonidoQ;
        SoundEffect sonidoR;
        SoundEffect sonidoS;
        SoundEffect sonidoT;
        SoundEffect sonidoV;
        SoundEffect sonidoW;
        SoundEffect sonidoX;
        SoundEffect sonidoY;
        SoundEffect sonidoZ;



        //-----------------Animales
        SoundEffect silabaPE;
        SoundEffect silabaRRO;
        SoundEffect silabaGA;
        SoundEffect silabaTO;

        SoundEffect silabaCO;
        SoundEffect silabaNE;
        SoundEffect silabaJO;

        SoundEffect silabaLE;
        SoundEffect silabaON;

        SoundEffect silabaCE;
        SoundEffect silabaBRA;

        SoundEffect silabaJI;
        SoundEffect silabaRA;
        SoundEffect silabaFA;


        SoundEffect silabaCA;
        SoundEffect silabaBA;
        SoundEffect silabaLLO;


        SoundEffect silabaE;
        SoundEffect silabaFAN;
        SoundEffect silabaTE;


        SoundEffect silabaCER;
        SoundEffect silabaDO;

        SoundEffect silabaFO;

        SoundEffect silabaLO;
        SoundEffect silabaBO;


        SoundEffect silabaTI;
        SoundEffect silabaGRE;
        SoundEffect silabaVA;
        //------Comidas--------


        SoundEffect silabaA;
        SoundEffect silabaRROZ;

        SoundEffect silabaPI;
        SoundEffect silabaÑA;

        SoundEffect silabaTOR;
        SoundEffect silabaTA;
        SoundEffect silabaJA;
        SoundEffect silabaMO;
        SoundEffect silabaNA;
        SoundEffect silabaDA;
        SoundEffect silabaCHU;
        SoundEffect silabaEM;
        SoundEffect silabaPA;
        SoundEffect silabaRE;
        SoundEffect silabaAL;
        SoundEffect silabaLLE;

        //-----------Objetos----------

        SoundEffect silabaME;
        SoundEffect silabaSA;
        SoundEffect silabaSI;
        SoundEffect silabaLLA;
        SoundEffect silabaLA;
        SoundEffect silabaPIZ;
        SoundEffect silabaRRA;
        SoundEffect silabaDOR;
        SoundEffect silabaCHI;
        SoundEffect silabaLOJ;
        SoundEffect silabaCU;
        SoundEffect silabaCHA;
        SoundEffect silabaNO;
        SoundEffect silabaJE;

        //-----------------------------

        int nivelTermimado;




        public GameComponentCollection Components { get; private set; }

        #region Atributos compartidos
        private KinectDetector SharedKinectDetector
        {
            get
            {
                return (KinectDetector)this.Game.Services.GetService(typeof(KinectDetector));
            }
        }

        private SpriteBatch SharedSpriteBatch
        {
            get
            {
                return (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }
        }
        #endregion

        public GameBScreen(Game game)
            : base(game)
        {
            UltimoSegundo = -1;

            //agrego la imgendel animal

        }

        public override void Initialize()
        {
            this.Components = new GameComponentCollection();
            escenarioActual = new EscenarioB(this.Game);

            jugador = new Jugador(this.Game, RealizarJugada, SesionPartida.Instancia.generoAlumno);
            //canastaA = new Canasta("valdeAzul", this.Game, TipoObjeto.Azul);
            //canastaB = new Canasta("valdeRojo", this.Game, TipoObjeto.Rojo);

            //canastaA.setPosicion(10, this.Game.GraphicsDevice.Viewport.Height / 2+10);
            //canastaA.ActualizarDimensiones(232,245);


            //canastaB.ActualizarDimensiones(232, 245);
            //canastaB.setPosicion(this.Game.GraphicsDevice.Viewport.Width - canastaB.frame.Width - 10, this.Game.GraphicsDevice.Viewport.Height / 2);

            inicioValidarJugador = DateTime.Now;
            objetosCapturables = new ObjetosCapturables(this.Game);
            //objetosCapturables.LoadWord();
            //objetosCapturables.LoadAnswer();

            backgroundMusic = this.Game.Content.Load<Song>("sonidos/backgroundMusic1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.7f;

            Components.Add(escenarioActual);
            Components.Add(jugador);
            Components.Add(objetosCapturables);
            //Components.Add(canastaA);
            //Components.Add(canastaB);

            fuenteSprite = this.Game.Content.Load<SpriteFont>("FuenteMensaje");
            burbujaSound = this.Game.Content.Load<SoundEffect>("sonidos/blop");
            letreroNivel = new Letrero(this.Game, fuenteSprite);

            mensaje = new Mensaje(this.Game, fuenteSprite);
            letreroNivel = new Letrero(this.Game, fuenteSprite);
            exitoSound = this.Game.Content.Load<SoundEffect>("sonidos/exito");

            sonidoA = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_a");
            sonidoE = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_e");
            sonidoI = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_i");
            sonidoO = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_o");
            sonidoU = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_u");


            sonidoB = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_b");
            sonidoC = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_c");
            sonidoD = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_d");
            sonidoF = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_f");
            sonidoG = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_g");
            
            sonidoJ = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_j");
            sonidoK = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_k");
            sonidoL = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_l");
            sonidoM = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_m");
            sonidoN = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_n");
            sonidoÑ = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_ñ");
         
            sonidoP = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_p");
            sonidoQ = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_q");
            sonidoR = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_r");
            sonidoS = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_s");
            sonidoT = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_t");
            sonidoV = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_v");
            sonidoW = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_w");
            sonidoX = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_x");
            sonidoY = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_y");
            sonidoZ = this.Game.Content.Load<SoundEffect>("Sonidos_letras/sound_z");


            //Animales
            silabaPE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_pe");
            silabaRRO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_rro");

            silabaGA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ga");
            silabaTO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_to");

            silabaCO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_co");
            silabaNE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ne");
            silabaJO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_jo");


            silabaLE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_le");
            silabaON = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_on");

            silabaCE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ce");
            silabaBRA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_bra");


            silabaJI = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ji");
            silabaRA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ra");
            silabaFA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_fa");

            silabaCA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ca");
            silabaBA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ba");
            silabaLLO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_llo");

            silabaE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_e");
            silabaFAN = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_fan");
            silabaTE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_te");



            silabaCER = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_cer");
            silabaDO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_do");

            silabaFO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_fo");

            silabaLO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_lo");
            silabaBO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_bo");

            silabaTI = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ti");
            silabaGRE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_gre");
            silabaVA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_va");


            //Comidas
            silabaA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_a");
            silabaRROZ = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_rroz");

            silabaPI = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_pi");
            silabaÑA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ña");


            silabaTOR = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_tor");
            silabaTA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ta");


            silabaJA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_ja");
            silabaMO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_mo");
            silabaNA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_na");
            silabaDA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_da");


            silabaCHU = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_chu");



            silabaEM = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_em");
            silabaPA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_pa");

            silabaRE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_re");
            silabaAL = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_al");

            silabaLLE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_lle");
            //Objetos
            silabaME = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_me");
            silabaSA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_sa");
            silabaSI = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_si");
            silabaLLA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_lla");
            silabaLA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_la");
            silabaPIZ = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_piz");
            silabaRRA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_rra");
            silabaDOR = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_dor");
            silabaCHI = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_chi");
            silabaLOJ = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_loj");
            silabaCU = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_cu");
            silabaCHA = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_cha");
            silabaNO = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_no");
            silabaJE = this.Game.Content.Load<SoundEffect>("Sonidos_silabas/sound_je");


            guardarSound = this.Game.Content.Load<SoundEffect>("sonidos/guardar");
            //errorGuardarSound = this.Game.Content.Load<SoundEffect>("sonidos/errorGuardar");
            errorGuardarSound = this.Game.Content.Load<SoundEffect>("sonidos/blop1");


            Components.Add(mensaje);
            //Components.Add(letreroNivel);
         //   MediaPlayer.Play(backgroundMusic);

            nivelTermimado = objetosCapturables.getNivel();
        }
        public void ReanudarPartida()
        {
            inicioValidarJugador = DateTime.Now;
            //MediaPlayer.Play(backgroundMusic);

        }

        public bool RealizarJugada(Jugador jugadorActual)
        {

            this.EjecutarAccionProceso(jugadorActual);
            this.SonidoNiveles(jugadorActual);
            if (this.EjecutarAccionPrincipal2(jugadorActual))
                return true;
            if (this.EjecutarAccionPrincipal(jugadorActual))
                return true;



            return false;
        }

        private void SonidoNiveles(Jugador jugadorActual)
        {
            try
            {
                if (nivelTermimado == objetosCapturables.getNivel())
                {
                    jugadorActual.LimpiarManos();
                    if (objetosCapturables.dameFinDelJuego())
                    {
                        nivelTermimado++;
                    }
                    
                    if(VariablesGlobales.tipoObjeto==1)
                    {




                    //sonidoAnimal = this.Game.Content.Load<SoundEffect>("Sonidos_animales/sound_" + objetosCapturables.Animales[nivelTermimado]);
             
                        //System.Threading.Thread.Sleep(4000);
             
                        //sonidoAnimal.Play();
                    VariablesGlobales.sonidoAnimal = true;
                    }
                       nivelTermimado++;

                }
            }
            catch (Exception Ex)
            {
 
            }
            //if (nivelTermimado != objetosCapturables.getNivel())
            //{
            //    jugadorActual.LimpiarManos();
            //:)
            //    //stop

            //    try
            //    {
            //        sonidoAnimal = this.Game.Content.Load<SoundEffect>("Sonidos_animales/sound_" + objetosCapturables.Animales[nivelTermimado]);
            //        sonidoAnimal.Play();
            //    }
            //    catch (Exception Ex)
            //    {
            //    }
            //    //              Sonidos_animales\sound_vaca
            //    if (nivelTermimado + 1 < objetosCapturables.niveles.Count)
            //        nivelTermimado++;
            //    else if (nivelTermimado + 1 == objetosCapturables.niveles.Count)
            //    {
            //        nivelTermimado++;
            //        //ganaste
            //        mensaje.mensaje = "Ganaste";
            //        mensaje.posicion = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            //        mensaje.IniciarAnimacion();
            //    }

            //}
        }

        private bool EjecutarAccionProceso(Jugador jugadorActual)
        {
            try
            {
                for (int i = 0; i < jugadorActual.manos.Length; i++)
                {
                    if (jugadorActual.manos[i].objetoSostenido != null) continue;

                    ObjetoCapturable objeto = objetosCapturables.DarObjetoCapturado(jugadorActual.manos[i].frame);

                    if (objeto != null)
                    {
                        jugadorActual.manos[i].objetoSostenido = objeto;
                        objeto.capturado = true;


                        switch (objeto.respuesta)
                        {
                            case "a": sonidoA.Play(); break;
                            case "e": sonidoE.Play(); break;
                            case "i": sonidoI.Play(); break;
                            case "o": sonidoO.Play(); break;
                            case "u": sonidoU.Play(); break;


                            case "b": sonidoB.Play(); break;
                            case "c": sonidoC.Play(); break;
                            case "d": sonidoD.Play(); break;
                            case "f": sonidoF.Play(); break;
                            case "g": sonidoG.Play(); break;

                            case "j": sonidoJ.Play(); break;
                            case "k": sonidoK.Play(); break;
                            case "l": sonidoL.Play(); break;
                            case "m": sonidoM.Play(); break;
                            case "n": sonidoN.Play(); break;
                            case "ñ": sonidoÑ.Play(); break;
                            case "p": sonidoP.Play(); break;
                            case "q": sonidoQ.Play(); break;
                            case "r": sonidoR.Play(); break;
                            case "s": sonidoS.Play(); break;
                            case "t": sonidoT.Play(); break;
                            case "v": sonidoV.Play(); break;
                            case "w": sonidoW.Play(); break;
                            case "x": sonidoX.Play(); break;
                            case "y": sonidoY.Play(); break;
                            case "z": sonidoZ.Play(); break;
                        }
                        burbujaSound.Play();
                    }
                    else
                    {
                        if (jugadorActual.LevantaManoDerecha() && i == (int)TipoMano.Derecha && UltimoSegundo != DateTime.Now.Second)
                        {
                            UltimoSegundo = DateTime.Now.Second;
                        }
                        else if (jugadorActual.LevantaManoIzquierda() && i == (int)TipoMano.Izquierda && UltimoSegundo != DateTime.Now.Second)
                        {
                            UltimoSegundo = DateTime.Now.Second;
                        }
                    }
                }
                return true;
            }catch (Exception e){return false;}
        }

        private bool EjecutarAccionPrincipal(Jugador jugadorActual) //Coje objeto
        {
            for (int i = 0; i < jugadorActual.manos.Length; i++)
            {
                if (jugadorActual.manos[i].objetoSostenido != null)
                {
                    List<ObjetoCapturable> objetosRespuesta = objetosCapturables.ListaRespuestas();
                    foreach (ObjetoCapturable g in objetosRespuesta)
                    {
                        if (jugadorActual.manos[i].frame.Intersects(g.frame)  )
                        {
                            if (g.animal)
                            {
                                return false;
                            }

                            if (g.colocarObjeto(jugadorActual.manos[i].objetoSostenido as ObjetoCapturable))
                            {
                                //objetosCapturables.RemoverObjetoCapturado(jugadorActual.manos[i].objetoSostenido as ObjetoCapturable);
                                jugadorActual.manos[i].objetoSostenido = null;
                                mensaje.mensaje = "Bien Hecho";
                                mensaje.posicion = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
                                mensaje.IniciarAnimacion();



                                if (VariablesGlobales.nivel == 1 || VariablesGlobales.nivel == 2 || VariablesGlobales.nivel == 3)
                                    switch (VariablesGlobales.silabaActual)
                                    {
                                        //Animales
                                        case "pe": silabaPE.Play(); break;
                                        case "rro": silabaRRO.Play(); break;
                                        case "ga": silabaGA.Play(); break;
                                        case "to": silabaTO.Play(); break;
                                        case "co": silabaCO.Play(); break;
                                        case "ne": silabaNE.Play(); break;
                                        case "jo": silabaJO.Play(); break;
                                        case "le": silabaLE.Play(); break;
                                        case "on": silabaON.Play(); break;
                                        case "e": silabaE.Play(); break;
                                        case "ce": silabaCE.Play(); break;
                                        case "bra": silabaBRA.Play(); break;
                                        case "ji": silabaJI.Play(); break;
                                        case "ra": silabaRA.Play(); break;
                                        case "fa": silabaFA.Play(); break;
                                        case "ca": silabaCA.Play(); break;
                                        case "ba": silabaBA.Play(); break;
                                        case "llo": silabaLLO.Play(); break;
                                        case "fan": silabaFAN.Play(); break;
                                        case "te": silabaTE.Play(); break;
                                        case "cer": silabaCER.Play(); break;
                                        case "do": silabaDO.Play(); break;
                                        case "fo": silabaFO.Play(); break;
                                        case "lo": silabaLO.Play(); break;
                                        case "bo": silabaBO.Play(); break;
                                        case "ti": silabaTI.Play(); break;
                                        case "gre": silabaGRE.Play(); break;
                                        case "va": silabaVA.Play(); break;
                                        //Comidas
                                        case "a": silabaA.Play(); break;
                                        case "rroz": silabaRROZ.Play(); break;
                                        case "pi": silabaPI.Play(); break;
                                        case "ña": silabaÑA.Play(); break;
                                        case "tor": silabaTOR.Play(); break;
                                        case "ta": silabaTA.Play(); break;
                                        case "ja": silabaJA.Play(); break;
                                        case "mo": silabaMO.Play(); break;
                                        case "na": silabaNA.Play(); break;
                                        case "da": silabaDA.Play(); break;
                                        case "chu": silabaCHU.Play(); break;
                                        case "em": silabaEM.Play(); break;
                                        case "pa": silabaPA.Play(); break;
                                        case "re": silabaRE.Play(); break;
                                        case "al": silabaAL.Play(); break;
                                        case "lle": silabaLLE.Play(); break;
                                        //Objetos
                                        case "me": silabaME.Play(); break;
                                        case "sa": silabaSA.Play(); break;
                                        case "lla": silabaLLA.Play(); break;
                                        case "la": silabaLA.Play(); break;
                                        case "piz": silabaPIZ.Play(); break;
                                        case "rra": silabaRRA.Play(); break;
                                        case "dor": silabaDOR.Play(); break;
                                        case "chi": silabaCHI.Play(); break;
                                        case "loj": silabaLOJ.Play(); break;
                                        case "cu": silabaCU.Play(); break;
                                        case "cha": silabaCHA.Play(); break;
                                        case "no": silabaNO.Play(); break;
                                        case "je": silabaJE.Play(); break;
                                        case "si": silabaSI.Play(); break;

                                    }
                                else
                                    switch (g.respuesta)
                                    {
                                        case "a": sonidoA.Play(); break;
                                        case "e": sonidoE.Play(); break;
                                        case "i": sonidoI.Play(); break;
                                        case "o": sonidoO.Play(); break;
                                        case "u": sonidoU.Play(); break;
                                        case "b": sonidoB.Play(); break;
                                        case "c": sonidoC.Play(); break;
                                        case "d": sonidoD.Play(); break;
                                        case "f": sonidoF.Play(); break;
                                        case "g": sonidoG.Play(); break;
                                        case "j": sonidoJ.Play(); break;
                                        case "k": sonidoK.Play(); break;
                                        case "l": sonidoL.Play(); break;
                                        case "m": sonidoM.Play(); break;
                                        case "n": sonidoN.Play(); break;
                                        case "ñ": sonidoÑ.Play(); break;
                                        case "p": sonidoP.Play(); break;
                                        case "q": sonidoQ.Play(); break;
                                        case "r": sonidoR.Play(); break;
                                        case "s": sonidoS.Play(); break;
                                        case "t": sonidoT.Play(); break;
                                        case "v": sonidoV.Play(); break;
                                        case "w": sonidoW.Play(); break;
                                        case "x": sonidoX.Play(); break;
                                        case "y": sonidoY.Play(); break;
                                        case "z": sonidoZ.Play(); break;
                                    } 
                             
                                // exitoSound.Play();
                                //PartidaDetalleDALC.Instancia.GuardarPartidaDetalle(new BE.PartidaDetalleBE(SesionPartida.Instancia.codigoPartida, ACCION_AGRUPAR_OBJETO, true));
                                return true;
                            }
                            else
                            {
                                (jugadorActual.manos[i].objetoSostenido as ObjetoCapturable).IniciarAnimacionSalto();
                                jugadorActual.manos[i].objetoSostenido = null;
                                mensaje.mensaje = (SesionPartida.Instancia.fase == 1 || SesionPartida.Instancia.fase % 2 != 0) ? "No correspode ahi !!" : "No es su tamaño";
                                mensaje.posicion = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
                                mensaje.IniciarAnimacion();
                                errorGuardarSound.Play();

                                //(jugadorActual.manos[i].objetoSostenido as ObjetoCapturable).PosX = 400;
                                //(jugadorActual.manos[i].objetoSostenido as ObjetoCapturable).PosY = 400;
                                //PartidaDetalleDALC.Instancia.GuardarPartidaDetalle(new BE.PartidaDetalleBE(SesionPartida.Instancia.codigoPartida, ACCION_AGRUPAR_OBJETO, false));
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        private bool EjecutarAccionPrincipal2(Jugador jugadorActual) // 
        {

          
            if (jugadorActual.manos[(int)TipoMano.Izquierda].frame.Intersects(jugadorActual.manos[(int)TipoMano.Derecha].frame))
            {

                if (!Jugador.juntoManos)
                {
                    Jugador.juntoManos = true;
                }
                else
                {
                    return false;
                }

                if (jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido != null && jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido != null)
                {

                    jugadorActual.accionRealizada = true;
                    //PartidaDetalleDALC.Instancia.GuardarPartidaDetalle(new BE.PartidaDetalleBE(SesionPartida.Instancia.codigoPartida, ACCION_REVENTAR_GLOBO, jugadorActual.accionRealizada));
                    mensaje.mensaje = "plop";
                    mensaje.posicion = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
                    mensaje.IniciarAnimacion();
                    (jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido as ObjetoCapturable).IniciarAnimacionSalto();
                    (jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido as ObjetoCapturable).IniciarAnimacionSalto();
                    jugadorActual.LimpiarManos();
                    guardarSound.Play();
                    Jugador.juntoManos = true;

                }
                else if (jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido == null && jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido != null)
                {

                    jugadorActual.accionRealizada = true;
                    //PartidaDetalleDALC.Instancia.GuardarPartidaDetalle(new BE.PartidaDetalleBE(SesionPartida.Instancia.codigoPartida, ACCION_REVENTAR_GLOBO, jugadorActual.accionRealizada));
                    //mensaje.mensaje = "Bien Hecho";
                    //mensaje.posicion = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
                    //mensaje.IniciarAnimacion();
                    jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido = jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido;
                    //jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido.Dispose();
                    jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido = null;
                    //jugadorActual.LimpiarManos();
                    //guardarSound.Play();
                    //Jugador.juntoManos = true;

                }
                else if (jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido != null && jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido == null)
                {

                    jugadorActual.accionRealizada = true;
                    //PartidaDetalleDALC.Instancia.GuardarPartidaDetalle(new BE.PartidaDetalleBE(SesionPartida.Instancia.codigoPartida, ACCION_REVENTAR_GLOBO, jugadorActual.accionRealizada));
                    //mensaje.mensaje = "Bien Hecho";
                    //mensaje.posicion = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
                    //mensaje.IniciarAnimacion();
                    jugadorActual.manos[(int)TipoMano.Derecha].objetoSostenido = jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido;
                    //jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido.Dispose();
                    jugadorActual.manos[(int)TipoMano.Izquierda].objetoSostenido = null;

                    //jugadorActual.LimpiarManos();

                    //guardarSound.Play();
                    //Jugador.juntoManos = true;

                }
                else
                {
                    jugadorActual.accionRealizada = false;
                    //PartidaDetalleDALC.Instancia.GuardarPartidaDetalle(new BE.PartidaDetalleBE(SesionPartida.Instancia.codigoPartida, ACCION_REVENTAR_GLOBO, jugadorActual.accionRealizada));
                }

                return true;
            }
            else
            {
                Jugador.juntoManos = false;
            }


            return false;

        }

        public override void Update(GameTime gameTime)
        {
            /*if (VariablesGlobales.pronunciarPalabra == true)
            {
                if (contadorPantallGrande > 99 && contadorPantallGrande < 102)
                {
                    switch (VariablesGlobales.palabraActual)
                    {
                        case "gato":

                            System.Threading.Thread.Sleep(600);

                            silabaGA.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaTO.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                        case "leon":

                            System.Threading.Thread.Sleep(600);

                            silabaLE.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaON.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                        case "perro":

                            System.Threading.Thread.Sleep(600);

                            silabaPE.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaRRO.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                        case "conejo":

                            System.Threading.Thread.Sleep(600);

                            silabaCO.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaNE.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaJO.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                    }
                }
            }*/

            if (SesionPartida.Instancia.tiempoSegundo == 0)
            {
                letreroNivel.frase = "Agrupa por color";
                letreroNivel.IniciarAnimacion();
            }
            if (new TimeSpan(0, 0, SesionPartida.Instancia.tiempoSegundo).Minutes >= TIEMPO_JUEGO)
            {
                SesionPartida.Instancia.finPartida = true;
                MediaPlayer.Stop();
                salirScreenEventArgs(this, new ScreenEventArgs(TipoScreen.Game));
                return;
            }
            contadorIntervalo++;
            if (contadorIntervalo % 60 == 0)
            {
                contadorIntervalo = 0;
                SesionPartida.Instancia.tiempoSegundo++;
            }



            foreach (IUpdateable udb in Components) udb.Update(gameTime);

            if (nivelTermimado > objetosCapturables.dameNumeroAnimales())
            {
                //SesionPartida.Instancia.finNiveles = true;
                inicioValidarJugador = DateTime.Now;
                MediaPlayer.Pause();
                salirScreenEventArgs(this, new ScreenEventArgs(TipoScreen.Game));
                return;
            }
            else if (!jugador.hayJugador && (DateTime.Now - inicioValidarJugador).TotalSeconds >= 2.0)
            {
                inicioValidarJugador = DateTime.Now;
                MediaPlayer.Pause();
                salirScreenEventArgs(this, new ScreenEventArgs(TipoScreen.Game));
                return;
            }
            else if (jugador.hayJugador)
            {
                inicioValidarJugador = DateTime.Now;
            }

            if (objetosCapturables.ContarObjetos() == 0)
            {
                SesionPartida.Instancia.fase++;

                if (SesionPartida.Instancia.fase % 2 == 0)
                {
                    letreroNivel.frase = "Agrupa por tamaño";

                    letreroNivel.IniciarAnimacion();
                    // IniciarClasificacionXTamanio();
                }
                else
                {
                    letreroNivel.frase = "Agrupa por color";
                    letreroNivel.IniciarAnimacion();
                    Random randomCantidad = new Random();
                    //IniciarClasificacionXColor(TipoObjeto.Rojo, TipoObjeto.Amarillo, randomCantidad.Next(6, 9), randomCantidad.Next(8, 10));
                }
            }


            base.Update(gameTime);
        }

        //public void IniciarClasificacionXTamanio()
        //{
        //    canastaA.LimpiarCanasta();
        //    canastaB.LimpiarCanasta();

        //    objetosCapturables.CrearObjetoTamanio();
        //    this.Components.Remove(canastaA);
        //    this.Components.Remove(canastaB);

        //    canastaA = new Canasta(canastaNombrePorTipo(TipoObjeto.Grande), this.Game, TipoObjeto.Grande);
        //    canastaB = new Canasta(canastaNombrePorTipo(TipoObjeto.Pequenio), this.Game, TipoObjeto.Pequenio);

        //    canastaA.setPosicion(10, this.Game.GraphicsDevice.Viewport.Height / 2 + 10);
        //    canastaA.ActualizarDimensiones(232, 245);

        //    canastaB.ActualizarDimensiones(232, 245);
        //    canastaB.setPosicion(this.Game.GraphicsDevice.Viewport.Width - canastaB.frame.Width - 10, this.Game.GraphicsDevice.Viewport.Height / 2);

        //    this.Components.Add(canastaA);
        //    this.Components.Add(canastaB);
        //}

        //public void IniciarClasificacionXColor(TipoObjeto colorIzquierda, TipoObjeto colorDerecha,int cantidadIzquierda,int cantidadDerecha)
        //{
        //    canastaA.LimpiarCanasta();
        //    canastaB.LimpiarCanasta();

        //    this.Components.Remove(canastaA);
        //    this.Components.Remove(canastaB);

        //    objetosCapturables.CrearObjetosColor(colorIzquierda,colorDerecha,cantidadIzquierda,cantidadDerecha);

        //    canastaA = new Canasta(canastaNombrePorTipo(colorIzquierda), this.Game, colorIzquierda);
        //    canastaB = new Canasta(canastaNombrePorTipo(colorDerecha), this.Game, colorDerecha);

        //    canastaA.setPosicion(10, this.Game.GraphicsDevice.Viewport.Height / 2 + 10);
        //    canastaA.ActualizarDimensiones(232, 245);

        //    canastaB.ActualizarDimensiones(232, 245);
        //    canastaB.setPosicion(this.Game.GraphicsDevice.Viewport.Width - canastaB.frame.Width - 10, this.Game.GraphicsDevice.Viewport.Height / 2);

        //    this.Components.Add(canastaA);
        //    this.Components.Add(canastaB);




        //}

        //public string canastaNombrePorTipo(TipoObjeto color)
        //{
        //    if (color == TipoObjeto.Amarillo)
        //        return "valdeAmarillo";
        //    else if (color == TipoObjeto.Azul)
        //        return "valdeAzul";
        //    else if (color == TipoObjeto.Rojo)
        //        return "valdeRojo";
        //    else if (color == TipoObjeto.Grande)
        //        return "valdeGrande";
        //    else if (color == TipoObjeto.Pequenio)
        //        return "valdeChico";

        //    return "";
        //}

        public override void Draw(GameTime gameTime)
        {

        /*           
        foreach (IDrawable dwb in Components)
        dwb.Draw(gameTime);
        */

            if (VariablesGlobales.pronunciarPalabra == false)
            {
                IDrawable a = (IDrawable)Components.ElementAt(0);
                a.Draw(gameTime);

                IDrawable b = (IDrawable)Components.ElementAt(1);
                b.Draw(gameTime);


                IDrawable c = (IDrawable)Components.ElementAt(2);
                c.Draw(gameTime);

                palabraactual = VariablesGlobales.palabraActual;


                if(VariablesGlobales.sonidoAnimal )
                  if(VariablesGlobales.tipoObjeto==1)
                  {

                      /*if (nivelTermimado > 0)
                          nivelTermimado--;*/

                      int nivelterminadoaux = 0;

                      if(nivelTermimado >0)
                       nivelterminadoaux = nivelTermimado - 1;
                      
                      sonidoAnimal = this.Game.Content.Load<SoundEffect>("Sonidos_animales/sound_" + objetosCapturables.Animales[nivelterminadoaux]);
             
                   //   System.Threading.Thread.Sleep(4000);
             
                      sonidoAnimal.Play();

                      VariablesGlobales.sonidoAnimal = false;
                  }
            }
            else
            {
                
                contadorPantallGrande = contadorPantallGrande + 1;
                IDrawable a = (IDrawable)Components.ElementAt(0);
                a.Draw(gameTime);

                imgObj = new ImagenObjeto(this.Game);
                this.Components.Add(imgObj);
                IDrawable b = (IDrawable)Components.Last() ;
                b.Draw(gameTime);
                                
                if (contadorPantallGrande > 70)
                {
                    switch (palabraactual)
                    {
                        case "gato":

                            System.Threading.Thread.Sleep(600);
                            silabaGA.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaTO.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                        case "leon":

                            System.Threading.Thread.Sleep(600);

                            silabaLE.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaON.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                        case "perro":

                            System.Threading.Thread.Sleep(600);

                            silabaPE.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaRRO.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                        case "conejo":

                            System.Threading.Thread.Sleep(600);

                            silabaCO.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaNE.Play();
                            System.Threading.Thread.Sleep(500);
                            silabaJO.Play();
                            System.Threading.Thread.Sleep(600);
                            break;

                    }
                    VariablesGlobales.pronunciarPalabra = false;
                    contadorPantallGrande = 0;
                }

                //VariablesGlobales.pronunciarPalabra = false;
            }
            
            SharedSpriteBatch.Begin();
            SharedSpriteBatch.DrawString(fuenteSprite, string.Format("{0}", new TimeSpan(0, 0, SesionPartida.Instancia.tiempoSegundo).ToString(@"mm\:ss")), new Vector2(0, 0), Color.Black);
            SharedSpriteBatch.End();

            base.Draw(gameTime);
        }


    }

}
