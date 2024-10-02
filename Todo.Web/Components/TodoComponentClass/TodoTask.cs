namespace Todo.Web.Components.TodoComponent{
    public class TodoTask {
            public required string Name { get; set; }
            public int? Duration { get; set; }
            public string? TimeStart { get; set; }
            public string? TimeEnd { get; set; }
            public string? StartDate { get; set; }
            public string? EndDate { get; set; }
            public string? Description {get; set;}
    }
}