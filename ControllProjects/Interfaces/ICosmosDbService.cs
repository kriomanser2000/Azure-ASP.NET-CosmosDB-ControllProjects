using ControllProjects.Models;

namespace ControllProjects.Interfaces
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectAsync(string id);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(string id, Project project);
        Task DeleteProjectAsync(string id);
    }
}
