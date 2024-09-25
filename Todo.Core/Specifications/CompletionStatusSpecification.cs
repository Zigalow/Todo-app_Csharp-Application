using Todo.Core.Entities;

namespace Todo.Core.Specifications;

public class CompletionStatusSpecification : ISpecification<TodoItem>
{
    public bool IsSatisfiedBy(TodoItem item)
    {
        throw new NotImplementedException();
    }

    public ISpecification<TodoItem> And(ISpecification<TodoItem> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<TodoItem> Or(ISpecification<TodoItem> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<TodoItem> Not()
    {
        throw new NotImplementedException();
    }
}