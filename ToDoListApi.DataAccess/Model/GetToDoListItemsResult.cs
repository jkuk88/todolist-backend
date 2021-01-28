using System.Collections.Generic;
using ToDoListApi.DataAccess.Entities;

namespace ToDoListApi.DataAccess.Model
{
    public class GetToDoListItemsResult
    {
        public int PageCount { get; set; }
        public IEnumerable<ToDoListItem> Items { get; set; }
    }
}
