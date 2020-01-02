using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavGame.Character
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public Clip[] clips;

        Dictionary<string, AudioClip> dict = new Dictionary<string, AudioClip>();

        bool isMute = false;
        float savedVolume;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            foreach (Clip clip in clips)
            {
                dict.Add(clip.name, clip.audio);
            }
        }

        public void Play(string clipName, Vector3 position)
        {
            if (dict.ContainsKey(clipName))
            {
                AudioSource.PlayClipAtPoint(dict[clipName], position);
            }
        }

        public void Mute()
        {
            if (!isMute)
            {
                savedVolume = AudioListener.volume;
                AudioListener.volume = 0;
                isMute = true;
            }
        }

        public void UnMute()
        {
            if (isMute)
            {
                AudioListener.volume = savedVolume;
                isMute = false;
            }
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
        }

        [Serializable]
        public class Clip
        {
            public string name;
            public AudioClip audio;
        }
    }
}
