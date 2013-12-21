using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using PhotoGalery.Data.ModelConfigurations;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data
{
	public class PhotoGaleryDataContext: DbContext
	{
		public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<UserAlbum> UserAlbums { get; set; }
		public DbSet<Device> Devices { get; set; }
        public DbSet<Photo> Photos { get; set; }
		public DbSet<Manufacturer> Manufacturers { get; set; }
		public DbSet<Model> Models { get; set; }

		public PhotoGaleryDataContext()
            : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new RolesConfiguration());
            modelBuilder.Configurations.Add(new UsersConfiguration());
            modelBuilder.Configurations.Add(new TokensConfiguration());
			modelBuilder.Configurations.Add(new AlbumsConfiguration());
            modelBuilder.Configurations.Add(new DeviceConfiguration());
			modelBuilder.Configurations.Add(new PhotoConfiguration());
			modelBuilder.Configurations.Add(new ManufacturerConfiguration());
			modelBuilder.Configurations.Add(new ModelConfiguration());
        }
	}
}
