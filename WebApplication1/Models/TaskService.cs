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
                IsCompleted = false
            },
            new TodoTask
            {
                Id = 2,
                Title = "Enviar informe",
                Description = "Preparar y enviar el informe de progreso semanal",
                DueDate = DateTime.Now.AddDays(2),
                IsCompleted = false
            },
            new TodoTask
            {
                Id = 3,
                Title = "Reunión con el equipo",
                Description = "Discutir el progreso y asignar nuevas tareas",
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = true
            }
        };

        public List<TodoTask> GetAllTasks()
        {
            return _tasks;
        }

        public List<TodoTask> GetCompletedTasks()
        {
            return _tasks.Where(t => t.IsCompleted).ToList();
        }

        public List<TodoTask> GetPendingTasks()
        {
            return _tasks.Where(t => !t.IsCompleted).ToList();
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
                existingTask.IsCompleted = task.IsCompleted;
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
                task.IsCompleted = !task.IsCompleted;
            }
        }
    }
}