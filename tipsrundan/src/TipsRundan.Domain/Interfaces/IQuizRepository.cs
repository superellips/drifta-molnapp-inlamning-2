using TipsRundan.Domain.Entities;

namespace TipsRundan.Domain.Interfaces;

public interface IQuizRepository
{
    public Quiz Create(Quiz quiz);
    public Quiz ReadById(Guid id);
    public List<Quiz> ReadAll();
    public Quiz Update(Quiz quiz);
    public Quiz Delete(Quiz quiz);
}