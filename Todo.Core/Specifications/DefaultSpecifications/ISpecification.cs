namespace Todo.Core.Specifications;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T item);
    
    ISpecification<T> And(ISpecification<T> specification);
    ISpecification<T> Or(ISpecification<T> specification);
    ISpecification<T> Not();
}