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

        public async Task<Student?> Delete(string id)
        {
            var deleteResult = await _studentCollection.FindOneAndDeleteAsync(id);
            return deleteResult;
        }

        public async Task<IEnumerable<Student>> GetbyTextFilter(string textFilter)
        {
            var filter = Builders<Student>.Filter.Text(textFilter);
            return await _studentCollection.Find(filter).ToListAsync();
        }

        public async Task<Student?> GetbyUsername(string username)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Username, username);
            return await _studentCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> GetManyfromIds(List<string> ids)
        {
            var filter = Builders<Student>.Filter.In(s => s.ID, ids);
            return await _studentCollection.Find(filter).ToListAsync();
        }

        public  async Task<IEnumerable<Student>> GetManyRange(int start, int end)
        {
            var sort = Builders<Student>.Sort.Descending(s => s.ID);
            return await _studentCollection.Find(_ => true).Skip(start).Sort(sort).Limit(end - start).ToListAsync();
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

        public async Task<bool> UpdateStringFields(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<Student>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Student>.Update;
            List<UpdateDefinition<Student>> subUpdates = new List<UpdateDefinition<Student>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<Student>.Update.Set(parameter.fieldName, parameter.value.ToString()));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<Student>.Update.Push(parameter.fieldName, parameter.value.ToString()));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<Student>.Update.Pull(parameter.fieldName, parameter.value.ToString()));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);

            UpdateResult result = await _studentCollection.UpdateOneAsync(filter, combinedUpdate);
            return result.ModifiedCount > 0;
        }
    }
}
