using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;



namespace ArmaPalabra.Gameplay
{


    public class ObjetosCapturables : DrawableGameComponent
    {

        private const int INDICE_AZUL = 0;
        private const int INDICE_ROJO = 1;
        private const int INDICE_AMARILLO = 2;
        private string[][] colorObjetos = {
                                                new string[]{ "objeto1","objeto2","objeto3","objeto4","objeto7","objeto9","objeto10"},//azul
                                                new string[]{ "objeto5", "objeto6","objeto8","objeto11","objeto12" },//rojo
                                               new string[] { "objeto13","objeto14","objeto15","objeto16","objeto17","objeto18" }//amarillo
                                               };


        List<ObjetoCapturable> lstObjetos;
        public List<int> niveles;
        public List<String> Letras;
        public List<String> Animales;


        public List<String> Comidas;
        public List<String> Objetos;


        public List<int> lstLetraNumero;
        public List<Point> vectorPosAux;


        private int indice = 0;
        private int contadorSegundo = 0;





        int initNivel = 0;
        int finNivel = 0;
        int nivel = 0;
        bool nivelTreminado = false;
        bool findeJuego = false;



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


        public ObjetosCapturables(Game game)
            : base(game)
        {
            //this.CrearObjetosColor(TipoObjeto.Azul,TipoObjeto.Rojo,7,5);
            this.LoadResources();
        }
        public List<ObjetoCapturable> ListaPreguntas()
        {
            return lstObjetos.FindAll(x => x.pregunta);
        }

        public List<ObjetoCapturable> ListaRespuestas()
        {
           
            return lstObjetos.FindAll(x => !x.pregunta);
        }



