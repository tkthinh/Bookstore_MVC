using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
  public class Category
   {
      [Key]
      public int Id { get; set; }
      [Required]
      [MaxLength(30)]
      [Display(Name = "Category Name")]
      public string Name { get; set; }
      [Display(Name = "Display Order")]
      [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100 only!")]
      public int DisplayOrder { get; set; }
   }
}
