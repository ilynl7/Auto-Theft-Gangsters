using UnityEngine;

public class SlotLineItemLogic : MonoBehaviour
{
	private Transform mTra;

	public TweenScale scaleanima;

	public UISprite iconSp;

	public int idx;

	private Vector3 endPos;

	public SlotIconData curiconinfo;

	public string iconid;

	public void Reset(SlotIconData curcion, int index)
	{
		if (mTra == null)
		{
			mTra = base.transform;
		}
		ResetPos(index);
		UpdateInfo(curcion);
		StopAnima();
	}

	public void ResetPos(int index)
	{
		mTra.localPosition = new Vector3(0f, 360 - index * 90, 0f);
		idx = index;
	}

	public void UpdateInfo(SlotIconData curcion)
	{
		iconSp.spriteName = curcion.IconName;
		iconSp.MakePixelPerfect();
		curiconinfo = curcion;
		iconid = curiconinfo.ID;
	}

	public void PlayeAnima()
	{
		scaleanima.PlayForward();
	}

	public void StopAnima()
	{
		if (scaleanima.enabled)
		{
			if (scaleanima.mAmountPerDelta < 0f)
			{
				scaleanima.mAmountPerDelta = 0f - scaleanima.mAmountPerDelta;
			}
			scaleanima.ResetToBeginning();
			scaleanima.enabled = false;
		}
	}

	public void SetMoveToUp(int countnum)
	{
		mTra.localPosition = new Vector3(0f, 360 + countnum * 90, 0f);
	}

	public void MoveTo(int end)
	{
		mTra.localPosition = new Vector3(0f, 360 - end * 90, 0f);
	}
}
