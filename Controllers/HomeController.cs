using BingoWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BingoWebApplication.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        public List<Carton> listCartones = new List<Carton>();

        public HomeController( ILogger<HomeController> logger ) {
            _logger = logger;
        }
        public IActionResult Privacy( ) {
            return View( );
        }
        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error( ) {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }


        public bool modificado=false;
        public IActionResult Index( ) {
            if ( modificado!=true) {
                for ( int i = 0; i < 4; i++ ) {
                    listCartones.Add( GetBingoCard( ) );
                }
            }
            return View( listCartones );//que se mande una Lista de 4 Cartones
        }




        public Carton GetBingoCard( ) {
            //-------------------------------generamos los numeros del carton
            var genRandom = new Random();
            Carton carton = new Carton();

            for ( int c = 0; c < 9; c++ ) {
                for ( int f = 0; f < 3; f++ ) {
                    int nuevoNumero = 0;
                    bool encontreUnoNuevo = true;
                    while ( encontreUnoNuevo ) { //V
                        if ( c == 0 ) { //columna 0
                            nuevoNumero = genRandom.Next( 1, 10 ); //1 al 9
                        } else { //todas las demas columnas
                            nuevoNumero = genRandom.Next( c * 10, c * 10 + 10 ); //4*10=40  4*10+10=50 Next(40,50)  del 40 al 49
                        }

                        //buscamos si el nuevoNumero existe en la columna
                        for ( int f2 = 0; f2 < 3; f2++ ) {
                            if ( carton.Matriz[f2, c].Numero == nuevoNumero ) {
                                encontreUnoNuevo = true;
                                break; //volver al while()
                            } else {
                                encontreUnoNuevo = false; //no vuelve a entrar en el while => lo asigna a la matriz
                            }
                        }//si salio del bucle y no encontro repetidos, encontreUnoNuevo=true y sale del bucle

                    }//while

                    carton.Matriz[f, c].Numero = nuevoNumero; //asignacion
                }//for de filas
            }//for de columnas

            //-------------------------------ordenamos las columnas de menor a mayor
            //metodo de ordenacion Burbuja
            for ( int c = 0; c < 9; c++ ) {
                for ( int f = 0; f < 3; f++ ) {
                    for ( int k = f + 1; k < 3; k++ ) {//compara el elem con el de la sig fila
                        if ( carton.Matriz[f, c].Numero > carton.Matriz[k, c].Numero ) {//se hace el intercambio
                            int aux = carton.Matriz[f, c].Numero;
                            carton.Matriz[f, c].Numero = carton.Matriz[k, c].Numero;
                            carton.Matriz[k, c].Numero = aux;
                        }
                    }
                }
            }

            //-------------------------------borramos celdas
            var borrados = 0;
            while ( borrados < 12 ) {

                var filaABorrar = genRandom.Next(0, 3);
                var columnaABorrar = genRandom.Next(0, 9);

                //-------------------------------------------------------------

                //si ya tiene cero, no borrar (ya esta borrado!)
                if ( carton.Matriz[filaABorrar, columnaABorrar].Numero == 0 ) { continue; }


                //-------------------------------------------------------------

                //contamos cuantos ceros hay en esta FILA
                var cerosEnFila = 0;
                for ( int c = 0; c < 9; c++ ) {
                    if ( carton.Matriz[filaABorrar, c].Numero == 0 ) {
                        cerosEnFila++;
                    }
                }
                //contamos cuantos ceros hay en esta COLUMNA
                var cerosEnColumna = 0;
                for ( int f = 0; f < 3; f++ ) {
                    if ( carton.Matriz[f, columnaABorrar].Numero == 0 ) cerosEnColumna++;
                }

                // si ya hay 4 ceros en la fila o si ya hay 2 ceros en la columna => no hago nada
                if ( cerosEnFila == 4 || cerosEnColumna == 2 ) {
                    continue;
                }//no hace nada

                //-------------------------------------------------------------

                //contamos cuantos items tenemos en cada columna
                var itemsPorColuma = new int[9];
                for ( int c = 0; c < 9; c++ ) {
                    for ( int f = 0; f < 3; f++ ) {
                        if ( carton.Matriz[f, c].Numero != 0 ) itemsPorColuma[c]++;
                    }
                }
                //contamos cuantas columnas hay con un solo numero
                var columnasConUnSoloNumero = 0;
                for ( int c = 0; c < 9; c++ ) {
                    if ( itemsPorColuma[c] == 1 ) columnasConUnSoloNumero++;
                }

                //si hay 3 columnas con 1 solo numero, a partir de ahora debo borrar solo las columnas que tienen 3 items
                if ( columnasConUnSoloNumero == 3 && itemsPorColuma[columnaABorrar] != 3 ) {
                    continue;
                }

                //-------------------------------------------------------------//-------------------------------------------------------------
                //si no se cumplieron las opciones anteriores, BORRAR EL NUMERO
                carton.Matriz[filaABorrar, columnaABorrar].Numero = 00;
                borrados++;

            }//while

            return carton;
        } //GetBingoCard

        public int bolilla;
        public void LanzarBolilla( ) {
            //tira bolilla
            bolilla = new Random().Next( 1, 91 );
            ViewBag.Bolilla = bolilla;
            Console.WriteLine( "Bolilla", bolilla );

            //comprobar si el num está en los 4 cartones
            foreach ( Carton carton in listCartones ) {
                for ( int f = 0; f < 3; f++ ) {
                    for ( int c = 0; c < 9; c++ ) {
                        if ( carton.Matriz[f, c].Numero == bolilla ) {
                            //numero encontrado
                            carton.Matriz[f, c].Acertado = true;
                            modificado = true;
                            Console.WriteLine( "Numero encontrado: ", carton.Matriz[f, c].Numero );
                            Console.WriteLine( "Modificado: ", carton.Matriz[f, c].Acertado );
                        }//if 
                    }//for columnas
                }//for filas
            }//foreach


        }//void



    }//class
}//namespace