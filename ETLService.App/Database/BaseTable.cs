using System;
using System.Data;

namespace ETLService.App.Database
{
    public class BaseTable:IDisposable
    {
        protected IDbConnection connection = null;

        private DatabaseType dbType;

        public BaseTable(DatabaseType dbType)
        {
            this.dbType = dbType;
        }

        protected virtual void Connect()
        {
            if(this.connection == null)
                this.connection = new DatabaseManagement().CreateConnection(this.dbType);
            if (this.connection.State == ConnectionState.Closed)
                this.connection.Open();
        }

        public void Dispose()
        {
            if (this.connection.State != ConnectionState.Closed)
                this.connection.Close();
        }

        protected IDbDataAdapter GetAdapter()
        {
            return new System.Data.SQLite.SQLiteDataAdapter();
        }
    }
}
