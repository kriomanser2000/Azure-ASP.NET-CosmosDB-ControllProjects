using ControllProjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using ControllProjects.Interfaces;
using ControllProjects.Services;

namespace ControllProjects.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;
        public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            var query = _container.GetItemQueryIterator<Project>(new QueryDefinition("SELECT * FROM c"));
            List<Project> results = new List<Project>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task<Project> GetProjectAsync(string id)
        {
            try
            {
                ItemResponse<Project> response = await _container.ReadItemAsync<Project>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
        public async Task AddProjectAsync(Project project)
        {
            await _container.CreateItemAsync(project, new PartitionKey(project.Id));
        }
        public async Task UpdateProjectAsync(string id, Project project)
        {
            await _container.UpsertItemAsync(project, new PartitionKey(id));
        }
        public async Task DeleteProjectAsync(string id)
        {
            await _container.DeleteItemAsync<Project>(id, new PartitionKey(id));
        }
    }
}