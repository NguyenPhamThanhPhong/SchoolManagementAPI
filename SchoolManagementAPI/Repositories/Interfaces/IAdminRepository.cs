using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        public Task Create(Admin lecturer);

        public Task<Admin?> GetbyUsername(string username);
        public Task<Admin?> GetbyID(string id);
        public Task<IEnumerable<Admin>> GetAll();
        public Task<bool> Delete(string id);

        public Task<bool> UpdatebyParameters(string id, List<UpdateParameter> parameters);
        public Task<bool> UpdatebyInstance(string id, Admin instance);
    }
}
