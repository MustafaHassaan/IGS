using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IGS.Appdata
{
    public class IGSDb
    {
		public SQLiteConnection Conn { get; set; }
		public IGSDb()
		{
			Initialize();
		}
		void Initialize()
		{
			//Connection string
			Conn = new SQLiteConnection(Configure.DbPath,
										SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);

			Conn.CreateTable<AuthUser>(CreateFlags.None);
		}
	}
}
