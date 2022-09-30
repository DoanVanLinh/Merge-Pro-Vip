using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{

    #region Singleton
    public static SoundManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [SerializeField] private List<Sound> listSounds;

    private void Start()
    {
        foreach (Sound sound in listSounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        Sound sound = listSounds.Where(s => s.name == name).FirstOrDefault();
        if (sound == null)
            return;

        sound.source.Play();
    }
    public void Stop(string name)
    {
        Sound sound = listSounds.Where(s => s.name == name).FirstOrDefault();
        if (sound == null)
            return;

        sound.source.Stop();
    }

    public void ChangeVolumnTheme(float volumn)
    {
        foreach (Sound sound in listSounds)
        {
            if (sound.name == "Theme")
            {
                if (volumn == 0)
                    sound.source.enabled = false;
                else
                    sound.source.enabled = true;
            }
        }
    }
    public void ChangeVolumnEffect(float volumn)
    {
        foreach (Sound sound in listSounds)
        {
            if (sound.name == "Theme" || sound.name == "Theme 1")
                continue;

            if (volumn == 0)
                sound.source.enabled = false;
            else
                sound.source.enabled = true;
        }
    }
    public void PlaySoundButtonClick()
    {
        Play("Button Click");
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool playOnAwake;
    public bool loop;
    [Range(0f, 1f)]
    public float volume;

    //[HideInInspector]
    public AudioSource source;
}
