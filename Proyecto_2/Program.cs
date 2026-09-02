using System;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto_2
{
  class Program
    {
        static void Main(string[] args)
        {
            // vamos a crear el menu de inicio para empezar
            bool repetir=true;
            int opcion=100;
            while (repetir)
            {
            Console.WriteLine("+===================================+");
            Console.WriteLine("|        Programa de libreria       |");
            Console.WriteLine("+===================================+");
            Console.WriteLine("1. Gestion de libros");
            Console.WriteLine("2. Lector Xml");
            Console.WriteLine("0. Salir");

            Console.WriteLine("Ingrese una opcion");
            try
            {
            opcion=int.Parse(Console.ReadLine());
            

                
            }
            catch (System.Exception)
            {
                Console.WriteLine("Dato no reconocido");

            }
                        switch (opcion)
            {
                case 1: break;
                case 2: break;
                case 0: Console.WriteLine("Gracias por usar el programa de la libreria de datos"); repetir=false; break;
                
                default: Console.WriteLine("Opcion no validad intentelo de nuevo....."); break;
            }

            }


        



        }
        
    }
    

}