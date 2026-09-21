
namespace Proyecto2.servicios
{
 public class Catalogo

    {
        
        public Listacategorias RaizCategorias {get; private set;}
        public ListaLibros Todos{get; private set;}

        public Catalogo()
        {
            RaizCategorias=new Listacategorias();
            Todos =new ListaLibros();
        }
        public NodoCategoria obtenerocrear(string nombreCategoria, string nombrePadre)
        {
            nombreCategoria=nombreCategoria.Trim();
            NodoCategoria encontrada= RaizCategorias.Buscar(nombreCategoria);
            if (encontrada!= null)
            {
                return encontrada;
            }
            NodoCategoria nuevaCat=new NodoCategoria(nombreCategoria);
            if (string.IsNullOrEmpty(nombrePadre))
            {
                RaizCategorias.Insertar(nuevaCat);
            }
            else
            {
                nombrePadre=nombrePadre.Trim();
                NodoCategoria padre= RaizCategorias.Buscar(nombrePadre);
                if (padre!= null)
                {
                    padre.SubCategorias.Insertar(nuevaCat);

                }
                else
                {
                    NodoCategoria nuevoPadre =new NodoCategoria(nombrePadre);
                    nuevoPadre.SubCategorias.Insertar(nuevaCat);
                    RaizCategorias.Insertar(nuevoPadre);
                }
            }
            return nuevaCat;
        }
        public void RegistrarLibro(int isbn, string titulo, string autor,string nombreCategoria)
        {
            Libro nuevoLibro =new Libro(isbn, titulo, autor,nombreCategoria);
            Todos.Insertar(nuevoLibro);
            NodoCategoria cat=RaizCategorias.Buscar(nombreCategoria);
            if (cat!= null)
            {
                cat.LibrosAsociados.Insertar(nuevoLibro);

            }
            else
            {
                NodoCategoria nuevaCat = obtenerocrear(nombreCategoria, null);
                nuevaCat.LibrosAsociados.Insertar(nuevoLibro);
                
            
            }
        }

    }   
}