namespace Todo.Core.Specifications;

public class DateRangeSpecification : ISpecification<DateTime>
{
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    public bool IsSatisfiedBy(DateTime item)
    {
        throw new NotImplementedException();
    }

    public ISpecification<DateTime> And(ISpecification<DateTime> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<DateTime> Or(ISpecification<DateTime> specification)
    {
        throw new NotImplementedException();
    }

    public ISpecification<DateTime> Not()
    {
        throw new NotImplementedException();
    }
}