using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArmaPalabra.GamePlay;
using System.Collections;


namespace ArmaPalabra.Gameplay
{
    public class ObjetoCapturable : ObjetoSimple
    {
        public int posicionInicialX;
        public int posicionInicialY;
        public int posicionIniAnimacionX;
        public int posicionIniAnimacionY;
        public double anguloAnimacion;
        public const int desplazamientoX = 3;
        public bool capturado { get; set; }
        public Texture2D burbuja;
        public Rectangle burbujaFrame;
        public double anguloOscilacion { get; set; }
        public const double angulo = 0.04;
        public bool animado;
        public DateTime tiempoIniciado;
        public const int MIN_LADO_DEFAULT = 65;
        public const int MAX_LADO_DEFAULT = 140;
        // KEVIN

        private bool activo;
        public bool direccion;
        public bool pregunta;
        public String respuesta;
        public Point posAux;
        public bool animal;       
        int key;
        bool utilizado;

       

        public ObjetoCapturable(string nombreImagen, Game game ,int posicionInicialX, int posicionInicialY,bool pregunta,string respuesta)
            : base(nombreImagen, game,posicionInicialX, posicionInicialY)
        {
            this.anguloOscilacion = 0;
            this.posicionInicialX = posicionInicialX;
            this.posicionInicialY = posicionInicialY;
            burbujaFrame = new Rectangle(posicionIniAnimacionX, posicionIniAnimacionY, 0, 0);
            animado = false;
            capturado = (pregunta)?false:true;
            // kevin
            direccion = false;
            utilizado = false;
            this.pregunta = pregunta;

            if (!pregunta)
            {
                this.PosX = posicionInicialX;
                this.PosY = posicionInicialY;

                this.posAux.X = posicionInicialX;
                this.posAux.Y = posicionInicialY;
            }

            if (!pregunta)//si es una respuesta lo pongo activo => es para pasar al siguiente nivel
            {
                this.activo = true;

            }
            else
            {
                this.activo = false;
            }

            this.respuesta = respuesta;
            if (this.respuesta.Equals("animal"))
            {
                this.animal = true;
            }
            else
            { 
                this.animal = false;
            }
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            int lado = ( MIN_LADO_DEFAULT);
            if (animal) { lado *= 4; }
            
            burbuja = Game.Content.Load<Texture2D>("burbuja");
            int WideSide = (base.frame.Height > base.frame.Width) ? base.frame.Height : base.frame.Width;
            burbujaFrame = new Rectangle(base.frame.X , base.frame.Y , WideSide+16, WideSide+16);

            if (base.texturaImagen.Bounds.Width < base.texturaImagen.Bounds.Height)
            {
                base.frame = new Rectangle(base.frame.X, base.frame.Y,(lado * base.texturaImagen.Bounds.Width) / base.texturaImagen.Bounds.Height, lado);

            }
            else
            {
                base.frame = new Rectangle(base.frame.X, base.frame.Y, lado, (lado * base.texturaImagen.Bounds.Height) / base.texturaImagen.Bounds.Width);

            }
        }

        public void  MoverEnX()
        {
                if (direccion)
                {
                    PosX = PosX + desplazamientoX;
                }
                else
                {
                    PosX = PosX - desplazamientoX;
                }
           
        }
       
        public void MoverEnY()
        {
            int posicionFinalY = PosY + (int)(10.0 * Math.Sin(2*anguloOscilacion));
            //posicionInicialY = posicionFinalY;                
                frame = new Rectangle(frame.X+(burbujaFrame.Width-frame.Width)/2,posicionFinalY+(burbujaFrame.Height-frame.Height)/2,frame.Width,frame.Height);
                burbujaFrame = new Rectangle(frame.X-8, posicionFinalY-8, burbujaFrame.Width, burbujaFrame.Height);
        }
        public void MoverEnX_Aux()
        {
            if (direccion)
            {
                this.posAux.X = this.posAux.X + desplazamientoX;
                // se esta añadiendo en esta parte
                posicionInicialX = this.posAux.X ;


            }
            else
            {
                this.posAux.X = this.posAux.X - desplazamientoX;
                // se esta añadiendo en esta parte
                posicionInicialX = this.posAux.X ;
            }

        }
        public void MoverEnY_Aux()
        {
            posicionInicialY = posAux.Y + (int)(10.0 * Math.Sin(2 * anguloOscilacion));
        }

        public void ReiniciarObjeto()
        {

            this.PosX = posicionInicialX;
            this.PosY = posicionInicialY;
            capturado = false;

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (!capturado)
            {
                SharedSpriteBatch.Begin();
                SharedSpriteBatch.Draw(burbuja, burbujaFrame, Color.White);
                SharedSpriteBatch.End();
            }
        }

        public void IniciarAnimacionSalto()
        {
            tiempoIniciado = DateTime.Now;
            posicionIniAnimacionX = PosX;
            posicionIniAnimacionY = PosY;
            anguloAnimacion = anguloOscilacion;
            animado = true;
        }

