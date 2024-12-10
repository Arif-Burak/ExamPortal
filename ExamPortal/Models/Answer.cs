// Models/Answer.cs
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
