// Models/ExamParticipation.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.Models
{
    public class ExamParticipation
    {
        public int Id { get; set; }

        // İlişkili Kullanıcı (Student)
        [Required]
        public string StudentId { get; set; } // ApplicationUser'ın Id'si string olduğu için
        public ApplicationUser Student { get; set; }

        // İlişkili Sınav
        [Required]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        // Doğru ve Yanlış cevap sayıları
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }

        // Öğrencinin verdiği cevaplar
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
