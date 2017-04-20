using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Environment
{
    [CreateAssetMenu]
    public class SoundGroup : ScriptableObject
    {


        public AudioClip[] clips;

        public Util.FloatRange volume;

        public Util.FloatRange pitch;

        public void Play(AudioSource source)
        {
            if (clips.Length == 0) return;

            source.clip = clips[Random.Range(0, clips.Length)];
            source.volume = volume.RandomInRange;
            source.pitch = pitch.RandomInRange;
            source.Play();
        }

    }
}
