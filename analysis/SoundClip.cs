using System;
using UnityEngine;

[Serializable]
public class SoundClip
{
	private AudioClip m_AudioClip;

	public int m_priority = 128;

	public string m_name = string.Empty;

	public string m_path = string.Empty;

	public float m_minDistance = 10f;

	public float m_volume = 1f;

	public float m_delay;

	public float m_panLevel;

	public float m_spread;

	public bool m_isLoop;

	public int m_curMaxPlayingCount = 1;

	public int m_audio_id = -1;

	public float m_LastActiveTime;

	public AudioClip AudioClip
	{
		get
		{
			return m_AudioClip;
		}
		set
		{
			m_AudioClip = value;
		}
	}
}
