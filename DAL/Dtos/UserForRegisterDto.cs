using System;

namespace PicScapeAPI.DAL.Dtos
{
    public class UserForRegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}