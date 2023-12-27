using SchoolManagementAPI.Models.Entities;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IFacultyRepository
    {
        public Task<IEnumerable<Faculty>> GetAll();
        public Task<Faculty> GetOne(string id);
        public Task Create(Faculty faculty);
        public Task<bool> UpdatebyInstance(Faculty faculty);
        public Task Delete(string id);
    }
}
