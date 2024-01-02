using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly IMongoCollection<Semester> _semesterCollection;
        private readonly SortDefinition<Semester> _sortSemester;
        public SemesterRepository(DatabaseConfig databaseConfig)
        {
            _semesterCollection = databaseConfig.SemesterCollection;
            _sortSemester = Builders<Semester>.Sort.Descending(s => s.ID);
        }
        public Task Create(Semester semester)
        {
            return _semesterCollection.InsertOneAsync(semester);
        }

        public Task Delete(string id)
        {
            return _semesterCollection.DeleteOneAsync(s=>s.ID == id);
        }

        public async Task<IEnumerable<Semester>> GetAll()
        {
            var sort = Builders<Semester>.Sort.Ascending(s => s.ID);
            return await _semesterCollection.Find(_ => true).Sort(sort).ToListAsync();
        }

        public Task<Semester> GetOne(string id)
        {
            return _semesterCollection.Find(s => s.ID == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatebyInstance(Semester semester)
        {
            var result = await _semesterCollection.ReplaceOneAsync(s => s.ID == semester.ID, semester);
            return result.ModifiedCount > 0;
        }
    }
}
