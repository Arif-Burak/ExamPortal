// Models/StudentAnswer.cs
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }

        // İlişkili Katılım
        [Required]
        public int ExamParticipationId { get; set; }
        public ExamParticipation ExamParticipation { get; set; }

        // İlişkili Soru
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        // Seçilen Cevap
        [Required]
        public int SelectedAnswerId { get; set; }
        public Answer SelectedAnswer { get; set; }
    }
}
