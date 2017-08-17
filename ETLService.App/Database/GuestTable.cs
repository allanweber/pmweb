using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace ETLService.App.Database
{
    public class GuestTable : BaseTable
    {
        public GuestTable() : base(DatabaseType.StagInt) { }

        public void InsertBulk(List<string> lines)
        {
            List<Guest> guests = new List<Guest>();
            string[] lineParts = null;
            foreach (string line in lines)
            {
                lineParts = line.Split("\t".ToCharArray());
                if (lineParts.Length != 4) continue;

                guests.Add
                (
                    new Guest
                    {
                        IDHospede = string.Concat(Utilities.GetRandomNumber().ToString(), Utilities.GetRandomLetter()),
                        Email = lineParts[0],
                        Nome = lineParts[1],
                        DataHosped = lineParts[2].ToDateTime(),
                        DataNasc = lineParts[3].ToDateTime()
                    }
                );
            }

            if(guests.Count > 0)
            {
                string command = @"INSERT INTO HOSPEDES(IDHOSPEDE,EMAIL,NOME,DATANASC,DATAHOSPED)
                                   VALUES(@IDHospede,@Email,@Nome,@DataNasc,@DataHosped)";

                this.Connect();
                this.connection.Execute(command, guests.ToArray());
            }
        }

        public List<Guest> GetGuests()
        {
            this.Connect();
            return this.connection.Query<Guest>("SELECT IDHOSPEDE,EMAIL,NOME,DATANASC,DATAHOSPED FROM HOSPEDES").ToList();
        }
    }

    public class Guest
    {
        public string IDHospede { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime DataNasc { get; set; }
        public DateTime DataHosped { get; set; }
    }
}
