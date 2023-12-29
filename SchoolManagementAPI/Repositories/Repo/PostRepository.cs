using MongoDB.Bson;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _postCollection;

        public PostRepository(DatabaseConfig databaseConfig)
        {
            _postCollection = databaseConfig.PostCollection;
        }

        public Task Create(Post post)
        {
            return _postCollection.InsertOneAsync(post);
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _postCollection.DeleteOneAsync(s => s.ID == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Post>> GetbyFilter(string textFilter)
        {
            var filter = BsonDocument.Parse(textFilter);
            return await _postCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetManyRange(int start, int end)
        {
            return await _postCollection.Find(_=>true).Skip(start).Limit(start-end).ToListAsync();
        }

        public Task<Post> GetOne(string id)
        {
            return _postCollection.Find(s=>s.ID==id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatebyInstance(Post post)
        {
            var result = await _postCollection.ReplaceOneAsync(p=>p.ID==post.ID,post);
            return result.ModifiedCount > 0;
        }
    }
}
