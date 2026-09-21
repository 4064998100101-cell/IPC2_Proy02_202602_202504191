using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto2.Models
{
   public class Libro
    {
        public int Isbn { get; set; }  
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string CategoriaNombre { get; set; }

        public Libro(int isbn, string titulo, string autor, string categoriaNombre)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
            CategoriaNombre = categoriaNombre;
        }
    }
}