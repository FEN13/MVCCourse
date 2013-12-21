using System;

namespace PhotoGalery.DTOs
{
    public class AdvancedSearch
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int DeviceId { get; set; }
        public int Model { get; set; }
        public int FocusDistance { get; set; }
        public int Diafragm { get; set; }
        public int ShutterSpeed { get; set; }
        public int ISO { get; set; }
        public bool IsUsedFlash { get; set; }
    }
}
