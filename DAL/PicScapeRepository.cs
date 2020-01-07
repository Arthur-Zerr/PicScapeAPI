using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PicScapeAPI.DAL
{
    public class PicScapeRepository
    {
        private readonly PicScapeContext picscapeContext;

        public PicScapeRepository(PicScapeContext picscapeContext)
        {
            this.picscapeContext = picscapeContext;
        }


        public async Task<string> GetUserid(string username)
        {
            string result = "";
            result = await picscapeContext.Users.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefaultAsync();

            return result ?? "";
        }
    }
}