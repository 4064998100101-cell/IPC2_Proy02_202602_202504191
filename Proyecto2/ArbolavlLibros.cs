using System;
using System.Text;

namespace Proyecto2
{
    public class Nodoarbol
    {
        public Libro Valor { get; set; }
        public Nodoarbol izquierda { get; set; }
        public Nodoarbol derecha { get; set; }
        public int Altura { get; set; }

        public Nodoarbol(Libro libro)
        {
            Valor = libro;
            izquierda = null;
            derecha = null;
            Altura = 1;
        }
    }

    public class ArbolAvlLibros
    {
        public Nodoarbol Raiz { get; private set; }

        private int obteneraltura(Nodoarbol nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private int obtenerbalance(Nodoarbol nodo)
        {
            return nodo == null ? 0 : obteneraltura(nodo.izquierda) - obteneraltura(nodo.derecha);
        }

        private int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        private Nodoarbol RotarDerecha(Nodoarbol y)
        {
            Nodoarbol x = y.izquierda;
            Nodoarbol T2 = x.derecha;

            x.derecha = y;
            y.izquierda = T2;

            y.Altura = Max(obteneraltura(y.izquierda), obteneraltura(y.derecha)) + 1;
            x.Altura = Max(obteneraltura(x.izquierda), obteneraltura(x.derecha)) + 1;

            return x;
        }

        private Nodoarbol RotarIzquierda(Nodoarbol x)
        {
            Nodoarbol y = x.derecha;
            Nodoarbol T2 = y.izquierda;

            y.izquierda = x;
            x.derecha = T2;

            x.Altura = Max(obteneraltura(x.izquierda), obteneraltura(x.derecha)) + 1;
            y.Altura = Max(obteneraltura(y.izquierda), obteneraltura(y.derecha)) + 1;

            return y;
        }

        public void Insertar(Libro libro)
        {
            Raiz = insertarRec(Raiz, libro);
        }

        private Nodoarbol insertarRec(Nodoarbol nodo, Libro libro)
        {
            if (nodo == null)
            {
                return new Nodoarbol(libro);
            }

            if (libro.Isbn < nodo.Valor.Isbn)
            {
                nodo.izquierda = insertarRec(nodo.izquierda, libro);
            }
            else if (libro.Isbn > nodo.Valor.Isbn)
            {
                nodo.derecha = insertarRec(nodo.derecha, libro);
            }
            else
            {
                return nodo; // No se permiten duplicados
            }

            nodo.Altura = 1 + Max(obteneraltura(nodo.izquierda), obteneraltura(nodo.derecha));
            int balance = obtenerbalance(nodo);

            // Casos de desbalanceo
            if (balance > 1 && libro.Isbn < nodo.izquierda.Valor.Isbn)
                return RotarDerecha(nodo);

            if (balance < -1 && libro.Isbn > nodo.derecha.Valor.Isbn)
                return RotarIzquierda(nodo);

            if (balance > 1 && libro.Isbn > nodo.izquierda.Valor.Isbn)
            {
                nodo.izquierda = RotarIzquierda(nodo.izquierda);
                return RotarDerecha(nodo);
            }

            if (balance < -1 && libro.Isbn < nodo.derecha.Valor.Isbn)
            {
                nodo.derecha = RotarDerecha(nodo.derecha);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        public void Eliminar(int isbn)
        {
            Raiz = eliminarRec(Raiz, isbn);
        }

        private Nodoarbol eliminarRec(Nodoarbol raiz, int isbn)
        {
            if (raiz == null) return null;

            if (isbn < raiz.Valor.Isbn)
            {
                raiz.izquierda = eliminarRec(raiz.izquierda, isbn);
            }
            else if (isbn > raiz.Valor.Isbn)
            {
                raiz.derecha = eliminarRec(raiz.derecha, isbn);
            }
            else
            {
                if ((raiz.izquierda == null) || (raiz.derecha == null))
                {
                    Nodoarbol temp = raiz.izquierda ?? raiz.derecha;
                    if (temp == null)
                    {
                        raiz = null;
                    }
                    else
                    {
                        raiz = temp;
                    }
                }
                else
                {
                    Nodoarbol temp = obtenerNodoMinimo(raiz.derecha);
                    raiz.Valor = temp.Valor;
                    raiz.derecha = eliminarRec(raiz.derecha, temp.Valor.Isbn);
                }
            }

            if (raiz == null) return null;

            raiz.Altura = 1 + Max(obteneraltura(raiz.izquierda), obteneraltura(raiz.derecha));
            int balance = obtenerbalance(raiz);

            if (balance > 1 && obtenerbalance(raiz.izquierda) >= 0)
                return RotarDerecha(raiz);

            if (balance > 1 && obtenerbalance(raiz.izquierda) < 0)
            {
                raiz.izquierda = RotarIzquierda(raiz.izquierda);
                return RotarDerecha(raiz);
            }

            if (balance < -1 && obtenerbalance(raiz.derecha) <= 0)
                return RotarIzquierda(raiz);

            if (balance < -1 && obtenerbalance(raiz.derecha) > 0)
            {
                raiz.derecha = RotarDerecha(raiz.derecha);
                return RotarIzquierda(raiz);
            }

            return raiz;
        }

        private Nodoarbol obtenerNodoMinimo(Nodoarbol nodo)
        {
            Nodoarbol actual = nodo;
            while (actual.izquierda != null)
                actual = actual.izquierda;
            return actual;
        }

        public Libro Buscar(int isbn)
        {
            return BuscarRec(Raiz, isbn);
        }

        private Libro BuscarRec(Nodoarbol nodo, int isbn)
        {
            if (nodo == null) return null;
            if (nodo.Valor.Isbn == isbn) return nodo.Valor;

            if (isbn < nodo.Valor.Isbn)
            {
                return BuscarRec(nodo.izquierda, isbn);
            }
            return BuscarRec(nodo.derecha, isbn);
        }

        public string ObtenerLibrosOrdenadosInOrder()
        {
            StringBuilder sb = new StringBuilder();
            InOrderRec(Raiz, sb);
            return sb.ToString();
        }

        private void InOrderRec(Nodoarbol nodo, StringBuilder sb)
        {
            if (nodo != null)
            {
                InOrderRec(nodo.izquierda, sb);
                sb.AppendLine($"ISBN: {nodo.Valor.Isbn} - Título: {nodo.Valor.Titulo} (Autor: {nodo.Valor.Autor})<br/>");
                InOrderRec(nodo.derecha, sb);
            }
        }

        public string GenerarDot(string nombreCategoria)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph AVL {");
            sb.AppendLine("   node [shape=box, style=\"rounded,filled\", fillcolor=\"#e8f4f8\", fontname=\"Arial\"];");
            sb.AppendLine("   edge [fontname=\"Arial\"];");
            sb.AppendLine($"   label=\"Árbol AVL - Categoria: {nombreCategoria}\\n\";");
            sb.AppendLine("   labelloc=\"top\";");
            sb.AppendLine("   fontsize=16;");
            
            GenerarDotRec(Raiz, sb);
            
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarDotRec(Nodoarbol nodo, StringBuilder sb)
        {
            if (nodo == null) return;

            string idActual = $"isbn_{nodo.Valor.Isbn}";
            sb.AppendLine($"   {idActual} [label=\"ISBN: {nodo.Valor.Isbn}\\n{nodo.Valor.Titulo}\\nAutor: {nodo.Valor.Autor}\"];");

            if (nodo.izquierda != null)
            {
                string idIzq = $"isbn_{nodo.izquierda.Valor.Isbn}";
                sb.AppendLine($"   {idActual} -> {idIzq};");
                GenerarDotRec(nodo.izquierda, sb);
            }

            if (nodo.derecha != null)
            {
                string idDer = $"isbn_{nodo.derecha.Valor.Isbn}";
                sb.AppendLine($"   {idActual} -> {idDer};");
                GenerarDotRec(nodo.derecha, sb);
            }
        }
        
    }
}