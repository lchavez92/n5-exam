namespace N5.Exam.Shared.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
