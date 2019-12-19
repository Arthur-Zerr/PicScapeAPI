using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PicScapeAPI.DAL.Models;

namespace PicScapeAPI.DAL
{
    public class PicScapeContext : IdentityDbContext
    {
        public PicScapeContext(DbContextOptions<PicScapeContext> options) : base(options)
        {
        }
        public DbSet<Picture> Pictures { get; set; }
    }
}
