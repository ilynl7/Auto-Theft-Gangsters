using UnityEngine;

internal class StopWatch : MonoBehaviour
{
	public bool DrawButtons = true;

	public bool DrawDigitalWatch = true;

	public bool DrawAnalogWatch = true;

	public float FontBackMargin = 8f;

	public float FontBackSizeOffset = -29f;

	public Vector2 FontPosOffset = new Vector2(14f, 13f);

	public Vector2 ButtonsPosition = new Vector2(0f, 0f);

	public Vector2 ButtonsScale = new Vector2(80f, 40f);

	public Vector2 DigitalPosition = new Vector2(255f, -230f);

	public Vector2 AnalogPosition = new Vector2(-240f, 140f);

	public Vector2 BigHandPos = new Vector2(0f, 0f);

	public Vector2 SmallHandPos = new Vector2(0f, -53f);

	public Color DigitalNumbersColor = Color.yellow;

	private vp_Timer.Handle m_Timer = new vp_Timer.Handle();

	public Texture ImagePixel;

	public Texture ImageDigitalDisplay;

	public Texture ImageStopWatch;

	public Texture ImageHandBig;

	public Texture ImageHandSmall;

	public Texture ImageButtonPlay;

	public Texture ImageButtonPause;

	public Texture ImageButtonStop;

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

	private void OnGUI()
	{
		if (DrawDigitalWatch)
		{
			DrawDigital();
		}
		if (DrawButtons)
		{
			DrawDemoButtons();
		}
		if (DrawAnalogWatch)
		{
			DrawAnalog();
		}
	}

	private void DrawDemoButtons()
	{
		if (GUI.Button(new Rect((float)Screen.width * 0.5f + ButtonsPosition.x - ButtonsScale.x / 2f, (float)Screen.height * 0.5f + ButtonsPosition.y - ButtonsScale.y, ButtonsScale.x, ButtonsScale.y), (!m_Timer.Active) ? ImageButtonPlay : ImageButtonStop))
		{
			if (!m_Timer.Active)
			{
				Run();
			}
			else
			{
				Stop();
			}
		}
		if (!m_Timer.Active)
		{
			GUI.enabled = false;
		}
		if (GUI.Button(new Rect((float)Screen.width * 0.5f + ButtonsPosition.x - ButtonsScale.x / 2f, (float)Screen.height * 0.5f + ButtonsPosition.y + 5f, ButtonsScale.x, ButtonsScale.y), (!m_Timer.Paused) ? ImageButtonPause : ImageButtonPlay))
		{
			Pause();
		}
		GUI.enabled = true;
	}

	private void DrawDigital()
	{
		string empty = string.Empty;
		empty = vp_TimeUtility.TimeToString(m_Timer.Duration, showHours: false, showMinutes: true, showSeconds: true, showTenths: false, showHundredths: true, showMilliSeconds: false);
		Rect rect = new Rect((float)Screen.width * 0.5f + DigitalPosition.x - (float)ImageDigitalDisplay.width * 0.5f, (float)Screen.height * 0.5f + DigitalPosition.y - (float)ImageDigitalDisplay.height * 0.5f, (float)ImageDigitalDisplay.width + FontBackSizeOffset, (float)ImageDigitalDisplay.height + FontBackSizeOffset);
		GUI.color = Color.black;
		GUI.DrawTexture(new Rect(rect.x - FontBackMargin + FontPosOffset.x, rect.y - FontBackMargin + FontPosOffset.y, rect.width - 1f + FontBackMargin * 2f + 1f, rect.height - 1f + FontBackMargin * 2f + 1f), ImagePixel);
		GUI.color = DigitalNumbersColor;
		GUI.DrawTexture(new Rect(rect.x + 1f + FontPosOffset.x, rect.y + 2f + FontPosOffset.y, rect.width - 1f, rect.height - 1f), ImagePixel);
		GUI.color = Color.black;
		GUI.Label(new Rect(rect.x + FontPosOffset.x, rect.y + FontPosOffset.y, rect.width, rect.height), empty, DigitalDisplayStyle);
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(rect.x, rect.y, ImageDigitalDisplay.width, ImageDigitalDisplay.height), ImageDigitalDisplay);
	}

	private void DrawAnalog()
	{
		GUI.color = Color.white;
		Vector2 vector = new Vector2((float)Screen.width * 0.5f + AnalogPosition.x - (float)(ImageStopWatch.width / 2), (float)Screen.height * 0.5f + AnalogPosition.y - (float)(ImageStopWatch.height / 2));
		Vector3 zero = Vector3.zero;
		zero.x = 0f;
		zero.y = vp_TimeUtility.TimeToDegrees(m_Timer.Duration, includeHours: false, includeMinutes: false, includeSeconds: false);
		zero.z = vp_TimeUtility.TimeToDegrees(m_Timer.Duration);
		GUIUtility.RotateAroundPivot(zero.z, vector + new Vector2((float)(ImageStopWatch.width / 2) + BigHandPos.x, (float)(ImageStopWatch.height / 2) + BigHandPos.y));
		GUI.DrawTexture(new Rect(vector.x + BigHandPos.x, vector.y + BigHandPos.y, ImageStopWatch.width, ImageStopWatch.height), ImageHandBig);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
		GUIUtility.RotateAroundPivot(zero.y, vector + new Vector2((float)(ImageStopWatch.width / 2) + SmallHandPos.x, (float)(ImageStopWatch.height / 2) + SmallHandPos.y));
		GUI.DrawTexture(new Rect(vector.x + SmallHandPos.x, vector.y + SmallHandPos.y, ImageStopWatch.width, ImageStopWatch.height), ImageHandSmall);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(vector.x, vector.y, ImageStopWatch.width, ImageStopWatch.height), ImageStopWatch);
	}

	public void Run()
	{
		vp_Timer.Start(m_Timer);
		if (!base.audio.isPlaying)
		{
			base.audio.Play();
		}
	}

	public void Stop()
	{
		m_Timer.Cancel();
		if (base.audio.isPlaying)
		{
			base.audio.Stop();
		}
	}

	public void Pause()
	{
		m_Timer.Paused = !m_Timer.Paused;
		if (!base.audio.isPlaying && m_Timer.Active && !m_Timer.Paused)
		{
			base.audio.Play();
		}
		else if (base.audio.isPlaying && (!m_Timer.Active || m_Timer.Paused))
		{
			base.audio.Stop();
		}
	}
}
