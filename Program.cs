// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using SuperConvert.Extentions;
using System.Data;

namespace FileSytemPrac
{
    class Program
    {
        static void Main(string[] args)
        {
            ////path to root 
            //string rootPath = @"C:\Users\hkgna\Downloads\exam data\data";

            ////Search all directories for .csv files and print each one to console
            //var files = Directory.GetFiles(rootPath, "*.csv", SearchOption.AllDirectories);

            //foreach (string file in files)
            //{
            //    //Console.WriteLine(file name, last write time, and size);
            //    var info = new FileInfo(file);
            //    Console.WriteLine($"{Path.GetFileName(file)}: { info.Length}, {info.LastWriteTimeUtc} bytes");
            //} 
            //Console.ReadLine();

            var firstTable = ReadCSVFile(@"C:\Users\hkgna\Downloads\exam data\data\source 1.csv");
            //var secondTable = ReadCSVFile(@"C:\Users\hkgna\Downloads\exam data\data\source 2.csv");
            //var thirdTable = ReadCSVFile(@"C:\Users\hkgna\Downloads\exam data\data\source 3.csv");
            //var fourthTable = ReadCSVFile(@"C:\Users\hkgna\Downloads\exam data\data\source 4.csv");

            //Console.WriteLine(firstTable);
            //Console.WriteLine(secondTable);
            //Console.WriteLine(thirdTable);
            //Console.WriteLine(fourthTable);
        }

   
        private static string ReadCSVFile(string csv_file_path)
        {
            //Establishing new instance of DataTable and others variables needed later
            DataTable csvData = new DataTable();
            string jsonString = string.Empty;
            string datetime = DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss");
            
            //Parse through given file, assign columns, assign other data to rows until null
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields;
                    bool tableCreated = false;
                    while (tableCreated == false)
                    {
                        colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                            //Handles null or nonexistant essential columns
                            //if (datecolumn.ColumnName != "Total Interest")
                            //{
                            //    Console.WriteLine("Sorry the Total Interest was not found! To process, the Total Interest and Loan Amount columns must be present");
                            //    break;
                            //}
                            //else if (datecolumn.ColumnName != "Loan Amount")
                            //{
                            //    Console.WriteLine("Sorry the Loan Amount was not found! To process, the Total Interest and Loan Amount columns must be present");
                            //    break;
                            //} else { 
                            
                            //    csvData.Columns.Add("Total Expenses");
                            //}

                        }
                        tableCreated = true;
                    }
                    while (!csvReader.EndOfData)
                    {
                        //csvData.Rows.Add(csvReader.ReadLine());
                        string[] fieldData = csvReader.ReadFields();

                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error: Parsing CSV";
            }
            //Convert to Json for console view then convert to Csv for Excel or Notepad view
            jsonString = JsonConvert.SerializeObject(csvData);
            string csvPath = jsonString.ToCsv(@"C:\Users\hkgna\Downloads\exam data\data", Path.GetFileNameWithoutExtension(csv_file_path) + $" {datetime}");
            return jsonString;

        }
    }
}

