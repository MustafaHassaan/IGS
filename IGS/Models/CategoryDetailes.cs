using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IGS.Models
{
    public class CategoryDetailes
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string DetailesName { get; set; }
        public string CategoryName { get; set; }
        public int CatId { get; set; }

        public virtual Categories Cat { get; set; }

        public override string ToString()
        {
            return this.DetailesName;
        }
    }
}
