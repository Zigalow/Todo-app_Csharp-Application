namespace Todo.Web.Components.TodoComponent{
    public class TodoList{
        public required string Name { get; set; }
        public List<TodoTask>? Tasks { get; set; }
    }
}