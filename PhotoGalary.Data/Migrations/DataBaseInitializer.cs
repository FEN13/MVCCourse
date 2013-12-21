using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using DevOne.Security.Cryptography.BCrypt;
using PhotoGalery.Data.Models;


namespace PhotoGalery.Data.Migrations
{
    public class DataBaseInitializer : IDatabaseInitializer<PhotoGaleryDataContext>
    {
        public const string DefaultPassword = "1234567";
        public void InitializeDatabase(PhotoGaleryDataContext context)
        {
            var administratorRole = context.Roles.SingleOrDefault(role => role.Name == "Administrator") ?? new Role { Id = 1, Name = "Administrator" };
            var userRole = context.Roles.SingleOrDefault(role => role.Name == "User") ?? new Role { Id = 2, Name = "User" };
            context.Roles.AddOrUpdate(r => r.Name, administratorRole, userRole);
            
            context.Users.Add(
                new User
                {
                    Id = 1,
                    FirstName = Faker.NameFaker.FirstName(),
                    LastName = Faker.NameFaker.LastName(),
                    Email = "admin@test.com",
                    IsActivated = true,
                    IsLocked = false,
                    Password = BCryptHelper.HashPassword(DefaultPassword, BCryptHelper.GenerateSalt(12)),
                    Role = administratorRole,
                    CreditCardNumber = "1234567891234567",
                    CreatedAt = Faker.DateTimeFaker.DateTime(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(-5)),
                    LastActiveDate = DateTime.Now
                });
            context.Users.Add(
               new RegularUser()
               {
                   Id = 2,
                   FirstName = Faker.NameFaker.FirstName(),
                   LastName = Faker.NameFaker.LastName(),
                   Email = "user@test.com",
                   IsActivated = true,
                   IsLocked = false,
                   Password = BCryptHelper.HashPassword(DefaultPassword, BCryptHelper.GenerateSalt(12)),
                   Role = userRole,
                   CreatedAt = Faker.DateTimeFaker.DateTime(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(-5)),
                   LastActiveDate = DateTime.Now
               });
            for (var i = context.Users.Count(); i < 3; i++)
            {
                var user = new RegularUser
                {
                    Id = i + 1,
                    FirstName = Faker.NameFaker.FirstName(),
                    LastName = Faker.NameFaker.LastName(),
                    Email = Faker.InternetFaker.Email(),
                    IsActivated = Faker.BooleanFaker.Boolean(),
                    IsLocked = Faker.BooleanFaker.Boolean(),
                    CreatedAt = Faker.DateTimeFaker.DateTime(DateTime.Now.AddMonths(-3), DateTime.Now.AddDays(-5)),
                    Password = BCryptHelper.HashPassword(DefaultPassword, BCryptHelper.GenerateSalt(12)),
                    Role = userRole,
                    LastActiveDate = DateTime.Now,
                    PhotoLimit = 30,
                    AlbumLimit = 5
                };
                user.LastActiveDate = Faker.DateTimeFaker.DateTime(user.CreatedAt, DateTime.Now);
                if (!context.Users.Any(u => u.Email == user.Email))
                {
                    context.Users.Add(user);
                }
            }

            context.Albums.AddOrUpdate(new Album
            {
                Id = 1,
                Name = "Portrait",
            });

            context.Albums.AddOrUpdate(new Album
            {
                Id = 2,
                Name = "Genre",
            });

            context.Albums.AddOrUpdate(new Album
            {
                Id = 3,
                Name = "Lanscape",
            });

            context.Albums.AddOrUpdate(new Album
            {
                Id = 4,
                Name = "Cats",
            });

            context.Albums.AddOrUpdate(new Album
            {
                Id = 5,
                Name = "Cars",
            });

            context.SaveChanges();
            var usr = context.Users.SingleOrDefault(u => u.Id == 1);
            if (!context.UserAlbums.Any())
            {
                List<Album> albs = context.Albums.ToList();
                foreach (var alb in albs)
                {
                    context.UserAlbums.AddOrUpdate(new UserAlbum { CreatedAt = DateTime.Now, User = usr, Album = alb });
                }
            }
            context.SaveChanges();

            context.Manufacturers.Add(new Manufacturer { Id = 1, Name = "Canon", Models = new List<Model> { new Model { Name = "PowerShot SX150" }, new Model { Name = "PowerShot SX160" }, new Model { Name = "PowerShot A2500" }, new Model { Name = "PowerShot A1400" }, new Model { Name = "PowerShot SX500" }, new Model { Name = "PowerShot A4050" }, new Model { Name = "PowerShot SX50" }, new Model { Name = "PowerShot SX270" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 2, Name = "Nikon", Models = new List<Model> { new Model { Name = "Coolpix L820" }, new Model { Name = "Coolpix S3500" }, new Model { Name = "Coolpix L27" }, new Model { Name = "Coolpix L28" }, new Model { Name = "Coolpix S6400" }, new Model { Name = "Coolpix P520" }, new Model { Name = "Coolpix L820" }, new Model { Name = "Coolpix S3500" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 3, Name = "Kodak", Models = new List<Model> { new Model { Name = "Easyshare M577" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 4, Name = "Benq", Models = new List<Model> { new Model { Name = "GH650" }, new Model { Name = "G1" }, new Model { Name = "GH600" }, new Model { Name = "GH200" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 5, Name = "Casio", Models = new List<Model> { new Model { Name = "Exilim EX-S200" }, new Model { Name = "Exilim EX-ZR700" }, new Model { Name = "Exilim QV-R300" }, new Model { Name = "Exilim EX-ZR1000" }, new Model { Name = "Exilim EX-ZS20" }, new Model { Name = "Exilim EX-ZS200" }, new Model { Name = "Exilim EX-Z32" }, new Model { Name = "Exilim EX-ZR400" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 6, Name = "Fuji", Models = new List<Model> { new Model { Name = "FinePix S2980" }, new Model { Name = "FinePix HS25EXR" }, new Model { Name = "FinePix AX550" }, new Model { Name = "FinePix S4800" }, new Model { Name = "FinePix X20" }, new Model { Name = "FinePix X100S" }, new Model { Name = "FinePix SL240" }, new Model { Name = "FinePix X100" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 7, Name = "Genius", Models = new List<Model> { new Model { Name = "G-Shot 507" }, new Model { Name = "G-Shot 507X" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 8, Name = "Olympus", Models = new List<Model> { new Model { Name = "VH-410 " }, new Model { Name = "VG-160" }, new Model { Name = "VR-340" }, new Model { Name = "VR-350" }, new Model { Name = "VG-180" }, new Model { Name = "SP-620" }, new Model { Name = "SZ-14" }, new Model { Name = "SZ-15" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 9, Name = "Panasonic", Models = new List<Model> { new Model { Name = "LUMIX DMC-FZ72EE-K" }, new Model { Name = "LUMIX DMC-LZ20EE-K" }, new Model { Name = "LUMIX DMC-G6XEE-K" }, new Model { Name = "LUMIX DMC-FZ62EE-K" }, new Model { Name = "LUMIX DMC-GF5KAEE-K" }, new Model { Name = "LUMIX DMC-G5KEE-K" }, new Model { Name = "LUMIX DMC-FZ200EEK" }, new Model { Name = "LUMIX DMC-LF1EE-K" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 10, Name = "Pentax", Models = new List<Model> { new Model { Name = "Optio X-5" }, new Model { Name = "Optio WG-10" }, new Model { Name = "Optio WG-3" }, new Model { Name = "Optio WG-3 GPS" }, new Model { Name = "MX-1" }, new Model { Name = "Q7" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 11, Name = "Ricoh", Models = new List<Model> { new Model { Name = "GXR Body" }, new Model { Name = "GR" }, new Model { Name = "CX6" }, new Model { Name = "GR IV" }, new Model { Name = "HZ15" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 12, Name = "Samsung", Models = new List<Model> { new Model { Name = "ES95" }, new Model { Name = "WB200F" }, new Model { Name = "WB800F" }, new Model { Name = "ST80" }, new Model { Name = "WB2100" }, new Model { Name = "NX-300" }, new Model { Name = "DV150F" }, new Model { Name = "ST72" } } });
            context.Manufacturers.Add(new Manufacturer { Id = 13, Name = "Sony", Models = new List<Model> { new Model { Name = "Cyber-Shot H200" }, new Model { Name = "Cyber-Shot H100" }, new Model { Name = "Cyber-Shot WX60" }, new Model { Name = "Cyber-Shot HX1" }, new Model { Name = "Cyber-Shot W730" }, new Model { Name = "Cyber-Shot RX1R" }, new Model { Name = "NEX-3NL" }, new Model { Name = "Cyber-Shot TX30" } } });
            context.SaveChanges();
        }
    }
}
