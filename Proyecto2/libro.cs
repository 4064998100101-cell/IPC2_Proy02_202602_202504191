
namespace Proyecto2
{
   public class Libro
    {
        public long Isbn { get; set; }  
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string CategoriaNombre { get; set; }

        public Libro(long isbn, string titulo, string autor, string categoriaNombre)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
            CategoriaNombre = categoriaNombre;
        }
    }
}