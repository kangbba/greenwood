using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private Dictionary<string, AudioClip> _bgmClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _sfxClips = new Dictionary<string, AudioClip>();

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    private string _currentBGM = null;
    private string _currentSFX = null;

    public string CurrentBGM => _currentBGM;
    public string CurrentSFX => _currentSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadSounds();
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Resources/BGMs 및 Resources/SFXs 폴더에서 모든 사운드를 로드
    /// </summary>
    private void LoadSounds()
    {
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("BGMs");
        foreach (var clip in bgmClips)
        {
            if (!_bgmClips.ContainsKey(clip.name))
            {
                _bgmClips.Add(clip.name, clip);
            }
        }

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("SFXs");
        foreach (var clip in sfxClips)
        {
            if (!_sfxClips.ContainsKey(clip.name))
            {
                _sfxClips.Add(clip.name, clip);
            }
        }
    }

    /// <summary>
    /// AudioSource 초기화 (BGM, SFX 용)
    /// </summary>
    private void InitializeAudioSources()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;
    }

    /// <summary>
    /// 사운드 재생 (BGM 또는 SFX 구분)
    /// </summary>
    public void PlaySound(string soundID, bool isBgm, float volume = 1f, bool loop = false, float fadeDuration = 1f)
    {
        if (isBgm)
        {
            if (!_bgmClips.TryGetValue(soundID, out AudioClip clip))
            {
                Debug.LogError($"[SoundManager] BGM '{soundID}' not found!");
                return;
            }

            // ✅ 기존 BGM 즉시 정지
            StopBGM(0f);

            _currentBGM = soundID;

            _bgmSource.clip = clip;
            _bgmSource.volume = 0;
            _bgmSource.loop = true;
            _bgmSource.Play();
            _bgmSource.DOFade(volume, fadeDuration);
        }
        else
        {
            if (!_sfxClips.TryGetValue(soundID, out AudioClip clip))
            {
                Debug.LogError($"[SoundManager] SFX '{soundID}' not found!");
                return;
            }

            // ✅ 기존 SFX 즉시 정지
            StopSFX();

            _currentSFX = soundID;

            _sfxSource.clip = clip;
            _sfxSource.loop = loop;
            _sfxSource.volume = volume;
            _sfxSource.Play();
        }
    }

    /// <summary>
    /// BGM 정지 (페이드 아웃 포함)
    /// </summary>
    public void StopBGM(float fadeDuration = 1f)
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.DOFade(0, fadeDuration).OnComplete(() =>
            {
                _bgmSource.Stop();
                _currentBGM = null;
            });
        }
    }

    /// <summary>
    /// 현재 재생 중인 SFX 정지
    /// </summary>
    public void StopSFX()
    {
        if(_sfxSource == null){
            return;
        }
        _sfxSource.Stop();
        _currentSFX = null;
    }

    /// <summary>
    /// SFX 페이드 아웃 후 정지
    /// </summary>
    public void StopSFX(float fadeDuration)
    {
        if(_sfxSource == null){
            return;
        }
        _sfxSource.DOFade(0, fadeDuration).OnComplete(() =>
        {
            _sfxSource.Stop();
            _currentSFX = null;
        });
    }

    /// <summary>
    /// 현재 BGM이 재생 중인지 확인
    /// </summary>
    public bool IsBGMPlaying() => _bgmSource.isPlaying;

    /// <summary>
    /// 현재 SFX가 재생 중인지 확인
    /// </summary>
    public bool IsSFXPlaying() => _sfxSource.isPlaying;
}