        public void LoadResources()
        {


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





            Random r = new Random();
            switch (VariablesGlobales.tipoObjeto)
            {
                case 1:

                    Animales = new List<string>();

                    Animales.Add("leon");


                   
                    Animales.Add("gato");

                          Animales.Add("perro");
                    //Animales.Add("cebra");
                    //Animales.Add("conejo");

                    //        Animales.Add("jirafa");


                    //          Animales.Add("caballo");
                    //Animales.Add("camello");

                    //  Animales.Add("elefante");
                    //  Animales.Add("abeja");

                    //Animales.Add("cerdo");
                    //Animales.Add("foca");
                    //Animales.Add("lobo");

                    //Animales.Add("tigre");
                    //Animales.Add("vaca");

                    //  Animales.Add("gallina");
                    //  Animales.Add("pato");


                    //  Animales.Add("loro");

                    //  Animales.Add("oso");
                    //  Animales.Add("jaguar");



                    //------Desordenar
                    //Random r = new Random();           
                    for (int i = Animales.Count - 1; i > 0; i--)
                    {
                        int rand = r.Next((i + 1));
                        String temp = Animales[i];
                        Animales[i] = Animales[rand];
                        Animales[rand] = temp;
                    }
                    //-----------------------------------

                    Letras = new List<string>();
                    niveles = new List<int>();
                    for (int i = 0; i < Animales.Count; i++)
                    {
                        string s = Animales[i];
                        string[] a = s.Select(c => c.ToString()).ToArray();
                        niveles.Add(a.Count());

                        for (int j = 0; j < a.Count(); j++)
                        {
                            Letras.Add(a[j]);
                        }
                    }

                    break;

                case 2:


                    Animales = new List<string>();

                    Animales.Add("arroz");
                    Animales.Add("piña");

                    Animales.Add("pera");
                    Animales.Add("torta");

                    Animales.Add("jamonada");
                    Animales.Add("lechuga");

                    Animales.Add("empanada");
                    Animales.Add("cereal");

                    //Animales.Add("sandia");
                    Animales.Add("galleta");




                    //------Desordenar

                    for (int i = Animales.Count - 1; i > 0; i--)
                    {
                        int rand = r.Next((i + 1));
                        String temp = Animales[i];
                        Animales[i] = Animales[rand];
                        Animales[rand] = temp;
                    }
                    //-----------------------------------

                    Letras = new List<string>();
                    niveles = new List<int>();
                    for (int i = 0; i < Animales.Count; i++)
                    {
                        string s = Animales[i];
                        string[] a = s.Select(c => c.ToString()).ToArray();
                        niveles.Add(a.Count());

                        for (int j = 0; j < a.Count(); j++)
                        {
                            Letras.Add(a[j]);

                        }
                    }




                    break;
                case 3:


                    Animales = new List<string>();

                    Animales.Add("mesa");
                    Animales.Add("caja");

                    Animales.Add("silla");
                    Animales.Add("lapiz");

                    Animales.Add("borrador");
                    Animales.Add("mochila");

                    Animales.Add("pelota");
                    Animales.Add("reloj");

                    Animales.Add("cuchara");
                    Animales.Add("telefono");

                    Animales.Add("tijera");


                    //------Desordenar
                    // Random r = new Random();           
                    for (int i = Animales.Count - 1; i > 0; i--)
                    {
                        int rand = r.Next((i + 1));
                        String temp = Animales[i];
                        Animales[i] = Animales[rand];
                        Animales[rand] = temp;
                    }
                    //-----------------------------------

                    Letras = new List<string>();
                    niveles = new List<int>();
                    for (int i = 0; i < Animales.Count; i++)
                    {
                        string s = Animales[i];
                        string[] a = s.Select(c => c.ToString()).ToArray();
                        niveles.Add(a.Count());

                        for (int j = 0; j < a.Count(); j++)
                        {
                            Letras.Add(a[j]);

                        }
                    }


                    break;



            }


            
            
      
           
            /*
          
    */  /*      Animales.Add("perro");
            Animales.Add("cebra");

            Animales.Add("jirafa");
            Animales.Add("conejo");

            Animales.Add("caballo");
            Animales.Add("camello");

            Animales.Add("elefante");
            */



         /*   Letras.Add("e");
            Letras.Add("r");
            Letras.Add("r");
            Letras.Add("o");
            Letras.Add("p");


            Letras.Add("e");
            Letras.Add("r");
            Letras.Add("b");
            Letras.Add("c");
            Letras.Add("a");


            Letras.Add("j");
            Letras.Add("f");
            Letras.Add("a");
            Letras.Add("r");
            Letras.Add("a");
            Letras.Add("i");

            Letras.Add("j");
            Letras.Add("e");
            Letras.Add("o");
            Letras.Add("c");
            Letras.Add("n");
            Letras.Add("o");

            Letras.Add("c");
            Letras.Add("a");
            Letras.Add("l");
            Letras.Add("o");
            Letras.Add("l");
            Letras.Add("b");
            Letras.Add("a");

            Letras.Add("l");
            Letras.Add("e");
            Letras.Add("a");
            Letras.Add("c");
            Letras.Add("m");
            Letras.Add("l");
            Letras.Add("o");

          



            Letras.Add("l");
            Letras.Add("e");
            Letras.Add("f");
            Letras.Add("e");
            Letras.Add("t");
            Letras.Add("n");
            Letras.Add("a");
            Letras.Add("e");
            */

                //  Letras.Add("s");
                //  Letras.Add("o");
                //  Letras.Add("o");


                //  Letras.Add("b");
                //  Letras.Add("l");
                //  Letras.Add("o");
                //  Letras.Add("o");

                //  Letras.Add("o");
                //  Letras.Add("l");
                //  Letras.Add("r");
                //  Letras.Add("o");

    

                //  Letras.Add("o");
                //  Letras.Add("f");
                //  Letras.Add("a");
                //  Letras.Add("c");

                //  Letras.Add("c");
                //  Letras.Add("a");
                //  Letras.Add("a");
                //  Letras.Add("v");

                //  Letras.Add("i");
                //  Letras.Add("r");
                //  Letras.Add("e");
                //  Letras.Add("g");
                //  Letras.Add("t");

                //  //Letras.Add("a");
                //  //Letras.Add("l");
                //  //Letras.Add("a");
                //  //Letras.Add("m");
                //  //Letras.Add("l");

                //  Letras.Add("c");
                //  Letras.Add("r");
                //  Letras.Add("o");
                //  Letras.Add("d");
                //  Letras.Add("e");


            
    


       
            /*    niveles.Add(5);
                niveles.Add(5);
                niveles.Add(6);
                niveles.Add(6);  
                niveles.Add(7);  
                niveles.Add(7);
                niveles.Add(8);
           
         */

            lstObjetos = new List<ObjetoCapturable>();
            //vectorPosAux = new List<Point>();


            //qui cargo todas las letras         
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_a", this.Game, 0,  0, false, "a"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_b", this.Game, 0,  0, false, "b"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_c", this.Game, 0,  0, false, "c"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_d", this.Game, 0,  0, false, "d"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_e", this.Game, 0,  0, false, "e"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_f", this.Game, 0,  0, false, "f"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_g", this.Game, 0,  0, false, "g"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_h", this.Game, 0,  0, false, "h"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_i", this.Game, 0,  0, false, "i"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_j", this.Game, 0,  0, false, "j"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_k", this.Game, 0,  0, false, "k"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_l", this.Game, 0,  0, false, "l"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_m", this.Game, 0,  0, false, "m"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_ni", this.Game, 0, 0, false, "n"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_n", this.Game, 0, 0, false, "n"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_o", this.Game, 0, 0, false, "o"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_p", this.Game, 0, 0, false, "p"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_q", this.Game, 0, 0, false, "q"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_r", this.Game, 0, 0, false, "r"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_s", this.Game, 0, 0, false, "s"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_t", this.Game, 0, 0, false, "t"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_u", this.Game, 0, 0, false, "u"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_v", this.Game, 0, 0, false, "v"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_w", this.Game, 0, 0, false, "w"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_x", this.Game, 0, 0, false, "x"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_y", this.Game, 0, 0, false, "y"));
            //lstObjetos.Add(new ObjetoCapturable("letras/letra_z", this.Game, 0, 0, false, "z"));

