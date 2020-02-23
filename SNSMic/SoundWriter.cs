using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NAudio.Wave;

namespace SNSMic
{
    public partial class SoundWriter
    {
        public Stream waveStream;
        public bool isRecording;
        public string filePath;

        public void Initialize(WaveInEvent waveIn)
        {
            filePath = @".\" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString().Replace(":", "") + ".wav";
            waveStream = new MemoryStream();
            isRecording = true;
        }

        public void Write(WaveInEventArgs args)
        {
            waveStream.Write(args.Buffer, 0, args.BytesRecorded);
            //waveStream.Flush();
        }

        virtual public async void Close()
        {
            isRecording = false;
        }
    }
}
