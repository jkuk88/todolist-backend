using Microsoft.EntityFrameworkCore;
using ToDoListApi.DataAccess.Entities;

namespace ToDoListApi.DataAccess.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ToDoListItem> ToDoListItems { get; set; }
    }
}
