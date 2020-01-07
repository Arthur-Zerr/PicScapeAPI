using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PicScapeAPI.DAL.Models
{
    public class User : IdentityUser
    {

        /*
            var id: Int
            var Username: String
            
            var UserPicUrl: String
            
            var FirstName: String
            var LastName: String
            
            var City: String
            var Country: String
            
        */
        public string Name { get; set; }
        public string Firstname { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Birthday { get; set; }
        public List<Picture> Pictures { get; set; }
    }
}
