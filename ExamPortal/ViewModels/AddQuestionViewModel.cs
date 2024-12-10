using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamPortal.ViewModels
{
    public class AddQuestionViewModel
    {
        [Required]
        public int ExamId { get; set; }

        [Required(ErrorMessage = "Soru metni zorunludur.")]
        [Display(Name = "Soru Metni")]
        public string Text { get; set; }

        [Required(ErrorMessage = "En az dört cevap girmelisiniz.")]
        [MinLength(4, ErrorMessage = "En az dört cevap girmelisiniz.")]
        public List<string> Answers { get; set; } = new List<string> { "", "", "", "" }; // Varsayılan 4 boş string

        [Required(ErrorMessage = "Doğru cevabı seçmelisiniz.")]
        [Range(0, 3, ErrorMessage = "Doğru cevap 1 ile 4 arasında olmalıdır.")]
        public int CorrectAnswerIndex { get; set; }
    }
}
