using System.Collections.Generic;

namespace PhotoGalery.DTOs
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Model> Models { get; set; }
    }
}