            lstLetraNumero = new List<int>();
            //for (int i = 0; i < Letras.Count; i++)
            //{
            //    switch (Letras[i])
            //    {

            //        case "a": lstLetraNumero.Add(0); break;
            //        case "b": lstLetraNumero.Add(1); break;
            //        case "c": lstLetraNumero.Add(2); break;
            //        case "d": lstLetraNumero.Add(3); break;
            //        case "e": lstLetraNumero.Add(4); break;
            //        case "f": lstLetraNumero.Add(5); break;
            //        case "g": lstLetraNumero.Add(6); break;
            //        case "h": lstLetraNumero.Add(7); break;
            //        case "i": lstLetraNumero.Add(8); break;
            //        case "j": lstLetraNumero.Add(9); break;
            //        case "k": lstLetraNumero.Add(10); break;
            //        case "l": lstLetraNumero.Add(11); break;
            //        case "m": lstLetraNumero.Add(12); break;
            //        case "ni": lstLetraNumero.Add(13); break;//ñ
            //        case "n": lstLetraNumero.Add(14); break;
            //        case "o": lstLetraNumero.Add(15); break;
            //        case "p": lstLetraNumero.Add(16); break;
            //        case "q": lstLetraNumero.Add(17); break;
            //        case "r": lstLetraNumero.Add(18); break;
            //        case "s": lstLetraNumero.Add(19); break;
            //        case "t": lstLetraNumero.Add(20); break;
            //        case "u": lstLetraNumero.Add(21); break;
            //        case "v": lstLetraNumero.Add(22); break;
            //        case "w": lstLetraNumero.Add(23); break;
            //        case "x": lstLetraNumero.Add(24); break;
            //        case "y": lstLetraNumero.Add(25); break;
            //        case "z": lstLetraNumero.Add(26); break;


            //    }

