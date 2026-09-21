namespace Proyecto2
{
    public class NodoLibro
    {
        public Models.libro Valor{get; private set;}
        public NodoLibro Siguiente {get; private set;}

        public NodoLibro(Models.libro libro)
        {
            Valor =libro;
            Siguiente= null;
        }

    }
    public class ListaLibros
    {
        public NodoLibro Cabeza {get; private set;}
        public int Tamaño {get; private set;}
        public ListaLibros()
        {
            Cabeza=null;
            Tamaño=0;
        }
        public void Insertar (Models.Libro nuevoLibro)
        {
            NodoLibro nuevoNodo=new NodoLibro(nuevoLibro);
            if (Cabeza==null)
            {
                Cabeza=nuevoLibro;
            }
            else
            {
                NodoLibro actual=Cabeza;
                while (actual.Siguiente!= null)
                {
                    actual=actual.Siguiente;
                }
                actual.Siguiente=nuevoNodo;
            }
            Tamaño ++;
        }

        public Models.libro BuscarISBN(int isbn)
        {
            NodoLibro actual=Cabeza;
            while (actual!= null)
            {
                if(actual.Valor.isbn==isbn)
                {
                    return actual.Valor;
                }
                actual=actual.Siguiente;
            }
            return null;
        }
    }
    
}