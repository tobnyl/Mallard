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
    public Audio[] GlobalSounds;

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

	Pool<AudioSource> sourcePool;

    void Awake()
    {
		sourcePool = new Pool<AudioSource>(() =>
		{
			var source = Instantiate(AudioSourcePrefab);
			source.transform.parent = this.transform;
			return source;
		});
		sourcePool.onGet = (AudioSource source) =>
		{
			source.gameObject.SetActive(true);
		};
		sourcePool.onFree = (AudioSource source) =>
		{
			source.gameObject.SetActive(false);
		};
		sourcePool.Grow(32);

        //_clipList = new List<string>();
        _clipList = new List<QueuedAudio>();
        _audioSourceCollidingDistance = AudioSourcePrefab.minDistance * 2f;

        InitializeGlobalSounds();

        if (_instance == null)
        {
            _instance = GetComponent<AudioManager>();
        }
    }

    private void InitializeGlobalSounds()
    {
        foreach (var sound in GlobalSounds)
        {
            Play(sound, Vector3.zero);
        }
    }




    void LateUpdate()
    {
        _clipList.Clear();
    }

    public AudioSourceExtended Play(Audio audio, Vector3 position, bool muteAllActiveSfx = false)
    {
		AudioSourceExtended _audioSource = null;

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
            //_musicAudioSource.Stop();
        }

        return _audioSource;
    }

	public void Return(AudioSourceExtended source)
	{
		sourcePool.Return(source.Source);
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
        //_musicAudioSource.Play();
    }

	private AudioSourceExtended Play(AudioClip clip, AudioMixerGroup group, Vector3 position, bool loop, float minVol = 1.0f, float maxVol = 1.0f, float minPitch = 1.0f, float maxPitch = 1.0f)
    {
		AudioSource audioSource = sourcePool.Get();
     
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

        //Debug.Log("Vol: " + audioSource.volume + " | Pitch: " + audioSource.pitch);

        AudioSourceExtended audioSourceExtended = audioSource.GetComponent<AudioSourceExtended>();
		audioSourceExtended.Source = audioSource;
        audioSourceExtended.Duration = clip.length;
        audioSourceExtended.Loop = loop; // I don't like this code...


        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = (group != null ? group : Master);
        audioSource.Play();

        if (!AllowDuplicatesPerFrame)
        {
            _clipList.Add(new QueuedAudio { Name = clip.name, Position = position });
        }

		audioSourceExtended.Run();

        return audioSourceExtended;
    }
}