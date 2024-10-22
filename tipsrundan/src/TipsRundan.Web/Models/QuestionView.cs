using TipsRundan.Application.DataTransferObjects;

namespace TipsRundan.Web.Models;

/// <summary>
/// The view model used for presenting the questions of a quiz and reveiving the user inputs.
/// </summary>
public class QuestionView
{
    public QuestionView() { }
    public QuestionView(QuestionDTO q)
    {
        Inquiry = q.Inquiry;
        Options = q.Options;
    }

    public int Number { get; set; }
    public int LastQuestion { get; set; }
    public int NextQuestion { get; set; }
    public string? Inquiry { get; set; }
    public List<string>? Options { get; set; } = new List<string> { "", "", "" };
    public int Answer { get; set; }
}