using AutoMapper.Internal;
using ExcelDataReader;
using System.Data;
using System.Reflection;

namespace PALM.DeveloperTools.API.Helpers.Parsers
{
    public static class ExcelParser
    {
        private const string _worksheetName = "Input";

        public static IEnumerable<T> ParseExcelUploadTemplate<T>(MemoryStream stream) where T : new()
        {
            using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration() 
                { 
                    ConfigureDataTable = reader => new ExcelDataTableConfiguration 
                    { 
                        UseHeaderRow = true
                    } 
                });

                return ParseDataSet<T>(dataSet);
            }            
        }

        private static IEnumerable<T> ParseDataSet<T>(DataSet dataSet) where T : new()
        {
            DataTable worksheet = dataSet.Tables[_worksheetName];

            IEnumerable<PropertyInfo> instanceProperties = typeof(T).GetProperties();

            List<Exception> exceptions = new List<Exception>();
        
            var results = worksheet.Rows.Cast<DataRow>().Select((row, c) =>
            {
                T instance = new();                

                foreach(var property in instanceProperties)
                {
                    var value = row[property.Name];

                    property.SetValue(instance, value);
                }

                return instance;
            });
        
            return results;
        }
    }
}
