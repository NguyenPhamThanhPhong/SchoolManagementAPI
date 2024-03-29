﻿using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using SchoolManagementAPI.Services.Configs;
using Microsoft.AspNetCore.Http;

#pragma warning disable CS0618

namespace SchoolManagementAPI.Services.CloudinaryService
{
    public class CloudinaryHandler
    {
        private readonly Cloudinary _cloudinary;
        private readonly string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv" };

        public CloudinaryHandler(CloudinaryConfig cloudinaryConfig)
        {
            Account account = new Account(
                cloudinaryConfig.CloudName,
                cloudinaryConfig.APIKey,
                cloudinaryConfig.APISecretKey);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<Dictionary<string, string?>> UploadImages(List<IFormFile>? files, string folderName)
        {
            var results = new Dictionary<string, string?>();

            if (files == null)
                return results;

            var uploadTasks = files.Select(async file =>
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(file.FileName)).ToLower();

                if (imageExtensions.Contains(fileExtension))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(fileName, stream),
                            Folder = folderName
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        results.Add(file.FileName, uploadResult.SecureUri.AbsoluteUri);
                    }
                }
                else if (videoExtensions.Contains(fileExtension))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploadParams = new VideoUploadParams
                        {
                            File = new FileDescription(fileName, stream),
                            Folder = folderName
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        results.Add(file.FileName, uploadResult.SecureUri.AbsoluteUri);
                    }
                }
                else
                {
                    using (var stream = file.OpenReadStream())
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploadParams = new RawUploadParams
                        {
                            File = new FileDescription(fileName, stream),
                            Folder = folderName
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        results.Add(file.FileName, uploadResult.SecureUri.AbsoluteUri);
                    }
                }
            });

            await Task.WhenAll(uploadTasks); // Wait for all tasks to complete

            return results;
        }

        public async Task<string?> UploadSingleImage(IFormFile file, string folderName)
        {
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, stream),
                        Folder = folderName
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return (uploadResult.SecureUrl).AbsoluteUri;
                }
            }
            return null;
        }
        public async Task Delete(string? Url)
        {
            if (string.IsNullOrEmpty(Url))
                return;
            var publicId = GetPublicIdFromUrl(Url);
            Console.WriteLine(publicId);

            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DeletionParams(publicId);

                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                // Check if the deletion was successful
                if (deletionResult.Result.ToLower() == "ok")
                {
                    Console.WriteLine($"File deleted successfully. Public ID: {publicId}");
                }
                else
                {
                    Console.WriteLine($"failed Public ID: {publicId}");
                    Console.WriteLine($"Failed to delete file. Error: {deletionResult.Result}");
                    Console.WriteLine($"Failed to delete file. Error: {deletionResult.Error?.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Cloudinary URL");
            }
        }

        public async Task DeleteMany(List<string> urls)
        {
            List<Task> tasks = new List<Task>();
            foreach(string url in urls)
                tasks.Append(Delete(url));
            await Task.WhenAll(tasks);
        }

        private string? GetPublicIdFromUrl(string cloudinaryUrl)
        {
            Uri uri;
            if (Uri.TryCreate(cloudinaryUrl, UriKind.Absolute, out uri))
            {
                const string uploadPathSegment = "upload/";

                int uploadIndex = uri.Segments.ToList().FindIndex(segment => segment.Equals(uploadPathSegment, StringComparison.OrdinalIgnoreCase));
                if (uploadIndex != -1 && uploadIndex < uri.Segments.Length - 1)
                {
                    int startIndex = uploadIndex + 1;
                    int length = uri.Segments.Length - startIndex;

                    // Use ArraySegment to get the desired segments
                    ArraySegment<string> segments = new ArraySegment<string>(uri.Segments, startIndex, length);

                    // Concatenate the segments and trim any trailing '/'
                    return string.Join("", segments.Select(s => s.TrimEnd('/')));
                }
            }

            // If the URL format doesn't match, return null
            return null;
        }


    }
}
