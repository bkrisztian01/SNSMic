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
        private string keyName;
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
            waveStream.Close();
        }

        //private async Task UploadFileAsync()
        //{
        //    keyName = filePath.Substring(2);

        //    // Create list to store upload part responses.
        //    List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

        //    // Setup information required to initiate the multipart upload.
        //    InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
        //    {
        //        BucketName = bucketName,
        //        Key = keyName
        //    };

        //    // Initiate the upload.
        //    InitiateMultipartUploadResponse initResponse =
        //        await s3Client.InitiateMultipartUploadAsync(initiateRequest);

        //    // Upload parts.
        //    long contentLength = new FileInfo(filePath).Length;
        //    long partSize = 5 * (long)Math.Pow(2, 20); // 5 MB

        //    try
        //    {
        //        Console.WriteLine("Uploading parts");

        //        long filePosition = 0;
        //        for (int i = 1; filePosition < contentLength; i++)
        //        {
        //            UploadPartRequest uploadRequest = new UploadPartRequest
        //            {
        //                BucketName = bucketName,
        //                Key = keyName,
        //                UploadId = initResponse.UploadId,
        //                PartNumber = i,
        //                PartSize = partSize,
        //                FilePosition = filePosition,
        //                FilePath = filePath
        //            };

        //            // Track upload progress.
        //            uploadRequest.StreamTransferProgress +=
        //                new EventHandler<StreamTransferProgressArgs>(UploadPartProgressEventCallback);

        //            // Upload a part and add the response to our list.
        //            uploadResponses.Add(await s3Client.UploadPartAsync(uploadRequest));

        //            filePosition += partSize;
        //        }

        //        // Setup to complete the upload.
        //        CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
        //        {
        //            BucketName = bucketName,
        //            Key = keyName,
        //            UploadId = initResponse.UploadId
        //        };
        //        completeRequest.AddPartETags(uploadResponses);

        //        // Complete the upload.
        //        CompleteMultipartUploadResponse completeUploadResponse =
        //            await s3Client.CompleteMultipartUploadAsync(completeRequest);
        //    }
        //    catch (Exception exception)
        //    {
        //        Debug.WriteLine("An AmazonS3Exception was thrown: { 0}", exception.Message);

        //        // Abort the upload.
        //        AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
        //        {
        //            BucketName = bucketName,
        //            Key = keyName,
        //            UploadId = initResponse.UploadId
        //        };
        //        await s3Client.AbortMultipartUploadAsync(abortMPURequest);
        //    }

        //}

        public void UploadPartProgressEventCallback(object sender, StreamTransferProgressArgs e)
        {
            // Process event. 
            Debug.WriteLine("{0}/{1}", e.TransferredBytes, e.TotalBytes);
        }

        public async Task UploadFileAsync()
        {
            keyName = filePath.Substring(2);
            transferUtility.Upload(waveStream, bucketName, keyName);
        }
    }
}
