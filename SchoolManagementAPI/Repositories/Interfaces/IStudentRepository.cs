using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        public Task Create(Student student);

        public Task<Student> GetLogin(string username);
        public Task<IEnumerable<Student>> GetAll();


        public Task Delete(string id);
        public Task Update(string id, List<UpdateParameter> parameters);
    }
}
