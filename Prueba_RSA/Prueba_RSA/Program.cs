using System;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace Prueba_RSA
{
    class Program
    {
        private static string contenido;
        static void Main(string[] args)
        {
            string carpet = @"C:\Users\luisp\Desktop\Landivar\Cuarto año\Octavo Ciclo\Estructuras 2\Lab\InPutConv\inputs2";
            Console.WriteLine("Ingrese el DPI para buscar la converzación");
            string  identificador = Console.ReadLine();
            try
            {
                string[] archivos = Directory.GetFiles(carpet, $"*{identificador}*.txt");
                if (archivos.Length > 0)
                {
                    Console.WriteLine("Archivos encontrados con nombres similares:");
                    foreach ( string archivo in archivos)
                    {
                        Console.WriteLine(archivo); //Mostrar en consola el direccion y nombre del archivo
                        string nomArchCDesi = "Descifrado" + $"{Path.GetFileNameWithoutExtension(archivo)}_{identificador}.txt"; //Nombre que va a tener el nuevo archivo sin cifrar
                        string rutaArchivoDesi = Path.Combine(@"C:\Users\luisp\Desktop\Landivar\Cuarto año\Octavo Ciclo\Estructuras 2\Lab\InPutConv\", nomArchCDesi); //Ruta y el nombre
                        Console.WriteLine(" ");
                        EscribiRArchi(archivo, rutaArchivoDesi); //Metodo para guardar (contenido, ruta del arhivo)  
                    }  
                }
                else
                {
                    Console.WriteLine("No se encontraron archivos con nombres similares.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
            BigInteger maximo = BigInteger.Pow(9, 4); // Valor máximo del rango ( ,num de ceros )
            BigInteger numeroPrimoAleatorio1 = Generar_Llaves.GenerarNumero(maximo); //P
            BigInteger numeroPrimoAleatorio2 = Generar_Llaves.GenerarNumero(maximo); //Q
            BigInteger ResMulti = Generar_Llaves.MultiNumeros(numeroPrimoAleatorio1, numeroPrimoAleatorio2); // Esta es mi variable N
            BigInteger ResMulti2Z = Generar_Llaves.GenerarZ(numeroPrimoAleatorio1, numeroPrimoAleatorio2);
            BigInteger Coprimo = Generar_Llaves.EncontrarCoprimo(ResMulti2Z); //K
            BigInteger Key_Privada = Generar_Llaves.ClavePrivada(Coprimo, ResMulti2Z); //J
            BigInteger M = Generar_Llaves.TxtAInteger(contenido);
            BigInteger C = Generar_Llaves.Cifrado(M, Coprimo, ResMulti); //Metodo para aplicar RSA
            /*Console.WriteLine("Número primo aleatorio menor a  " + maximo + " : " + numeroPrimoAleatorio1);
            Console.WriteLine("Número primo 2 aleatorio menor a  " + maximo + " : " + numeroPrimoAleatorio2);
            Console.WriteLine("Resultado multiplicacion N : " + ResMulti);
            Console.WriteLine("Resultado Z : " + ResMulti2Z);
            Console.WriteLine("Coprimo K : " + Coprimo);
            Console.WriteLine("Llave privada : " + Key_Privada);
            Console.WriteLine(" " );*/
            //Console.WriteLine("valor de M" + M); 
            Console.WriteLine("Mensaje cifrado: " + C);
            Console.WriteLine(" ");
            string opcion; 
            Console.WriteLine(" ¿Desea validar que la conversacion no a sido modificada?  ");
            opcion = Console.ReadLine(); 
            if (opcion == "Si" || opcion == "si" || opcion == "SI")
            {
                string arrego  = Generar_Llaves.BigIntegerToText(M);
                string[] archivos2 = Directory.GetFiles(carpet, $"*{identificador}*.txt");
                if (archivos2.Length > 0)
                {
                    foreach (string archivo in archivos2)
                    {
                        string nomArchCDesi = "Descifrado" + $"{Path.GetFileNameWithoutExtension(archivo)}_{identificador}.txt"; //Nombre que va a tener el nuevo archivo sin cifrar
                        string rutaArchivoDesi = Path.Combine(@"C:\Users\luisp\Desktop\Landivar\Cuarto año\Octavo Ciclo\Estructuras 2\Lab\InPutConv\", nomArchCDesi); //Ruta y el nombre
                        Console.WriteLine(" ");
                        EscribiRArchi2(archivo, rutaArchivoDesi); //Metodo para guardar (contenido, ruta del arhivo)  
                        Console.WriteLine(" ");
                        string hash2 = CrearHash(arrego);
                        string hash1 = CrearHash(contenido);
                        if (hash2 == hash1)
                        {
                            Console.WriteLine("La conversacion no se ha modificado ");
                        }
                        else
                        {
                            Console.WriteLine("La conversacion fue comprometida");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron archivos con nombres similares.");
                }
            }
            else
            {
                Console.WriteLine("Fin de la verificación"); 
            }  
        }
        static void EscribiRArchi(string rutaOrigen, string rutaDestino)
        {
            try
            {
                contenido = File.ReadAllText(rutaOrigen);
                CrearHash(contenido); //Crear el hash  del contenido del archivo 
                File.WriteAllText(rutaDestino,contenido);
            }
            catch (Exception)
            {
                Console.WriteLine("Error al escribir en el archivo");
            }
        }
        static string CrearHash (string mensaje)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(mensaje);
                byte[] hashBytes = sHA256.ComputeHash(inputBytes);
                // Convierte los bytes del hash en una cadena hexadecimal
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                Console.WriteLine("Firma digital: " + hash);
                Console.WriteLine("");
                return hash;
            }
        }
        static void EscribiRArchi2(string rutaOrigen, string rutaDestino)
        {
            try
            {
                contenido = File.ReadAllText(rutaOrigen);
                
                File.WriteAllText(rutaDestino, contenido);
            }
            catch (Exception)
            {
                Console.WriteLine("Error al escribir en el archivo");
            }
        }
    }
}
