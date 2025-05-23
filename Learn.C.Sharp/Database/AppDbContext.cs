﻿using Learn.C.Sharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace Learn.C.Sharp.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRouts { set; get; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { set; get; }
        public DbSet<ShoppingCart> ShoppingCarts { set; get; }
        public DbSet<LineItem> LineItems { set; get; }
        public DbSet<Order> Orders { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var touristRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                @"/Database/touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristRouteJsonData)!;
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);
            //modelBuilder.Entity<TouristRoute>()
            //   .Property(tr => tr.CreateTime)
            //   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            var touristRoutePictureJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                @"/Database/touristRoutePicturesMockData.json");
            IList<TouristRoutePicture> touristPictureRoutes = JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristRoutePictureJsonData)!;
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristPictureRoutes);
            //modelBuilder.Entity<TouristRoutePicture>()
            //    .HasOne(p => p.TouristRoute)
            //    .WithMany(r => r.TouristRoutePictures)
            //    .HasForeignKey(p => p.TouristRouteId)
            //    .OnDelete(DeleteBehavior.SetNull);

            // 初始化用户与角色的种子数据
            // 1. 更新用户与角色的外键关系
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(x => x.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            // 2. 添加角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"; // guid 
            var editorRoleId = "dacc15f0-bffd-477c-aa59-27b40b93b14e";// guid 
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole
                {
                    Id = editorRoleId,
                    Name = "Editor",
                    NormalizedName = "Editor".ToUpper()
                }
            );

            // 3. 添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Fake123$");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. 给用户加入管理员权限
            // 通过使用 linking table：IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });

            // 5. 初始化 ShoppingCart
            modelBuilder.Entity<ShoppingCart>()
                .HasData(new ShoppingCart()
                {
                    Id = new Guid("0e09d08e-0ad4-42c0-a0f2-d1c9e3c99a28"),
                    UserId = adminUserId,
                });

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