            //}
            LoadWord(); //Solo para la primera palabra
            LoadAnswer();
            LoadAnimal();
            VariablesGlobales.palabraActual = Animales[nivel];
        }

        public bool ubicateQuestion()
        {
            int width = this.Game.GraphicsDevice.Viewport.Width;            
            int i = 1;
            try
            {
                foreach (ObjetoCapturable oc in lstObjetos)
                {
                    if (oc.pregunta)
                    {
                        oc.PosX = i * (width / 15);//12
                        oc.PosY = 100;
                        i++;
                    }
                }
                return true;

            }catch(Exception e)
            {
            return false;
            }


       

        }
  
        public bool ubicateAnswer()
        {
            float width = this.Game.GraphicsDevice.Viewport.Width;
            char[] animal = Animales[nivel].ToCharArray();
            int longitudAnimal = animal.Length;
            int espacio = 0;            
            double i = ((width - (longitudAnimal * ObjetoCapturable.MIN_LADO_DEFAULT + espacio)) / 2.0) - (ObjetoCapturable.MIN_LADO_DEFAULT /2 );
            try
            {
                foreach (ObjetoCapturable oc in lstObjetos)
                {


                    if (!oc.pregunta)
                    {
                        oc.PosX = (int)i - 30;
                        oc.PosY = 475;
                        i = (int)i + ((int)width / 12) + espacio;
                    }

                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public void ubicateAnimal()
        {
            int width = this.Game.GraphicsDevice.Viewport.Width;

            foreach (ObjetoCapturable oc in lstObjetos)
            {
                if (!oc.pregunta && oc.animal)
                {
                    oc.PosX = 700;
                    oc.PosY = 250;

                }
            }

        }

        public bool LoadWord()
        {
            if (nivelTreminado)
            {
                DelteAll();
                if (nivel < niveles.Count)// +1 para que no se salga del rango al momento de cargar el siguiente nivel
                {
                    initNivel += niveles[nivel];
                    nivel++;
                    finNivel += niveles[nivel];
                    nivelTreminado = false;

                    VariablesGlobales.palabraActual = Animales[nivel];
                }
            }
            else
            {
                finNivel = niveles[nivel];
            }


            List<String> letrasAuxiliar;
            letrasAuxiliar = new List<String>();

            try
            {
                for (int i = initNivel; i < finNivel; i++)
                {
                    switch (VariablesGlobales.nivel)
                    {
                        case 1:
                            if (Letras[i] == "a" || Letras[i] == "e" || Letras[i] == "i" || Letras[i] == "o" || Letras[i] == "u")
                                //AddQuestion(Letras[i]);
                                letrasAuxiliar.Add(Letras[i]);
                            break;

                        case 2:

                            if (Letras[i] != "a" && Letras[i] != "e" && Letras[i] != "i" && Letras[i] != "o" && Letras[i] != "u")
                                // AddQuestion(Letras[i]);
                                letrasAuxiliar.Add(Letras[i]);

                            break;
                        case 3:

                            //AddQuestion(Letras[i]);

                            letrasAuxiliar.Add(Letras[i]);
                            break;

                    }

                }


                // aqui le podemos agregar mas complejidad  dependiendo de la palabra 
                Random r = new Random();
                for (int j = 0; j < 3; j++)
                {
                    //de la "a" a la "z"

                    Char complement = (char)r.Next('a', 'z');

                    if (VariablesGlobales.nivel == 2)
                    {
                        if (complement != 'a' && complement != 'e' && complement != 'i' && complement != 'o' && complement != 'u')
                        {


                            letrasAuxiliar.Add(complement.ToString());
                        }
                        else
                            j--;


                    }
                    if (VariablesGlobales.nivel == 1)
                    {
                        if (complement == 'a' || complement == 'e' || complement == 'i' || complement == 'o' || complement == 'u')
                        {

                            letrasAuxiliar.Add(complement.ToString());

                        }
                        else
                            j--;

                    }
                    if (VariablesGlobales.nivel == 3)
                    {

                        letrasAuxiliar.Add(complement.ToString());
                        // AddQuestionComplement(complement.ToString());
                    }



                }



                //------Desordenar
                for (int i = letrasAuxiliar.Count - 1; i > 0; i--)
                {
                    int rand = r.Next((i + 1));
                    String temp = letrasAuxiliar[i];
                    letrasAuxiliar[i] = letrasAuxiliar[rand];
                    letrasAuxiliar[rand] = temp;
                }
                //--------------------
                for (int i = 0; i < letrasAuxiliar.Count; i++)
                {
                    AddQuestion(letrasAuxiliar[i]);
                }
                ubicateQuestion();

                return true;
            }
            catch (Exception e)
            {

                return false;
            }

        }
//--------------------------
        public bool LoadAnswer()
        {
            char[] animal = Animales[nivel].ToCharArray();

            try
            {
                for (int i = 0; i < animal.Length; i++)
                {



                    switch (VariablesGlobales.nivel )
                    {

                        case 1:
                        
                    if (animal[i] == 'a' || animal[i] == 'e' || animal[i] == 'i' || animal[i] == 'o' || animal[i] == 'u')
                        AddAnswer(animal[i].ToString());
                    else
                        AddAnswerConsonante(animal[i].ToString());

                    break;
                        case 2:

                    if (animal[i] != 'a' && animal[i] != 'e' && animal[i] != 'i' && animal[i] != 'o' && animal[i] != 'u')
                        AddAnswer(animal[i].ToString());
                    else
                        AddAnswerConsonante(animal[i].ToString());

                    break;

                        case 3:

                    AddAnswer(animal[i].ToString());

                    break;

                    }
            
            
            
            }
                ubicateAnswer();

                return true;
            }
            catch (Exception e)
            {
                return false;            
            }
        }
//------------------------------------------
        public void LoadAnimal()
        {
            if (nivel < niveles.Count)
            {
                AddAnimal(Animales[nivel]);
                ubicateAnimal();
            }
        }



        public void DelteAll()
        {
            //for (int i = initNivel; i <  finNivel; i++)
            //{
            //    lstObjetos[i].Activo = false; // aqui le podemos agregar mas complejidad 
            //    lstObjetos[i].pregunta = false;
            //}
            lstObjetos.Clear();
        }

        public void AddQuestion(String letter)
        {
            lstObjetos.Add(new ObjetoCapturable("letras/letra_" + letter, this.Game, 0, 0, true, letter));
        }

        public void AddQuestionComplement(String letter)
        {
            lstObjetos.Add(new ObjetoCapturable("letras/letra_" + letter, this.Game, 0, 0, true, letter));
        }
        public void AddAnswer(String letter)
        {
            lstObjetos.Add(new ObjetoCapturable("base_Letter", this.Game, 0, 0, false, letter));
        }
        //--------------------------------
        public bool AddAnswerConsonante(String letter)
        {
            try
            {
                lstObjetos.Add(new ObjetoCapturable("letras/letra_" + letter, this.Game, 0, 0, false, letter));
                return true;
            }catch(Exception e)
            {
            return false;            
            }

        }
        //----------------------------------------------------
        public void AddAnimal(String animal)
        {
           switch (VariablesGlobales.tipoObjeto)
           {
        
               case 1:
               lstObjetos.Add(new ObjetoCapturable("animales/img_" + animal, this.Game, 0, 0, false, "animal"));

               break;

               case 2:
               lstObjetos.Add(new ObjetoCapturable("comidas/img_" + animal, this.Game, 0, 0, false, "animal"));

               break;

               case 3:
               lstObjetos.Add(new ObjetoCapturable("objetos/img_" + animal, this.Game, 0, 0, false, "animal"));

               break;
        
        }
        
        
        
        }

        public void CrearObjetoTamanio()
        {
            lstObjetos = new List<ObjetoCapturable>();
            Random random = new Random();
            int total = random.Next(18, 25);
            for (int i = 0; i < total; i++)
            {
                //int numero = random.Next(1, 18);
                //lstObjetos.Add(new ObjetoCapturable("objeto"+numero, this.Game,  (numero%2==0)?TipoObjeto.Grande:TipoObjeto.Pequenio, this.Game.Window.ClientBounds.Width + 10, 35) { Visible = false, Enabled = false });
            }
        }
        public override void Draw(GameTime gameTime)
        {
            lstObjetos.Last().Draw(gameTime);
            foreach (ObjetoCapturable oc in lstObjetos)
            {
                if (oc.Visible && !oc.respuesta.Equals("animal"))
                    oc.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {

            if (levelDone())
            {
                nivelTreminado = true;

                VariablesGlobales.pronunciarPalabra = true;

                /*switch (VariablesGlobales.palabraActual)
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

                }*/

                
                

                //Deberia de haber una especie de animacion entre niveles 
                if (nivel + 1 < niveles.Count)
                {
                    LoadWord();
                    LoadAnswer();
                    LoadAnimal();
                }
                else if(nivel+1 == niveles.Count) //Validadción para el último nivel
                {
                    findeJuego = true;
                    
                    SesionPartida.Instancia.finPartida = true;
                  
                    nivel++;
                }

            }
            if (gameTime.TotalGameTime.Milliseconds == 0) return;

            List<ObjetoCapturable> objectosPregunta = lstObjetos.FindAll(x => (x as ObjetoCapturable).pregunta == true && (x as ObjetoCapturable).capturado == false);
            List<ObjetoCapturable> objetosRespuesta = lstObjetos.FindAll(x => (x as ObjetoCapturable).pregunta == false);

            checkDirection();


            if (Screens.GameBScreen.contadorIntervalo % 30 == 0 && lstObjetos.Count > 0)
            {
                if (lstObjetos.Count <= indice)
                    indice = 0;

                int cantidadEnPantalla = lstObjetos.Count(x => x.Enabled == true && x.capturado == false);

                if (cantidadEnPantalla <= 6)
                {

                    if (!lstObjetos[indice].Enabled)
                    {
                        List<ObjetoCapturable> activados = lstObjetos.FindAll(x => x.Enabled == true && x.capturado == false);
                        if (activados.Count == 0)
                        {
                            lstObjetos[indice].Enabled = lstObjetos[indice].Visible = true;
                            indice++;
                            //System.Diagnostics.Debug.WriteLine("Indice siguiente {0}", indice);
                        }
                        else
                        {
                            bool isConfirmEnable = true;
                            for (int i = 0; i < activados.Count; i++)
                            {
                                //System.Diagnostics.Debug.WriteLine("{0} > {1}", activados[i].frame.Right + 10, this.Game.GraphicsDevice.Viewport.Width);
                                if (activados[i].frame.Right + 10 > this.Game.GraphicsDevice.Viewport.Width)
                                {

                                    isConfirmEnable = false;
                                    break;
                                }
                            }
                            lstObjetos[indice].Enabled = lstObjetos[indice].Visible = isConfirmEnable;
                            if (isConfirmEnable)
                                indice++;

                        }
                    }
                    else
                    {
                        indice++;
                    }
                }
            }

            foreach (ObjetoCapturable oc in lstObjetos)
            {
                if (oc.Enabled)
                    oc.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public ObjetoCapturable DarObjetoCapturado(Rectangle areaObjeto)
        {
            foreach (ObjetoCapturable oc in lstObjetos)
            {
                if (oc.frame.Intersects(areaObjeto) && !oc.capturado && !oc.animado && oc.pregunta)
                    return oc;
            }

            return null;
        }

        public void RemoverObjetoCapturado(ObjetoCapturable objeto)
        {
            lstObjetos.Remove(objeto);
        }

        public int ContarObjetos()
        {
            return lstObjetos.Count;
        }

        public void checkDirection()
        {

            foreach (ObjetoCapturable ocp in ListaPreguntas())
            {
                if (ocp.PosX < 0)
                {
                    foreach (ObjetoCapturable oc in ListaPreguntas())
                        oc.setDireccion(true);
                }
                else if (ocp.PosX + ocp.frame.Width > this.Game.GraphicsDevice.Viewport.Width)
                {
                    foreach (ObjetoCapturable oc in ListaPreguntas())
                        oc.setDireccion(false);
                }
            }


        }
        //------------------------------------------
        public bool levelDone()
        {

            foreach (ObjetoCapturable oc in ListaRespuestas())
            {

                switch (VariablesGlobales.nivel)
                {
                    case 1:


                        if (oc.respuesta == "a" || oc.respuesta == "e" || oc.respuesta == "i" || oc.respuesta == "o" || oc.respuesta == "u")

                            if (!oc.animal)
                            {
                                if (oc.Activo)
                                {
                                    return false;
                                }
                            }
                        break;


                    case 2 :

                        if (oc.respuesta != "a" && oc.respuesta != "e" && oc.respuesta != "i" && oc.respuesta != "o" && oc.respuesta != "u")
                            if (!oc.animal)
                            {
                                if (oc.Activo)
                                {
                                    return false;
                                }
                            }

                        break;

                    case 3:

                             if (!oc.animal)
                            {
                                if (oc.Activo)
                                {
                                    return false;
                                }
                            }


                        break;
                }

               

            }

          
            return true;

        }
        //--------------------------------------------------
        public int getNivel()
        {
            return nivel;
        }

        public int dameNumeroAnimales()
        {
            return Animales.Count;
        }

        public bool dameFinDelJuego()
        {
            return findeJuego;
        }

    }
}
