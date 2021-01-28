namespace ToDoListApi.DataAccess.Model
{
    public class ToDoListItemFilter
    {
        public bool? IsCompleted { get; set; }
        public string Description { get; set; }

        public ToDoListItemFilter(bool? isCompleted, string description)
        {
            IsCompleted = isCompleted;
            Description = description;
        }

        public ToDoListItemFilter()
        {

        }
    }
}
