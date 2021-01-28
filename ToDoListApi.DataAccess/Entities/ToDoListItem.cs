using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.DataAccess.Entities
{
    public class ToDoListItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }

    }
}
