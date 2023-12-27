using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly IMongoCollection<Faculty> _facultyCollection;
        public FacultyRepository(DatabaseConfig databaseConfig)
        {
            _facultyCollection = databaseConfig.FacultyCollection;
        }
        public Task Create(Faculty faculty)
        {
            return _facultyCollection.InsertOneAsync(faculty);
        }

        public Task Delete(string id)
        {
            return _facultyCollection.DeleteOneAsync(id);
        }

        public async Task<IEnumerable<Faculty>> GetAll()
        {
            return await _facultyCollection.Find(_ => true).ToListAsync(); 
        }

        public  Task<Faculty> GetOne(string id)
        {
            return  _facultyCollection.Find(s => s.ID == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatebyInstance(Faculty faculty)
        {
            var result = await _facultyCollection.ReplaceOneAsync(f=>f.ID==faculty.ID, faculty);
            return result.ModifiedCount > 0;
        }
    }
}
