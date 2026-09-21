namespace Proyecto2
{
    public class Nodoarbol
    {
        public libro Valor {get; private set;}
        public Nodoarbol izquierda {get; set;}
        public Nodoarbol derecha {get; set;}
        public int Altura {get; set;}
        public Nodoarbol(libro libro)
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
            return nodo=null ? 0: obteneraltura(nodo.izquierda)=obteneraltura(nodo.derecha);
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
    }
}