using UnityEngine;

public class TestComponent : MonoBehaviour
{
	private Color m_StatusColor = Color.white;

	private string m_StatusString = string.Empty;

	private void Update()
	{
		m_StatusColor = Color.Lerp(m_StatusColor, new Color(1f, 1f, 1f, 0f), Time.deltaTime * 2.75f);
	}

	private void OnGUI()
	{
		GUI.color = m_StatusColor;
		GUI.Label(new Rect(Screen.width - 190, Screen.height - (Screen.height - 95), 400f, 50f), m_StatusString);
	}

	public void Test(string s)
	{
		m_StatusColor = Color.yellow;
		m_StatusString = s;
	}
}
