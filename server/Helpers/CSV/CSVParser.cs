using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Helpers.CSV
{
    public static class CSVParser<T>
    {
        public static List<T> TryParse(string filePath,string delimiter = ",", Encoding? encoding = null)
        {
            if (encoding is null)
            {
                encoding = Encoding.UTF8;
            }

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
            };

            using var reader = new StreamReader(filePath, encoding);
            using var csv = new CsvReader(reader, csvConfig);

            var records = csv.GetRecords<T>();

            return records.ToList();
        }
    }
}
