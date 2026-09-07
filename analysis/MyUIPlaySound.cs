using UnityEngine;

public class MyUIPlaySound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom
	}

	public int AudioId = 4;

	public Trigger trigger;

	public float delay = -1f;

	public bool IsAwakePlay;

	private vp_Timer.Handle timer = new vp_Timer.Handle();

	private void Awake()
	{
		if (IsAwakePlay)
		{
			Play();
		}
	}

	private void OnHover(bool isOver)
	{
	}

	private void OnPress(bool isPressed)
	{
	}

	private void OnClick()
	{
		if (trigger == Trigger.OnClick)
		{
			Play();
		}
	}

	private void OnSelect(bool isSelected)
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		timer.Cancel();
	}

	private void PlaySound()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(AudioId);
	}

	public void Play()
	{
		if (delay > 0f)
		{
			vp_Timer.In(delay, PlaySound, timer);
		}
		else
		{
			PlaySound();
		}
	}
}
