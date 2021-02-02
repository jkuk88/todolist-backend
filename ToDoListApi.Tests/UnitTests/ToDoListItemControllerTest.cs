using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ToDoListApi.Controllers;
using ToDoListApi.DataAccess.Entities;
using ToDoListApi.DataAccess.Interfaces;
using Xunit;

namespace ToDoListApi.Tests.UnitTests
{
    public class ToDoListItemControllerTest
    {
        [Fact]
        public async Task Get_ByIdShouldWork()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ILogger<ToDoListItemController>>();
                mock.Mock<IToDoListItemRepository>()
                    .Setup(x => x.GetToDoListItemAsync(1))
                    .Returns(GetSamleToDoListItem(1));

                var controller = mock.Create<ToDoListItemController>();
                var expected = await GetSamleToDoListItem(1);

                var actual = controller.Get(1);

                Assert.True(actual.IsCompleted);
                Assert.IsType<OkObjectResult>(actual.Result.Result);
                Assert.IsType<ToDoListItem>(((OkObjectResult)actual.Result.Result).Value);
                ToDoListItem item = (ToDoListItem)((OkObjectResult)actual.Result.Result).Value;
                Assert.Equal(expected.Id, item.Id);
                Assert.Equal(expected.Description, item.Description);
                Assert.Equal(expected.IsCompleted, item.IsCompleted);
                Assert.Equal(expected.IsDeleted, item.IsDeleted);
            }
        }

        [Fact]
        public async Task Get_ByIdShouldFail()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ILogger<ToDoListItemController>>();
                mock.Mock<IToDoListItemRepository>()
                    .Setup(x => x.GetToDoListItemAsync(1))
                    .Throws(new InvalidOperationException());

                var controller = mock.Create<ToDoListItemController>();
                var expected = await GetSamleToDoListItem(1);

                var actual = controller.Get(1);

                Assert.True(actual.IsCompleted);
                Assert.IsType<BadRequestResult>(actual.Result.Result);
            }
        }

        async Task<ToDoListItem> GetSamleToDoListItem(int id)
        {
            var item = new ToDoListItem
            {
                Id = id,
                Description = $"Sample {id}",
                IsCompleted = false,
                IsDeleted = false
            };

            return item;
        }
    }
}
