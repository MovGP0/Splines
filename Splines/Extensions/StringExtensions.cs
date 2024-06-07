namespace Splines.Extensions;

public static class StringExtensions
{
    public static string ToValueTableString(this string[,] m)
    {
        int rowCount = m.GetLength(0);
        int colCount = m.GetLength(1);
        string[] r = new string[rowCount];
        for (int i = 0; i < rowCount; i++)
            r[i] = "";

        for (int c = 0; c < colCount; c++) {
            string endBit = c == colCount - 1 ? "" : ", ";

            int colWidth = 4; // min width
            string[] columnEntries = new string[rowCount];
            for (int row = 0; row < rowCount; row++) {
                string s = m[row, c].StartsWith("-") ? "" : " ";
                columnEntries[row] = $"{s}{m[row, c]}{endBit}";
                colWidth = Mathfs.Max(colWidth, columnEntries[row].Length);
            }

            for (int row = 0; row < rowCount; row++) {
                r[row] += columnEntries[row].PadRight(colWidth, ' ');
            }
        }

        return string.Join("\n", r);
    }
}
