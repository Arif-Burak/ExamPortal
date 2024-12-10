// ViewModels/TakeExamViewModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.ViewModels
{
    public class TakeExamViewModel
    {
        [Required]
        public int ParticipationId { get; set; }

        [Required]
        public int ExamId { get; set; }

        public string ExamTitle { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public int QuestionId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int SelectedAnswerId { get; set; }

        public List<AnswerViewModel> Answers { get; set; }
    }

    public class AnswerViewModel
    {
        public int AnswerId { get; set; }
        public string Text { get; set; }
    }
}
