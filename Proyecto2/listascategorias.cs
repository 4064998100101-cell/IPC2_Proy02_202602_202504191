namespace Proyecto2.Estructuras
{
    public class Nodocat 
    {
        public Models.NodoCategoria Valor { get; set; }
        public Nodocat Siguiente { get; set; }

        public Nodocat(Models.NodoCategoria categoria)
        {
            Valor = categoria;
            Siguiente = null;
        }
    }
    public class Listacategorias
    {
       public Nodocat Cabeza { get; private set; }
        public int Tamanio { get; private set; }

        public Listacategorias()
        {
            Cabeza = null;
            Tamanio = 0;
        }

        public void Insertar(Models.NodoCategoria nuevaCategoria)
        {
            Nodocat nuevoNodo = new Nodocat(nuevaCategoria);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
            }
            else
            {
                if (string.Compare(nuevaCategoria.Nombre, Cabeza.Valor.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    nuevoNodo.Siguiente = Cabeza;
                    Cabeza = nuevoNodo;
                }
                else
                {
                    Nodocat actual = Cabeza;
                    while (actual.Siguiente != null && 
                           string.Compare(actual.Siguiente.Valor.Nombre, nuevaCategoria.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        actual = actual.Siguiente;
                    }
                    nuevoNodo.Siguiente = actual.Siguiente;
                    actual.Siguiente = nuevoNodo;
                }
            }
            Tamanio++;
        }

        public Models.NodoCategoria Buscar(string nombre)
        {
            Nodocat actual = Cabeza;
            while (actual != null)
            {
                if (string.Equals(actual.Valor.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Valor;
                }
                var encontradaEnHijas = actual.Valor.SubCategorias.Buscar(nombre);
                if (encontradaEnHijas != null) return encontradaEnHijas;

                actual = actual.Siguiente;
            }
            return null;
        }
    }
}