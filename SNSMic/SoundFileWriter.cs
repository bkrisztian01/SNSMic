using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SNSMic
{
    public class SoundFileWriter : SoundWriter
    {
        override public async void Close()
        {
            base.Close();
            using (FileStream  f = new FileStream(filePath, FileMode.Create))
            {
                waveStream.Seek(0, SeekOrigin.Begin);
                waveStream.CopyTo(f);
            }
            waveStream.Close();
        }
    }
}
