

namespace BingoWebApplication.Models {
    public class Carton {


        public ElementoNumero[,] Matriz = new ElementoNumero[3,9];

        public Carton( ) {
            for ( int f = 0; f < 3; f++ ) {
                for ( int c = 0; c < 9; c++ ) {
                    Matriz[f, c] = new ElementoNumero { Numero = 0, Acertado = false };
                }
            }
        }



    }
}
