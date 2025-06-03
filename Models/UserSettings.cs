using System.ComponentModel.DataAnnotations;

namespace ZametkiApp.Models
{
    public class UserSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Привязка к пользователю (будет через Identity)

        public bool EnableNotifications { get; set; } = true;
        public bool EnableSound { get; set; } = true;
        public bool EnableSnooze { get; set; } = true;
    }
}
