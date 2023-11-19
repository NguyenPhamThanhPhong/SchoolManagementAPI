using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ILecturerRepository
    {
        public Task Create(Lecturer lecturer);

        public Task<Lecturer> GetLogin(string username);
        public Task<IEnumerable<Lecturer>> GetAll();

        public Task Delete(string id);

        public Task<bool> Update(string id, List<UpdateParameter> parameters);
    }
}
