using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;
using WebRazon.Services;

namespace WebRazon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<object>> Search([FromQuery] string term)
        {
            if (string.IsNullOrEmpty(term) || term.Length < 2)
            {
                return new List<object>();
            }

            var results = _taskService.GetAllTasks()
                .Where(t => (t.Nombre?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                           (t.Descripcion?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false))
                .Take(10)
                .Select(t => new
                {
                    id = t.Id,
                    nombre = t.Nombre,
                    descripcion = t.Descripcion,
                    estado = t.EstaCompletada ? "Completada" : 
                            t.EstaCancelada ? "Cancelada" : 
                            "Activa"
                });

            return Ok(results);
        }
    }
}