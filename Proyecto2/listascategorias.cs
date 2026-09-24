using System;
using System.Text;

namespace Proyecto2
{
    public class Nodocat 
    {
        public NodoCategoria Valor { get; set; }
        public Nodocat Siguiente { get; set; }

        public Nodocat(NodoCategoria categoria)
        {
            Valor = categoria;
            Siguiente = null;
        }
    }

    public class Listacategorias
    {
        public Nodocat Cabeza { get; private set; }
        public int Tamanio { get; private set; }

        public Listacategorias()
        {
            Cabeza = null;
            Tamanio = 0;
        }

        public void Insertar(NodoCategoria nuevaCategoria)
        {
            Nodocat nuevoNodo = new Nodocat(nuevaCategoria);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
            }
            else
            {
                if (string.Compare(nuevaCategoria.Nombre, Cabeza.Valor.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    nuevoNodo.Siguiente = Cabeza;
                    Cabeza = nuevoNodo;
                }
                else
                {
                    Nodocat actual = Cabeza;
                    while (actual.Siguiente != null && 
                           string.Compare(actual.Siguiente.Valor.Nombre, nuevaCategoria.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        actual = actual.Siguiente;
                    }
                    nuevoNodo.Siguiente = actual.Siguiente;
                    actual.Siguiente = nuevoNodo;
                }
            }
            Tamanio++;
        }

        public NodoCategoria Buscar(string nombre)
        {
            Nodocat actual = Cabeza;
            while (actual != null)
            {
                if (string.Equals(actual.Valor.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                {
                    return actual.Valor;
                }
                var encontradaEnHijas = actual.Valor.SubCategorias.Buscar(nombre);
                if (encontradaEnHijas != null) return encontradaEnHijas;

                actual = actual.Siguiente;
            }
            return null;
        }

        // --- MÉTODOS DE GRAPHVIZ INTEGRADOS CORRECTAMENTE ---

        public string GenerarDotCategorias()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph Categorias {");
            sb.AppendLine("   node [shape=folder, style=\"rounded,filled\", fillcolor=\"#fff3cd\", fontname=\"Arial\"];");
            sb.AppendLine("   edge [fontname=\"Arial\"];");
            sb.AppendLine("   label=\"Jerarquía de Categorías y Subcategorías\\n\";");
            sb.AppendLine("   labelloc=\"top\";");
            sb.AppendLine("   fontsize=16;");

            // Recorremos la lista de categorías principales de forma segura
            Nodocat actual = Cabeza;
            while (actual != null)
            {
                GenerarDotCategoriasRec(actual.Valor, sb);
                actual = actual.Siguiente;
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarDotCategoriasRec(NodoCategoria nodo, StringBuilder sb)
        {
            if (nodo == null) return;

            string idActual = $"cat_{nodo.Nombre.Replace(" ", "_")}";
            sb.AppendLine($"   {idActual} [label=\"{nodo.Nombre}\"];");

            // Si tiene subcategorías, las recorremos y enlazamos
            if (nodo.SubCategorias != null && nodo.SubCategorias.Cabeza != null)
            {
                Nodocat actualSub = nodo.SubCategorias.Cabeza;
                while (actualSub != null)
                {
                    string idSub = $"cat_{actualSub.Valor.Nombre.Replace(" ", "_")}";
                    sb.AppendLine($"   {idActual} -> {idSub};");
                    GenerarDotCategoriasRec(actualSub.Valor, sb);
                    actualSub = actualSub.Siguiente;
                }
            }
        }
    }
}