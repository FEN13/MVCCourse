﻿using System.Collections.Generic;

namespace PhotoGalery.Data.Models
{
	public class Role
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<User> Users { get; set; }

		public Role()
		{
			Users = new HashSet<User>();
		}
	}
}
