using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models
{
    public class TaskService
    {
        private static List<TodoTask> _tasks = new List<TodoTask>
        {
            new TodoTask
            {
                Id = 1,
                Title = "Completar proyecto de desarrollo web",
                Description = "Finalizar el proyecto de la aplicación de tareas para la asignatura Web III",
                DueDate = DateTime.Now.AddDays(7),
                Status = TodoTaskStatus.Pending,
                IsUrgent = true
            },
            new TodoTask
            {
                Id = 2,
                Title = "Enviar informe",
                Description = "Preparar y enviar el informe de progreso semanal",
                DueDate = DateTime.Now.AddDays(2),
                Status = TodoTaskStatus.Pending,
                IsUrgent = false
            },
            new TodoTask
            {
                Id = 3,
                Title = "Reunión con el equipo",
                Description = "Discutir el progreso y asignar nuevas tareas",
                DueDate = DateTime.Now.AddDays(1),
                Status = TodoTaskStatus.Completed,
                CompletedDate = DateTime.Now.AddDays(-1),
                IsUrgent = false
            },
            new TodoTask
            {
                Id = 4,
                Title = "Actualización de software",
                Description = "Instalar últimas actualizaciones de software en los servidores",
                DueDate = DateTime.Now.AddDays(-5),
                Status = TodoTaskStatus.Cancelled,
                CancelledDate = DateTime.Now.AddDays(-2),
                IsUrgent = false
            },
            new TodoTask
            {
                Id = 5,
                Title = "Revisión de código",
                Description = "Realizar revisión de código del módulo de autenticación",
                DueDate = DateTime.Now.AddDays(-3),
                Status = TodoTaskStatus.Cancelled,
                CancelledDate = DateTime.Now.AddDays(-1),
                IsUrgent = true
            },
            new TodoTask
            {
                Id = 6,
                Title = "Presentación del proyecto",
                Description = "Preparar presentación para mostrar avances del proyecto al cliente",
                DueDate = DateTime.Now.AddDays(3),
                Status = TodoTaskStatus.Pending,
                IsUrgent = true
            },
            new TodoTask
            {
                Id = 7,
                Title = "Actualizar documentación",
                Description = "Revisar y actualizar la documentación técnica del sistema",
                DueDate = DateTime.Now.AddDays(5),
                Status = TodoTaskStatus.Pending,
                IsUrgent = false
            }
        };

        public List<TodoTask> GetAllTasks()
        {
            return _tasks;
        }

        public List<TodoTask> GetCompletedTasks()
        {
            return _tasks.Where(t => t.Status == TodoTaskStatus.Completed).ToList();
        }

        public List<TodoTask> GetPendingTasks()
        {
            return _tasks.Where(t => t.Status == TodoTaskStatus.Pending).ToList();
        }

        public List<TodoTask> GetCancelledTasks()
        {
            return _tasks.Where(t => t.Status == TodoTaskStatus.Cancelled).ToList();
        }

        public List<TodoTask> GetUrgentTasks()
        {
            return _tasks.Where(t => t.IsUrgent && t.Status == TodoTaskStatus.Pending).ToList();
        }

        public List<TodoTask> GetTasksDueToday()
        {
            var today = DateTime.Today;
            return _tasks.Where(t => t.Status == TodoTaskStatus.Pending && 
                                     t.DueDate.HasValue && 
                                     t.DueDate.Value.Date == today).ToList();
        }

        public List<TodoTask> GetTasksDueSoon(int days = 3)
        {
            var today = DateTime.Today;
            var cutoffDate = today.AddDays(days);
            return _tasks.Where(t => t.Status == TodoTaskStatus.Pending && 
                                     t.DueDate.HasValue && 
                                     t.DueDate.Value.Date > today &&
                                     t.DueDate.Value.Date <= cutoffDate).ToList();
        }

        public List<TodoTask> GetOverdueTasks()
        {
            var today = DateTime.Today;
            return _tasks.Where(t => t.Status == TodoTaskStatus.Pending && 
                                     t.DueDate.HasValue && 
                                     t.DueDate.Value.Date < today).ToList();
        }

        public TodoTask GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(TodoTask task)
        {
            task.Id = _tasks.Any() ? _tasks.Max(t => t.Id) + 1 : 1;
            _tasks.Add(task);
        }

        public void UpdateTask(TodoTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Status = task.Status;
                existingTask.IsUrgent = task.IsUrgent;
                
                // Actualizar fechas según corresponda
                existingTask.CompletedDate = task.CompletedDate;
                existingTask.CancelledDate = task.CancelledDate;
            }
        }

        public void DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }

        public void ToggleTaskCompletion(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                if (task.Status == TodoTaskStatus.Completed)
                {
                    task.Status = TodoTaskStatus.Pending;
                    task.CompletedDate = null;
                }
                else if (task.Status == TodoTaskStatus.Pending)
                {
                    task.Status = TodoTaskStatus.Completed;
                    task.CompletedDate = DateTime.Now;
                }
            }
        }
        
        public void CancelTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null && task.Status != TodoTaskStatus.Cancelled)
            {
                task.Status = TodoTaskStatus.Cancelled;
                task.CancelledDate = DateTime.Now;
            }
        }

        public void ToggleTaskUrgent(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsUrgent = !task.IsUrgent;
            }
        }

        public Dictionary<string, int> GetTaskStatistics()
        {
            return new Dictionary<string, int>
            {
                { "total", _tasks.Count },
                { "pending", GetPendingTasks().Count },
                { "completed", GetCompletedTasks().Count },
                { "cancelled", GetCancelledTasks().Count },
                { "urgent", GetUrgentTasks().Count },
                { "dueToday", GetTasksDueToday().Count },
                { "dueSoon", GetTasksDueSoon().Count },
                { "overdue", GetOverdueTasks().Count }
            };
        }

        public void ReactivateTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null && task.Status == TodoTaskStatus.Cancelled)
            {
                task.Status = TodoTaskStatus.Pending;
                task.CancelledDate = null;
            }
        }
    }
}