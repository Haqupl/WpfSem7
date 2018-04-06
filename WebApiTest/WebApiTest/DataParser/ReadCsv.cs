using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiTest.DataParser
{
    public class ReadCsv
    {
        public void ReadExcel1()
        {
            var fileName = string.Format("{0}\\DYPLOMY_WSIZ_1996_2015.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; data source={0}; Extended Properties=Excel 12.0 Xml;HDR=YES", fileName);
    
            var adapter = new OleDbDataAdapter("SELECT * FROM [DYPLOMY]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "anyNameHere");

            DataTable data = ds.Tables["anyNameHere"];
        }

        public void ReadExcel()
        {
            var fileName = string.Format("{0}\\DYPLOMY_WSIZ_1996_2015.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            using (var reader = new StreamReader(fileName))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listA.Add(values[0]);
                    listB.Add(values[1]);
                }
            }
        }
    }
}