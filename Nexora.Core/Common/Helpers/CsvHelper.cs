using Nexora.Core.Common.Extensions;

namespace Nexora.Core.Common.Helpers
{
    public static class CsvHelper
    {
        public static List<T> MappingToObject<T>(Stream file) where T : class, new()
        {
            var result = new List<T>();

            using var reader = new StreamReader(file);

            var columns = GetLineData(reader.ReadLine());

            if (columns.HasValue() && ValidateColumns<T>(columns))
            {
                string? line;

                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var rowData = GetLineData(line);

                    if (rowData.HasValue())
                    {
                        var data = new Dictionary<string, string>();

                        for (var i = 0; i < columns.Count; i++)
                        {
                            data.Add(columns[i], rowData[i]);
                        }

                        result.Add(data.ToObject<T>());
                    }
                }
            }

            return result;
        }

        private static List<string> GetLineData(string? line)
        {
            if (!string.IsNullOrEmpty(line))
                return line.Contains(',') ? line.Split(',').ToList() : new List<string> { line.ToString() };

            return new List<string>();
        }

        private static bool ValidateColumns<T>(List<string> columns)
        {
            var properties = typeof(T).GetProperties();

            return !(properties.Count() != columns.Count || properties.Any(x => !columns.Any(y => y == x.Name)));
        }
    }
}