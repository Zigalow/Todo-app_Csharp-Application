using Todo.Core.Entities;
using Todo.Core.Specifications.DefaultSpecifications;

namespace Todo.Core.Specifications;

public class PrioritySpecification : ISpecification<Priority>
{
    public bool IsSatisfiedBy(Priority item)
    {
        throw new NotImplementedException();
    }

    public ISpecification<Priority> And(ISpecification<Priority> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<Priority> Or(ISpecification<Priority> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<Priority> Not()
    {
        throw new NotImplementedException();
    }
}