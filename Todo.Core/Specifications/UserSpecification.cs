using Todo.Core.Entities;

namespace Todo.Core.Specifications;

public class UserSpecification : ISpecification<ApplicationUser>
{
    public bool IsSatisfiedBy(ApplicationUser item)
    {
        throw new NotImplementedException();
    }

    public ISpecification<ApplicationUser> And(ISpecification<ApplicationUser> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<ApplicationUser> Or(ISpecification<ApplicationUser> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<ApplicationUser> Not()
    {
        throw new NotImplementedException();
    }
}