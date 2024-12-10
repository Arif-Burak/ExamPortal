// Data/ApplicationDbContext.cs
using ExamPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Mevcut DbSet'leriniz
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExamParticipation> ExamParticipations { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exam ve Question ilişkisi
            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Question ve Answer ilişkisi
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            // ExamParticipation ve ApplicationUser ilişkisi
            modelBuilder.Entity<ExamParticipation>()
                .HasOne(ep => ep.Student)
                .WithMany()
                .HasForeignKey(ep => ep.StudentId)
                .OnDelete(DeleteBehavior.Restrict); 

            // ExamParticipation ve Exam ilişkisi
            modelBuilder.Entity<ExamParticipation>()
                .HasOne(ep => ep.Exam)
                .WithMany(e => e.ExamParticipations)
                .HasForeignKey(ep => ep.ExamId)
                .OnDelete(DeleteBehavior.Restrict); 

            // StudentAnswer ve ExamParticipation ilişkisi
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.ExamParticipation)
                .WithMany(ep => ep.StudentAnswers)
                .HasForeignKey(sa => sa.ExamParticipationId)
                .OnDelete(DeleteBehavior.Restrict); 

            // StudentAnswer ve Question ilişkisi
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            // StudentAnswer ve Answer ilişkisi
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.SelectedAnswer)
                .WithMany()
                .HasForeignKey(sa => sa.SelectedAnswerId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
