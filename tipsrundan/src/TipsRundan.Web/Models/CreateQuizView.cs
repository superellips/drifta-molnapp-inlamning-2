namespace TipsRundan.Web.Models;

    public class CreateQuizView // för att skapa ett quiz och lägga till frågor och titel och beskrivning.
{
    public string Title { get; set; } // Titel för quizet, get;set; används för att kunna sätta och hämta värden
    public string Description { get; set; } // Beskrivning av quizet
    public List<QuestionView> Questions { get; set; } = new List<QuestionView>(); // Lista över frågor i quizet
    public List<int> CorrectAnswers { get; set; } = new List<int>();
}