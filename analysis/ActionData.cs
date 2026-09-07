using UnityEngine;

public class ActionData
{
	public string ID = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string AnimName = string.Empty;

	public int AnimWrapMode;

	public int AnimDurationTime;

	public int AnimCanBeBreak;

	[ServerExclude("ServerNoUse")]
	public string NextActionName = string.Empty;

	public string FxEffID = string.Empty;

	public int CrossInTime = -1;

	public int CrossOutTime = -1;

	public int SoundID = -1;

	public float CrossInTimeSecond => (float)CrossInTime / 1000f;

	public float CrossOutTimeSecond => (float)CrossOutTime / 1000f;

	public float AnimDurationTimeSecond => (float)AnimDurationTime / 1000f;

	public WrapMode AnimationWrapMode => AnimWrapMode switch
	{
		0 => WrapMode.Once, 
		1 => WrapMode.Loop, 
		2 => WrapMode.ClampForever, 
		_ => WrapMode.Once, 
	};

	public string GetAnimaFileName()
	{
		string[] array = ID.Split('_');
		if (array.Length > 2)
		{
			return $"{array[0]}_{array[1]}";
		}
		return null;
	}
}
