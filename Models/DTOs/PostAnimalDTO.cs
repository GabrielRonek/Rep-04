using System.ComponentModel.DataAnnotations;

namespace Cw3.Models.DTOs
{
    public class PostAnimalDTO
    {
        [Required]
        public int IDAnimal { get; set; }
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Description {  get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Area {  get; set; } = string.Empty;
    }
}
