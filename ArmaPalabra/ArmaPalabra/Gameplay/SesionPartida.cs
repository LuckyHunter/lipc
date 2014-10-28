using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaPalabra.Screens;

namespace ArmaPalabra.Gameplay
{
    public class SesionPartida
    {
        public string nombreCompletoJugador { get; set; }
        public long codigoPartida { get; set; }
        public Genero generoAlumno { get; set; }
        public int ventana;
        public int ventanaPrevia;
        public int tiempoSegundo;
        public bool finPartida;
        public string nombreArchivo;
        public int fase { get; set; }
        private static SesionPartida instancia;
        public string direccionServidor { get; set; }
        private static readonly object padlock = new object();

        //kevin
        public bool finNiveles;

        private SesionPartida()
        {
            ventana = ventanaPrevia = (int)TipoScreen.Ninguno;
            tiempoSegundo = 0;
            fase = 1;
            finPartida = false;
            finNiveles = false;
            //partidaActual = new PartidaBE();
            //partidaDetalles = new List<PartidaDetalleBE>();
        }

        public void IniciarPartida()
        {
            //DateTime fechaActual = DateTime.Now;
            //SesionPartida.Instancia.partidaActual.horaInicio = fechaActual;
            //partidaDetalles.Clear();
            //nombreArchivo = string.Format("Alum{0}-{1}-{2}{3}{4}{5}", partidaActual.codigoAlumno, nombreCompletoJugador, fechaActual.Hour, fechaActual.Minute, fechaActual.Second, fechaActual.Millisecond);
        }


        public static SesionPartida Instancia
        {
            get
            {
                lock (padlock)
                {
                    if (instancia == null)
                    {
                        instancia = new SesionPartida();
                    }
                    return instancia;
                }
            }
        }

        public int Ventana
        {
            set
            {
                ventanaPrevia = ventana;
                ventana = value;
            }
        }

        //public void EnviarPartida(Action<Response> resultado)
        //{
        //    GameService gameService = new GameService();
        //    Uri gameRequest = new Uri(direccionServidor);/*"http://localhost:2617/api/partida/"*/

        //    Partida partidaContract = TranslatorEntityToContract.partidaBEtoPartida(partidaActual, partidaDetalles);
        //    gameService.obtenerPOSTResponse(gameRequest, partidaContract, resultado);

        //}

    }
}
