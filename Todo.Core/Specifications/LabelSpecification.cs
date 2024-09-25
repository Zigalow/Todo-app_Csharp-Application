using Todo.Core.Entities;

namespace Todo.Core.Specifications;

public class LabelSpecification : ISpecification<Label>
{
    public bool IsSatisfiedBy(Label item)
    {
        throw new NotImplementedException();
    }

    public ISpecification<Label> And(ISpecification<Label> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<Label> Or(ISpecification<Label> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<Label> Not()
    {
        throw new NotImplementedException();
    }
}