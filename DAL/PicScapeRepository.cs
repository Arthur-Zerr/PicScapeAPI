using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicScapeAPI.DAL.Models;

namespace PicScapeAPI.DAL
{
    public class PicScapeRepository
    {
        private readonly PicScapeContext picscapeContext;

        public PicScapeRepository(PicScapeContext picscapeContext)
        {
            this.picscapeContext = picscapeContext;
        }

        #region User
        public async Task<string> GetUserid(string username)
        {
            string result = "";
            result = await picscapeContext.Users.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefaultAsync();

            return result ?? "";
        }

        public async Task<bool> UpdateUserData(User user)
        {
            var tempuser = await picscapeContext.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            if(tempuser == null)
                return false;

            tempuser.Name = user.Name;
            tempuser.Firstname = user.Firstname;
            tempuser.City = user.City;
            tempuser.Country = user.Country;
            tempuser.Birthday = user.Birthday;

            if(await picscapeContext.SaveChangesAsync() >= 0)
                return true;

            return false;
        }
        #endregion

        #region Picture
        public async Task<ProfilePicture> GetCurrentProfilePictureAsync(string UserId)
        {
            var returnPicture = await picscapeContext.ProfilePictures
                .Where(x => x.UserID == UserId && x.isCurrentPicture == true)
                .FirstOrDefaultAsync();
                
            return returnPicture;
        }

        public async Task<int> SaveProfilePicture(ProfilePicture picture)
        {
            var lastPicture = await GetCurrentProfilePictureAsync(picture.UserID);
            if(lastPicture != null)
                lastPicture.isCurrentPicture = false;

            await picscapeContext.ProfilePictures.AddAsync(picture);
            return await picscapeContext.SaveChangesAsync();
        }
        #endregion
    }
}