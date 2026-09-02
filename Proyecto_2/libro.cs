using System.Diagnostics.Contracts;

namespace Proyecto_2
{
    public class libro
    {
        public bool Estado {get; private set;}
        public string Codigounico{ get; private set;}
        public string Categoria {get; private set;}

        public  libro(string codigounico, string categoria, bool estado=true)
        {
           Estado=estado;
           Codigounico=codigounico;
           Categoria=categoria; 
        }

        private string IBN()
        {
            return "yo gane";
        }

    }
}