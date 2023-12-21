using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class SchoolClassRepository : ISchoolClassRepository
    {
        private readonly IMongoCollection<SchoolClass> _schoolClassCollection;

        public SchoolClassRepository(IMongoCollection<SchoolClass> schoolClasses)
        {
            _schoolClassCollection = schoolClasses;
        }

        public Task Create(SchoolClass schoolClass)
        {
            return _schoolClassCollection.InsertOneAsync(schoolClass);
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SchoolClass?> GetbyTextFilter(string textFilter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SchoolClass>> GetfromIds(List<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SchoolClass>> GetManyRange(int start, int end)
        {
            throw new NotImplementedException();
        }

        public Task<SchoolClass?> GetSingle(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatebyInstance(SchoolClass schoolClass)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateByParameters(string id,IEnumerable<UpdateParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
