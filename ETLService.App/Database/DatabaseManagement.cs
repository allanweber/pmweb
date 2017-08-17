using System.Data;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace ETLService.App.Database
{
    public enum DatabaseType
    {
        StagInt,
        Cli
    }

    public class DatabaseManagement
    {
        public const string STAG_INT = "STAG_INT";
        public const string CLI = "CLI";
        public const string DBEXENSION = ".sqlite";

        public string StagIntName { get { return string.Format("{0}{1}", STAG_INT, DBEXENSION); } }

        public string CliName { get { return string.Format("{0}{1}", CLI, DBEXENSION); } }

        public DatabaseManagement() { }

        public void Verify()
        {
            if (!File.Exists(this.StagIntName))
            {
                SQLiteConnection.CreateFile(this.StagIntName);
                this.createStagIntTables();
            }
            else
                this.tablesStagIntExists();

            if (!File.Exists(this.CliName))
            {
                SQLiteConnection.CreateFile(this.CliName);
                this.createCliTables();
            }
            else
                this.tablesCliExists();

        }

        private void tablesStagIntExists()
        {
            IDbConnection conn = this.CreateConnection(DatabaseType.StagInt);

            if (conn.QueryFirstOrDefault<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='HOSPEDES';") == 0)
                this.createStagIntTables();
        }

        private void tablesCliExists()
        {
            IDbConnection conn = this.CreateConnection(DatabaseType.Cli);

            if (conn.QueryFirstOrDefault<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='PESSOAS';") == 0)
                this.createCliTables();
        }

        private void createStagIntTables()
        {
            IDbConnection conn = this.CreateConnection(DatabaseType.StagInt);

            string commad = @"CREATE TABLE HOSPEDES(
                               IDHOSPEDE VARCHAR(10) PRIMARY KEY,
                               EMAIL VARCHAR(200) NOT NULL,
                               NOME VARCHAR(200) NOT NULL,
                               DATANASC DATE,
                               DATAHOSPED DATE NOT NULL
                            );";

            conn.Execute(commad);
            conn.Close();
        }

        private void createCliTables()
        {
            IDbConnection conn = this.CreateConnection(DatabaseType.Cli);

            string commad = @"CREATE TABLE PESSOAS(
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                NOME VARCHAR(200) NOT NULL,
                                EMAIL VARCHAR(200) NOT NULL,
                                IDEXTERNO VARCHAR(10) NOT NULL,
                                DATACADASTRO DATE NOT NULL,
                                DATANASC DATE,
                                ULTIMAHOSP DATE,
                                QTDEHOSPEDAG INT DEFAULT 0 NOT NULL,
                                DATAATUALIZACAO DATE
                            );";

            conn.Execute(commad);
            conn.Close();
        }

        public IDbConnection CreateConnection(DatabaseType dbType)
        {
            SQLiteConnection conn = null;

            if(dbType == DatabaseType.StagInt)
                conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;",this.StagIntName));
            else
                conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", this.CliName));

            conn.Open();
            return conn;
        }
    }
}
