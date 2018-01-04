using System;
using System.Collections.Generic;
using System.Text;
using System.Media;

namespace CloudDeck.Sound
{
    public static class DeckPlayer
    {
        public static void Play(String file)
        {
            SoundPlayer soundPlayer=new SoundPlayer("content/sounds/"+file+".wav");
            soundPlayer.Play();
        }
    }
}
