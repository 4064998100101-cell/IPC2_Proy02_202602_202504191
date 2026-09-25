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

        
            var categoriasElements = config.Element("listaCategorias")?.Elements("categoria") 
                                  ?? config.Element("lista_categorias")?.Elements("categoria") 
                                  ?? config.Elements("categoria");

            bool huboProgreso = true;
            while (huboProgreso)
            {
                huboProgreso = false;

                foreach (var catElement in categoriasElements)
                {
                    string nombreCategoria = catElement.Value?.Trim();
                    string nombrePadre = catElement.Attribute("padre")?.Value?.Trim();

                    if (string.IsNullOrEmpty(nombreCategoria)) continue;

                    // Si la categoría ya fue creada en el catálogo, la saltamos
                    if (_catalogo.RaizCategorias.Buscar(nombreCategoria) != null) continue;

                    if (string.IsNullOrEmpty(nombrePadre))
                    {
                        // Es una categoría raíz válida
                        _catalogo.ObtenerOCrearCategoria(nombreCategoria, null);
                        huboProgreso = true;
                    }
                    else
                    {
                        // Verificamos si el padre ya existe en el sistema
                        var padreEncontrado = _catalogo.RaizCategorias.Buscar(nombrePadre);
                        if (padreEncontrado != null)
                        {
                            _catalogo.ObtenerOCrearCategoria(nombreCategoria, nombrePadre);
                            huboProgreso = true;
                        }
                    }
                }
            }

            var librosElements = config.Element("listaLibros")?.Elements("libro") 
                              ?? config.Element("lista_libros")?.Elements("libro") 
                              ?? config.Elements("libro");

            foreach (var libroElement in librosElements)
            {
                string textoIsbn = libroElement.Element("ISBN")?.Value ?? libroElement.Element("isbn")?.Value;
                string titulo = libroElement.Element("titulo")?.Value ?? "";
                string autor = libroElement.Element("autor")?.Value ?? "";
                string categoria = libroElement.Element("categoria")?.Value ?? "";

    
                if (long.TryParse(textoIsbn, out long isbn) && isbn > 0 && !string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(categoria))
                {
                
                    if (_catalogo.TodosLosLibrosGlobal.BuscarPorIsbn(isbn) != null)
                    {
                        continue; 
                    }

                 
                    var categoriaDestino = _catalogo.RaizCategorias.Buscar(categoria);
                    if (categoriaDestino == null)
                    {
                        continue; 
                    }

                    _catalogo.RegistrarLibro(isbn, titulo, autor, categoria);
                }
            }
        }
    }
}