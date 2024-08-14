using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HarmoniX_Controller.Controllers
{
    public class S3Controller
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public S3Controller()
        {
            // Load the s3bucketkeys.json file directly
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)  // Set the base path to the application directory
                .AddJsonFile("s3bucketkeys.json", optional: false, reloadOnChange: true)
                .Build();

            var accessKey = configuration["AWS:AccessKey"];
            var secretKey = configuration["AWS:SecretKey"];
            var region = configuration["AWS:Region"];
            _bucketName = configuration["AWS:BucketName"];

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) ||
                string.IsNullOrEmpty(region) || string.IsNullOrEmpty(_bucketName))
            {
                throw new InvalidOperationException("AWS credentials, region, or bucket name is not set.");
            }

            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
            };
            _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        }

        public async Task<string> UploadFileAsync(string key, Stream fileStream)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            return response.ETag;
        }

        public async Task<Stream> RetrieveFileAsync(string key)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }
    }
}
