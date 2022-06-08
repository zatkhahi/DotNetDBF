using DotNetDBF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyTest
{
    class Program
    {
        static void AddFields(string dbf_path, DBFField[] newFields)
        {
            List<object[]> recoreds = null;
            var fields = new List<DBFField>();
            using (
            Stream fis =
                File.Open(dbf_path, FileMode.OpenOrCreate,
                          FileAccess.ReadWrite))
            {
                var reader = new DBFReader(fis);
                reader.CharEncoding = System.Text.Encoding.GetEncoding("ISO-8859-6");
                fields.AddRange(reader.Fields);
                recoreds = new List<object[]>(reader.RecordCount);

                for (int i = 0; i < reader.RecordCount; i++)
                {
                    var row = reader.NextRecord();
                    recoreds.Add(row);
                }
            }
            using ( Stream fis = File.Open(dbf_path + ".tmp",FileMode.Create, FileAccess.ReadWrite))
            {
                var writer = new DBFWriter();

                List<DBFField> addeFields = new List<DBFField>();
                foreach (var newfield in newFields)
                    if (!fields.Any(f=> f.Name.ToUpper() == newfield.Name.ToUpper()))
                        addeFields.Add(newfield);
                fields.AddRange(addeFields);
                writer.Fields = fields.ToArray();

                for (int i = 0; i < recoreds.Count; i++)
                {
                    var row = recoreds[i];
                    var newrow = row.ToList();
                    foreach (var added_field in addeFields)
                        newrow.Add(null);
                    writer.AddRecord(newrow.ToArray());
                }
                writer.Write(fis);
            }

            //File.Copy(dbf_path + ".tmp", dbf_path, true);
            //File.Delete(dbf_path + ".tmp");

        }
        static void Main(string[] args)
        {
            string dbf_path = @"E:\Projects\GPNET\trunk\ThirdParty\dotnetdbf\pipes.dbf";
            var f1 = new DBFField("D2", NativeDbType.Numeric, 19, 11);
            var f2 = new DBFField("S2", NativeDbType.Char, 50);
            AddFields(dbf_path, new[] { f1, f2 });            


            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        static void CreateDBF()
        {

        }
    }
}
