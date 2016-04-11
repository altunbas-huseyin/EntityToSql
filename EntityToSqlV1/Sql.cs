using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntityToSqlV1
{
    public static class Sql
    {

        /// <summary>
        /// Insert the specified tip.
        /// </summary>
        /// <param name="tip">Tip.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>

        public static string Insert<T>(T tip, String TableName = "", String PrimaryKey = "", String DateTimeFormat="")
        {
            if (TableName == "")
            {
                TableName = tip.GetType().Name;
            }

            string Fields = Sql.GetTablesField(tip, PrimaryKey);
            string Values = Sql.GetTablesFieldValues(tip, PrimaryKey,DateTimeFormat);
            string sql = "INSERT INTO " + TableName + " (" + Fields + ") Values(" + Values + ")";
            sql += " SELECT SCOPE_IDENTITY()";
            return sql;

        }

        public static string Update<T>(T tip, String TableName = "", String PrimaryKey = "", String DateTimeFormat="")
        {
            if (TableName == "")
            {
                TableName = tip.GetType().Name;
            }
            string sql = "UPDATE " + TableName + " SET ";
            foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties())
            {
                if (propertyInfo.Name == PrimaryKey)
                {
                    continue;
                }
                if (Sql.GetPropertyInfoValues(tip, propertyInfo,DateTimeFormat) == "-1")
                { continue; }
                sql += propertyInfo.Name + "=" + Sql.GetPropertyInfoValues(tip, propertyInfo,DateTimeFormat) + ",";
            }
            sql = sql.Remove(sql.Length - 1);

            return sql + " ";
        }

        static string GetPropertyInfoValues<T>(T tip, PropertyInfo propertyInfo, String DateTimeFormat)
        {

            object o = propertyInfo.GetValue(tip, null); //throws exception TargetParameterCountException for String type
            if (o == null) { return "-1"; }

            string str = "";

            string FieldValue = propertyInfo.GetValue(tip, null).ToString();

            switch (propertyInfo.PropertyType.ToString())
            {
                case "System.Int":
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                case "System.Int16":
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                case "System.Int32":
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                case "System.Int64":
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                case "System.Float":
                    str += Sql.Tirnaksiz(FieldValue.Replace(",","."));
                    break;
                case "System.Decimal":
                    str += Sql.Tirnaksiz(FieldValue.Replace(",","."));
                    break;
                case "System.Double":
                    str += Sql.Tirnaksiz(FieldValue.Replace(",","."));
                    break;
                default:
                    str += Sql.Tirnakli(FieldValue,  DateTimeFormat);
                    break;
            }

            return str;
        }

      
        static string GetTablesField<T>(T tip, String PrimaryKey = "")
        {
            string str = "";
            foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties())
            {
                if (propertyInfo.Name == PrimaryKey)
                {
                    continue;
                }

                object o = propertyInfo.GetValue(tip, null); //throws exception TargetParameterCountException for String type
                if (o == null) { continue; }

                str += propertyInfo.Name + ",";
            }
            return str = str.Remove(str.Length - 1);
        }

        static string GetTablesFieldValues<T>(T tip, String PrimaryKey = "", String DateTimeFormat="")
        {
            string str = "";
            foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties())
            {

                if (propertyInfo.Name == PrimaryKey)
                {
                    continue;
                }

                object o = propertyInfo.GetValue(tip, null); //throws exception TargetParameterCountException for String type
                if (o == null) { continue; }

                string val = GetPropertyInfoValues(tip, propertyInfo, DateTimeFormat);
                if (val == "-1")
                { continue; }
                str += val + ",";
            }
            return str = str.Remove(str.Length - 1);
        }


        /// <summary>
        /// Tirnakli the specified val.
        /// </summary>
        /// <param name="val">Value.</param>
        public static string Tirnakli(String val, String DateTimeFormat)
        {
            //Burada örneğin 2.3 bir değer gelirse mallaşıyor ve onu datetime a çeviriyor. Ondan dolayı virgül yoksa datetime için işleme alıyorum.
            DateTime d;
            if (!val.Contains(","))
            {
                if (DateTime.TryParse(val, out d))
                {
                    return "'" + d.ToString(DateTimeFormat) + "'";
                }
            }
            return "'" + val + "'";
        }


        /// <summary>
        /// Tirnaksiz the specified val.
        /// </summary>
        /// <param name="val">Value.</param>
        public static string Tirnaksiz(String val)
        {
            return val;
        }


    }
}
