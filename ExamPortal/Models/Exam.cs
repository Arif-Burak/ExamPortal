// Models/Exam.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Description { get; set; }

        // Soru ilişkisi
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        // Katılımcı ilişkisi
        public ICollection<ExamParticipation> ExamParticipations { get; set; } = new List<ExamParticipation>();
    }
}
