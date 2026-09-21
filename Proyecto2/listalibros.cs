namespace Proyecto2.Estructuras
{
   public class NodoLibro
    {
        public Models.Libro Valor { get; set; }
        public NodoLibro Siguiente { get; set; }

        public NodoLibro(Models.Libro libro)
        {
            Valor = libro;
            Siguiente = null;
        }
    

    }
    public class ListaLibros
    {
       public NodoLibro Cabeza { get; private set; }
        public int Tamanio { get; private set; }

        public ListaLibros()
        {
            Cabeza = null;
            Tamanio = 0;
        }

        public void Insertar(Models.Libro nuevoLibro)
        {
            NodoLibro nuevoNodo = new NodoLibro(nuevoLibro);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
            }
            else
            {
                NodoLibro actual = Cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            Tamanio++;
        }

        public Models.Libro BuscarPorIsbn(int isbn)
        {
            NodoLibro actual = Cabeza;
            while (actual != null)
            {
                if (actual.Valor.Isbn == isbn)
                {
                    return actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return null;
        }
    }
    
}