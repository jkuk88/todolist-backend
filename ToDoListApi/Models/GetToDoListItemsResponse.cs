using System.Collections.Generic;
using ToDoListApi.DataAccess.Entities;

namespace ToDoListApi.Models
{
    public class GetToDoListItemsResponse
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<ToDoListItem> Items { get; set; }
    }
}
