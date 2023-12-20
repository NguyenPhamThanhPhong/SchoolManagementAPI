using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IMongoCollection<Student> _studentCollection;

        public StudentRepository(DatabaseConfig databaseConfigs)
        {
            _studentCollection = databaseConfigs.StudentCollection;
        }

        public async Task Create(Student student)
        {
            await _studentCollection.InsertOneAsync(student);
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _studentCollection.DeleteOneAsync(s => s.ID == id);
            return deleteResult.DeletedCount > 0;
        }

        public Task<Student> GetbyTextFilter(string textFilter)
        {
            var filter = Builders<Student>.Filter.Text(textFilter);
            return _studentCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task<Student> GetbyUsername(string username)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Username,username);
            return _studentCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> GetManyfromIds(List<string> ids)
        {
            var filter = Builders<Student>.Filter.In(s => s.ID,ids);
            return await _studentCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetManyRange(int start, int end)
        {
            return await _studentCollection.Find(_ => true).Skip(start).Limit(end - start).ToListAsync();
        }

        public async Task<bool> UpdatebyInstance(string id, Student instance)
        {
             var deleteResult = await _studentCollection.ReplaceOneAsync(s => s.ID == id, instance);
            return deleteResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<Student>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Student>.Update;
            List<UpdateDefinition<Student>> subUpdates = new List<UpdateDefinition<Student>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<Student>.Update.Set(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<Student>.Update.Push(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<Student>.Update.Pull(parameter.fieldName, parameter.value));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);

            UpdateResult result = await _studentCollection.UpdateOneAsync(filter, combinedUpdate);
            return result.ModifiedCount > 0;
        }
    }
}
