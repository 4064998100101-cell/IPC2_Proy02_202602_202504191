namespace Proyecto2
{
    public class Catalogo
    {
        public Listacategorias RaizCategorias { get; private set; }
        public ListaLibros TodosLosLibrosGlobal { get; private set; }

        public Catalogo()
        {
            RaizCategorias = new Listacategorias();
            TodosLosLibrosGlobal = new ListaLibros();
        }

        public NodoCategoria ObtenerOCrearCategoria(string nombreCategoria, string nombrePadre)
        {
            if (string.IsNullOrWhiteSpace(nombreCategoria)) return null;
            
            nombreCategoria = nombreCategoria.Trim();
            NodoCategoria encontrada = RaizCategorias.Buscar(nombreCategoria);
            if (encontrada != null) return encontrada;

            NodoCategoria nuevaCat = new NodoCategoria(nombreCategoria);

            if (string.IsNullOrEmpty(nombrePadre))
            {
                RaizCategorias.Insertar(nuevaCat);
            }
            else
            {
                nombrePadre = nombrePadre.Trim();
                NodoCategoria padre = RaizCategorias.Buscar(nombrePadre);
                if (padre != null)
                {
                    padre.SubCategorias.Insertar(nuevaCat);
                }
                else
                {
                    NodoCategoria nuevoPadre = new NodoCategoria(nombrePadre);
                    nuevoPadre.SubCategorias.Insertar(nuevaCat);
                    RaizCategorias.Insertar(nuevoPadre);
                }
            }

            return nuevaCat;
        }

        public void RegistrarLibro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(nombreCategoria)) return;

            Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);
            
            // Insertar en la lista global
            TodosLosLibrosGlobal.Insertar(nuevoLibro);

            // Buscar o crear la categoría y asociar el libro
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
    }
}