using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	public class ManufacturerConfiguration:EntityTypeConfiguration<Manufacturer>
	{
		public ManufacturerConfiguration()
		{
			Property(p => p.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);
			HasMany(p=>p.Models).WithRequired().WillCascadeOnDelete(true);
		}
	}
}
