using System.Data.Entity.Migrations;

namespace PhotoGalery.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<PhotoGaleryDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(PhotoGaleryDataContext context)
        {
	        new DataBaseInitializer().InitializeDatabase(context);
        }
    }
}
