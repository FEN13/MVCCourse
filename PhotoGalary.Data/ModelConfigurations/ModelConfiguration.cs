using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	public class ModelConfiguration:EntityTypeConfiguration<Model>
	{
		public ModelConfiguration()
		{
			Property(p => p.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);
            HasRequired(a => a.Manufacturer).WithMany(x => x.Models).Map(x => x.MapKey("Manufacturer_Id"));
		}
	}
}
