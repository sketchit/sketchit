using NAudio.Wave;
using SketchIt.Api;
using System;

namespace SketchIt.Sound
{
    public static partial class ExtensionMethods
    {
        static WaveOut _waveOut;

        public static AudioIn CreateAudioIn(this Applet applet)
        {
            return new AudioIn(applet);
        }
    }

    public class AudioIn : IDisposable
    {
        WaveIn _device;
        bool _hasData;
        byte[] _data;

        public AudioIn(Applet applet)
        {
            applet.GetSketch().Exited += (o, e) => Stop();
        }

        public void Start()
        {
            _device = new WaveIn();
            _device.DataAvailable += WaveInDataAvailable;
            _device.StartRecording();
        }

        private void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            _data = e.Buffer;
            _hasData = true;
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            _device?.StopRecording();
            _device?.Dispose();
        }

        public float[] Read()
        {
            float[] result = new float[_data.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToInt16(_data, i * 2) / (float)Int16.MaxValue;
            }

            _hasData = false;

            return result;
        }

        public bool Available => _hasData;
    }
}
