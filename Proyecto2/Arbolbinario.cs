namespace Proyecto2
{
    public class NodoArbolBinario
    {
        public Libro Valor { get; set; }
        public NodoArbolBinario Izquierda { get; set; }
        public NodoArbolBinario Derecha { get; set; }

        public NodoArbolBinario(Libro libro)
        {
            Valor = libro;
            Izquierda = null;
            Derecha = null;
        }
    }

    public class ArbolBinario
    {
        public NodoArbolBinario Raiz { get; private set; }

        public void Insertar(Libro libro)
        {
            Raiz = InsertarRec(Raiz, libro);
        }

        private NodoArbolBinario InsertarRec(NodoArbolBinario nodo, Libro libro)
        {
            if (nodo == null)
            {
                return new NodoArbolBinario(libro);
            }

            if (libro.Isbn < nodo.Valor.Isbn)
            {
                nodo.Izquierda = InsertarRec(nodo.Izquierda, libro);
            }
            else if (libro.Isbn > nodo.Valor.Isbn)
            {
                nodo.Derecha = InsertarRec(nodo.Derecha, libro);
            }

            return nodo;
        }

        public Libro BuscarPorIsbn(long isbn)
        {
            return BuscarRec(Raiz, isbn);
        }

        private Libro BuscarRec(NodoArbolBinario nodo, long isbn)
        {
            if (nodo == null) return null;
            if (nodo.Valor.Isbn == isbn) return nodo.Valor;

            if (isbn < nodo.Valor.Isbn)
            {
                return BuscarRec(nodo.Izquierda, isbn);
            }
            return BuscarRec(nodo.Derecha, isbn);
        }

        public Libro ObtenerMenorIsbn()
        {
            if (Raiz == null) return null;
            NodoArbolBinario actual = Raiz;
            while (actual.Izquierda != null)
            {
                actual = actual.Izquierda;
            }
            return actual.Valor;
        }

        public Libro ObtenerMayorIsbn()
        {
            if (Raiz == null) return null;
            NodoArbolBinario actual = Raiz;
            while (actual.Derecha != null)
            {
                actual = actual.Derecha;
            }
            return actual.Valor;
        }
    }
}