// Repositories/Repository.cs
using ExamPortal.Data;
using ExamPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamPortal.Repositories
{
    public class Repository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        // *** Exam CRUD İşlemleri *** //

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            return await _context.Exams
                                 .Include(e => e.Questions)
                                     .ThenInclude(q => q.Answers)
                                 .ToListAsync();
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            return await _context.Exams
                                 .Include(e => e.Questions)
                                     .ThenInclude(q => q.Answers)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddExamAsync(Exam exam)
        {
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExamAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Question>> GetQuestionsByExamIdAsync(int examId)
        {
            return await _context.Questions
                                 .Include(q => q.Answers)
                                 .Where(q => q.ExamId == examId)
                                 .ToListAsync();
        }

        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _context.Answers
                                 .Where(a => a.QuestionId == questionId)
                                 .ToListAsync();
        }

        public async Task AddAnswerAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnswerAsync(Answer answer)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
        }

        // *** ExamParticipation İşlemleri *** //

        public async Task<ExamParticipation> AddExamParticipationAsync(ExamParticipation participation)
        {
            _context.ExamParticipations.Add(participation);
            await _context.SaveChangesAsync();
            return participation;
        }

        public async Task<ExamParticipation> GetExamParticipationByIdAsync(int id)
        {
            return await _context.ExamParticipations
                                 .Include(ep => ep.Exam)
                                 .Include(ep => ep.Student)
                                 .Include(ep => ep.StudentAnswers)
                                     .ThenInclude(sa => sa.SelectedAnswer)
                                 .FirstOrDefaultAsync(ep => ep.Id == id);
        }

        public async Task<List<ExamParticipation>> GetExamParticipationsAsync(int examId)
        {
            return await _context.ExamParticipations
                                 .Include(ep => ep.Student)
                                 .Include(ep => ep.StudentAnswers)
                                     .ThenInclude(sa => sa.SelectedAnswer)
                                 .Where(ep => ep.ExamId == examId)
                                 .ToListAsync();
        }

        public async Task AddStudentAnswerAsync(StudentAnswer studentAnswer)
        {
            _context.StudentAnswers.Add(studentAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetCorrectAnswerIdAsync(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            return question?.CorrectAnswerId;
        }
        // Repositories/Repository.cs

        public async Task<ExamParticipation> GetExamParticipationAsync(int examId, string studentId)
        {
            return await _context.ExamParticipations
                                 .FirstOrDefaultAsync(ep => ep.ExamId == examId && ep.StudentId == studentId);
        }
        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _context.Answers
                                 .FirstOrDefaultAsync(a => a.Id == id);

        }
        public async Task UpdateExamParticipationAsync(ExamParticipation participation)
        {
            _context.ExamParticipations.Update(participation);
            await _context.SaveChangesAsync();
        }

        // Transaction Başlatma (Opsiyonel)
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
