using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using NAudio.Wave;

namespace SNSMic
{
    public partial class SoundWriter
    {
        public MemoryStream memoryStream;
        public bool isRecording;
        public string filePath;
        public WaveFileWriter wf;
        WaveInEvent waveIn;

        public void Initialize(WaveInEvent waveIn)
        {
            filePath = DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + DateTime.Now.ToLongTimeString().Replace(":", "-") + ".wav";
            memoryStream = new MemoryStream();
            wf = new WaveFileWriter(memoryStream, waveIn.WaveFormat);
            isRecording = true;
            this.waveIn = waveIn;
        }

        public void Write(WaveInEventArgs args)
        {
            wf.Write(args.Buffer, 0, args.BytesRecorded);
        }

        virtual public async void Close()
        {
            isRecording = false;
            wf.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
        }
    }
}
