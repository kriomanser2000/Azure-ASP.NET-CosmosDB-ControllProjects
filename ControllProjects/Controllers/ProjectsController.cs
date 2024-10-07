using ControllProjects.Models;
using Microsoft.AspNetCore.Mvc;
using ControllProjects.Interfaces;
using ControllProjects.Services;
using Microsoft.Extensions.Logging;

namespace ControllProjects.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(ICosmosDbService cosmosDbService, ILogger<ProjectsController> logger)
        {
            _cosmosDbService = cosmosDbService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var projects = await _cosmosDbService.GetProjectsAsync();

            if (projects == null || !projects.Any()) 
            {
                ViewBag.Message = "Проекти не знайдені.";
                projects = new List<Project>();
            }
            return View(projects);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _cosmosDbService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid().ToString();
                _logger.LogInformation("Project Id: {ProjectId}", project.Id);
                await _cosmosDbService.AddProjectAsync(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _cosmosDbService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,StartDate,EndDate")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _cosmosDbService.UpdateProjectAsync(id, project);
                }
                catch (Exception)
                {
                    if (await _cosmosDbService.GetProjectAsync(project.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await _cosmosDbService.GetProjectAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _cosmosDbService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}