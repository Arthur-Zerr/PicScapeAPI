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
        public DbSet<UserActivity> UserActivitys { get; set; }
        public DbSet<UserRegistrationActivity> UserRegistrationActivitys { get; set; }
    }
}
