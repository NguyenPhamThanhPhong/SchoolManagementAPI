using SchoolManagementAPI.Models.Entities;

namespace SchoolManagementAPI.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public Task Create(Post post);
        public Task<Post> GetOne(string id);
        public Task<IEnumerable<Post>> GetManyRange(int start, int end);
        public Task<IEnumerable<Post>> GetbyFilter(string textFilter);
        public Task<bool> UpdatebyInstance(Post post);
        public Task<bool> Delete(string id);
    }
}
