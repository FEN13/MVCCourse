using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	public class RolesConfiguration: EntityTypeConfiguration<Role>
	{
		public RolesConfiguration()
		{
			Property(p => p.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(250);
		}
		  
	}
}
