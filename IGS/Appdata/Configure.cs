using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IGS.Appdata
{
    public class Configure
    {
		//Dbfn = Data Base File Name
		private static string Dbfn = "IGSDb.db3";
		public static string DbPath
		{
			get
			{
				string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				return Path.Combine(baseFolder, Dbfn);
			}
		}
	}
}
