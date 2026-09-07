using UnityEngine;

internal class Clock : MonoBehaviour
{
	public bool DrawDigitalClock = true;

	public bool DrawAnalogClock = true;

	public Vector2 FontPosOffset = new Vector2(14f, 13f);

	public float FontBackMargin = 8f;

	public float FontBackSizeOffset = -29f;

	public Vector2 DigitalPosition = new Vector2(-255f, -230f);

	public Vector2 AnalogPosition = new Vector2(240f, 140f);

	public Vector2 HandPos = new Vector2(0f, 0f);

	public Color DigitalNumbersColor = new Color(0.3f, 0.3f, 1f, 1f);

	private int m_LastSecond;

	public Texture ImagePixel;

	public Texture ImagePixelBlack;

	public Texture ImageDigitalDisplay;

	public Texture ImageClockFace;

	public Texture ImageClockHandHour;

	public Texture ImageClockHandMinute;

	public Texture ImageClockHandSecond;

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
		m_LastSecond = vp_TimeUtility.SystemTimeToUnits().seconds;
	}

	private void OnGUI()
	{
		if (DrawDigitalClock)
		{
			DrawDigital();
		}
		if (DrawAnalogClock)
		{
			DrawAnalog();
		}
		if (m_LastSecond != vp_TimeUtility.SystemTimeToUnits().seconds)
		{
			base.audio.Play();
			m_LastSecond = vp_TimeUtility.SystemTimeToUnits().seconds;
		}
	}

	private void DrawDigital()
	{
		string empty = string.Empty;
		empty = vp_TimeUtility.SystemTimeToString(showHours: true, showMinutes: true, showSeconds: true, showTenths: false, showHundredths: false, showMilliSeconds: false);
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
		Vector2 vector = new Vector2((float)Screen.width * 0.5f + AnalogPosition.x - (float)ImageClockFace.width * 0.5f, (float)Screen.height * 0.5f + AnalogPosition.y - (float)ImageClockFace.height * 0.5f);
		Vector3 zero = Vector3.zero;
		zero = vp_TimeUtility.SystemTimeToDegrees(smooth: false);
		zero.x = vp_TimeUtility.SystemTimeToDegrees().x;
		GUIUtility.RotateAroundPivot(zero.z, vector + new Vector2((float)ImageClockFace.width * 0.5f + HandPos.x, (float)ImageClockFace.height * 0.5f + HandPos.y));
		GUI.DrawTexture(new Rect(vector.x + HandPos.x, vector.y + HandPos.y, ImageClockFace.width, ImageClockFace.height), ImageClockHandSecond);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
		GUIUtility.RotateAroundPivot(zero.x, vector + new Vector2((float)ImageClockFace.width * 0.5f + HandPos.x, (float)ImageClockFace.height * 0.5f + HandPos.y));
		GUI.DrawTexture(new Rect(vector.x + HandPos.x, vector.y + HandPos.y, ImageClockFace.width, ImageClockFace.height), ImageClockHandHour);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
		GUIUtility.RotateAroundPivot(zero.y, vector + new Vector2((float)ImageClockFace.width * 0.5f + HandPos.x, (float)ImageClockFace.height * 0.5f + HandPos.y));
		GUI.DrawTexture(new Rect(vector.x + HandPos.x, vector.y + HandPos.y, ImageClockFace.width, ImageClockFace.height), ImageClockHandMinute);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(vector.x, vector.y, ImageClockFace.width, ImageClockFace.height), ImageClockFace);
	}
}
