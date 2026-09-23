namespace Proyecto2
{
    public class NodoCategoria
    {
        public string Nombre { get; set; }
        public ArbolAvlLibros LibrosAsociados { get; set; } 
        public Listacategorias SubCategorias { get; set; }
        public NodoCategoria Izquierda { get; set; }
        public NodoCategoria Derecha { get; set; }

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            LibrosAsociados = new ArbolAvlLibros();
            SubCategorias = new Listacategorias();
            Izquierda = null;
            Derecha = null;
        }
    }
}