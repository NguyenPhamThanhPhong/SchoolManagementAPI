using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IScheduleAggregationRepository
    {
        public Task<bool> Create(ScheduleAggregation scheduleAgg);
        public Task<ScheduleAggregation> GetOne(string id);
        public Task<IEnumerable<ScheduleAggregation>> GetMany(IEnumerable<string>ids);
        public Task<bool> UpdatebyParameters(IEnumerable<UpdateParameter> parameters);
        public Task<bool> UpdatebyInstance(ScheduleAggregation scheduleAgg);
        public Task<bool> Delete(string id);

    }
}
