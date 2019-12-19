using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PicScapeAPI.DAL.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Firstname { get; set; }
        public DateTime Birthday { get; set; }
        public List<Picture> Pictures { get; set; }
    }
}
