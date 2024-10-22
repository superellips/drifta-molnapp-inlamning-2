namespace TipsRundan.Application.Interfaces;

public interface ICryptographyService
{
    bool Validate(string potentialPasswordString, string actualPasswordHash);
    string HashPassword(string passwordString);
}