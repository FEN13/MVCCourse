using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;

namespace PhotoGalery.Data.Models
{
	public class Token
	{
		public const int TokenLength = 8;

		public int Id { get; set; }
		public string Value { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ValidTo { get; set; }
		public bool IsUsed { get; set; }

		public virtual User User { get; set; }

		public bool IsValid()
		{
			return !IsUsed && DateTime.UtcNow < ValidTo;
		}

		public static Token Generate()
		{
			var token = new Token
			{
				CreatedAt = DateTime.UtcNow,
				ValidTo = DateTime.UtcNow.AddMonths(1),
				Value = GetTokenValue(),
				IsUsed = false
			};

			return token;
		}

		public static Token Generate(bool shortLived)
		{
			var token = Generate();

			if (shortLived)
			{
				token.ValidTo = token.CreatedAt.AddDays(1);
			}

			return token;
		}

		private static string GetTokenValue()
		{
			var data = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
			var hashData = new SHA1Managed().ComputeHash(data);
			var hash = new SoapHexBinary(new ArraySegment<byte>(hashData, 0, TokenLength).Array).ToString();
			return hash.ToLowerInvariant();
		}
	}
}
