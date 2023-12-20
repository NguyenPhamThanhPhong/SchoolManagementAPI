using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IMongoCollection<Subject> _subjectCollection;

        public SubjectRepository(IMongoCollection<Subject> subjects)
        {
            _subjectCollection = subjects;
        }

        public Task Create(Subject subject)
        {
            return _subjectCollection.InsertOneAsync(subject);
        }

        public Task DeleteMany(IEnumerable<string> ids)
        {
            var filter = Builders<Subject>.Filter.In(s=>s.ID,ids);
            return _subjectCollection.DeleteManyAsync(filter);
        }

        public async Task<IEnumerable<Subject>> GetAll()
        {
            return await _subjectCollection.Find(_ => true).ToListAsync();
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
