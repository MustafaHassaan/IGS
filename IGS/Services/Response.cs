using System;
using System.Collections.Generic;
using System.Text;

namespace IGS.Services
{
    class Response
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Id { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }
        public bool ISAdmin { get; set; }
    }
}
