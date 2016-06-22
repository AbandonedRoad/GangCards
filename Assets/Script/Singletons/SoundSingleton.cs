using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Singleton
{
    public class SoundSingleton
    {
        private static SoundSingleton _instance;
        private AudioSource _audio;

        /// <summary>
        /// Gets instance
        /// </summary>
        public static SoundSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SoundSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Prepare instance
        /// </summary>
        private void Init()
        {
            var output = GameObject.Find("SoundPlayback");

            _audio = output.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays a clip from an item.
        /// </summary>
        /// <param name="item"></param>
        public void PlayItemSound(IItem item)
        {
            if (item.AudioClip != null)
            {
                _audio.PlayOneShot(item.AudioClip);
            }
        }
    }
}
