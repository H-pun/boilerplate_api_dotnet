using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace DataAccess.Entities
{
    public class Admin : BaseEntities
    {   
        public string idUser { get; set; }
        public string name { get; set; }
    }
}
