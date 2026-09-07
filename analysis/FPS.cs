using UnityEngine;

public class FPS : MonoBehaviour
{
	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private string format;

	private void Start()
	{
		if (!base.guiText)
		{
			base.enabled = false;
		}
		else
		{
			timeleft = updateInterval;
		}
	}

	private void showNetBack(string str)
	{
		str = str + "Recive=" + NetLogic.nReceiveCount;
		str = str + "Send=" + NetLogic.nSendCount;
		base.guiText.text = str;
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			float num = accum / (float)frames;
			format = $"{num:F2}";
			base.guiText.text = format;
			if (num < 20f)
			{
				base.guiText.material.color = Color.red;
			}
			else if (num < 30f)
			{
				base.guiText.material.color = Color.yellow;
			}
			else
			{
				base.guiText.material.color = Color.green;
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
		showNetBack(format);
	}
}
