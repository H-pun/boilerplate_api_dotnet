using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using DataAccess.Entities;

namespace WebAPI.Models
{
    public class AdminModel
    {   
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }
}
