using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// DataList 的摘要说明
/// </summary>
public static class DataList
{
    public static List<T> ToList<T>(this DataTable dt)
    {
        var dataColumn = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

        var properties = typeof(T).GetProperties();
        string columnName = string.Empty;

        return dt.AsEnumerable().Select(row =>
        {
            var t = Activator.CreateInstance<T>();
            foreach (var p in properties)
            {
                columnName = p.Name;
                if (dataColumn.Contains(columnName))
                {
                    if (!p.CanWrite)
                        continue;

                    object value = row[columnName];
                    Type type = p.PropertyType;

                    if (value != DBNull.Value)
                    {
                        p.SetValue(t, Convert.ChangeType(value, type), null);
                    }
                }
            }
            return t;
        }).ToList();
    }

    public static DataTable ToDataTable<T>(this List<T> items)
    {
        DataTable dataTable = new DataTable();

        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            dataTable.Columns.Add(prop.Name, prop.PropertyType);
        }

        foreach (T obj in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                values[i] = Props[i].GetValue(obj, null);
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }
}