using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Proyecto2
{
    public class CatalogoModel : PageModel
    {
        private readonly Catalogo _catalogo;
        private readonly Xml _xmlService;
        private ListaLibros _libro;

        public CatalogoModel(Catalogo catalogo, Xml xmlService)
        {
            _catalogo = catalogo;
            _xmlService = xmlService;
        }

        // Propiedades para formularios
        [BindProperty] public IFormFile ArchivoXml { get; set; }
        [BindProperty] public string NuevaCategoria { get; set; }
        [BindProperty] public string CategoriaPadre { get; set; }

        [BindProperty] public long Isbn { get; set; }
        [BindProperty] public string Titulo { get; set; }
        [BindProperty] public string Autor { get; set; }
        [BindProperty] public string CategoriaNombre { get; set; }

        [BindProperty] public long IsbnBuscado { get; set; }
        [BindProperty] public long IsbnAEliminar { get; set; }
        [BindProperty] public string CategoriaParaEliminar { get; set; }
        
        // Resultados para mostrar en la vista
        public Libro LibroEncontrado { get; set; }
        public string ResultadoExtremo { get; set; }
        public string Mensaje { get; set; }
        public string RutaImagenAvl { get; set; }
        public string RutaImagenCategorias { get; set; }
        public string LibrosOrdenadosHtml { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoriaSeleccionada { get; set; }

        public void OnGet()
        {
            GenerarReportesVisuales();
        }

        private void CompilarGraphviz(string dotPath, string imgPath)
        {
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
            }
            catch
            {
                // Graphviz no disponible o error en PATH
            }
        }

        // Opción 1: Cargar Archivo XML
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
                    
                    Mensaje = "¡Archivo XML procesado e integrado exitosamente aplicando restricciones y linking diferido!";
                    GenerarReportesVisuales();
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

        // Opción 2: Gestión de Categorías
        public IActionResult OnPostAgregarCategoria()
        {
            if (!string.IsNullOrEmpty(NuevaCategoria))
            {
                var resultado = _catalogo.ObtenerOCrearCategoria(NuevaCategoria, CategoriaPadre);
                if (resultado != null)
                {
                    Mensaje = $"Categoría '{NuevaCategoria}' agregada o enlazada correctamente.";
                }
                else
                {
                    Mensaje = $"No se pudo crear la categoría '{NuevaCategoria}'. Verifique que el padre exista.";
                }
                GenerarReportesVisuales();
            }
            return Page();
        }

        // Opción 3: Registro de Libros
        public IActionResult OnPostRegistrarLibro()
        {
            if (Isbn > 0 && !string.IsNullOrEmpty(Titulo) && !string.IsNullOrEmpty(CategoriaNombre))
            {
                bool exito = _catalogo.RegistrarLibro(Isbn, Titulo, Autor, CategoriaNombre);
                if (exito)
                {
                    Mensaje = $"Libro con ISBN {Isbn} registrado exitosamente.";
                }
                else
                {
                    Mensaje = $"Error: El ISBN {Isbn} ya existe globalmente o la categoría '{CategoriaNombre}' no existe.";
                }
                GenerarReportesVisuales();
            }
            else
            {
                Mensaje = "Complete todos los campos obligatorios para registrar el libro.";
            }
            return Page();
        }

        // Opción: Eliminar Libro
        public IActionResult OnPostEliminarLibro()
        {

            if (IsbnAEliminar > 0 && !string.IsNullOrEmpty(CategoriaParaEliminar))
            {
                var cat = _catalogo.RaizCategorias.Buscar(CategoriaParaEliminar);
                var libroGlobal=_catalogo.TodosLosLibrosGlobal.BuscarPorIsbn(IsbnAEliminar);

                if (cat != null && libroGlobal!= null)
                {
                    
                    cat.LibrosAsociados.Eliminar(IsbnAEliminar);
                   _catalogo.TodosLosLibrosGlobal.eliminar(IsbnAEliminar);
                    Mensaje = $"Libro con ISBN {IsbnAEliminar} eliminado de la categoría '{CategoriaParaEliminar}'.";
                    GenerarReportesVisuales();
                }
                else
                {
                    Mensaje = $"No se encontró la categoría '{CategoriaParaEliminar}'.";
                }
            }
            else
            {
                Mensaje = "Ingrese un ISBN válido y la categoría para eliminar.";
            }
            return Page();
        }

        // Opción 4: Buscar libro por ISBN (Árbol Binario Global)
        public IActionResult OnPostBuscarIsbn()
        {
            LibroEncontrado = _catalogo.TodosLosLibrosGlobal.BuscarPorIsbn(IsbnBuscado);
            if (LibroEncontrado == null)
            {
                Mensaje = $"No se encontró ningún libro registrado con el ISBN {IsbnBuscado}.";
            }
            GenerarReportesVisuales();
            return Page();
        }

        // Opción 5: Mostrar Libro con menor o mayor ISBN (Árbol Binario Global)
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
            GenerarReportesVisuales();
            return Page();
        }

        // Método auxiliar para centralizar la generación de imágenes con Graphviz
        private void GenerarReportesVisuales()
        {
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(wwwRootPath)) Directory.CreateDirectory(wwwRootPath);

            // 1. Generar gráfico AVL si se seleccionó una categoría
            if (!string.IsNullOrEmpty(CategoriaSeleccionada))
            {
                var cat = _catalogo.RaizCategorias.Buscar(CategoriaSeleccionada);
                if (cat != null)
                {
                    string dotContent = cat.LibrosAsociados.GenerarDot(cat.Nombre);
                    string dotPath = Path.Combine(wwwRootPath, "avl_temp.dot");
                    string imgPath = Path.Combine(wwwRootPath, "avl_temp.png");

                    System.IO.File.WriteAllText(dotPath, dotContent);
                    CompilarGraphviz(dotPath, imgPath);
                    RutaImagenAvl = "/avl_temp.png";

                    LibrosOrdenadosHtml = cat.LibrosAsociados.ObtenerLibrosOrdenadosInOrder();
                }
            }

            // 2. Generar gráfico general de la jerarquía de categorías
            if (_catalogo.RaizCategorias.Cabeza != null)
            {
                string dotCatContent = _catalogo.RaizCategorias.GenerarDotCategorias();
                string dotCatPath = Path.Combine(wwwRootPath, "cat_temp.dot");
                string imgCatPath = Path.Combine(wwwRootPath, "cat_temp.png");

                System.IO.File.WriteAllText(dotCatPath, dotCatContent);
                CompilarGraphviz(dotCatPath, imgCatPath);
                RutaImagenCategorias = "/cat_temp.png";
            }
        }
    }
}