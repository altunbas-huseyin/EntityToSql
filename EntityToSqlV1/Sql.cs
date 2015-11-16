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

        public static string Update<T>(T tip, String Where, String TableName = "", String PrimaryKey = "")
        {
            if (TableName == "")
            {
                TableName = tip.GetType().Name;
            }
            string sql = "UPDATE TABLE " + TableName + " ";
            foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties())
            {
                if (propertyInfo.Name == PrimaryKey)
                {
                    continue;
                }
                if (Sql.GetPropertyInfoValues(tip, propertyInfo) == "-1")
                { continue; }
                sql += propertyInfo.Name + "=" + Sql.GetPropertyInfoValues(tip, propertyInfo) + ",";
            }
            sql = sql.Remove(sql.Length - 1);

            return sql + " " + Where;
        }

        static string GetPropertyInfoValues<T>(T tip, PropertyInfo propertyInfo)
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
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                case "System.Decimal":
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                case "System.Double":
                    str += Sql.Tirnaksiz(FieldValue);
                    break;
                default:
                    str += Sql.Tirnakli(FieldValue);
                    break;
            }

            return str;
        }

        public static string Insert<T>(T tip, String TableName = "", String PrimaryKey = "")
        {
            if (TableName == "")
            {
                TableName = tip.GetType().Name;
            }

            string Fields = Sql.GetTablesField(tip, PrimaryKey);
            string Values = Sql.GetTablesFieldValues(tip, PrimaryKey);
            string sql = "INSERT INTO " + TableName + " (" + Fields + ") Values(" + Values + ")";

            return sql;

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

        static string GetTablesFieldValues<T>(T tip, String PrimaryKey = "")
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

                string val = GetPropertyInfoValues(tip, propertyInfo);
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
        public static string Tirnakli(String val)
        {
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
