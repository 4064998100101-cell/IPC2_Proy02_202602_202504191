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
        
    }
}