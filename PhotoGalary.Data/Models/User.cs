using System;
using System.Collections.Generic;
using DevOne.Security.Cryptography.BCrypt;

namespace PhotoGalery.Data.Models
{
	public class User
	{
		public int Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public bool IsLocked { get; set; }
		public bool IsActivated { get; set; }

		public DateTime LastActiveDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public string CreditCardNumber { get; set; }

		public Role Role { get; set; }
		public ICollection<Token> Tokens { get; set; }
		public ICollection<UserAlbum> Albums { get; set; }

		public User()
		{
			CreatedAt = DateTime.UtcNow;
			LastActiveDate = DateTime.UtcNow;
			Tokens = new HashSet<Token>();
			Albums = new HashSet<UserAlbum>();
		}

		public bool CheckPassword(string password)
		{
			return BCryptHelper.CheckPassword(password, Password);
		}

		public void ChangePassword(string password)
		{
			Password = BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(12));
		}
	}
}
