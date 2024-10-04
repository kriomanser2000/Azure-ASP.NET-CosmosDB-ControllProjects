using ControllProjects.Models;
using Microsoft.AspNetCore.Mvc;
using ControllProjects.Interfaces;
using ControllProjects.Services;

namespace ControllProjects.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        public ProjectsController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }
        // get: Projects
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
        // get: Projects/Details/
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
        // get: Projects/Create
        public IActionResult Create()
        {
            return View();
        }
        // post: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        // get: Projects/Edit/
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
        // post: Projects/Edit/
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
        // get: Projects/Delete/
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
        // post: Projects/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _cosmosDbService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}