using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ISchoolClassRepository
    {
        public Task Create(SchoolClass schoolClass);
        public Task Delete(string id);

        public Task<SchoolClass> GetSingle(string id);
        public Task<SchoolClass> GetMany(IEnumerable<string> ids, bool isAll);


        public Task UpdateByParameters(IEnumerable<UpdateParameter> parameters);
        public Task UpdatebyInstance(SchoolClass schoolClass);

    }
}
