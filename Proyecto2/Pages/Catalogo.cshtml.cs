using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Proyecto2.Pages
{
    public class CatalogoModel : PageModel
    {
        private readonly Catalogo _catalogo;
        private readonly Xml _xmlService;

        public CatalogoModel(Catalogo catalogo, Xml xmlService)
        {
            _catalogo = catalogo;
            _xmlService = xmlService;
        }

        // Propiedades para formularios
        [BindProperty] public IFormFile ArchivoXml { get; set; }
        [BindProperty] public string NuevaCategoria { get; set; }
        [BindProperty] public string CategoriaPadre { get; set; }

        [BindProperty] public int Isbn { get; set; }
        [BindProperty] public string Titulo { get; set; }
        [BindProperty] public string Autor { get; set; }
        [BindProperty] public string CategoriaNombre { get; set; }

        [BindProperty] public int IsbnBuscado { get; set; }
        
        // Resultados para mostrar en la vista
        public Libro LibroEncontrado { get; set; }
        public string ResultadoExtremo { get; set; }
        public string Mensaje { get; set; }
        public string RutaImagenAvl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoriaSeleccionada { get; set; }

        public void OnGet()
        {
            // Generar gráfico AVL si se seleccionó una categoría
            if (!string.IsNullOrEmpty(CategoriaSeleccionada))
            {
                var cat = _catalogo.RaizCategorias.Buscar(CategoriaSeleccionada);
                if (cat != null)
                {
                    string dotContent = cat.LibrosAsociados.GenerarDot(cat.Nombre);
                    string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    if (!Directory.Exists(wwwRootPath)) Directory.CreateDirectory(wwwRootPath);

                    string dotPath = Path.Combine(wwwRootPath, "avl_temp.dot");
                    string imgPath = Path.Combine(wwwRootPath, "avl_temp.png");

                    System.IO.File.WriteAllText(dotPath, dotContent);

                    try
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "dot",
                            Arguments = $"-Tpng \"{dotPath}\" -o \"{imgPath}\"",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using (var proc = Process.Start(startInfo))
                        {
                            proc?.WaitForExit();
                        }
                        RutaImagenAvl = "/avl_temp.png";
                    }
                    catch
                    {
                        Mensaje = "Error al compilar Graphviz. Asegúrese de tenerlo instalado y agregado al PATH del sistema.";
                    }
                }
                else
                {
                    Mensaje = $"No se encontró la categoría '{CategoriaSeleccionada}'.";
                }
            }
        }

        // Opción 1: Cargar Archivo XML de configuración incremental
        public async Task<IActionResult> OnPostCargarXmlAsync()
        {
            if (ArchivoXml != null && ArchivoXml.Length > 0)
            {
                try
                {
                    var rutaTemporal = Path.GetTempFileName();
                    using (var stream = new FileStream(rutaTemporal, FileMode.Create))
                    {
                        await ArchivoXml.CopyToAsync(stream);
                    }
                    _xmlService.ProcesarArchivoXml(rutaTemporal);
                    if (System.IO.File.Exists(rutaTemporal)) System.IO.File.Delete(rutaTemporal);
                    Mensaje = "¡Archivo XML procesado e integrado exitosamente en las estructuras!";
                }
                catch (Exception ex)
                {
                    Mensaje = $"Error al leer el archivo XML: {ex.Message}";
                }
            }
            else
            {
                Mensaje = "Por favor, seleccione un archivo XML válido.";
            }
            return Page();
        }

        // Opción 2: Gestión de Categorías (Agregar nueva categoría / Subcategoría)
        public IActionResult OnPostAgregarCategoria()
        {
            if (!string.IsNullOrEmpty(NuevaCategoria))
            {
                _catalogo.ObtenerOCrearCategoria(NuevaCategoria, CategoriaPadre);
                Mensaje = $"Categoría '{NuevaCategoria}' agregada o enlazada correctamente.";
            }
            return Page();
        }

        // Opción 3: Gestión de Libros (Registrar nuevo libro en el AVL correspondiente)
        public IActionResult OnPostRegistrarLibro()
        {
            if (Isbn > 0 && !string.IsNullOrEmpty(Titulo) && !string.IsNullOrEmpty(CategoriaNombre))
            {
                _catalogo.RegistrarLibro(Isbn, Titulo, Autor, CategoriaNombre);
                Mensaje = $"Libro con ISBN {Isbn} registrado exitosamente en el Árbol AVL de '{CategoriaNombre}'.";
            }
            else
            {
                Mensaje = "Complete todos los campos obligatorios para registrar el libro.";
            }
            return Page();
        }

        // Opción 4: Buscar libro por ISBN
        public IActionResult OnPostBuscarIsbn()
        {
            LibroEncontrado = _catalogo.TodosLosLibrosGlobal.BuscarPorIsbn(IsbnBuscado);
            if (LibroEncontrado == null)
            {
                Mensaje = $"No se encontró ningún libro registrado con el ISBN {IsbnBuscado}.";
            }
            return Page();
        }

        // Opción 5: Mostrar Libro con menor o mayor ISBN
        public IActionResult OnPostMenorMayorIsbn(string accion)
        {
            if (accion == "menor")
            {
                var libroMenor = _catalogo.TodosLosLibrosGlobal.ObtenerMenorIsbn();
                ResultadoExtremo = libroMenor != null 
                    ? $"Menor ISBN -> ISBN: {libroMenor.Isbn} | Título: {libroMenor.Titulo} | Autor: {libroMenor.Autor}"
                    : "No hay libros registrados en el catálogo global.";
            }
            else if (accion == "mayor")
            {
                var libroMayor = _catalogo.TodosLosLibrosGlobal.ObtenerMayorIsbn();
                ResultadoExtremo = libroMayor != null 
                    ? $"Mayor ISBN -> ISBN: {libroMayor.Isbn} | Título: {libroMayor.Titulo} | Autor: {libroMayor.Autor}"
                    : "No hay libros registrados en el catálogo global.";
            }
            return Page();
        }
    }
}