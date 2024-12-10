// Controllers/AdminController.cs
using ExamPortal.Data;
using ExamPortal.Models;
using ExamPortal.Repositories;
using ExamPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace ExamPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Repository _repository;

        public AdminController(Repository repository)
        {
            _repository = repository;
        }

        // *** Exam CRUD İşlemleri *** //

        // Tüm sınavları listele
        public async Task<IActionResult> Index()
        {
            var exams = await _repository.GetAllExamsAsync();
            return View(exams);
        }

        // Yeni sınav oluşturma sayfasını göster
        public IActionResult Create()
        {
            return View();
        }

        // Yeni sınav ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddExamAsync(exam);
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // Sınav düzenleme sayfasını göster
        public async Task<IActionResult> Edit(int id)
        {
            var exam = await _repository.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // Sınav güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Exam exam)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateExamAsync(exam);
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // Sınav silme sayfasını göster
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _repository.GetExamByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // Sınav silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteExamAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // *** Soru Ekleme İşlemleri *** //

        // Soru ekleme sayfasını göster
        public async Task<IActionResult> AddQuestion(int examId)
        {
            var exam = await _repository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            var viewModel = new AddQuestionViewModel
            {
                ExamId = examId
            };

            return View(viewModel);
        }

        // Soru ekleme işlemini gerçekleştir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion(AddQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Model doğrulaması başarısızsa aynı sayfayı göster
            }

            // 1. Soru ekle
            var question = new Question
            {
                ExamId = model.ExamId,
                Text = model.Text
            };
            await _repository.AddQuestionAsync(question);

            // 2. Cevapları ekle ve SaveChanges çağır
            var answerIds = new List<int>();
            foreach (var answerText in model.Answers)
            {
                var answer = new Answer
                {
                    Text = answerText,
                    QuestionId = question.Id
                };
                await _repository.AddAnswerAsync(answer);
                answerIds.Add(answer.Id);
            }

            // 3. Doğru cevabı belirle (cevapların kesin olarak veritabanında olduğundan emin olundu)
            if (model.CorrectAnswerIndex >= 0 && model.CorrectAnswerIndex < answerIds.Count)
            {
                question.CorrectAnswerId = answerIds[model.CorrectAnswerIndex];
                await _repository.UpdateQuestionAsync(question);
            }
            else
            {
                ModelState.AddModelError("CorrectAnswerIndex", "Geçerli bir doğru cevap seçmelisiniz.");
                return View(model); // Hatalı giriş durumunda aynı formu geri yükle
            }

            // İşlem başarıyla tamamlandıktan sonra
            return RedirectToAction(nameof(Index));
        }






        // *** Katılımcıların Performansını Görüntüleme *** //

        public async Task<IActionResult> ExamParticipants(int examId)
        {
            var exam = await _repository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            var participations = await _repository.GetExamParticipationsAsync(examId);

            var viewModel = participations.Select(p => new ExamParticipantViewModel
            {
                StudentName = p.Student.FullName,
                CorrectAnswers = p.CorrectAnswers,
                IncorrectAnswers = p.IncorrectAnswers
            }).ToList();

            ViewBag.ExamTitle = exam.Title;

            return View(viewModel);
        }
    }
}
