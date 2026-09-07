using UnityEngine;

public class MainFuncBtnLogic : MonoBehaviour
{
	public UISprite IconSp;

	public UISprite TipsFlag;

	public UIWidget LockFlag;

	public UILabel BtnLabel;

	private FUNCTION_TYPE curType;

	public TweenRotation DirAnima;

	public void SetState(FUNCTION_TYPE functype, bool Islockshow = false)
	{
		curType = functype;
		if (!Islockshow)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(functype))
			{
				NGUITools.SetActive(base.gameObject, state: true);
				IconSp.alpha = 1f;
			}
			else
			{
				NGUITools.SetActive(base.gameObject, state: false);
			}
			return;
		}
		NGUITools.SetActive(base.gameObject, state: true);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(functype))
		{
			if (LockFlag != null)
			{
				LockFlag.alpha = 0f;
			}
			IconSp.alpha = 1f;
		}
		else if (LockFlag != null)
		{
			LockFlag.alpha = 1f;
		}
	}

	public void SetTips(bool istrue, FUNCTION_TYPE functype)
	{
		curType = functype;
		if (TipsFlag != null)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(functype))
			{
				TipsFlag.enabled = istrue;
			}
			else
			{
				TipsFlag.enabled = false;
			}
		}
	}

	public void SetDirState(bool isopen)
	{
		if (isopen)
		{
			DirAnima.ResetToBeginning();
			DirAnima.enabled = false;
		}
		else
		{
			DirAnima.PlayForward();
			DirAnima.transform.localRotation = Quaternion.Euler(DirAnima.to);
			DirAnima.enabled = false;
		}
	}

	public void ShowDirAnima(bool isopen)
	{
		if (isopen)
		{
			DirAnima.PlayForward();
		}
		else
		{
			DirAnima.PlayReverse();
		}
	}
}
