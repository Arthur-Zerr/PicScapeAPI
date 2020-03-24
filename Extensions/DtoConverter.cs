using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.DAL.Models;

namespace PicScapeAPI.Extensions
{
    public static class DtoConverter
    {

        public static UserForReturnDto ToUserForReturnDto(this User user)
        {
            var temp = new UserForReturnDto{
                Id = user.Id,
                City = user.City,
                Country = user.Country,
                Firstname = user.Firstname,
                LastName = user.Name,
                Username = user.UserName
            };
            return temp;
        }

        public static PictureDto ToPictureDto(this Picture picture)
        {
            var temp = new PictureDto
            {
                id = picture.ID,
                Img = picture.Img,
                UserID = picture.UserID
            };

            return temp;
        }

        public static PictureDataDto ToPictureDataDto(this Picture picture)
        {
            var temp = new PictureDataDto
            {
                Id = picture.ID,
                Title = picture.Title,
                UserID = picture.UserID,
                UploadDate = picture.UploadDate.ToSwiftString()
            };

            return temp;
        }
    }
}