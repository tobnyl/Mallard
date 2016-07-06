using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System.Linq;

public class QueuedAudio
{
    public string Name { get; set; }
    public Vector3 Position { get; set; }
}

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioSourcePrefab;
    public AudioMixerGroup Master;
    public bool AllowDuplicatesPerFrame;
    public Audio Music;

    public static AudioManager Instance
    {
        get { return _instance; }
    }
    public bool MuteAllSfx { get; set; }

    private static AudioManager _instance;
    //private List<string> _clipList;
    private List<QueuedAudio> _clipList;
    private float _audioSourceCollidingDistance;
    private AudioSource _musicAudioSource;

    void Awake()
    {
        //_clipList = new List<string>();
        _clipList = new List<QueuedAudio>();
        _audioSourceCollidingDistance = AudioSourcePrefab.minDistance * 2f;

        InitializeMusic(Music);

        if (_instance == null)
        {
            _instance = GetComponent<AudioManager>();
        }
    }

    private void InitializeMusic(Audio music)
    {
        _musicAudioSource = gameObject.AddComponent<AudioSource>();
        _musicAudioSource.clip = music.Clip;
        _musicAudioSource.outputAudioMixerGroup = (music.Output != null ? music.Output : Master);
        _musicAudioSource.loop = music.Loop;
        _musicAudioSource.mute = music.Mute;
        _musicAudioSource.Play();
    }

    public void PlayMusic()
    {
        _musicAudioSource.Play();
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }



    void LateUpdate()
    {
        _clipList.Clear();
    }

    public GameObject Play(Audio audio, Vector3 position, bool muteAllActiveSfx = false)
    {
        GameObject _audioSource = null;

        if (muteAllActiveSfx)
        {
            var sfxPlaying = GetComponentsInChildren<AudioSource>();

            foreach (var sfx in sfxPlaying)
            {
                sfx.Stop();
            }

            StartCoroutine(MuteAllActiveSfxDuration(audio.Clip.length));
        }

        if (!audio.IsNull() && !audio.Mute && !MuteAllSfx && !AlreadyInQueueAndOutOfDistance(audio, position))
        {
            _audioSource = Play(audio.Clip, audio.Output, position, audio.Loop, audio.MinVolume, audio.MaxVolume, audio.MinPitch, audio.MaxPitch);
        }

        if (muteAllActiveSfx)
        {
            MuteAllSfx = muteAllActiveSfx;
            _musicAudioSource.Stop();
        }

        return _audioSource;
    }

    private bool AlreadyInQueueAndOutOfDistance(Audio audio, Vector3 position)
    {
        var clips = _clipList.Where(x => x.Name == audio.Clip.name);

        foreach (var clip in clips)
        {
            // Nu satte jag denna till 30 (Min distance * 2) så länge...
            if (Vector3.Distance(clip.Position, position) < _audioSourceCollidingDistance)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator MuteAllActiveSfxDuration(float length)
    {
        yield return new WaitForSeconds(length);

        MuteAllSfx = false;
        _musicAudioSource.Play();
    }

    private GameObject Play(AudioClip clip, AudioMixerGroup group, Vector3 position, bool loop, float minVol = 1.0f, float maxVol = 1.0f, float minPitch = 1.0f, float maxPitch = 1.0f)
    {
        AudioSource audioSource = Instantiate(AudioSourcePrefab, transform.position, transform.rotation) as AudioSource;
        audioSource.transform.parent = this.transform;
        audioSource.transform.position = position;
        audioSource.loop = loop;

        if (minPitch != 1.0f || maxPitch != 1.0f)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }

        if (minVol != 1.0f || maxVol != 1.0f)
        {
            audioSource.volume = Random.Range(minVol, maxVol);
        }

        AudioSourceExtended audioSourceExtended = audioSource.GetComponent<AudioSourceExtended>();
        audioSourceExtended.Duration = clip.length;
        audioSourceExtended.Loop = loop; // I don't like this code...


        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = (group != null ? group : Master);
        audioSource.Play();

        if (!AllowDuplicatesPerFrame)
        {
            _clipList.Add(new QueuedAudio { Name = clip.name, Position = position });
        }

        return audioSource.gameObject;
    }
}