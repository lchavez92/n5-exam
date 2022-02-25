using N5.Exam.Shared.DbFactories;

namespace N5.Exam.Infrastructure
{
    public class N5ExamContextFactory : BaseContextFactory<N5ExamContext>
    {
        public N5ExamContextFactory() : base("DefaultConnection") { }
    }
}
