using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.DAL.Models;
using PicScapeAPI.Extensions;

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

        public async Task<List<User>> FindUserByUsername(string Username)
        {
            var temp = await picscapeContext.Users.Where(x => x.UserName.Contains(Username)).ToListAsync();
            return temp;
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

        public async Task<int> AddPictureToUser(string userId, Picture picture)
        {
            var tempUser = await picscapeContext.Users.Include(x => x.Pictures).Where(x => x.Id == userId).FirstOrDefaultAsync();
            tempUser.Pictures.Add(picture);

            return await picscapeContext.SaveChangesAsync();
        }

        public async Task<List<PictureDto>> GetUserPictures(string userid, int page, int pageSize)
        {
            var maxCount = await picscapeContext.Pictures.Where(x => x.UserID == userid).CountAsync(); 
            var tempPages = (double)maxCount / pageSize;
            var maxPages = (int)Math.Ceiling (tempPages);

            var skip = (page - 1) * pageSize;
            var resutList = await picscapeContext.Pictures.Where(x => x.UserID == userid).OrderByDescending(x => x.UploadDate).Skip(skip).Take(pageSize).Select(x => x.ToPictureDto()).ToListAsync();
            Console.WriteLine($"{userid}: page: {page}, pagesize:{pageSize}");
            return resutList;
        }

        public async Task<PictureDataDto> GetPictureData(int id )
        {
            var tempPicture = await picscapeContext.Pictures.Where(x => x.ID == id).FirstOrDefaultAsync();
            
            if(tempPicture == null)
                return null;
                
            return tempPicture.ToPictureDataDto();
        }
        #endregion

        #region UploadId
        public async Task<string> GetPictureUploadId(int id )
        {
            var tempPicture = await picscapeContext.Pictures.Where(x => x.ID == id).FirstOrDefaultAsync();

            if(tempPicture == null)
                return String.Empty;

            return tempPicture.UploadId;
        }

        public async Task<string> GetProfilePictureUploadId(int id)
        {
            var tempPicture = await picscapeContext.ProfilePictures.Where(x => x.ID == id).FirstOrDefaultAsync();

            if(tempPicture == null)
                return String.Empty;

            return tempPicture.UploadId;
        }
        #endregion

        #region Count
        public async Task<int> GetPictureCount()
        {
            return await picscapeContext.Pictures.CountAsync();
        }

        public async Task<int> GetProfilePictureCount()
        {
            return await picscapeContext.ProfilePictures.CountAsync();
        }

        #endregion
    }
}