using ETLService.App.Database;
using System.Collections.Generic;

namespace ETLService.App
{
    public class ETLApp
    {
        public ETLApp()
        {
            this.instance();
        }

        private void instance()
        {
            new DatabaseManagement().Verify();
        }

        public void MomentT1()
        {
            new PeopleTable().LoadPeople();
            new ExportExcel().ExportPeople();
        }

        public void MomentT2()
        {
            List<string> lines = ResourceFile.LoadFile();

            new GuestTable().InsertBulk(lines);
        }
    }
}
