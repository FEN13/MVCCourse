using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	class TokensConfiguration : EntityTypeConfiguration<Token>
	{
		public TokensConfiguration()
		{
			Property(p => p.Value).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);
		}
	}
}
