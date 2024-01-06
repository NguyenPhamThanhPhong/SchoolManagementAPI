using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.CloudinaryService;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly IMapper _mapper;
        private readonly string _postFolderName;
        private readonly IMongoCollection<Post> _postCollection;

        public PostController(IPostRepository postRepository, IMapper mapper, 
            CloudinaryHandler cloudinaryHandler,CloudinaryConfig cloudinaryConfig,
            DatabaseConfig databaseConfig)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _cloudinaryHandler = cloudinaryHandler;
            _postFolderName = cloudinaryConfig.PostFolderName;
            _postCollection = databaseConfig.PostCollection;
        }

        [HttpPost("/post-create")]
        public async Task<IActionResult> Create([FromForm] PostCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var post = _mapper.Map<Post>(request);
            post.FileUrls = await _cloudinaryHandler.UploadImages(request.Files,_postFolderName);
            await _postRepository.Create(post);
            return Ok(post);
        }

        [HttpGet("/post-get-by-id/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var post = await _postRepository.GetOne(id);
            return Ok(post);
        }
        [HttpGet("/post-get-many-range/{start}/{end}")]
        public async Task<IActionResult> Get(int start, int end)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var posts = await _postRepository.GetManyRange(start, end);
            return Ok(posts);
        }

        [HttpPost("/post-update-instance")]
        public async Task<IActionResult> UpdateInstance([FromForm] PostUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var post = _mapper.Map<Post>(request);
            if(request.PrevUrls !=null && request.PrevUrls?.Count > 0)
                foreach (var url in request.PrevUrls)
                    await _cloudinaryHandler.Delete(url);

            if (request.Files != null && request.Files.Count > 0)
                post.FileUrls = await _cloudinaryHandler.UploadImages(request.Files,_postFolderName);
            await _postRepository.Create(post);
            return Ok(post);
        }

        [HttpDelete("/post-delete/{id}")]
        public async Task<IActionResult> Delete(string id,[FromBody]List<string> prevUrls)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isDeleted = await _postRepository.Delete(id);
            if(prevUrls!=null && prevUrls.Count > 0)
                foreach (var url in prevUrls)
                    await _cloudinaryHandler.Delete(url);
            return Ok(isDeleted);
        }
        [HttpDelete("/post-delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteManyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var filter = Builders<Post>.Filter.In(s => s.ID, request.Ids);
            Task deletePost = _postCollection.DeleteManyAsync(filter);

            List<Task> taskList = new List<Task>();

            if (request.PrevUrls != null && request.PrevUrls.Count > 0)
                foreach (var url in request.PrevUrls)
                    taskList.Add(_cloudinaryHandler.Delete(url));

            await Task.WhenAll(deletePost, Task.WhenAll(taskList));
            return Ok();
        }
    }
}
