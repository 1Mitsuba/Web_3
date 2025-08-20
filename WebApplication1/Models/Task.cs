using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public enum TodoTaskStatus
    {
        Pending,
        Completed,
        Cancelled
    }

    public class TodoTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TodoTaskStatus Status { get; set; } = TodoTaskStatus.Pending;

        public bool IsUrgent { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? CancelledDate { get; set; }

        // Propiedades de compatibilidad con el código existente
        public bool IsCompleted => Status == TodoTaskStatus.Completed;
    }
}