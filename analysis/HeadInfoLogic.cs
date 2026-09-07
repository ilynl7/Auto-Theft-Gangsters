using UnityEngine;

public class HeadInfoLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public HPLineLogic mHpLineLogic;

	public virtual void Init()
	{
	}

	public virtual void SetHpVal(float val)
	{
		if (mHpLineLogic != null)
		{
			mHpLineLogic.ChangeVal(val);
		}
	}

	public virtual void ForceSetHpVal(float val)
	{
		if (mHpLineLogic != null)
		{
			mHpLineLogic.ForceSetVal(val);
		}
	}
}
