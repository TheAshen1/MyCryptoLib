using CryptoSystems.Models;
using System.Collections.Generic;
using System.Data;

namespace CryptoSystems.UI.Util
{
    public static class BindingHelper
    {
        public static DataView GetBindable2DArray<T>(T[,] array)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < array.GetLength(1); i++)
            {
                dataTable.Columns.Add(i.ToString(), typeof(Ref<T>));
            }
            for (int i = 0; i < array.GetLength(0); i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            DataView dataView = new DataView(dataTable);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    int a = i;
                    int b = j;
                    Ref<T> refT = new Ref<T>(() => array[a, b], z => { array[a, b] = z; });
                    dataView[i][j] = refT;
                }
            }
            return dataView;
        }

        public static DataView GetBindable2DArray(MatrixInt matrix)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                dataTable.Columns.Add(i.ToString(), typeof(Ref<int>));
            }
            for (int i = 0; i < matrix.RowCount; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            DataView dataView = new DataView(dataTable);
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    int a = i;
                    int b = j;
                    dataView[a][b] = new Ref<int>(() => matrix[a, b], z => { matrix[a, b] = z; });
                }
            }
            return dataView;
        }

        public static DataView GetBindable2DArray(List<Point> points)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < 3; i++)
            {
                dataTable.Columns.Add(i.ToString(), typeof(Ref<int>));
            }
            for (int i = 0; i < points.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            DataView dataView = new DataView(dataTable);
            for (int i = 0; i < points.Count; i++)
            {
                int a = i;
                dataView[a][0] = new Ref<int>(() => points[a].x, v => {; });
                dataView[a][1] = new Ref<int>(() => points[a].y, v => {; });
                dataView[a][2] = new Ref<int>(() => points[a].z, v => {; });
            }
            return dataView;
        }

        public static DataView GetBindable2DArray<T>(IList<T> items)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < items.Count; i++)
            {
                dataTable.Columns.Add(i.ToString(), typeof(Ref<T>));
            }
            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);
            DataView dataView = new DataView(dataTable);
            for (int i = 0; i < items.Count; i++)
            {
                int a = i;
                dataView[0][a] = new Ref<T>(() => items[a], v => { items[a] = v; });
            }
            return dataView;
        }
    }
}
