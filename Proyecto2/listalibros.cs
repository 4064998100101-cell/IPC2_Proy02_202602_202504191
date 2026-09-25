namespace Proyecto2
{
    public class NodoLibro
    {
        public Libro Valor { get; set; }
        public NodoLibro Siguiente { get; set; }

        public NodoLibro(Libro libro)
        {
            Valor = libro;
            Siguiente = null;
        }
    }

    public class ListaLibros
    {
        public NodoLibro Cabeza { get; private set; }
        public int Tamanio { get; private set; }

        public ListaLibros()
        {
            Cabeza = null;
            Tamanio = 0;
        }

        public void Insertar(Libro nuevoLibro)
        {
            NodoLibro nuevoNodo = new NodoLibro(nuevoLibro);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
            }
            else
            {
                NodoLibro actual = Cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            Tamanio++;
        }

        public Libro BuscarPorIsbn(long isbn)
        {
            NodoLibro actual = Cabeza;
            while (actual != null)
            {
                if (actual.Valor.Isbn == isbn)
                {
                    return actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return null;
        }

        public Libro ObtenerMenorIsbn()
        {
            if (Cabeza == null) return null;
            NodoLibro actual = Cabeza;
            Libro menor = actual.Valor;
            while (actual != null)
            {
                if (actual.Valor.Isbn < menor.Isbn)
                {
                    menor = actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return menor;
        }

        public Libro ObtenerMayorIsbn()
        {
            if (Cabeza == null) return null;
            NodoLibro actual = Cabeza;
            Libro mayor = actual.Valor;
            while (actual != null)
            {
                if (actual.Valor.Isbn > mayor.Isbn)
                {
                    mayor = actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return mayor;
        }

        public void eliminar(long isbn)
        {
            if (Cabeza == null) return;

            if (Cabeza.Valor.Isbn == isbn)
            {
                Cabeza = Cabeza.Siguiente;
                Tamanio--;
                return;
            }

            NodoLibro actual = Cabeza;
            while (actual.Siguiente != null)
            {
                if (actual.Siguiente.Valor.Isbn == isbn)
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    Tamanio--;
                    return;
                }
                actual = actual.Siguiente;
            }
        }


        public string GenerarDot(string nombreCategoria)
        {
            var dot = new System.Text.StringBuilder();
            dot.AppendLine("digraph G {");
            dot.AppendLine("node [shape=box, style=filled, fillcolor=lightblue];");
            dot.AppendLine($"label=\"Libros de la Categoría: {nombreCategoria}\";");
            
            NodoLibro actual = Cabeza;
            int id = 0;
            string nodoAnterior = null;

            while (actual != null)
            {
                string nombreNodo = $"nodo_{id}";
                dot.AppendLine($"{nombreNodo} [label=\"ISBN: {actual.Valor.Isbn}\\nTítulo: {actual.Valor.Titulo}\\nAutor: {actual.Valor.Autor}\"];");
                
                if (nodoAnterior != null)
                {
                    dot.AppendLine($"{nodoAnterior} -> {nombreNodo};");
                }
                
                nodoAnterior = nombreNodo;
                actual = actual.Siguiente;
                id++;
            }

            dot.AppendLine("}");
            return dot.ToString();
        }

        public string ObtenerLibrosOrdenadosInOrder()
        {
            if (Cabeza == null) return "<p class='text-muted'>No hay libros asociados a esta categoría.</p>";

            var html = new System.Text.StringBuilder();
            html.AppendLine("<ul class='list-group'>");

            NodoLibro actual = Cabeza;
            while (actual != null)
            {
                html.AppendLine($"<li class='list-group-item'><strong>ISBN:</strong> {actual.Valor.Isbn} | <strong>Título:</strong> {actual.Valor.Titulo} | <strong>Autor:</strong> {actual.Valor.Autor}</li>");
                actual = actual.Siguiente;
            }

            html.AppendLine("</ul>");
            return html.ToString();
        }
    }
}