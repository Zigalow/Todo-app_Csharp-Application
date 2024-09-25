using Todo.Core.Entities;
using Todo.Core.Specifications;

namespace Todo.Core.Calendar;

public static class CalendarManager
{
    public static List<TodoItem> GetItemForDate(DateTime time, List<TodoItem> items)
    {
        return items.Where(td => td.DueDate == time).ToList();
    }

    public static List<TodoItem> GetItemForDateRange(DateTime start, DateTime end, List<TodoItem> items)
    {
        return items.Where(td => td.DueDate >= start && td.DueDate <= end).ToList();
    }

    public static List<TodoItem> FilterItems(ISpecification<TodoItem> specification)
    {
        throw new NotImplementedException();
    }
}