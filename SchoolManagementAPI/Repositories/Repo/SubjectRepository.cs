using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Configs;
using System.Text.Json;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IMongoCollection<Subject> _subjectCollection;
        private readonly IMongoCollection<Faculty> _facultyCollection;

        public SubjectRepository(DatabaseConfig databaseConfig)
        {
            _subjectCollection = databaseConfig.SubjectCollection;
            _facultyCollection = databaseConfig.FacultyCollection;
        }

        public async Task Create(Subject subject)
        {
            await _subjectCollection.InsertOneAsync(subject);
            var filter = Builders<Faculty>.Filter.Eq(f => f.ID, subject.FacultyId);
            var update = Builders<Faculty>.Update.Push(f => f.SubjectIds, subject.ID);
            await _facultyCollection.UpdateOneAsync(filter, update);
        }

        public Task DeleteMany(IEnumerable<string> ids)
        {
            var filter = Builders<Subject>.Filter.In(s=>s.ID,ids);
            return _subjectCollection.DeleteManyAsync(filter);
        }

        public Task DeleteOne(string id)
        {
            var filter = Builders<Subject>.Filter.Eq(s=>s.ID,id);
            return _subjectCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Subject>> GetFromIds(IEnumerable<string> ids)
        {
            var filter = Builders<Subject>.Filter.In(s => s.ID, ids);
            return await _subjectCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetManyRange(int start, int end)
        {
            return await _subjectCollection.Find(_ => true).Skip(start).Limit(start-end).ToListAsync();
        }

        public async  Task<Subject?> GetOne(string id)
        {
            return await _subjectCollection.Find(s=>s.ID == id).FirstOrDefaultAsync();
        }
        

        public async Task<bool> UpdatebyInstance(Subject subject)
        {
            var deleteResult = await _subjectCollection.ReplaceOneAsync(s => s.ID == subject.ID,subject);
            return deleteResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdatebyParameters(string id,IEnumerable<UpdateParameter> parameters)
        {
            var filter = Builders<Subject>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Subject>.Update;
            List<UpdateDefinition<Subject>> subUpdates = new List<UpdateDefinition<Subject>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<Subject>.Update.Set(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<Subject>.Update.Push(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<Subject>.Update.Pull(parameter.fieldName, parameter.value));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);

            UpdateResult result = await _subjectCollection.UpdateOneAsync(filter, combinedUpdate);
            return result.ModifiedCount > 0;
        }
    }
}
