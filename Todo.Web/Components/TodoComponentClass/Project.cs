namespace Todo.Web.Components.TodoComponent
{
    public class Project{
        public required int Id { get; set; }
        public required string Name { get; set; }
        public List<TodoList>? TodoLists { get; set; }
    }
}