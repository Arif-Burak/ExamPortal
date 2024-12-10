// Models/Question.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Soru metni zorunludur.")]
        public string Text { get; set; }

        [Required]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        // CorrectAnswerId'yi nullable yapıyoruz
        public int? CorrectAnswerId { get; set; }
        public Answer CorrectAnswer { get; set; }

        // Öğrencilerin cevapları
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
