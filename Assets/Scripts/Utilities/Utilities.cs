using System.Text;
using UnityEngine;

namespace com.petrushevskiapps.Oxo.Utilities
{
    public class Utilities<T>
    {
        public static void PrintArray(T[] row)
        {
            StringBuilder sb = new StringBuilder();
            for (int i=0; i<row.Length; i++)
            {
                sb.Append(row[i] + ",");
            }
            Debug.Log($"ROW: {row.Length} -> {sb.ToString()}");
        }
        
        public static void PrintTable(int[,] table)
        {
            int printIndex = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (int tile in table)
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

    }
    
    
}