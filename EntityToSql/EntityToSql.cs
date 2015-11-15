using System;
using System.Reflection;

namespace EntityToSql
{
	public static class EntityToSql
	{
		
		/// <summary>
		/// Insert the specified tip.
		/// </summary>
		/// <param name="tip">Tip.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>

		public static string Update<T> (T tip, String Where, String TableName = "", String PrimaryKey = "")
		{  
			if (TableName == "") {
				TableName = tip.GetType ().Name;
			}
			string sql = "UPDATE TABLE " + TableName + " ";
			foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties()) {
				if (propertyInfo.Name == PrimaryKey) {
					continue;
				}

				sql += propertyInfo.Name + "=" + EntityToSql.GetPropertyInfoValues (tip, propertyInfo) + ",";
			}
			sql = sql.Remove (sql.Length - 1);

			return sql+ " " + Where;
		}

		static string GetPropertyInfoValues<T> (T tip, PropertyInfo propertyInfo)
		{
			string str = "";

			string FieldValue = propertyInfo.GetValue (tip, null).ToString ();
				
			switch (propertyInfo.PropertyType.ToString ()) {  
			case "System.Int":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;  
			case "System.Int16":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;
			case "System.Int32":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;
			case "System.Int64":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;
			case "System.Float":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;
			case "System.Decimal":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;
			case "System.Double":
				str += EntityToSql.Tirnaksiz (FieldValue);
				break;
			default:
				str += EntityToSql.Tirnakli (FieldValue);
				break; 
			} 
				
			return str;
		}

		public static string Insert<T> (T tip, String TableName = "", String PrimaryKey = "")
		{  
			if (TableName == "") {
				TableName = tip.GetType ().Name;
			}

			string Fields = EntityToSql.GetTablesField (tip, PrimaryKey);
			string Values = EntityToSql.GetTablesFieldValues (tip, PrimaryKey);
			string sql = "INSERT INTO " + TableName + " (" + Fields + ") Values(" + Values + ")";

			return sql;

		}

		static string GetTablesField<T> (T tip, String PrimaryKey = "")
		{
			string str = "";
			foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties()) {
				if (propertyInfo.Name == PrimaryKey) {
					continue;
				}
				str += propertyInfo.Name + ",";
			}
			return str = str.Remove (str.Length - 1);
		}

		static string GetTablesFieldValues<T> (T tip, String PrimaryKey = "")
		{
			string str = "";
			foreach (PropertyInfo propertyInfo in tip.GetType().GetProperties()) {

				if (propertyInfo.Name == PrimaryKey) {
					continue;
				}

				str += GetPropertyInfoValues (tip,propertyInfo);

				str += ",";
			}
			return str = str.Remove (str.Length - 1);
		}


		/// <summary>
		/// Tirnakli the specified val.
		/// </summary>
		/// <param name="val">Value.</param>
		public static string Tirnakli (String val)
		{
			return "'" + val + "'";
		}


		/// <summary>
		/// Tirnaksiz the specified val.
		/// </summary>
		/// <param name="val">Value.</param>
		public static string Tirnaksiz (String val)
		{
			return val;
		}


	}
}

