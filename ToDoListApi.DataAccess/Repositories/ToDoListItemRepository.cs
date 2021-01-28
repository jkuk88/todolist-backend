using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApi.DataAccess.DataAccess;
using ToDoListApi.DataAccess.Entities;
using ToDoListApi.DataAccess.Interfaces;
using ToDoListApi.DataAccess.Model;

namespace ToDoListApi.DataAccess.Repositories
{
    public class ToDoListItemRepository : IToDoListItemRepository
    {
        private readonly ApplicationDbContext context;

        public ToDoListItemRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CompleteToDoListItemAsync(int id)
        {
            var item = await GetToDoListItemAsync(id);
            item.IsCompleted = true;
            await context.SaveChangesAsync();
        }

        public async Task UncompleteToDoListItemAsync(int id)
        {
            var item = await GetToDoListItemAsync(id);
            item.IsCompleted = false;
            await context.SaveChangesAsync();
        }

        public async Task<ToDoListItem> CreateToDoListItemAsync(string description)
        {
            var item = new ToDoListItem
            {
                Description = description,
                IsCompleted = false,
                IsDeleted = false
            };

            await context.ToDoListItems.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteToDoListItemAsync(int id)
        {
            var item = await GetToDoListItemAsync(id);
            item.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<ToDoListItem> GetToDoListItemAsync(int id)
        {
            var item = await context.ToDoListItems.SingleAsync(i => i.Id == id && !i.IsDeleted);
            return item;
        }

        public async Task<GetToDoListItemsResult> GetToDoListItemsAsync(int? pageSize, int? pageNumber, ToDoListItemFilter filter)
        {
            GetToDoListItemsResult result = new GetToDoListItemsResult();

            IQueryable<ToDoListItem> items = context.ToDoListItems.Where(i => !i.IsDeleted);

            if (filter != null && filter.IsCompleted.HasValue)
            {
                items = items.Where(i => i.IsCompleted == filter.IsCompleted.Value);
            }
            if (filter != null && !string.IsNullOrEmpty(filter.Description))
            {
                items = items.Where(i => i.Description.ToLower().Contains(filter.Description.ToLower()));
            }

            items = items.OrderBy(i => i.Id);

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                int totalRowNumber = await items.CountAsync();
                result.PageCount = (int)Math.Ceiling((decimal)totalRowNumber / pageSize.Value);
                items = items.Skip(pageSize.Value * pageNumber.Value).Take(pageSize.Value);
            }
            else
            {
                result.PageCount = 1;
            }

            result.Items = await items.ToArrayAsync();
            return result;
        }

        public async Task UpdateToDoListItemDescriptionAsync(int id, string description)
        {
            var item = await GetToDoListItemAsync(id);
            item.Description = description;
            await context.SaveChangesAsync();
        }
    }
}
