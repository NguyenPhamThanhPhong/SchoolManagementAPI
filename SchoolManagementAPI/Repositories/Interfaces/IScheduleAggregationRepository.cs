using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IScheduleAggregationRepository
    {
        public Task Create(ScheduleAggregation scheduleAgg);
        public Task<ScheduleAggregation> GetOne(string id);
        public Task<IEnumerable<ScheduleAggregation>> GetMany(IEnumerable<string>ids);
        public Task UpdatebyParameters(IEnumerable<UpdateParameter> parameters);
        public Task UpdatebyInstance(ScheduleAggregation scheduleAgg);
        public Task Delete(string id);

    }
}
