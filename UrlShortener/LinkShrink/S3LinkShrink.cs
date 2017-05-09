using Amazon.S3;
using System;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace LinkShrink
{
    public class S3LinkShrink : ILinkShrink
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IShrinkStrategy _shrinkStrategy;
        private readonly string _bucketName;
        private readonly string _baseUrl;

        public S3LinkShrink(IAmazonS3 s3Client, IShrinkStrategy shrinkStrategy, string bucketName, string baseUrl)
        {
            _s3Client = s3Client;
            _shrinkStrategy = shrinkStrategy;
            _bucketName = bucketName;
            _baseUrl = baseUrl;
        }
        

        public async Task<Uri> Shrink(Uri longLink)
        {
            var shortPath = _shrinkStrategy.GetShortPath(longLink);
            
            return await Shrink(longLink, shortPath);
        }

        public async Task<Uri> Shrink(Uri longLink, string shortPath)
        {
            await _s3Client.PutObjectAsync(new PutObjectRequest()
            {
                BucketName = _bucketName,
                CannedACL = S3CannedACL.PublicRead,
                ContentBody = $"redirect: {longLink}",
                ContentType = "text/html",
                WebsiteRedirectLocation = longLink.AbsoluteUri,
                Key = shortPath
            });

            return new Uri(_baseUrl + shortPath);
        }
    }
}
