using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface ISchoolClassRepository
    {
        public Task Create(SchoolClass schoolClass);
        public Task<bool> Delete(string id);

        public Task<SchoolClass?> GetSingle(string id);
        public Task<SchoolClass?> GetbyTextFilter(string textFilter);
        public Task<IEnumerable<SchoolClass>> GetManyRange(int start, int end);
        public Task<IEnumerable<SchoolClass>> GetfromIds(List<string> ids);

        public Task UpdateByParameters(IEnumerable<UpdateParameter> parameters);
        public Task UpdatebyInstance(SchoolClass schoolClass);

    }
}
