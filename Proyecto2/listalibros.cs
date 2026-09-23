namespace Proyecto2
{
    public class NodoLibro
    {
        public Libro Valor { get; set; }
        public NodoLibro Siguiente { get; set; }

        public NodoLibro(Libro libro)
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

        public void Insertar(Libro nuevoLibro)
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

        public Libro BuscarPorIsbn(int isbn)
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

        public Libro ObtenerMenorIsbn()
        {
            if (Cabeza == null) return null;
            NodoLibro actual = Cabeza;
            Libro menor = actual.Valor;
            while (actual != null)
            {
                if (actual.Valor.Isbn < menor.Isbn)
                {
                    menor = actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return menor;
        }

        public Libro ObtenerMayorIsbn()
        {
            if (Cabeza == null) return null;
            NodoLibro actual = Cabeza;
            Libro mayor = actual.Valor;
            while (actual != null)
            {
                if (actual.Valor.Isbn > mayor.Isbn)
                {
                    mayor = actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return mayor;
        }
    }
}