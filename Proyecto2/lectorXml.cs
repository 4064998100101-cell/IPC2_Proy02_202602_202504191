using System.IO;
using System.Xml.Linq;

namespace Proyecto2
{
    public class Xml
    {
        private readonly Catalogo _catalogo;

        public Xml(Catalogo catalogoManager)
        {
            _catalogo = catalogoManager;
        }

        public void ProcesarArchivoXml(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo)) return;

            XDocument doc = XDocument.Load(rutaArchivo);
            XElement config = doc.Element("config");

            if (config == null) return;

            XElement listaCategorias = config.Element("listaCategorias");
            if (listaCategorias != null)
            {
                foreach (var catElement in listaCategorias.Elements("categoria"))
                {
                    string nombreCategoria = catElement.Value;
                    string nombrePadre = catElement.Attribute("padre")?.Value;
                    _catalogo.ObtenerOCrearCategoria(nombreCategoria, nombrePadre);
                }
            }

            // Procesar Lista de Libros (Opcional / Incremental)
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
                        _catalogo.RegistrarLibro(isbn, titulo, autor, categoria);
                    }
                }
            }
        }
    }
}