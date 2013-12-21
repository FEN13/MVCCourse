using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	public class PhotoConfiguration:EntityTypeConfiguration<Photo>
	{
		public PhotoConfiguration()
		{
			Property(p => p.Description).HasColumnType("VARCHAR").HasMaxLength(255);
			Property(p => p.Location).HasColumnType("VARCHAR").HasMaxLength(500);
			Property(p => p.Name).HasColumnType("VARCHAR").HasMaxLength(255);
            HasRequired(a => a.Album).WithMany(x => x.Photos).Map(x => x.MapKey("Album_Id"));
		}
	}
}
