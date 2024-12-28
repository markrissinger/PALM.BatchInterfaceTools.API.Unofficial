using ExcelDataReader;
using System.Data;

namespace PALM.DeveloperTools.API.Helpers.Parsers
{
    public class ExcelParser
    {
        private const string _worksheetName = "Input";

        public DataSet ParseExcelUploadTemplate(MemoryStream stream)
        {
            using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream))
            {
                return excelReader.AsDataSet(new ExcelDataSetConfiguration() 
                { 
                    ConfigureDataTable = reader => new ExcelDataTableConfiguration 
                    { 
                        UseHeaderRow = true
                    } 
                });
            }            
        }

        //public ICollection<T> ParseSpreadsheet<T>(DataSet dataSet) where T : new()
        //{
        //    DataTable worksheet = dataSet.Tables[_worksheetName];
        //
        //    var results = worksheet.Rows.Cast<DataRow>().Select((row, c) =>
        //    {
        //        T instance = new();
        //
        //    }).ToArray();
        //
        //    return results;
        //}
    }
}
