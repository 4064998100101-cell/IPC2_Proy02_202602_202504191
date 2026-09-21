namespace Proyecto2.Models
{
   
    public class NodoCategoria
    {
        public string Nombre { get; set; }
      
        public Estructuras.Listacategorias SubCategorias { get; set; }
        public Estructuras.ListaLibros LibrosAsociados { get; set; }

        public NodoCategoria(string nombre)
        {
            Nombre = nombre;
            SubCategorias = new Estructuras.Listacategorias();
            LibrosAsociados = new Estructuras.ListaLibros();
        }
    }

}