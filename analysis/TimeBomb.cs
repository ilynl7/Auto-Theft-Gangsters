using UnityEngine;

internal class TimeBomb : MonoBehaviour
{
	public Vector2 FontPosOffset = new Vector2(14f, 13f);

	public float FontBackMargin = 8f;

	public float FontBackSizeOffset = -29f;

	public Vector2 DigitalPosition = new Vector2(0f, 0f);

	private Vector2 m_StartPosition = new Vector2(0f, 0f);

	public Color DigitalNumbersColor = Color.red;

	private float m_TimeBombNumberAlpha = 1f;

	private float m_TimeBombFlashAlpha;

	public float BombTime = 11f;

	private int m_LastSecond;

	private vp_Timer.Handle m_Timer = new vp_Timer.Handle();

	private vp_Timer.Handle m_NumberBlinkTimer = new vp_Timer.Handle();

	private vp_Timer.Handle m_ExplosionDelayTimer = new vp_Timer.Handle();

	public Texture ImagePixel;

	public Texture ImageDigitalDisplay;

	public AudioClip m_ExplosionSound;

	public AudioClip m_BlipSound;

	public AudioClip m_RumbleFadeInSound;

	public AudioClip m_SoftTickLoopSound;

	public GUISkin Skin;

	private GUIStyle m_DigitalDisplayStyle;

	private GUIStyle DigitalDisplayStyle
	{
		get
		{
			if (m_DigitalDisplayStyle == null)
			{
				m_DigitalDisplayStyle = Skin.GetStyle("DigitalDisplay");
			}
			return m_DigitalDisplayStyle;
		}
	}

	private void Start()
	{
		m_StartPosition = DigitalPosition;
		ScheduleBomb(BombTime);
	}

	private void OnGUI()
	{
		if (m_Timer.Active || m_NumberBlinkTimer.Active || m_ExplosionDelayTimer.Active)
		{
			DrawDigital();
		}
		if (m_TimeBombFlashAlpha > 0f)
		{
			m_TimeBombFlashAlpha -= Time.deltaTime * 0.33f;
			GUI.color = new Color(1f, 1f, 1f, m_TimeBombFlashAlpha);
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), ImagePixel);
		}
		if (m_Timer.Active)
		{
			if (!base.audio.isPlaying)
			{
				base.audio.Play();
			}
			if (m_LastSecond != vp_TimeUtility.TimeToUnits(m_Timer.Duration).seconds)
			{
				base.audio.PlayOneShot(m_BlipSound);
				m_LastSecond = vp_TimeUtility.TimeToUnits(m_Timer.Duration).seconds;
			}
		}
		if (m_ExplosionDelayTimer.Active)
		{
			DigitalPosition = m_StartPosition;
			float num = Mathf.Max(0f, m_ExplosionDelayTimer.Duration * 5f);
			Vector2 vector = new Vector2(Random.value * num, Random.value * num);
			if (Random.value < 0.5f)
			{
				vector.x = 0f - vector.x;
			}
			if (Random.value < 0.5f)
			{
				vector.y = 0f - vector.y;
			}
			DigitalPosition += vector;
		}
	}

	private void DrawDigital()
	{
		Rect rect = new Rect((float)Screen.width * 0.5f + DigitalPosition.x - (float)ImageDigitalDisplay.width * 0.5f, (float)Screen.height * 0.5f + DigitalPosition.y - (float)ImageDigitalDisplay.height * 0.5f, (float)ImageDigitalDisplay.width + FontBackSizeOffset, (float)ImageDigitalDisplay.height + FontBackSizeOffset);
		GUI.color = Color.black;
		GUI.DrawTexture(new Rect(rect.x - FontBackMargin + FontPosOffset.x, rect.y - FontBackMargin + FontPosOffset.y, rect.width - 1f + FontBackMargin * 2f + 1f, rect.height - 1f + FontBackMargin * 2f + 1f), ImagePixel);
		Color digitalNumbersColor = DigitalNumbersColor;
		digitalNumbersColor.a = m_TimeBombNumberAlpha;
		GUI.color = digitalNumbersColor;
		GUI.DrawTexture(new Rect(rect.x + 1f + FontPosOffset.x, rect.y + 2f + FontPosOffset.y, rect.width - 1f, rect.height - 1f), ImagePixel);
		GUI.color = Color.black;
		GUI.Label(new Rect(rect.x + FontPosOffset.x, rect.y + FontPosOffset.y, rect.width, rect.height), vp_TimeUtility.TimeToString(m_Timer.DurationLeft, showHours: false, showMinutes: true, showSeconds: true, showTenths: false, showHundredths: true, showMilliSeconds: false), DigitalDisplayStyle);
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(rect.x, rect.y, ImageDigitalDisplay.width, ImageDigitalDisplay.height), ImageDigitalDisplay);
	}

	private void DoTimeBombFlash()
	{
		if (m_TimeBombFlashAlpha > 0f)
		{
			m_TimeBombFlashAlpha -= Time.deltaTime * 0.33f;
			GUI.color = new Color(1f, 1f, 1f, m_TimeBombFlashAlpha);
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), ImagePixel);
		}
	}

	private void ScheduleBomb(float time)
	{
		vp_Timer.In(time, delegate
		{
			base.audio.Stop();
			base.audio.PlayOneShot(m_RumbleFadeInSound);
			vp_Timer.In(0.33f, delegate
			{
				if (m_TimeBombNumberAlpha == 0f)
				{
					m_TimeBombNumberAlpha = 1f;
				}
				else
				{
					m_TimeBombNumberAlpha = 0f;
				}
			}, 7, m_NumberBlinkTimer);
			vp_Timer.In(2.66f, delegate
			{
				base.audio.Stop();
				base.audio.PlayOneShot(m_ExplosionSound);
				m_TimeBombFlashAlpha = 2.5f;
				m_TimeBombNumberAlpha = 1f;
				DigitalPosition = m_StartPosition;
			}, m_ExplosionDelayTimer);
		}, m_Timer);
	}

	private void CancelBomb()
	{
		base.audio.Stop();
		DigitalPosition = m_StartPosition;
		m_Timer.Cancel();
		m_TimeBombNumberAlpha = 1f;
		m_NumberBlinkTimer.Cancel();
		m_ExplosionDelayTimer.Cancel();
	}
}
