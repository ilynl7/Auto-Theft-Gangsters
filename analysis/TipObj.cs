using UnityEngine;

public class TipObj : MonoBehaviour
{
	public GameObject TargetObj;

	public Vector3 OffsetPos;

	public float Duration = -1f;

	public float TargetTime;

	public UILabel TipTextLabel;

	public UISprite TipTextBottomSprite;

	public SCREEN_DIRECTION TipPos;

	public string TipText;

	public TUTORIAL_STEP TutorialStep;

	public bool IsChild = true;

	public UITexture TipPic;

	public bool IsWorldObj;

	public GameObject HandRoot;

	public GameObject CircleRoot;

	private int tipLabelLength = 286;

	public GameObject TempRoot;

	public void Reset(GameObject targetObj, Vector3 offset, TUTORIAL_STEP step = TUTORIAL_STEP.INVALID, float duration = -1f, bool isChild = true, Transform parentObj = null, bool isWorldObj = false)
	{
		TargetObj = targetObj;
		OffsetPos = offset;
		Duration = duration;
		IsChild = isChild;
		if (IsChild)
		{
			TempRoot = new GameObject();
			TempRoot.transform.parent = TargetObj.transform;
			TempRoot.transform.localPosition = Vector3.zero;
			TempRoot.transform.localScale = Vector3.one;
			base.transform.parent = TempRoot.transform;
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = offset;
		}
		else
		{
			OffsetPos = new Vector3(offset.x * targetObj.transform.lossyScale.x, offset.y * targetObj.transform.lossyScale.y, offset.z * targetObj.transform.lossyScale.z);
			if (parentObj != null)
			{
				base.transform.parent = parentObj;
			}
		}
		NGUITools.SetActive(base.gameObject, state: true);
		if (Duration > 0f)
		{
			TargetTime = Time.time + Duration;
		}
		TutorialStep = step;
		IsWorldObj = isWorldObj;
	}

	public void Reset(GameObject targetObj, Vector3 offset, string tips, SCREEN_DIRECTION tipPos, Vector3 tipOffset, TUTORIAL_STEP step = TUTORIAL_STEP.INVALID, float duration = -1f, bool isChild = true, Transform parentObj = null, bool isWorldObj = false, bool isShowHand = true)
	{
		TargetObj = targetObj;
		OffsetPos = offset;
		Duration = duration;
		IsChild = isChild;
		if (IsChild)
		{
			TempRoot = new GameObject();
			TempRoot.transform.parent = TargetObj.transform;
			TempRoot.transform.localPosition = Vector3.zero;
			TempRoot.transform.localScale = Vector3.one;
			base.transform.parent = TempRoot.transform;
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = offset;
		}
		else
		{
			OffsetPos = new Vector3(offset.x * targetObj.transform.lossyScale.x, offset.y * targetObj.transform.lossyScale.y, offset.z * targetObj.transform.lossyScale.z);
			if (parentObj != null)
			{
				base.transform.parent = parentObj;
			}
		}
		NGUITools.SetActive(base.gameObject, UnityVersionUtil.IsActive(targetObj));
		TutorialStep = step;
		TipText = tips;
		if (string.IsNullOrEmpty(tips))
		{
			TipTextLabel.enabled = false;
			TipTextBottomSprite.alpha = 0f;
		}
		else
		{
			TipPos = tipPos;
			if (UnityVersionUtil.IsActive(targetObj))
			{
				TipTextLabel.enabled = true;
				TipTextBottomSprite.alpha = 1f;
			}
			TipTextLabel.text = tips;
			if (TipTextLabel.printedSize.x < (float)tipLabelLength)
			{
				TipTextLabel.width = Mathf.CeilToInt(TipTextLabel.printedSize.x);
			}
			else
			{
				TipTextLabel.width = tipLabelLength;
			}
			TipTextBottomSprite.width = TipTextLabel.width + 84;
			TipTextBottomSprite.height = TipTextLabel.height + 20;
			int width = TipTextBottomSprite.width;
			int height = TipTextBottomSprite.height;
			int num = 0;
			int num2 = 0;
			int num3 = 20;
			switch (tipPos)
			{
			case SCREEN_DIRECTION.TOP:
				num2 = num3 + height / 2 + 50;
				break;
			case SCREEN_DIRECTION.BOTTOM:
				num2 = -(num3 + height / 2 + 50);
				break;
			case SCREEN_DIRECTION.LEFT:
				num = -(num3 + width / 2 + 50);
				break;
			case SCREEN_DIRECTION.RIGHT:
				num = num3 + width / 2 + 50;
				break;
			case SCREEN_DIRECTION.TOP_LEFT:
				num2 = num3 + height / 2 + height / 2;
				num = -(num3 + width / 2 + width / 2);
				break;
			case SCREEN_DIRECTION.TOP_RIGHT:
				num2 = num3 + height / 2 + height / 2;
				num = num3 + width / 2 + width / 2;
				break;
			case SCREEN_DIRECTION.BOTTOM_LEFT:
				num2 = -(num3 + height / 2 + height / 2);
				num = -(num3 + width / 2 + width / 2);
				break;
			case SCREEN_DIRECTION.BOTTOM_RIGHT:
				num2 = -(num3 + height / 2 + height / 2);
				num = num3 + width / 2 + width / 2;
				break;
			}
			TipTextBottomSprite.transform.localPosition = new Vector3((float)num + tipOffset.x, (float)num2 + tipOffset.y, 0f);
			switch (tipPos)
			{
			case SCREEN_DIRECTION.LEFT:
				SetPicLeft();
				break;
			case SCREEN_DIRECTION.RIGHT:
				SetPicRight();
				break;
			default:
				if (TipTextBottomSprite.transform.position.x > SingletonUnity<FunctionTipsRootLogic>.Instance.CenterObj.position.x)
				{
					SetPicLeft();
				}
				else
				{
					SetPicRight();
				}
				break;
			}
		}
		if (Duration > 0f)
		{
			TargetTime = Time.time + Duration;
		}
		IsWorldObj = isWorldObj;
		if (isShowHand)
		{
			NGUITools.SetActive(HandRoot, state: true);
			NGUITools.SetActive(CircleRoot, state: true);
		}
		else
		{
			NGUITools.SetActive(HandRoot, state: false);
			NGUITools.SetActive(CircleRoot, state: false);
		}
	}

	private void SetPicRight()
	{
		TipPic.transform.localPosition = new Vector3(TipTextBottomSprite.width / 2, -TipTextBottomSprite.height / 2, 0f);
		TipPic.transform.localScale = new Vector3(1f, 1f, 1f);
		TipTextLabel.transform.localPosition = new Vector3(-TipTextBottomSprite.width / 2 + 10, 0f, 0f);
	}

	private void SetPicLeft()
	{
		TipPic.transform.localPosition = new Vector3(-TipTextBottomSprite.width / 2, -TipTextBottomSprite.height / 2, 0f);
		TipPic.transform.localScale = new Vector3(-1f, 1f, 1f);
		TipTextLabel.transform.localPosition = new Vector3(TipTextBottomSprite.width / 2 - 10 - TipTextLabel.width, 0f, 0f);
	}

	private void OnEnable()
	{
		if (TipTextLabel != null)
		{
			if (string.IsNullOrEmpty(TipText))
			{
				TipTextLabel.enabled = false;
				TipTextBottomSprite.alpha = 0f;
			}
			else
			{
				TipTextLabel.enabled = true;
				TipTextBottomSprite.alpha = 0f;
			}
		}
	}

	private void OnDestroy()
	{
		FunctionTipsRootLogic.RemoveFunctionTips(TargetObj);
	}
}
