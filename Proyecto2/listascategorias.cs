namespace Proyecto2
{
    public class Nodocat 
    {
        public Models.NodoCategoria Valor {get; private set;}
        public Nodocat Siguiente {get; set;}
        public Nodocat(Models.NodoCategoria categoria)
        {
            Valor = categoria;
            Siguiente = null;
        }
    }
    public class Listacategorias
    {
        public Nodocat Cabeza{ get; private set;}
        public int tamaño {get; private set;}
        public Listacategorias()
        {
            Cabeza=null;
            tamaño=0;
        }
        public void Insertar (Models.NodoCategoria nuevaCategoria)
        {
            Nodocat nuevonodo=new Nodocat(nuevaCategoria);
            if (Cabeza==null)
            {
                Cabeza=nuevonodo;
            }
            else
            {
                if (string.Compare(nuevaCategoria.nombre, Cabeza.Valor.nombre, StringComparison.OrdinalIgnoreCase) < 0)

                {
                nuevonodo.Siguiente=Cabeza;
                Cabeza=nuevonodo;    
                }
                else
                {
                    Nodocat actual=Cabeza;
                    while (actual.Siguiente!=null &&string.Compare(actual.Siguiente.Valor.nombre, nuevaCategoria.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        actual=actual.Siguiente;
                    }
                    nuevonodo.Siguiente=actual.Siguiente;
                    actual.Siguiente=nuevonodo;
                }
                

            }
            tamaño++;
        }
        public Models.NodoCategoria Buscar(string nombre)
        {
            Nodocat actual = Cabeza;
            while (actual != null)
            {
                if (string.Equals(actual.Valor.Nombre,nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Valor;
                }
                var encontradaHijas=actual.Valor.SubCategorias.Buscar(nombre);
                if (encontradaHijas!= null)
                {
                    return encontradaHijas;
                    
                }
                actual=actual.Siguiente;
            }
            return null;
        }
    }
}