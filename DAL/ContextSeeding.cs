using System;
using PicScapeAPI.DAL.Models;

namespace PicScapeAPI.DAL
{
    public class ContextSeeding
    {
        private readonly PicScapeContext picScapeContext;

        public ContextSeeding(PicScapeContext picScapeContext)
        {
            this.picScapeContext = picScapeContext;
        }


        public async void UserSeeding()
        {
            var user = new User{Username = "ArthurTest", UserCreated=DateTime.Now, Name="Zerr", Firstname="Arthur", Birthday=DateTime.Now};

            await picScapeContext.Users.AddAsync(user);
            await picScapeContext.SaveChangesAsync();
        }
    }
}