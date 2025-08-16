using System;
using System.ComponentModel.DataAnnotations;

namespace WebRazon.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la tarea es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Completada")]
        public bool EstaCompletada { get; set; }

        [Display(Name = "Cancelada")]
        public bool EstaCancelada { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public TaskItem()
        {
            EstaCompletada = false;
            EstaCancelada = false;
        }
    }
}