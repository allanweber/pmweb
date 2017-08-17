using ClosedXML.Excel;
using ETLService.App.Database;
using System;
using System.Data;

namespace ETLService.App
{
    public class ExportExcel
    {
        public void ExportPeople()
        {
            DataTable people = new PeopleTable().GetAllDT();

            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet("People");
            IXLWorksheet ws = workbook.Worksheet("People");

            for (int i = 0; i < people.Columns.Count; i++)
            {
                ws.Cell(1, i + 1).Value = people.Columns[i].ToString().ToUpper();
            }

            for (int row = 0; row < (people.Rows.Count); row++)
            {
                for (int col = 0; col < people.Columns.Count; col++)
                {
                    if (people.Rows[row][col].GetType() == typeof(DateTime))
                        ws.Cell(row + 2, col + 1).Value = people.Rows[row][col].ToString().ToDateTime().ToString("dd/MM/yyyy");
                    else
                        ws.Cell(row + 2, col + 1).Value = Convert.ToString(people.Rows[row][col]);
                }
            }

            workbook.SaveAs("People.xlsx");
        }
    }


}
