using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool MainAudioManager;

    public String MixerName;
    public String GroupName;

    public Sound[] sounds;

    public static AudioManager instance;



    // Start is called before the first frame update
    private void Awake()
    {
        /*
        if(MainAudioManager)
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        */

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            s.source.playOnAwake = false;


            //Load AudioMixer
            AudioMixer audioMixer = null;
            audioMixer = Resources.Load<AudioMixer>(MixerName);

            if(audioMixer == null )
                Debug.LogWarning("Mixer " + this.gameObject + " not found");
            
            else
            {
                //Find AudioMixerGroup you want to load
                AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups(GroupName);

                //Assign the AudioMixerGroup to AudioSource (Use first index)
                s.source.outputAudioMixerGroup = audioMixGroup[0];
            }

            
        }
    }

    // Update is called once per frame
    void Start()
    {
        //Play clips that are on awake
        //AKA theme music
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) 
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    public float LengthOfClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return 0f;
        }
        return s.source.clip.length;
    }

}

