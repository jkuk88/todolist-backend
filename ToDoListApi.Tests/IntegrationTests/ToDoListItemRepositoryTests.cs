using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApi.DataAccess.DataAccess;
using ToDoListApi.DataAccess.Repositories;
using Xunit;

namespace ToDoListApi.Tests.IntegrationTests
{
    public class ToDoListItemRepositoryTests
    {
        [Fact]
        public async Task Create()
        {
            string dbFileName = GetTestDbFileName();
            var context = GetTestContext(dbFileName);

            ToDoListItemRepository repository = new ToDoListItemRepository(context);

            var createResult = await repository.CreateToDoListItemAsync("This is a test description");
            
            Assert.Equal(1, createResult.Id);
            Assert.Equal("This is a test description", createResult.Description);
            Assert.False(createResult.IsCompleted);
            Assert.False(createResult.IsDeleted);

            DeleteTestDbFile(dbFileName);
        }

        [Fact]
        public async Task Create_Complete_Get()
        {
            string dbFileName = GetTestDbFileName();
            var context = GetTestContext(dbFileName);
            ToDoListItemRepository repository = new ToDoListItemRepository(context);

            var createResult = await repository.CreateToDoListItemAsync("This is a test description");
            await repository.CompleteToDoListItemAsync(1);
            var item = await repository.GetToDoListItemAsync(1);

            Assert.Equal(1, createResult.Id);
            Assert.Equal("This is a test description", createResult.Description);
            Assert.True(createResult.IsCompleted);
            Assert.False(createResult.IsDeleted);

            DeleteTestDbFile(dbFileName);
        }

        private string GetTestDbFileName()
        {
            return $"todolist_test_{Guid.NewGuid()}.db";
        }

        private ApplicationDbContext GetTestContext(string fileName)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlite($"Data Source={fileName}");
            ApplicationDbContext context = new ApplicationDbContext(builder.Options);
            context.Database.Migrate();

            return context;
        }

        private void DeleteTestDbFile(string fileName)
        {
            File.Delete(fileName);
        }
    }
}
