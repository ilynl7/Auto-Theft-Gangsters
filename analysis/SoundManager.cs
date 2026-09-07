using System.Collections;
using UnityEngine;

public class SoundManager : SingletonDontDestoryUnity<SoundManager>
{
	public class MyAudioSource
	{
		public int m_audioID;

		public AudioSource m_AudioSource;

		public MyAudioSource()
		{
			m_audioID = -1;
			m_AudioSource = null;
		}
	}

	private enum FadeMode
	{
		None,
		FadeIn,
		FadeOut
	}

	public SoundClipPools m_SoundClipPools;

	private MyAudioSource m_BGAudioSource = new MyAudioSource();

	private float m_CurBGVolume;

	public static int m_SFXChannelsCount = 30;

	private MyAudioSource[] m_SFXChannel = new MyAudioSource[m_SFXChannelsCount];

	private SoundClip m_NextSoundClip;

	public static bool m_EnableBGM = true;

	public static bool m_EnableSFX = true;

	private int m_lastMusicID = -1;

	public static float m_sfxVolume = 1f;

	public static float m_bgmVolume = 1f;

	private FadeMode m_fadeMode;

	private float m_fadeOutTime;

	private float m_fadeOutTimer;

	private float m_fadeInTime;

	private float m_fadeInTimer;

	public float sfxVolume
	{
		set
		{
			m_sfxVolume = value;
		}
	}

	public float bgmVolume
	{
		set
		{
			m_bgmVolume = value;
			m_BGAudioSource.m_AudioSource.volume = m_CurBGVolume * m_bgmVolume;
		}
	}

	public bool EnableSFX
	{
		get
		{
			return m_EnableSFX;
		}
		set
		{
			m_EnableSFX = value;
		}
	}

	public bool EnableBGM
	{
		get
		{
			return m_EnableBGM;
		}
		set
		{
			if (m_EnableBGM && !value)
			{
				if (m_BGAudioSource != null && m_BGAudioSource.m_AudioSource.isPlaying)
				{
					m_BGAudioSource.m_AudioSource.Stop();
				}
			}
			else if (!m_EnableBGM && value && m_BGAudioSource != null)
			{
				PlayBGMWithFade(m_lastMusicID, 0.1f, 0f);
			}
			m_EnableBGM = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (SingletonDontDestoryUnity<SoundManager>.Exists && base.IsInit)
		{
			for (int i = 0; i < m_SFXChannelsCount; i++)
			{
				m_SFXChannel[i] = new MyAudioSource();
				m_SFXChannel[i].m_audioID = -1;
				m_SFXChannel[i].m_AudioSource = base.gameObject.AddComponent<AudioSource>();
			}
			m_BGAudioSource.m_AudioSource = base.gameObject.AddComponent<AudioSource>();
			m_BGAudioSource.m_audioID = -1;
			m_SoundClipPools = new SoundClipPools();
		}
	}

	private void Start()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SystemMusic == 0 || GameSettingData.IsLowPhone)
		{
			EnableBGM = false;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SystemSoundEffect == 0 || GameSettingData.IsLowPhone)
		{
			EnableSFX = false;
		}
		sfxVolume = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SoundDragValue;
		bgmVolume = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MusicDragValue;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void FixedUpdate()
	{
		UpdateBGMusic();
	}

	private void UpdateBGMusic()
	{
		if (m_fadeMode == FadeMode.FadeOut)
		{
			if (Mathf.Abs(m_fadeOutTime) < 0.001f)
			{
				return;
			}
			m_fadeOutTimer += Time.deltaTime;
			m_BGAudioSource.m_AudioSource.volume = (1f - m_fadeOutTimer / m_fadeOutTime) * m_CurBGVolume * m_bgmVolume;
			if (m_fadeOutTimer >= m_fadeOutTime)
			{
				int audioID = m_BGAudioSource.m_audioID;
				SetBGMAudioSource(ref m_BGAudioSource, m_NextSoundClip, 1f);
				m_SoundClipPools.ForceRemoveClipByID(audioID);
				m_CurBGVolume = m_NextSoundClip.m_volume;
				if (m_fadeInTime > 0f)
				{
					m_fadeMode = FadeMode.FadeIn;
					m_fadeOutTimer = 0f;
					m_BGAudioSource.m_AudioSource.volume = 0f;
				}
				else
				{
					m_fadeMode = FadeMode.None;
				}
				m_BGAudioSource.m_AudioSource.Play();
			}
		}
		else if (m_fadeMode == FadeMode.FadeIn && !(Mathf.Abs(m_fadeInTime) < 0.001f))
		{
			m_fadeInTimer += Time.deltaTime;
			m_BGAudioSource.m_AudioSource.volume = m_fadeInTimer / m_fadeInTime * m_CurBGVolume * m_bgmVolume;
			if (m_fadeInTimer >= m_fadeInTime)
			{
				m_fadeMode = FadeMode.None;
				m_fadeInTimer = 0f;
				m_BGAudioSource.m_AudioSource.volume = m_CurBGVolume * m_bgmVolume;
			}
		}
	}

