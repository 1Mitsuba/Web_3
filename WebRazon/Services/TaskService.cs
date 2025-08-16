using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;

namespace WebRazon.Services
{
    public class TaskService
    {
        private List<TaskItem> _tasks = new List<TaskItem>();
        private int _nextId = 1;

        public TaskService()
        {
            // Agregar algunas tareas de ejemplo
            AddTask(new TaskItem { 
                Nombre = "Crear documentación del proyecto", 
                Descripcion = "Redactar la documentación técnica para el proyecto de gestión de tareas",
                FechaVencimiento = DateTime.Now.AddDays(7)
            });
            
            AddTask(new TaskItem { 
                Nombre = "Implementar autenticación de usuarios", 
                Descripcion = "Configurar el sistema de login y registro para la aplicación",
                FechaVencimiento = DateTime.Now.AddDays(3)
            });
            
            AddTask(new TaskItem { 
                Nombre = "Diseñar interfaz de usuario", 
                Descripcion = "Crear los mockups y diseño visual de la aplicación",
                FechaVencimiento = DateTime.Now.AddDays(5)
            });
            
            var completedTask = new TaskItem { 
                Nombre = "Configurar entorno de desarrollo", 
                Descripcion = "Instalar herramientas necesarias para el desarrollo",
                FechaVencimiento = DateTime.Now.AddDays(-1)
            };
            completedTask.EstaCompletada = true;
            AddTask(completedTask);
            
            var canceledTask = new TaskItem { 
                Nombre = "Reunión con stakeholders", 
                Descripcion = "Discutir requerimientos del proyecto",
                FechaVencimiento = DateTime.Now.AddDays(-2)
            };
            canceledTask.EstaCancelada = true;
            AddTask(canceledTask);
        }

        public List<TaskItem> GetAllTasks()
        {
            return _tasks.ToList();
        }

        public List<TaskItem> GetActiveTasks()
        {
            return _tasks.Where(t => !t.EstaCompletada && !t.EstaCancelada).ToList();
        }

        public List<TaskItem> GetCompletedTasks()
        {
            return _tasks.Where(t => t.EstaCompletada).ToList();
        }

        public List<TaskItem> GetCanceledTasks()
        {
            return _tasks.Where(t => t.EstaCancelada).ToList();
        }

        public TaskItem GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(TaskItem task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
        }

        public void UpdateTask(TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Nombre = task.Nombre;
                existingTask.Descripcion = task.Descripcion;
                existingTask.FechaVencimiento = task.FechaVencimiento;
                existingTask.EstaCompletada = task.EstaCompletada;
                existingTask.EstaCancelada = task.EstaCancelada;
            }
        }

        public void MarkAsCompleted(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.EstaCompletada = true;
                task.EstaCancelada = false;
            }
        }

        public void MarkAsActive(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.EstaCompletada = false;
                task.EstaCancelada = false;
            }
        }

        public void CancelTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.EstaCancelada = true;
                task.EstaCompletada = false;
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
    }
}