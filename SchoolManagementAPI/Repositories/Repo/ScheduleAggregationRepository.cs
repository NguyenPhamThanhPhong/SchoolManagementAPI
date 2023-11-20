using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class ScheduleAggregationRepository : IScheduleAggregationRepository
    {
        private readonly IMongoCollection<ScheduleAggregation> _scheduleAggregationCollection;

        public Task<bool> Create(ScheduleAggregation scheduleAgg)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScheduleAggregation>> GetMany(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduleAggregation> GetOne(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatebyInstance(ScheduleAggregation scheduleAgg)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatebyParameters(IEnumerable<UpdateParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
