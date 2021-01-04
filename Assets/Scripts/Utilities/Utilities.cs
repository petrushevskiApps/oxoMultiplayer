using System.Text;
using UnityEngine;

namespace com.petrushevskiapps.Oxo.Utilities
{
    public static class Utilities
    {
        public static void PrintArray<T>(T[] row)
        {
            StringBuilder sb = new StringBuilder();
            for (int i=0; i<row.Length; i++)
            {
                sb.Append(row[i] + ",");
            }
            Debug.Log($"ROW: {row.Length} -> {sb.ToString()}");
        }
        
        public static void PrintTable<T>(T[,] table)
        {
            int printIndex = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (T tile in table)
            {
                sb.Append(tile + " ");
                printIndex++;
                if (printIndex % 3 == 0 && printIndex > 0)
                {
                    sb.AppendLine();
                }
            }
            Debug.Log(sb);
        }
        
        public static int GetRowFromId<T>(int tileId, T[,] table) => tileId / (table.GetUpperBound(0) + 1);
        public static int GetColumnFromId<T>(int tileId, T[,] table) => tileId % (table.GetUpperBound(1) + 1);
    }
    
    
}