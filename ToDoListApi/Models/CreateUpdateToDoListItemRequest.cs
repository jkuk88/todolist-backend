using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Models
{
    public class CreateUpdateToDoListItemRequest
    {
        [Required]
        [MinLength(1)]
        public string Description { get; set; }
    }
}
