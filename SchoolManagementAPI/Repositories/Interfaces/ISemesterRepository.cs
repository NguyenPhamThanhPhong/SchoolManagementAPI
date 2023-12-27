using SchoolManagementAPI.Models.Entities;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ISemesterRepository
    {
        public Task<IEnumerable<Semester>> GetAll();
        public Task<Semester> GetOne(string id);
        public Task Create(Semester semester);
        public Task<bool> UpdatebyInstance(Semester semester);
        public Task Delete(string id);
        
    }
}
