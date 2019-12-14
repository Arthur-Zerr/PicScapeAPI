using System;
using System.Collections.Generic;

namespace PicScapeAPI.DAL.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Username { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }

        public DateTime UserCreated { get; set; }
        public DateTime Birthday { get; set; }

        public List<Picture> Pictures { get; set; }
    }
}
