using Microsoft.AspNetCore.Identity;

namespace ExamPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Ek kullanıcı özellikleri ekleyebilirsiniz (örneğin, Ad, Soyad vb.)
        public string FullName { get; set; }
    }
}
