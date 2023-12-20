using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        public Task Create(Student lecturer);

        public Task<Student> GetbyUsername(string username);
        public Task<Student> GetbyTextFilter(string textFilter);
        public Task<IEnumerable<Student>> GetManyfromIds(List<string> ids);
        public Task<IEnumerable<Student>> GetManyRange(int start, int end);
        public Task<bool> Delete(string id);

        public Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters);
        public Task<bool> UpdatebyInstance(string id, Student instance);
    }
}
