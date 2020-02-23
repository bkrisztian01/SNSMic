using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NAudio.Wave;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.S3;
using Amazon.S3.Model;

namespace SNSMic
{
    public partial class Form1 : Form
    {
        Amazon.RegionEndpoint region = Amazon.RegionEndpoint.EUCentral1;
        string awsAccessKeyId = "AKIA3WM726OZOBHNWOGJ";
        string awsSecretAccessKey = "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";
        string message = "Volume meter thingy";
        string topicArn = "arn:aws:sns:eu-central-1:804030968754:MyTopic";

        float max;
        bool isSnsEnabled = false;
        AmazonSimpleNotificationServiceClient client;
        PublishRequest request;
        DateTime lastPublish = DateTime.Now.AddSeconds(-11);
        bool soundDetected = false;
        WaveInEvent waveIn;
        SoundWriter soundWriter;
        //WaveFileWriter waveFile;
        //bool isRecording = false;

        private void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            max = 0;
            // interpret as 16 bit audio
            for (int index = 0; index < args.BytesRecorded; index += 2)
            {
                short sample = (short)((args.Buffer[index + 1] << 8) |
                                        args.Buffer[index + 0]);
                // to floating point
                var sample32 = sample / 32768f;
                // absolute value 
                if (sample32 < 0) sample32 = -sample32;
                // is this the max value?
                if (sample32 > max) max = sample32;
            }

            if (soundWriter.isRecording)
            {
                //    waveFile.Write(args.Buffer, 0, args.BytesRecorded);
                //    waveFile.Flush();
                soundWriter.Write(args);
            }
        }

        public Form1()
        {
            InitializeComponent();

            client = new AmazonSimpleNotificationServiceClient(region: region, awsAccessKeyId: awsAccessKeyId, awsSecretAccessKey: awsSecretAccessKey);
            request = new PublishRequest
            {
                Message = message,
                TopicArn = topicArn
            };

            //soundWriter = new SoundS3Writer(awsAccessKeyId, awsSecretAccessKey, region);
            soundWriter = new SoundFileWriter();

            waveIn = new WaveInEvent();
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = (max * 100).ToString();
            trackBar1.Value = Convert.ToInt32(max * 100);
            soundDetected = (max * 100) >= 30;
            
            if (isSnsEnabled && soundDetected && !soundWriter.isRecording && (DateTime.Now - lastPublish).TotalSeconds >= 10)
            {
                //string filename = @".\" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString().Replace(":","") + ".wav";
                //waveFile = new WaveFileWriter(filename, waveIn.WaveFormat);
                //isRecording = true;
                soundWriter.Initialize(waveIn);

                var response = client.Publish(request);
                lastPublish = DateTime.Now;
                //MessageBox.Show("Notification has been sent");
            }
            else if (isSnsEnabled && !soundDetected && soundWriter.isRecording)
            {
                //waveFile.Close();
                //isRecording = false;
                soundWriter.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isSnsEnabled)
            {
                button1.Text = "Enable SNS";
            }
            else
            {
                button1.Text = "Disable SNS";
            }
            isSnsEnabled = !isSnsEnabled;
        }
    }
}
