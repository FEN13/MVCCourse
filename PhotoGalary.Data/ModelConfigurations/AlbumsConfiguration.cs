using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	public class AlbumsConfiguration:EntityTypeConfiguration<Album>
	{
		public AlbumsConfiguration()
		{
			Property(p => p.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);
			HasMany(p=>p.Photos).WithRequired().WillCascadeOnDelete(true);
		}
	}
}
