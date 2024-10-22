using Microsoft.EntityFrameworkCore;
using TipsRundan.Infrastructure.DataModels;

namespace TipsRundan.Infrastructure.DbContexts;

public class SqliteDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(@"Data Source=/home/erika/quizdb.db");
        }
    }

    public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
        : base(options)
    { 
        Database.EnsureCreated();
    }
    public DbSet<QuizDataModel> Quizes { get; set; }
    public DbSet<QuestionDataModel> Questions { get; set; }
    public DbSet<AnswerDataModel> Answers { get; set; }
    public DbSet<UserDataModel> Users { get; set; }
}