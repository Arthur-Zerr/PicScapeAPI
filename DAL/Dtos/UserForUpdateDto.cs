using System;

namespace PicScapeAPI.DAL.Dtos
{
    public class UserForUpdateDto
    {
        public string Name { get; set; }
        public string Firstname { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string Birthday { get; set; }
    }
}