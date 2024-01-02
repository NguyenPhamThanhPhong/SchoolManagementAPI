using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly IMongoCollection<Faculty> _facultyCollection;
        private readonly SortDefinition<Faculty> _facultySort;
        public FacultyRepository(DatabaseConfig databaseConfig)
        {
            _facultyCollection = databaseConfig.FacultyCollection;
            _facultySort = Builders<Faculty>.Sort.Descending(s => s.ID);
        }
        public Task Create(Faculty faculty)
        {
            return _facultyCollection.InsertOneAsync(faculty);
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _facultyCollection.DeleteOneAsync(s=>s.ID==id); 
            return deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Faculty>> GetAll()
        {
            return await _facultyCollection.Find(_ => true).Sort(_facultySort).ToListAsync();
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
