using System.Collections.Generic;
using UnityEngine;

public class SoundClipPools
{
	public class SoundClipParam
	{
		public float m_Volume;

		public float m_FadeOutTime;

		public float m_FadeInTime;

		public int m_clip_id;

		public OnPlaySoundDelegate onPlaySound;

		public SoundClipParam(float volume, OnPlaySoundDelegate func = null)
		{
			m_Volume = volume;
			m_FadeInTime = 0f;
			m_FadeOutTime = 0f;
			onPlaySound = func;
		}

		public SoundClipParam(int clipId, float fadeOutTime, float fadeInTime, OnPlaySoundDelegate func = null)
		{
			m_Volume = 1f;
			m_FadeInTime = fadeInTime;
			m_FadeOutTime = fadeOutTime;
			m_clip_id = clipId;
			onPlaySound = func;
		}
	}

	public delegate void GetAudioClipDelegate(SoundClip soundClip, SoundClipParam param);

	public delegate void OnPlaySoundDelegate(AudioSource audioSource);

	private Dictionary<int, SoundClip> mAudioClipMap = new Dictionary<int, SoundClip>();

	public void GetSoundClip(int nSoundId, GetAudioClipDelegate delFun, SoundClipParam param)
	{
		if (nSoundId < 0)
		{
			return;
		}
		if (mAudioClipMap.ContainsKey(nSoundId))
		{
			mAudioClipMap[nSoundId].m_LastActiveTime = Time.realtimeSinceStartup;
			delFun?.Invoke(mAudioClipMap[nSoundId], param);
			return;
		}
		if (mAudioClipMap.Count > SoundManager.m_SFXChannelsCount)
		{
			RemoveLastUnUsedClip();
		}
		SoundData soundDataById = DataManager.GetSoundDataById(nSoundId);
		if (soundDataById == null)
		{
			Debug.LogError("sound id " + nSoundId + " is null");
			delFun?.Invoke(null, param);
			return;
		}
		string fullPathName = soundDataById.FullPathName;
		if (string.IsNullOrEmpty(fullPathName))
		{
			delFun?.Invoke(null, param);
			return;
		}
		string name = soundDataById.Name;
		if (string.IsNullOrEmpty(name))
		{
			delFun?.Invoke(null, param);
			return;
		}
		AudioClip audioClip = ResourcesManager.Load(fullPathName + "/" + name) as AudioClip;
		if (audioClip != null)
		{
			OnLoadSound(fullPathName + name, audioClip, soundDataById, delFun, param);
		}
		else if (UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<SoundManager>.Instance.gameObject))
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.StartCoroutine(BundleManager.LoadSound(fullPathName, name, OnLoadSound, soundDataById, delFun, param));
		}
	}

	private void OnLoadSound(string soundPath, AudioClip curAudioClip, object param1, object param2, object param3 = null)
	{
		SoundClip soundClip = new SoundClip();
		soundClip.AudioClip = curAudioClip;
		GetAudioClipDelegate getAudioClipDelegate = param2 as GetAudioClipDelegate;
		SoundClipParam param4 = param3 as SoundClipParam;
		SoundData soundData = param1 as SoundData;
		if (null == soundClip.AudioClip)
		{
			Debug.LogError("sound clip " + soundPath + " is null");
			getAudioClipDelegate?.Invoke(null, param4);
			return;
		}
		if (!soundClip.AudioClip.isReadyToPlay)
		{
			Debug.LogError("Cann't decompress the sound resource " + soundPath);
			getAudioClipDelegate?.Invoke(null, param4);
			return;
		}
		soundClip.m_LastActiveTime = Time.realtimeSinceStartup;
		soundClip.m_delay = soundData.Delay;
		soundClip.m_minDistance = soundData.MinDistance;
		soundClip.m_panLevel = soundData.PanLevel;
		soundClip.m_spread = soundData.Spread;
		soundClip.m_volume = soundData.Volume;
		soundClip.m_isLoop = soundData.IsLoop == 1;
		soundClip.m_path = soundPath;
		soundClip.m_name = soundData.Name;
		soundClip.m_audio_id = soundData.Id;
		soundClip.m_curMaxPlayingCount = soundData.CurMaxPlayingCount;
		if (!mAudioClipMap.ContainsKey(soundData.Id))
		{
			mAudioClipMap.Add(soundData.Id, soundClip);
		}
		getAudioClipDelegate?.Invoke(soundClip, param4);
	}

	private void RemoveLastUnUsedClip()
	{
		float num = 100000000f;
		int key = -1;
		foreach (SoundClip value in mAudioClipMap.Values)
		{
			if (num > value.m_LastActiveTime)
			{
				key = value.m_audio_id;
				num = value.m_LastActiveTime;
			}
		}
		mAudioClipMap.Remove(key);
	}

	public void ForceRemoveClipByID(int uid)
	{
		if (uid == -1)
		{
			return;
		}
		int key = -1;
		foreach (SoundClip value in mAudioClipMap.Values)
		{
			if (value.m_audio_id == uid)
			{
				key = value.m_audio_id;
				break;
			}
		}
		mAudioClipMap.Remove(key);
	}
}
