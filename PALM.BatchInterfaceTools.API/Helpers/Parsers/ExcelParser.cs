using AutoMapper.Internal;
using ExcelDataReader;
using System.Data;
using System.Reflection;
using System.Text;

namespace PALM.BatchInterfaceTools.API.Helpers.Parsers
{
    public static class ExcelParser
    {
        private const string _worksheetName = "Input";

        /// <summary>
        /// Parse memory stream of an Excel file and convert contents to an enumerable of specified class.
        /// </summary>
        /// <typeparam name="T">Type of instance that the return enumerable should be based on.</typeparam>
        /// <param name="stream">Memory stream based on Excel file contents.</param>
        /// <returns>Enumerable of type T.</returns>
        public static IEnumerable<T> ParseExcelUploadTemplate<T>(MemoryStream stream) where T : new()
        {
            // Added the below line to overcome "No data is available for encoding 1252" issue with .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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

        /// <summary>
        /// Parses the dataset into an enumerable of specified class. 
        /// </summary>
        /// <typeparam name="T">Type of instance that the return enumerable should be based on.</typeparam>
        /// <param name="dataSet">Dataset based on Excel file contents.</param>
        /// <returns>Enumerable of type T.</returns>
        private static IEnumerable<T> ParseDataSet<T>(DataSet dataSet) where T : new()
        {
            DataTable worksheet = dataSet.Tables[_worksheetName] ?? dataSet.Tables[0];

            IEnumerable<PropertyInfo> instanceProperties = typeof(T).GetProperties();

            List<Exception> exceptions = new List<Exception>();
        
            var results = worksheet.Rows.Cast<DataRow>().Select((row, index) =>
            {
                T instance = new();                

                foreach(var property in instanceProperties)
                {
                    var propertyType = property.PropertyType;

                    try
                    {
                        var value = ParsedValue(property, row[property.Name]);

                        property.SetValue(instance, value);
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException || ex is InvalidCastException || ex is ArgumentException)
                        {
                            exceptions.Add(new Exception($"{property.Name} on row {index + 1} was not able to be parsed correctly.", ex));
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return instance;
            }).ToList(); // ToList is needed in order to capture the exceptions here

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions.ToArray()) { };
            }
        
            return results;
        }

        private static object? ParsedValue(PropertyInfo propertyInfo, object value)
        {
            // Handle null instances
            if (value is System.DBNull || value is null)
                return null;

            var propertyType = propertyInfo.PropertyType;

            if (propertyType.IsEnum)
            {
                if (!string.IsNullOrWhiteSpace((string)value))
                {
                    return Enum.Parse(propertyType, value.ToString());
                }
                else
                {
                    return null;
                }
            }

            if (propertyType == typeof(DateOnly) || propertyType == typeof(DateOnly?))
            {
                return DateOnly.FromDateTime((DateTime)value);
            }

            if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                return int.Parse(value.ToString());
            }

            if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            {
                /////// DISTRIBUTIONPER ENTAGE IN TEMPLATE
                return decimal.Parse(value.ToString());
            }

            return value;
        }
    }
}
