using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HuggingFace.API;

public class SpeechRecognition : MonoBehaviour
    {
        public KeyCode activationKey = KeyCode.RightShift;
        private AudioClip clip;
        private byte[] bytes;
        private bool recording;
        public playerController playerController;

    private void Update()
        {
            if (Input.GetKeyDown(activationKey))
            {
                StartRecording();
            }
            if (recording && Microphone.GetPosition(null) >= clip.samples)
            {
                StopRecording();
            }
            if (Input.GetKeyUp(activationKey))
            {
                StopRecording();
            }
        }
        

        private void StartRecording() {

            if (recording) { return; }

            clip = Microphone.Start(null, false, 5, 44100);
            recording = true;
            Debug.Log("Recording...");
    }

        private void StopRecording() {
            if (!recording) { return; }
            var position = Microphone.GetPosition(null);
            Microphone.End(null);
            var samples = new float[position * clip.channels];
            clip.GetData(samples, 0);
            bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
            recording = false;
            SendRecording();
        }

        private void SendRecording() {
            HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
                Debug.Log(response);
                if (response.ToLower().Contains("shoot"))
                {
                    playerController.OnRangedAttack();
                }
                else if (response.ToLower().Contains("attack"))
                {
                    playerController.OnComboAttack();
                }
            }, error => {
                Debug.Log("Error with recording");
            });
        }

        private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
            using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
                using (var writer = new BinaryWriter(memoryStream)) {
                    writer.Write("RIFF".ToCharArray());
                    writer.Write(36 + samples.Length * 2);
                    writer.Write("WAVE".ToCharArray());
                    writer.Write("fmt ".ToCharArray());
                    writer.Write(16);
                    writer.Write((ushort)1);
                    writer.Write((ushort)channels);
                    writer.Write(frequency);
                    writer.Write(frequency * channels * 2);
                    writer.Write((ushort)(channels * 2));
                    writer.Write((ushort)16);
                    writer.Write("data".ToCharArray());
                    writer.Write(samples.Length * 2);

                    foreach (var sample in samples) {
                        writer.Write((short)(sample * short.MaxValue));
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
