using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marcel.DbModels.Model
{
    [Table("Dish", Schema = "dbo")]
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        public string MenuSectionTitle { get; set; }
        public string DishName { get; set; }
        public string DishDescription { get; set; }
    }
}