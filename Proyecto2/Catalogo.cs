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
            if (encontrada != null) {return encontrada;}
            
           
            if(!string.IsNullOrEmpty(nombrePadre))
            {
                nombrePadre=nombrePadre.Trim();
                NodoCategoria padre =RaizCategorias.Buscar(nombrePadre);

                if(padre== null)
                {
                    return null;
                }
                 NodoCategoria nuevaCat = new NodoCategoria(nombreCategoria);
                 padre.SubCategorias.Insertar(nuevaCat);
                 return nuevaCat;

            }
            else
            {
                NodoCategoria nuevaCat=new NodoCategoria(nombreCategoria);
                RaizCategorias.Insertar(nuevaCat);
                return nuevaCat;
            }


        }

        public bool RegistrarLibro(long isbn, string titulo, string autor, string nombreCategoria)
        {
            
            if (string.IsNullOrWhiteSpace(nombreCategoria)) return false;

            if(TodosLosLibrosGlobal.BuscarPorIsbn(isbn)!=null)
            {
                return false;
            }

            NodoCategoria cat=RaizCategorias.Buscar(nombreCategoria);
            if(cat==null)
            {
                return false;
            }
            Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);
            
         
            TodosLosLibrosGlobal.Insertar(nuevoLibro);
            cat.LibrosAsociados.Insertar(nuevoLibro);
            return true;

    
           
        }
    }
}