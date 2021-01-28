using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ToDoListApi.DataAccess.Entities;
using ToDoListApi.DataAccess.Interfaces;
using ToDoListApi.DataAccess.Model;
using ToDoListApi.Models;

namespace ToDoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListItemController : ControllerBase
    {
        private readonly ILogger<ToDoListItemController> logger;
        private readonly IToDoListItemRepository repository;

        public ToDoListItemController(ILogger<ToDoListItemController> logger, IToDoListItemRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet()]
        public async Task<ActionResult<GetToDoListItemsResponse>> Get(int? pageSize, int? pageNumber, bool? isCompletedFilter, string descriptionFilter)
        {
            try
            {
                ToDoListItemFilter filter = new ToDoListItemFilter(isCompletedFilter, descriptionFilter);
                var items = await repository.GetToDoListItemsAsync(pageSize, pageNumber, filter);

                GetToDoListItemsResponse result = new GetToDoListItemsResponse
                {
                    Items = items.Items,
                    PageCount = items.PageCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while querying ToDoListItems {0} {1} {2} {3}", pageSize, pageNumber, isCompletedFilter, descriptionFilter);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoListItem>> Get(int id)
        {
            try
            {
                var item = await repository.GetToDoListItemAsync(id);
                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogInformation(ex, "ToDoListItem cannot found by id {0}", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while querying a ToDoListItem by ID: {0}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        public async Task<ActionResult<ToDoListItem>> Post([FromBody] CreateUpdateToDoListItemRequest value)
        {
            try
            {
                var item = await repository.CreateToDoListItemAsync(value.Description);
                return Ok(item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while creating a new ToDoListItem: {0}", value);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                await repository.CompleteToDoListItemAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogInformation(ex, "ToDoListItem cannot found by id {0}", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while completing a ToDoListItem: {0}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/uncomplete")]
        public async Task<ActionResult> Uncomplete(int id)
        {
            try
            {
                await repository.UncompleteToDoListItemAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogInformation(ex, "ToDoListItem cannot found by id {0}", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while uncompleting a ToDoListItem: {0}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/update")]
        public async Task<ActionResult> Put(int id, [FromBody] CreateUpdateToDoListItemRequest value)
        {
            try
            {
                await repository.UpdateToDoListItemDescriptionAsync(id, value.Description);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogInformation(ex, "ToDoListItem cannot found by id {0}", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while updating a ToDoListItem: {0} {1}", id, value);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await repository.DeleteToDoListItemAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogInformation(ex, "ToDoListItem cannot found by id {0}", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while deleting a ToDoListItem: {0}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
