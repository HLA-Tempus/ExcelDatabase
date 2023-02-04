using System.Globalization;
using System.IO;
using ExcelDatabase.Editor.Library;
using NPOI.SS.UserModel;

namespace ExcelDatabase.Editor.Parser
{
    public static class ParseUtility
    {
        public static string GetCellValue(this IRow row, int index)
        {
            var cell = row.GetCell(index);
            var cellType = cell switch
            {
                null => CellType.Blank,
                _ when cell.CellType == CellType.Formula => cell.CachedFormulaResultType,
                _ => cell.CellType
            };

            return cellType switch
            {
                CellType.String => cell!.StringCellValue,
                CellType.Numeric => cell!.NumericCellValue.ToString(CultureInfo.InvariantCulture),
                CellType.Boolean => cell!.BooleanCellValue.ToString(),
                _ => string.Empty
            };
        }

        public static string WriteScript(TableType type, string tableName, string script)
        {
            var distDirectory = $"{Config.DistPath}/{type}";
            if (!Directory.Exists(distDirectory))
            {
                Directory.CreateDirectory(distDirectory);
            }

            var distPath = $"{distDirectory}/{tableName}.cs";
            File.WriteAllText(distPath, script);
            return distPath;
        }
    }
}