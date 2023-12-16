using Microsoft.VisualBasic;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly IMongoCollection<Lecturer> _lecturerCollection;
        private readonly FindOneAndUpdateOptions<Lecturer> _options;

        public LecturerRepository(IMongoCollection<Lecturer> lecturerCollection)
        {
            _lecturerCollection = lecturerCollection;
            this._options = new FindOneAndUpdateOptions<Lecturer>()
            {
                ReturnDocument = ReturnDocument.After
            };
        }

        public async Task Create(Lecturer lecturer)
        {
            await _lecturerCollection.InsertOneAsync(lecturer);
        }
        public Task<Lecturer> GetbyTextFilter(string textFilter)
        {
            var filter = Builders<Lecturer>.Filter.Text(textFilter);
            var options = new FindOptions<Lecturer>()
        }
        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Lecturer>.Filter.Eq(l => l.ID, id);
            var deleteResult =  await _lecturerCollection.DeleteOneAsync(filter);
            if (deleteResult.DeletedCount > 0)
                return true;
            return false;
        }

        public async Task<IEnumerable<Lecturer>> GetAll()
        {
            var filter = Builders<Lecturer>.Filter.Empty;
            return await _lecturerCollection.Find(filter).ToListAsync();
        }

        public async Task<Lecturer> GetLogin(string username)
        {
            var filter = Builders<Lecturer>.Filter.Eq(l=>l.AccountInfo.Username,username);
            return await _lecturerCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<Lecturer>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Lecturer>.Update;
            List<UpdateDefinition<Lecturer>> subUpdates = new List<UpdateDefinition<Lecturer>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<Lecturer>.Update.Set(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<Lecturer>.Update.Push(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<Lecturer>.Update.Pull(parameter.fieldName, parameter.value));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);

            UpdateResult result = await _lecturerCollection.UpdateOneAsync(filter, combinedUpdate);
            if(result.ModifiedCount>0)
                return true;
            return false;
        }
    }
}
