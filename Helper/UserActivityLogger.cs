using System.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicScapeAPI.DAL;
using PicScapeAPI.DAL.Models;

namespace PicScapeAPI.Helper
{
    public class UserActivityLogger
    {
        private readonly PicScapeContext picScapeContext;

        public UserActivityLogger(PicScapeContext picScapeContext)
        {
            this.picScapeContext = picScapeContext;
        }

        public async Task LogLogin(string UserId)
        {
            var entry = new UserActivity
            {
                UserId = UserId, 
                LoggedIn = DateTime.Now
            };

            await picScapeContext.UserActivitys.AddAsync(entry);
            await picScapeContext.SaveChangesAsync();
        }
        
        public async Task LogLogout(string UserId)
        {
            var lastentryList = await picScapeContext.UserActivitys.Where(x => x.UserId == UserId).ToListAsync();
            var lastentry = lastentryList.LastOrDefault();

            lastentry.LoggedOut = DateTime.Now;

            await picScapeContext.SaveChangesAsync();
        }

        public async Task LogRegistration(string UserId)
        {
            var entry = new UserRegistrationActivity
            {
                UserId = UserId,
                RegistrationTime = DateTime.Now
            };

            await picScapeContext.UserRegistrationActivitys.AddAsync(entry);
            await picScapeContext.SaveChangesAsync();
        }
    }
}