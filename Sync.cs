using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace ETLService
{
    public partial class Sync : ServiceBase
    {
        internal Thread SyncThread;
        Timer mainTimer;

        public Sync()
        {
            InitializeComponent();
            //this.AutoLog = false;
            if (!EventLog.SourceExists("ETLService"))
            {
                EventLog.CreateEventSource(
                    "ETLService", "ServiceLog");
            }
            eventLog.Source = "ETLService";
            eventLog.Log = "ServiceLog";
            
            
        }
#if DEBUG
        public void StartDebug(string[] args)
        {
            OnStart(args);
        }
#endif

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Serviço Iniciado", EventLogEntryType.Information);
            mainTimer = new Timer(new TimerCallback(mainTimer_Tick), null, 5000, new ETLService.App.Config.Configurations().GetEllapse());
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Serviço Parado", EventLogEntryType.Information);
        }

        private void mainTimer_Tick(object sender)
        {
            try
            {
                if (SyncThread != null && SyncThread.IsAlive)
                    eventLog.WriteEntry("Aguardando a Thread finalizar sua execução...", EventLogEntryType.Warning);
                else
                {
                    SyncThread = new Thread(TrySync);
                    SyncThread.Start();
                }
            }
            catch (Exception e)
            {
                eventLog.WriteEntry("Erro ao instanciar a camada de dados do serviço:\n"+e.Message+"\n"+e.InnerException+"\n"+e.StackTrace, 
                    EventLogEntryType.Error);
            }
        }

        protected override void OnContinue()
        {
            eventLog.WriteEntry("Serviço Retomado", EventLogEntryType.Information);
        }

        private void TrySync()
        {
            new ETLService.App.ETLApp().MomentT1();
        }
    }
}
