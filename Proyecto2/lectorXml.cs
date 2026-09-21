using System.Data.Common;
using System.IO;
using System.Xml.Linq;
namespace Proyecto2
{
    public class Xml
    {
        public void RegistrarLibro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);
            TodosLosLibrosGlobal.Insertar(nuevoLibro);

            NodoCategoria cat = RaizCategorias.Buscar(nombreCategoria);
            if (cat != null)
            {
                cat.LibrosAsociados.Insertar(nuevoLibro);
            }
            else
            {
                NodoCategoria nuevaCat = ObtenerOCrearCategoria(nombreCategoria, null);
                nuevaCat.LibrosAsociados.Insertar(nuevoLibro);
            }
        }

        public void ProcesarArchivoXml(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo)) return;

            XDocument doc = XDocument.Load(rutaArchivo);
            XElement config = doc.Element("config");

            if (config == null) return;

            // Procesar Categorías
            XElement listaCategorias = config.Element("listaCategorias");
            if (listaCategorias != null)
            {
                foreach (var catElement in listaCategorias.Elements("categoria"))
                {
                    string nombreCategoria = catElement.Value;
                    string nombrePadre = catElement.Attribute("padre")?.Value;
                    ObtenerOCrearCategoria(nombreCategoria, nombrePadre);
                }
            }

            // Procesar Libros
            XElement listaLibros = config.Element("listaLibros");
            if (listaLibros != null)
            {
                foreach (var libroElement in listaLibros.Elements("libro"))
                {
                    int isbn = int.Parse(libroElement.Element("ISBN")?.Value ?? "0");
                    string titulo = libroElement.Element("titulo")?.Value ?? "";
                    string autor = libroElement.Element("autor")?.Value ?? "";
                    string categoria = libroElement.Element("categoria")?.Value ?? "";

                    if (isbn > 0 && !string.IsNullOrEmpty(titulo))
                    {
                        RegistrarLibro(isbn, titulo, autor, categoria);
                    }
                }
            }
        }
    }
}