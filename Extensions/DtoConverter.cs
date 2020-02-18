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


    }
}