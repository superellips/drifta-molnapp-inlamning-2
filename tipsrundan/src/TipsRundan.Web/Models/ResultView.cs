namespace TipsRundan.Web.Models;

/// <summary>
/// The view model used for presenting the result after a quiz is finished by the user.
/// </summary>
public class ResultView
{
    public List<string> Questions { get; set; }
    public List<string[]> Options { get; set; }
    public List<int> ChoosenOptions { get; set; }
    public List<int> CorrectOptions { get; set; }
    public int NumberOfQuestions { get; set; }
    public int NumberOfCorrectAnswers { get; set; }
}