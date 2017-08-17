using System;
using System.ServiceProcess;

namespace ETLService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {

                // se estiver debugando, simula a execução do serviço...  
                if (System.Diagnostics.Debugger.IsAttached)
                {
#if DEBUG   // debugando como DEBUG
                    Sync service = new Sync();
                    service.StartDebug(new string[2]);
                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else   // debugando como Release
                                                    ServiceBase[] ServicesToRun;
                                                    ServicesToRun = new ServiceBase[] { new Sync() };
                                                    ServiceBase.Run(ServicesToRun);
#endif
                }
                else   // codigo original  
                {

                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] { new Sync() };
                    ServiceBase.Run(ServicesToRun);
                }

            }
            catch (Exception)
            {
                throw;
            }


            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new SisproPDVSync() 
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
