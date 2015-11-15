using System;
using System.Collections.Generic;
using System.Reflection;

namespace EntityToSql
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			User user = new User ();
			user.id = 1;
			user.Name = "Huseyin";
			user.Surname = "Altubas";
			user.Age = 32;

			string insert =	EntityToSql.Insert (user,"TabloAdıv2","id");
			Console.WriteLine (insert);


			string where = "Where id=2 and id=3";
			string update =	EntityToSql.Update (user,where,"TabloAdıv2","id");
			Console.WriteLine (update);

			Console.ReadLine ();

		}




	}

	public class User
	{
		public int id { get; set;}
		public string Name { get; set;}
		public string Surname { get; set;}
		public int Age { get; set;}
	}

}





