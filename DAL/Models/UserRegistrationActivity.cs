using System;

namespace PicScapeAPI.DAL.Models
{
    public class UserRegistrationActivity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}