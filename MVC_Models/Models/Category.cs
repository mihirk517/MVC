using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Length(2,30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [MaxLength(250)]
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
