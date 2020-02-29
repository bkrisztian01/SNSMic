using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Amazon.Runtime;

namespace SNSMic
{
    public class SoundS3Writer : SoundWriter
    {
        private IAmazonS3 s3Client;
        private string bucketName = "mikrofoncucc";
        private TransferUtility transferUtility;


        public SoundS3Writer(string awsAccessKeyId, string awsSecretAccessKey, Amazon.RegionEndpoint region)
        {
            s3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, region);
            transferUtility = new TransferUtility(s3Client);
        }

        override public async void Close()
        {
            base.Close();
            await UploadFileAsync();
            memoryStream.Close();
        }

        public async Task UploadFileAsync()
        {
            transferUtility.Upload(memoryStream, bucketName, filePath);
        }
    }
}
