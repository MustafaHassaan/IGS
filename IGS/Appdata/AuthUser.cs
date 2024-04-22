using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IGS.Appdata
{
    public class AuthUser
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }
        public bool UserAuthentication { get; set; }
        public bool UserAdmin { get; set; }


        public override string ToString()
        {
            return this.UserPhone + "(" + this.UserPassword + this.UserAuthentication + ")";
        }
    }
}