	private void PlayBGMWithFade(int nSoundclipId, float fadeOutTime, float fadeInTime)
	{
		m_SoundClipPools.GetSoundClip(nSoundclipId, OnPlayBGMWithFade, new SoundClipPools.SoundClipParam(nSoundclipId, fadeOutTime, fadeInTime));
	}

	private void OnPlayBGMWithFade(SoundClip bgSoundClip, SoundClipPools.SoundClipParam param)
	{
		if (m_BGAudioSource == null || bgSoundClip == null)
		{
			return;
		}
		m_lastMusicID = param.m_clip_id;
		if (m_BGAudioSource.m_AudioSource.isPlaying)
		{
			if (m_NextSoundClip != null && m_NextSoundClip.m_audio_id == bgSoundClip.m_audio_id)
			{
				return;
			}
			m_fadeOutTime = param.m_FadeOutTime;
			m_fadeInTime = param.m_FadeInTime;
			m_fadeOutTimer = 0f;
			m_fadeInTimer = 0f;
			if (m_fadeOutTime <= 0f)
			{
				int audioID = m_BGAudioSource.m_audioID;
				SetBGMAudioSource(ref m_BGAudioSource, bgSoundClip, 1f);
				m_SoundClipPools.ForceRemoveClipByID(audioID);
				m_CurBGVolume = bgSoundClip.m_volume;
				if (m_fadeInTime <= 0f)
				{
					m_BGAudioSource.m_AudioSource.Play();
					m_fadeMode = FadeMode.None;
				}
				else
				{
					m_BGAudioSource.m_AudioSource.volume = 0f;
					m_BGAudioSource.m_AudioSource.Play();
					m_fadeMode = FadeMode.FadeIn;
				}
			}
			else
			{
				m_NextSoundClip = bgSoundClip;
				m_fadeMode = FadeMode.FadeOut;
			}
		}
		else
		{
			m_fadeInTime = param.m_FadeInTime;
			m_fadeInTimer = 0f;
			int audioID2 = m_BGAudioSource.m_audioID;
			SetBGMAudioSource(ref m_BGAudioSource, bgSoundClip, 1f);
			m_SoundClipPools.ForceRemoveClipByID(audioID2);
			m_CurBGVolume = bgSoundClip.m_volume;
			if (m_fadeInTime <= 0f)
			{
				m_BGAudioSource.m_AudioSource.Play();
				m_fadeMode = FadeMode.None;
			}
			else
			{
				m_BGAudioSource.m_AudioSource.volume = 0f;
				m_BGAudioSource.m_AudioSource.Play();
				m_fadeMode = FadeMode.FadeIn;
			}
		}
	}

