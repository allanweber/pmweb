using ETLService.App;
using System;

namespace ETLService.LoadResource
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Inciando a leitura do resource 20170519_Clientes.txt ...");

                new ETLApp().MomentT2();

                Console.WriteLine("Leitura do resource 20170519_Clientes.txt concluída...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
