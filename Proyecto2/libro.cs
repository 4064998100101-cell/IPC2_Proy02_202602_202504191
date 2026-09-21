using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto2
{
    public class libro
    {
        public string Titulo{ get; private set;}
        public bool Estado {get; private set;}
        public string Autor{get; private set;}

        // seis digitos creados al azar
        public int isbn{ get; private set;}
        public string Categoria {get; private set;}

        public  libro( string autor, string titulo, int codigounico, string categoria, bool estado=true)
        {
           Autor =autor;
           Titulo=titulo; 
           Estado=estado;
           isbn=codigounico;
           Categoria=categoria; 
        }

        private string IBN()
        {
         return "mar";


        }

    }
}