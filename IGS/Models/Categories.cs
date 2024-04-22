using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace IGS.Models
{
    public class Categories
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageName { get; set; }
        public string CategoryImagePath { get; set; }
        public string UserPhone { get; set; }


        public override string ToString()
        {
            return this.CategoryName;
        }
    }
}
