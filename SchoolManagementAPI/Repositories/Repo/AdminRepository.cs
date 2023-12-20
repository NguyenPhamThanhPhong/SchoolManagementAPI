using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<Admin> _adminCollection;
        public AdminRepository(DatabaseConfig databaseConfig)
        {
            _adminCollection = databaseConfig.AdminCollection;
        }
        public Task Create(Admin lecturer)
        {
            return _adminCollection.InsertOneAsync(lecturer);
        }

        public async Task<bool> Delete(string id)
        {
            var deletion = await _adminCollection.DeleteOneAsync(d=>d.ID==id);
            return deletion.DeletedCount > 0;
        }

        public async Task<IEnumerable<Admin>> GetAll()
        {
            return await _adminCollection.Find(_ => true).ToListAsync();
        }

        public Task<Admin> GetbyID(string id)
        {
            return _adminCollection.Find(a=>a.ID==id).FirstOrDefaultAsync();
        }

        public Task<Admin> GetbyUsername(string username)
        {
            return _adminCollection.Find(a => a.Username == username).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatebyInstance(string id, Admin instance)
        {
            var deletion = await _adminCollection.ReplaceOneAsync(a => a.ID == id, instance); 
            return deletion.ModifiedCount > 0;
        }

        public async Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<Admin>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Admin>.Update;
            List<UpdateDefinition<Admin>> subUpdates = new List<UpdateDefinition<Admin>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<Admin>.Update.Set(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<Admin>.Update.Push(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<Admin>.Update.Pull(parameter.fieldName, parameter.value));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            UpdateResult result = await _adminCollection.UpdateOneAsync(filter, combinedUpdate);
            return result.ModifiedCount > 0;
        }
    }
}