	public void PlayBGMusic(int nClipID, float fadeOutTime, float fadeInTime)
	{
		if (AudioListener.volume == 0f || !GameSettingData.IsSoundCanPlay[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		if (nClipID < 0)
		{
			Debug.LogError("PlayBGM id < 0");
			return;
		}
		m_lastMusicID = nClipID;
		if (m_EnableBGM)
		{
			PlayBGMWithFade(nClipID, fadeOutTime, fadeInTime);
		}
	}

	public void StopBGM(float _fadeTime)
	{
		if (m_EnableBGM && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(StopBGMWithFade(_fadeTime));
		}
	}

	private IEnumerator StopBGMWithFade(float _fadeTime)
	{
		if (m_BGAudioSource != null && m_BGAudioSource.m_AudioSource.isPlaying)
		{
			float time = _fadeTime;
			while (time > 0f)
			{
				m_BGAudioSource.m_AudioSource.volume = time / m_fadeOutTime * m_bgmVolume;
				time -= Time.deltaTime;
				yield return null;
			}
			m_BGAudioSource.m_AudioSource.Stop();
		}
	}

	public void PlaySoundEffectAtPos2(int nSoundID, Vector3 playSoundPos, Vector3 listenerPos)
	{
		if (!GameSettingData.IsSoundCanPlay[GameSettingData.GetPhoneClass()] || nSoundID < 0)
		{
			return;
		}
		float num = 1f;
		listenerPos.y = 0f;
		playSoundPos.y = 0f;
		float num2 = Vector3.Distance(listenerPos, playSoundPos);
		num = 1f - num2 / 10f;
		if (num < 0.05f)
		{
			num = 0.05f;
			return;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		PlaySoundEffect(nSoundID, num);
	}

	public void PlaySoundEffect(int nSoundID, float volumeFactor = 1f, SoundClipPools.OnPlaySoundDelegate onPlaySound = null)
	{
		if (GameSettingData.IsSoundCanPlay[GameSettingData.GetPhoneClass()] && AudioListener.volume != 0f && m_EnableSFX)
		{
			if (base.name == null)
			{
				Debug.LogError("PlaySoundEffect name is null");
				return;
			}
			base.name = base.name.Trim();
			m_SoundClipPools.GetSoundClip(nSoundID, OnPlaySoundEffect, new SoundClipPools.SoundClipParam(volumeFactor, onPlaySound));
		}
	}

	private void OnPlaySoundEffect(SoundClip soundClip, SoundClipPools.SoundClipParam param)
	{
		if (soundClip == null)
		{
			Debug.LogError("soundClip is null");
		}
		else
		{
			PlaySoundEffect(soundClip, param.m_Volume, param.onPlaySound);
		}
	}

	private void PlaySoundEffect(SoundClip soundClip, float volumeFactor, SoundClipPools.OnPlaySoundDelegate onPlaySound = null)
	{
		if (!m_EnableSFX || string.IsNullOrEmpty(soundClip.m_path))
		{
			return;
		}
		if (soundClip.AudioClip == null)
		{
			Debug.Log("PlaySoundEffect soundClip.Audioclip is null");
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = -1;
		int num4 = -1;
		for (int i = 0; i < m_SFXChannelsCount; i++)
		{
			if (m_SFXChannel[i] == null)
			{
				return;
			}
			if (num3 == -1 && !m_SFXChannel[i].m_AudioSource.isPlaying)
			{
				num3 = i;
			}
			if (m_SFXChannel[i].m_AudioSource.clip == null)
			{
				continue;
			}
			if (m_SFXChannel[i].m_audioID == soundClip.m_audio_id)
			{
				if (num4 == -1 && !m_SFXChannel[i].m_AudioSource.isPlaying)
				{
					num4 = i;
				}
				if (m_SFXChannel[i].m_AudioSource.isPlaying)
				{
					num2++;
				}
				if (num2 >= soundClip.m_curMaxPlayingCount)
				{
					break;
				}
			}
			if (m_SFXChannel[num].m_AudioSource.priority < m_SFXChannel[i].m_AudioSource.priority)
			{
				num = i;
			}
		}
		if (num2 >= soundClip.m_curMaxPlayingCount)
		{
			return;
		}
		int nValidIndex = -1;
		if (num4 != -1)
		{
			nValidIndex = num4;
		}
		else if (num3 != -1)
		{
			nValidIndex = num3;
		}
		else
		{
			nValidIndex = num;
		}
		if (nValidIndex < 0 || nValidIndex >= m_SFXChannelsCount)
		{
			return;
		}
		m_SFXChannel[nValidIndex].m_AudioSource.Stop();
		SetSFXAudioSource(ref m_SFXChannel[nValidIndex], soundClip, volumeFactor);
		if (soundClip.m_delay > float.Epsilon)
		{
			vp_Timer.In(soundClip.m_delay, delegate
			{
				m_SFXChannel[nValidIndex].m_AudioSource.Play();
			});
		}
		else
		{
			m_SFXChannel[nValidIndex].m_AudioSource.Play();
		}
		onPlaySound?.Invoke(m_SFXChannel[nValidIndex].m_AudioSource);
	}

	public void StopSoundEffect(int nSoundID)
	{
		if (m_SFXChannel == null)
		{
			return;
		}
		for (int i = 0; i < m_SFXChannelsCount; i++)
		{
			if (!(m_SFXChannel[i].m_AudioSource == null) && m_SFXChannel[i].m_audioID == nSoundID)
			{
				m_SFXChannel[i].m_AudioSource.Stop();
			}
		}
	}

	public void StopAllSoundEffect()
	{
		if (m_SFXChannel == null)
		{
			return;
		}
		for (int i = 0; i < m_SFXChannelsCount; i++)
		{
			if (m_SFXChannel[i] != null)
			{
				m_SFXChannel[i].m_AudioSource.Stop();
			}
		}
	}

	private void SetSFXAudioSource(ref MyAudioSource audioSource, SoundClip clip, float volume)
	{
		if (clip != null)
		{
			audioSource.m_AudioSource.clip = clip.AudioClip;
			audioSource.m_AudioSource.volume = clip.m_volume * volume * m_sfxVolume;
			audioSource.m_AudioSource.spread = clip.m_spread;
			audioSource.m_AudioSource.priority = clip.m_priority;
			audioSource.m_AudioSource.panLevel = clip.m_panLevel;
			audioSource.m_AudioSource.minDistance = clip.m_minDistance;
			audioSource.m_AudioSource.loop = clip.m_isLoop;
			audioSource.m_audioID = clip.m_audio_id;
			audioSource.m_AudioSource.pitch = 1f;
		}
	}

	private void SetBGMAudioSource(ref MyAudioSource audioSource, SoundClip clip, float volume)
	{
		if (clip != null)
		{
			audioSource.m_AudioSource.clip = clip.AudioClip;
			audioSource.m_AudioSource.volume = clip.m_volume * volume * m_bgmVolume;
			audioSource.m_AudioSource.spread = clip.m_spread;
			audioSource.m_AudioSource.priority = clip.m_priority;
			audioSource.m_AudioSource.panLevel = clip.m_panLevel;
			audioSource.m_AudioSource.minDistance = clip.m_minDistance;
			audioSource.m_AudioSource.loop = clip.m_isLoop;
			audioSource.m_audioID = clip.m_audio_id;
			audioSource.m_AudioSource.pitch = 1f;
		}
	}
}
