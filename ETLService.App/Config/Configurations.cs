using System;
using System.Configuration;

namespace ETLService.App.Config
{
    public class Configurations
    {
        private Configuration Config = null;
        
        public Configurations()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            this.Config = ConfigurationManager.OpenExeConfiguration(System.IO.Path.Combine(dir, "ETLService.exe"));
        }

        public void SaveEllapse(int time)
        {
            if (this.Config.AppSettings.Settings["Ellapse"] != null)
            {
                this.Config.AppSettings.Settings["Ellapse"].Value = time.ToString();
            }
            else
            {
                this.Config.AppSettings.Settings.Add("Ellapse", time.ToString());
            }

            this.Config.Save();
        }

        public int GetEllapse()
        {
            int value = 10000;
            if (this.Config.AppSettings.Settings["Ellapse"] != null)
            {
                int.TryParse(this.Config.AppSettings.Settings["Ellapse"].Value, out value);
            }
            return value;
        }
    }
}
