using System;
using Microsoft.EntityFrameworkCore;
using PicScapeAPI.DAL.Models;

namespace PicScapeAPI.DAL
{
    public class PicScapeContext : DbContext
    {
        public PicScapeContext(DbContextOptions<PicScapeContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
    }
}
