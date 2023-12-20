using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using System.Collections.ObjectModel;

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

        public async Task<Lecturer?> Delete(string id)
        {
            var deleteResult = await _lecturerCollection.FindOneAndDeleteAsync(l => l.ID == id);
            return deleteResult;
        }

        public async Task<Lecturer?> GetbyTextFilter(string textFilter)
        {
            var filter = Builders<Lecturer>.Filter.Text(textFilter);
            var lecturer = await _lecturerCollection.Find(filter).FirstOrDefaultAsync();
            return lecturer;
        }

        public async Task<Lecturer?> GetbyUsername(string username)
        {
            var filter = Builders<Lecturer>.Filter.Eq(l=>l.Username, username);
            return await  _lecturerCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lecturer>> GetManyfromIds(List<string> ids)
        {
            var filter = Builders<Lecturer>.Filter.In(l => l.ID, ids);
            return await _lecturerCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Lecturer>> GetManyRange(int start, int end)
        {
            return await _lecturerCollection.Find(_ => true).Skip(start).Limit(end - start).ToListAsync();
        }

        public async Task<bool> UpdatebyInstance(Lecturer instance)
        {
            var updateResult = await _lecturerCollection.ReplaceOneAsync(l=>l.ID==instance.ID, instance); ;
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters)
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
            return result.ModifiedCount> 0;
        }

        public async Task<bool> UpdateStringFields(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<Lecturer>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Lecturer>.Update;
            List<UpdateDefinition<Lecturer>> subUpdates = new List<UpdateDefinition<Lecturer>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<Lecturer>.Update.Set(parameter.fieldName, parameter.value.ToString()));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<Lecturer>.Update.Push(parameter.fieldName, parameter.value.ToString()));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<Lecturer>.Update.Pull(parameter.fieldName, parameter.value.ToString()));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            UpdateResult result = await _lecturerCollection.UpdateOneAsync(filter, combinedUpdate);
            return result.ModifiedCount > 0;
        }
    }
}
