using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace PROG6221POE
{
    public static class AudioPlayer
    {
        public static void PlayGreeting(string filePath)
        {
           
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Audio file missing: " + filePath);

                return;
            }

            try
            {
                
                SoundPlayer player = new SoundPlayer(filePath);

                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Audio playback failed: " + ex.Message);
            }
        }
    }
}