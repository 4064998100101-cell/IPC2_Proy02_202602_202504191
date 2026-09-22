using System;
using System.Text;
namespace Proyecto2
{
    public class Nodoarbol
    {
        public Libro Valor {get; private set;}
        public Nodoarbol izquierda {get; set;}
        public Nodoarbol derecha {get; set;}
        public int Altura {get; set;}
        public Nodoarbol(Libro libro)
        {
            Valor=libro;
            izquierda=null;
            derecha=null;
            Altura=1;
        }
    }
    public class ArbolAvlLibros
    {
        public Nodoarbol Raiz{ get; private set;}
        private int obteneraltura(Nodoarbol nodo)
        {
            return nodo==null ? 0: nodo.Altura;
        }
        private int obtenerbalance(Nodoarbol nodo)
        {
            return nodo == null ? 0 : obteneraltura(nodo.izquierda) - obteneraltura(nodo.derecha);
        }
        
        private int Max(int a, int b)
        {
            return a>b? a:b;
        }
        private Nodoarbol RotarDerecha(Nodoarbol y)
        {
            Nodoarbol x=y.izquierda;
            Nodoarbol T2=x.derecha;
            x.derecha=y;
            y.izquierda=T2;
            y.Altura=Max(obteneraltura(y.izquierda), obteneraltura(y.derecha));
            x.Altura=Max(obteneraltura(x.izquierda), obteneraltura(x.derecha));
            return x;
        }
        private Nodoarbol RotarIzquierda(Nodoarbol x)
        {
            Nodoarbol y= x.derecha;
            Nodoarbol T2=y.izquierda;
            y.izquierda=x;
            x.derecha=T2;
            x.Altura=Max(obteneraltura(x.izquierda), obteneraltura(x.derecha));
            y.Altura=Max(obteneraltura(y.izquierda),obteneraltura(y.derecha));
            return y;
        }
    
    public void Insertar(Libro libro)
        {
            Raiz=insertarRec(Raiz, libro);
        }
        private Nodoarbol insertarRec(Nodoarbol nodo, Libro libro)
        {
            if (nodo==null)
            {
                return new Nodoarbol(libro);
            }
            if (libro.Isbn<nodo.Valor.Isbn)
            {
                nodo.izquierda=insertarRec(nodo.izquierda, libro);
            }
            else if (libro.Isbn> nodo.Valor.Isbn)
            {
                nodo.derecha=insertarRec(nodo.derecha, libro);
            }
            else
            {
                return nodo;
            }
        nodo.Altura=1+ Max(obteneraltura(nodo.izquierda), obteneraltura(nodo.derecha));

        int balance =obtenerbalance(nodo);
        if (balance>1 && libro.Isbn< nodo.izquierda.Valor.Isbn)
        {
            return RotarDerecha(nodo);
        }
        if (balance<-1 && libro.Isbn>nodo.derecha.Valor.Isbn)
        {
            return RotarIzquierda(nodo);
        }
        if (balance >1 && libro.Isbn>nodo.izquierda.Valor.Isbn)
        {
            nodo.izquierda=RotarIzquierda(nodo.izquierda);
            return RotarDerecha(nodo);
        }
        if (balance <-1 && libro.Isbn<nodo.derecha.Valor.Isbn)

        {
        nodo.derecha=RotarDerecha(nodo.derecha);
        return RotarIzquierda(nodo);    
        }
        return nodo;
        }
        public Libro Buscar(int isbn)
        {
            return BuscarRec(Raiz, isbn);
        }
        private Libro BuscarRec(Nodoarbol nodo, int isbn)
        {
            if (nodo==null || nodo.Valor.Isbn == isbn)
            {
                return nodo.Valor;
            }
            if (isbn<nodo.Valor.Isbn)
            {
                return BuscarRec(nodo.izquierda, isbn);
            }
            return BuscarRec(nodo.derecha, isbn);

        }
       public string GenerarDot(string nombreCategoria)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph AVL {");
            sb.AppendLine("  node [shape=box, style=\"rounded,filled\", fillcolor=\"#e8f4f8\", fontname=\"Arial\"];");
            sb.AppendLine("  edge [fontname=\"Arial\"];");
            sb.AppendLine($"  label=\"Árbol AVL - Categoria: {nombreCategoria}\\n\";");
            sb.AppendLine("  labelloc=\"top\";");
            sb.AppendLine("  fontsize=16;");
            
            GenerarDotRec(Raiz, sb);
            
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarDotRec(Nodoarbol nodo, StringBuilder sb)
        {
            if (nodo == null) return;

            string idActual = $"isbn_{nodo.Valor.Isbn}";
            sb.AppendLine($"  {idActual} [label=\"ISBN: {nodo.Valor.Isbn}\\n{nodo.Valor.Titulo}\\nAutor: {nodo.Valor.Autor}\"];");

            if (nodo.izquierda != null)
            {
                string idIzq = $"isbn_{nodo.izquierda.Valor.Isbn}";
                sb.AppendLine($"  {idActual} -> {idIzq};");
                GenerarDotRec(nodo.izquierda, sb);
            }

            if (nodo.derecha != null)
            {
                string idDer = $"isbn_{nodo.derecha.Valor.Isbn}";
                sb.AppendLine($"  {idActual} -> {idDer};");
                GenerarDotRec(nodo.derecha, sb);
            }
        } 
    }

}