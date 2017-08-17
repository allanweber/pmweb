using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;

namespace ETLService.App.Database
{
    public class PeopleTable : BaseTable
    {
        public PeopleTable() : base(DatabaseType.Cli) { }

        public void LoadPeople()
        {
            List<Guest> guests = new GuestTable().GetGuests();
            List<Person> peopleToAdd = new List<Person>();
            List<Person> peopleToUpdate = new List<Person>();
            Person currentperson = null;
            foreach (Guest guest in guests)
            {
                currentperson = this.GetPerson(guest.IDHospede);
                if (currentperson == null)
                    peopleToAdd.Add(
                        new Person
                        {
                            Nome = guest.Nome,
                            Email = guest.Email,
                            DataNasc = guest.DataNasc,
                            IdExterno = guest.IDHospede,
                            UltimaHosp = guest.DataHosped,
                            DataCadastro = DateTime.Now,
                        });
                else
                {
                    currentperson.Nome = guest.Nome;
                    currentperson.Email = guest.Email;
                    currentperson.DataAtualizacao = DateTime.Now;
                    currentperson.QtdeHospedag++;
                    currentperson.DataNasc = guest.DataNasc;
                    currentperson.UltimaHosp = guest.DataHosped > currentperson.UltimaHosp ? currentperson.UltimaHosp : currentperson.UltimaHosp;
                    peopleToUpdate.Add(currentperson);
                }
            }

            this.addPerson(peopleToAdd);
            this.updatePerson(peopleToUpdate);
        }

        private void addPerson(List<Person> peopleToAdd)
        {
            if (peopleToAdd.Count > 0)
            {
                this.Connect();
                this.connection.Execute(@"INSERT INTO PESSOAS(NOME,EMAIL,IDEXTERNO,DATACADASTRO,DATANASC,ULTIMAHOSP)
                                        VALUES(@Nome,@Email,@IdExterno,@DataCadastro,@DataNasc,@UltimaHosp)", peopleToAdd.ToArray());
            }
        }

        private void updatePerson(List<Person> peopleToUpdate)
        {
            if (peopleToUpdate.Count > 0)
            {
                this.Connect();
                this.connection.Execute(@"UPDATE PESSOAS SET 
                                        NOME = @Nome,EMAIL = @Email,DATANASC = @DataNasc,ULTIMAHOSP = @UltimaHosp
                                        ,QTDEHOSPEDAG = @QtdeHospedag,DATAATUALIZACAO = @DataAtualizacao
                                        WHERE ID = @ID"
                                        , peopleToUpdate.ToArray());
            }
        }

        public Person GetPerson(string idHospede)
        {
            this.Connect();
            return this.connection.Query<Person>
                (
                   @"SELECT ID,NOME,EMAIL,IDEXTERNO,DATACADASTRO,DATANASC,ULTIMAHOSP,QTDEHOSPEDAG,DATAATUALIZACAO FROM PESSOAS
                    WHERE IDEXTERNO = @IDEXTERNO",
                   new { IDEXTERNO = idHospede }
                ).FirstOrDefault();
        }

        public DataTable GetAllDT()
        {
            this.Connect();
            IDbCommand command = this.connection.CreateCommand();
            command.CommandText = "SELECT ID,NOME,EMAIL,IDEXTERNO,DATACADASTRO,DATANASC,ULTIMAHOSP,QTDEHOSPEDAG,DATAATUALIZACAO FROM PESSOAS";
            IDbDataAdapter adapter = this.GetAdapter();
            adapter.SelectCommand = command;
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            return dataset.Tables[0];
        }
    }

    public class Person
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string IdExterno { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataNasc { get; set; }
        public DateTime UltimaHosp { get; set; }
        public int QtdeHospedag { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
