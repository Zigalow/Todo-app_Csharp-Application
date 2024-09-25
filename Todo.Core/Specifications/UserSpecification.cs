using Todo.Core.Entities;

namespace Todo.Core.Specifications;

public class UserSpecification : ISpecification<User>
{
    public bool IsSatisfiedBy(User item)
    {
        throw new NotImplementedException();
    }

    public ISpecification<User> And(ISpecification<User> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<User> Or(ISpecification<User> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<User> Not()
    {
        throw new NotImplementedException();
    }
}