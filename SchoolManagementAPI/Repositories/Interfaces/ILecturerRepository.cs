using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ILecturerRepository
    {
        public Task Create(Lecturer lecturer);
        public Task<Lecturer?> Delete(string id);

        Task<IEnumerable<Lecturer>> GetAll();
        public Task<Lecturer?> GetbyUsername(string username);
        public Task<IEnumerable<Lecturer>> GetbyTextFilter(string textFilter);
        public Task<IEnumerable<Lecturer>> GetManyfromIds(List<string> ids);
        public Task<Lecturer?> GetFromId(string id);
        public Task<IEnumerable<Lecturer>> GetManyRange(int start, int end);

        public Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters);
        public Task<bool> UpdatebyInstance(Lecturer instance);
        public Task<bool> UpdateStringFields(string id, List<UpdateParameter> parameters);
        public Task UpdatebyFilter(FilterDefinition<Lecturer> filter, UpdateDefinition<Lecturer> update, bool isMany);
    }
}
