using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ILecturerRepository
    {
        public Task Create(Lecturer lecturer);

        public Task<Lecturer> GetbyUsername(string username);
        public Task<Lecturer> GetbyTextFilter(string textFilter);
        public Task<IEnumerable<Lecturer>> GetManyfromIds(List<string> ids);
        public Task<IEnumerable<Lecturer>> GetManyRange(int start, int end);
        public Task<bool> Delete(string id);

        public Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters);
        public Task<bool> UpdatebyInstance(string id, Lecturer instance);
    }
}
