using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZametkiApp.Models
{
    public class Note
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsPinned { get; set; } = false;

        public string ReminderText { get; set; }

        public DateTime? Deadline { get; set; }

        public RepeatType Repeat { get; set; } = RepeatType.None;

        // 👇 Добавлено это поле для связи с пользователем
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }

    public enum RepeatType
    {
        [Display(Name = "Без повторения")]
        None,

        [Display(Name = "Ежедневно")]
        Daily,

        [Display(Name = "Еженедельно")]
        Weekly,

        [Display(Name = "Ежемесячно")]
        Monthly
    }
}
