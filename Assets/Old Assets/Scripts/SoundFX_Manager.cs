using System.Collections.Generic;
using UnityEngine;

public class SoundFX_Manager : MonoBehaviour
{
    public enum SoundType
    {
        // List of Sounds
        Item_Pickup,
        Flashlight_Click,
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [HideInInspector]
        public AudioSource Source;
    }

    public static SoundFX_Manager Instance;

    public Sound[] AllSounds;


    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();

    private void Awake()
    {
        Instance = this;

        foreach (var s in AllSounds)
        {
            _soundDictionary[s.Type] = s;
        }
    }

    public void Play(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();


        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;


        audioSrc.Play();


        Destroy(soundObj, s.Clip.length);
    }

}
