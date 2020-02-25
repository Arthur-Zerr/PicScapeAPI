using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PicScapeAPI.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace PicScapeAPI.DAL
{
    public class PicScapeContext : IdentityDbContext<IdentityUser>
    {
        public PicScapeContext(DbContextOptions<PicScapeContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }
        public DbSet<UserActivity> UserActivitys { get; set; }
        public DbSet<UserRegistrationActivity> UserRegistrationActivitys { get; set; }
    }
}
