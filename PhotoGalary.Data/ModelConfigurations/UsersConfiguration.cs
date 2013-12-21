using System.Data.Entity.ModelConfiguration;
using PhotoGalery.Data.Models;

namespace PhotoGalery.Data.ModelConfigurations
{
	class UsersConfiguration : EntityTypeConfiguration<User>
	{
		public UsersConfiguration()
		{
			Property(p => p.Email).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);
			Property(p => p.FirstName).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);
			Property(p => p.LastName).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);
			Property(p => p.Password).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);
			HasMany(p => p.Albums).WithRequired(up => up.User).WillCascadeOnDelete(true);
			HasMany(u => u.Tokens).WithRequired(up => up.User).WillCascadeOnDelete(true);
		}
	}
}
