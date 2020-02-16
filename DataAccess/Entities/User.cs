using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace DataAccess.Entities
{
    public class User : BaseEntities
    {   
        public string email { get; set; }
        public string password { get; set; }
        public string appToken { get; set; }
        public int? idRole { get; set; }
    }
}
