namespace Proyecto2
{
   
    public class NodoCategoria
    {
        public string Nombre { get; set; }
      
        public Listacategorias SubCategorias { get; set; }
        public ListaLibros LibrosAsociados { get; set; }

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            SubCategorias = new Listacategorias();
            LibrosAsociados = new ListaLibros();
        }
    }

}