using System;
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
            if (!File.Exists(rutaArchivo))
            {
                throw new Exception("El archivo temporal no existe en el disco.");
            }

            XDocument doc = XDocument.Load(rutaArchivo);
            
            // Buscamos <config> o <configuracion> para ser flexibles
            XElement config = doc.Element("config") ?? doc.Element("configuracion");

            if (config == null)
            {
                throw new Exception("No se encontró la etiqueta raíz <config> o <configuracion> en el XML.");
            }

            XElement listaCategorias = config.Element("listaCategorias") ?? config.Element("lista_categorias");
            if (listaCategorias != null)
            {
                foreach (var catElement in listaCategorias.Elements("categoria"))
                {
                    string nombreCategoria = catElement.Value;
                    string nombrePadre = catElement.Attribute("padre")?.Value;
                    _catalogo.ObtenerOCrearCategoria(nombreCategoria, nombrePadre);
                }
            }

            XElement listaLibros = config.Element("listaLibros") ?? config.Element("lista_libros");
            if (listaLibros != null)
            {
                foreach (var libroElement in listaLibros.Elements("libro"))
                {
                    int isbn = int.Parse(libroElement.Element("ISBN")?.Value ?? libroElement.Element("isbn")?.Value ?? "0");
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