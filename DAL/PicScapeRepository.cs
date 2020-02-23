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
            lastPicture.isCurrentPicture = false;

            await picscapeContext.ProfilePictures.AddAsync(picture);
            return await picscapeContext.SaveChangesAsync();
        }
        #endregion
    }
}