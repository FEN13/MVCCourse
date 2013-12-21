namespace PhotoGalery.DTOs
{
    public class PhotoMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int FocusDistance { get; set; }
        public int Diafragm { get; set; }
        public float ShutterSpeed { get; set; }
        public int ISO { get; set; }
        public bool IsFlashUsed { get; set; }
        public string Description { get; set; }
        public int DeviceManufacturer { get; set; }
        public int DeviceModel { get; set; }
        public string AlbumName { get; set; }
        public bool IsCover { get; set; }
    }
}
