using System.ComponentModel.DataAnnotations;

namespace RealStateApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name can't be null or empty.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "ImageUrl can't be null or empty.")]
        public string? ImageUrl { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
