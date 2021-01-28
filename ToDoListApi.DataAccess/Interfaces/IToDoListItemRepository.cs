using System.Threading.Tasks;
using ToDoListApi.DataAccess.Entities;
using ToDoListApi.DataAccess.Model;

namespace ToDoListApi.DataAccess.Interfaces
{
    public interface IToDoListItemRepository
    {
        Task<ToDoListItem> CreateToDoListItemAsync(string description);
        Task<GetToDoListItemsResult> GetToDoListItemsAsync(int? pageSize, int? pageNumber, ToDoListItemFilter filter);
        Task<ToDoListItem> GetToDoListItemAsync(int id);
        Task CompleteToDoListItemAsync(int id);
        Task UncompleteToDoListItemAsync(int id);
        Task DeleteToDoListItemAsync(int id);
        Task UpdateToDoListItemDescriptionAsync(int id, string description);
    }
}
