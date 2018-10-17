using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Common
{
    public class ModelConvertHelper<T> where T : new()
    {

        public static List<T> ConvertToModel(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                var cname = dc.ColumnName;
                if (cname.StartsWith("ExtenKeyList"))
                    columns.Add(cname);
            }

            List<T> list = new List<T>();
            foreach (DataRow dataRow in dt.Rows)
            {
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] properties = t.GetType().GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo propertyInfo = properties[i];
                    string name = propertyInfo.Name;
                    if (dt.Columns.Contains(name) && propertyInfo.CanWrite)
                    {
                        object obj = dataRow[name];
                        if (obj != DBNull.Value)
                        {
                            if (name.Equals("ExtenKey"))
                            {
                                propertyInfo.SetValue(t, obj.ToString(), null);
                            }
                            else
                                propertyInfo.SetValue(t, obj, null);
                        }
                    }
                    else if (name.Equals("ExtenKeyList") && columns.Count > 0)
                    {
                        var str_list = new List<string>();
                        foreach (var cname in columns)
                        {
                            object obj = dataRow[cname];
                            if (obj != DBNull.Value)
                            {
                                str_list.Add(obj.ToString());
                            }
                            else
                                str_list.Add("");
                        }
                        propertyInfo.SetValue(t, str_list, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

    }
}