        public void Animar()
        {
            DateTime tiempoActual = DateTime.Now;

            TimeSpan tiempoTranscurrido = tiempoActual - tiempoIniciado;
                PosY = posicionIniAnimacionY + (int)((double)120.0 * Math.Sin(-1.0 * MathHelper.Pi / 2.1) * tiempoTranscurrido.TotalMilliseconds * 0.01 + 8 * Math.Pow(tiempoTranscurrido.TotalMilliseconds * 0.01, 2.0));
                PosX = posicionIniAnimacionX + (int)((double)120.0 * Math.Cos(-1.0 * MathHelper.Pi / 2.1) * tiempoTranscurrido.TotalMilliseconds * 0.01);
        }

        public override void Update(GameTime gameTime)
        {

            MoverEnX_Aux();
            MoverEnY_Aux();

            this.anguloOscilacion += angulo;
            if (anguloOscilacion >= 360)
            {
                anguloOscilacion = 0;
            }
            if (!animado && !capturado)
            {
                    MoverEnX();
                    MoverEnY();

                    posAux.X = this.PosX;
                    posAux.Y = this.PosY;
            }
            else if(animado)
            {
                
                if ((DateTime.Now - tiempoIniciado).Seconds < 2)
                {
                    Animar();
                }
                else
                {
                    animado = false;
                    this.ReiniciarObjeto();
                }
            }
            
         

            base.Update(gameTime);
        }

        public void setDireccion(bool nuevaDireccion)
        {
            this.direccion = nuevaDireccion;

        }
        public String getRepuesta()
        {
            return this.respuesta;
        }
        public bool getPregunta()
        {
            return this.pregunta;
        }
        public bool colocarObjeto(ObjetoCapturable objeto)
        {
            if (this.respuesta == objeto.getRepuesta() && utilizado == false)
            {
                objeto.PosX = this.PosX;
                objeto.PosY = this.PosY;
                this.Visible = false;
                this.Activo = false;
                //Setear el la propiedad ACTIVO del ANSWER
                utilizado = true;

                string s = VariablesGlobales.palabraActual;
                string[] a = s.Select(c => c.ToString()).ToArray();
                float width = this.Game.GraphicsDevice.Viewport.Width;
                int longitudAnimal = a.Count();
                int espacio = 0;
                double i = ((width - (longitudAnimal * ObjetoCapturable.MIN_LADO_DEFAULT + espacio)) / 2.0) -
                    (ObjetoCapturable.MIN_LADO_DEFAULT / 2);

                List<int> posiciones = new List<int>();

                foreach (string letra in a)
                {
                    posiciones.Add((int)i - 30);
                    i = (int)i + ((int)width / 12) + espacio;
                }

                int indicepos = posiciones.IndexOf(this.PosX);
                indicepos++;

                double indiceposrelativo = (double)indicepos / (double)posiciones.Count();
                SeparadorDeSilabas nuevoSeparador = new SeparadorDeSilabas();

                ArrayList listaPosiciones = new ArrayList();
                String palabras = VariablesGlobales.palabraActual;
                listaPosiciones = nuevoSeparador.PosicionSilabas(palabras);

                List<string> listaSilabas = new List<string>();
                int nele = palabras.Length;

                string silaba = "";
                for (int j = 0; j < listaPosiciones.Count; j++)
                {
                    //Validamos que no estemos en la ultima silabra de la cadena
                    if (listaPosiciones.Count - j != 1)
                    {
                        for (int k = (int)listaPosiciones[j]; k < (int)listaPosiciones[j + 1]; k++)
                        {
                            //Validamos que la letra ingresada no sea un espacio
                            if (palabras[k] != ' ')
                                silaba += palabras[k];
                        }
                        listaSilabas.Add(silaba);
                        silaba = "";
                    }
                    else
                    {
                        for (int k = (int)listaPosiciones[j]; k < nele; k++)
                        {
                            //Validamos que la letra ingresada no sea un espacio
                            if (palabras[k] != ' ')
                                silaba += palabras[k];
                            else
                                break;
                        }
                        listaSilabas.Add(silaba);
                        silaba = "";
                    }
                }

                double indicesilaba = (double)(listaSilabas.Count() * indiceposrelativo);
                double indicesilabaaux = indicesilaba;
                indicesilabaaux++;
                if (indicesilabaaux != 0)
                    indicesilabaaux--;


                // if(VariablesGlobales.nivel == 1)
                indicesilabaaux = indicesilabaaux - 0.5;

                string silabaactual = listaSilabas.ElementAt((int)indicesilabaaux);
                VariablesGlobales.silabaActual = silabaactual;
                return true;
            }
            return false;
          


        }
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }


        public Point PosAux
        {
            get { return posAux; }
            set { posAux = value; }
        }
        public int Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}
