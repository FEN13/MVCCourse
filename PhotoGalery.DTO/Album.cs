namespace PhotoGalery.DTOs
{
	public class Album
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string NewName { get; set; }
		public int CoverId { get; set; }
        public byte[] Cover { get; set; }
	}
}
