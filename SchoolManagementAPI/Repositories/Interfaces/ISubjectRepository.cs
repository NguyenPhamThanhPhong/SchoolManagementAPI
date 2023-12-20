using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        public Task Create(Subject subject);

        public Task<Subject?> GetOne(string id);
        public Task<IEnumerable<Subject>> GetAll();

        public Task<bool> UpdatebyParameters(string id,IEnumerable<UpdateParameter> parameters);
        public Task<bool> UpdatebyInstance(Subject subject);

        public Task DeleteMany(IEnumerable<string> ids);
    }
}
