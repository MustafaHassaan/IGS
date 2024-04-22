using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IGS.Models
{
    public class Users
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }
        public bool UserReadPrivacy { get; set; }
        public bool IsAdmin { get; set; }


        public override string ToString()
        {
            return this.UserPhone;
        }
    }
}
