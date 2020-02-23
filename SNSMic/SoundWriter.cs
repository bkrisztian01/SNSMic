using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace SNSMic
{
    public partial class SoundWriter
    {
        public WaveFileWriter waveFile;
        public bool isRecording;
        public string filePath;

        public void CreateSoundFile(WaveInEvent waveIn)
        {
            filePath = @".\" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString().Replace(":", "") + ".wav";
            waveFile = new WaveFileWriter(filePath, waveIn.WaveFormat);
            isRecording = true;
        }

        public void Write(WaveInEventArgs args)
        {
            waveFile.Write(args.Buffer, 0, args.BytesRecorded);
            waveFile.Flush();
        }

        public async void Close()
        {
            waveFile.Close();
            isRecording = false;
        }
    }
}
