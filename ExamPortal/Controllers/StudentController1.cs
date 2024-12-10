// Controllers/StudentController.cs
using ExamPortal.Data;
using ExamPortal.Models;
using ExamPortal.Repositories;
using ExamPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace ExamPortal.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly Repository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(Repository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        // Sınav listesini göster
        public async Task<IActionResult> Exams()
        {
            var exams = await _repository.GetAllExamsAsync();
            return View(exams);
        }

        // Sınavı başlatma (Katılım oluşturma)
        public async Task<IActionResult> StartExam(int examId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var exam = await _repository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            // Daha önce katılmışsa tekrar katılmasını engellemek isteyebilirsiniz
            var existingParticipation = await _repository.GetExamParticipationAsync(examId, user.Id);
            if (existingParticipation != null)
            {
                return RedirectToAction("TakeExam", new { participationId = existingParticipation.Id });
            }

            var participation = new ExamParticipation
            {
                StudentId = user.Id,
                ExamId = examId,
                CorrectAnswers = 0,
                IncorrectAnswers = 0
            };

            await _repository.AddExamParticipationAsync(participation);

            return RedirectToAction("TakeExam", new { participationId = participation.Id });
        }

        // Sınavı alma formunu göster
        public async Task<IActionResult> TakeExam(int participationId)
        {
            var participation = await _repository.GetExamParticipationByIdAsync(participationId);
            if (participation == null)
            {
                return NotFound("Katılım bulunamadı.");
            }

            var exam = participation.Exam;

            var viewModel = new TakeExamViewModel
            {
                ParticipationId = participationId,
                ExamId = exam.Id,
                ExamTitle = exam.Title,
                Questions = exam.Questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new AnswerViewModel
                    {
                        AnswerId = a.Id,
                        Text = a.Text
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        // Sınavı gönderme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExam(TakeExamViewModel model)
        {
            if (ModelState.IsValid)
            {
                var participation = await _repository.GetExamParticipationByIdAsync(model.ParticipationId);
                if (participation == null)
                {
                    return NotFound("Katılım bulunamadı.");
                }

                foreach (var question in model.Questions)
                {
                    var selectedAnswerId = question.SelectedAnswerId;
                    var selectedAnswer = await _repository.GetAnswerByIdAsync(selectedAnswerId);
                    if (selectedAnswer == null)
                    {
                        continue; // Geçersiz cevap, atla
                    }

                    var correctAnswerId = await _repository.GetCorrectAnswerIdAsync(question.QuestionId);
                    bool isCorrect = selectedAnswerId == correctAnswerId;

                    if (isCorrect)
                    {
                        participation.CorrectAnswers += 1;
                    }
                    else
                    {
                        participation.IncorrectAnswers += 1;
                    }

                    var studentAnswer = new StudentAnswer
                    {
                        ExamParticipationId = model.ParticipationId,
                        QuestionId = question.QuestionId,
                        SelectedAnswerId = selectedAnswerId
                    };

                    await _repository.AddStudentAnswerAsync(studentAnswer);
                }

                await _repository.UpdateExamParticipationAsync(participation);

                return RedirectToAction("ExamResult", new { participationId = model.ParticipationId });
            }

            return View("TakeExam", model);
        }

        // Sınav sonuçlarını göster
        public async Task<IActionResult> ExamResult(int participationId)
        {
            var participation = await _repository.GetExamParticipationByIdAsync(participationId);
            if (participation == null)
            {
                return NotFound("Katılım bulunamadı.");
            }

            var viewModel = new ExamResultViewModel
            {
                ExamTitle = participation.Exam.Title,
                CorrectAnswers = participation.CorrectAnswers,
                IncorrectAnswers = participation.IncorrectAnswers
            };

            return View(viewModel);
        }
    }
}
