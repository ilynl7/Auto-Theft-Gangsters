using System.Collections.Generic;
using UnityEngine;

public class FunctionTipsRootLogic : SingletonUnity<FunctionTipsRootLogic>
{
	public TipObj TipsObj;

	public TipObj HandTipsObj;

	public List<TipObj> EnableTipCircleData = new List<TipObj>();

	public List<TipObj> DisableTipCircleData = new List<TipObj>();

	public List<TipObj> EnableHandTipCircleData = new List<TipObj>();

	public List<TipObj> DisableHandTipCircleData = new List<TipObj>();

	public Transform CenterObj;

	private Camera mainCamera;

	private new void Awake()
	{
		base.Awake();
		NGUITools.SetActive(HandTipsObj.gameObject, state: false);
		NGUITools.SetActive(TipsObj.gameObject, state: false);
		for (int i = 0; i < DisableTipCircleData.Count; i++)
		{
			NGUITools.SetActive(DisableTipCircleData[i].gameObject, state: false);
		}
		for (int j = 0; j < DisableHandTipCircleData.Count; j++)
		{
			NGUITools.SetActive(DisableHandTipCircleData[j].gameObject, state: false);
		}
		if (Camera.main != null)
		{
			mainCamera = Camera.main;
		}
	}

	public static void AddFunctionTips(GameObject targetObj, Vector3 offset, float duration = -1f)
	{
		if (SingletonUnity<FunctionTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionTipsRootLogic>.Instance.AddTips(targetObj, offset, duration);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FunctionTipsRoot, delegate
		{
			SingletonUnity<FunctionTipsRootLogic>.Instance.AddTips(targetObj, offset, duration);
		});
	}

	public static void AddFunctionHandTips(GameObject targetObj, Vector3 offset, TUTORIAL_STEP step, bool isChild, string tips, SCREEN_DIRECTION tipPos, Vector3 tipOffset, float duration = -1f, Transform parentObj = null, bool isWorldObj = false, bool isShowHand = true)
	{
		if (SingletonUnity<FunctionTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionTipsRootLogic>.Instance.AddHandTips(targetObj, offset, step, isChild, tips, tipPos, tipOffset, duration, parentObj, isWorldObj, isShowHand);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FunctionTipsRoot, delegate
		{
			SingletonUnity<FunctionTipsRootLogic>.Instance.AddHandTips(targetObj, offset, step, isChild, tips, tipPos, tipOffset, duration, parentObj, isWorldObj, isShowHand);
		});
	}

	public static bool IsHandTipEnable()
	{
		if (SingletonUnity<FunctionTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionTipsRootLogic>.Instance.gameObject))
		{
			if (SingletonUnity<FunctionTipsRootLogic>.Instance.EnableHandTipCircleData.Count > 0)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static void RemoveFunctionTips(GameObject targetObj)
	{
		if (SingletonUnity<FunctionTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionTipsRootLogic>.Instance.RemoveTips(targetObj);
		}
	}

	public static void ClearHandTip()
	{
		if (SingletonUnity<FunctionTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionTipsRootLogic>.Instance.ClearHandTips();
		}
	}

	public void AddTips(GameObject targetObj, Vector3 offset, float duration = -1f)
	{
		for (int i = 0; i < EnableTipCircleData.Count; i++)
		{
			if (targetObj == EnableTipCircleData[i].TargetObj)
			{
				if (duration > 0f)
				{
					EnableTipCircleData[i].TargetTime = Time.time + duration;
				}
				return;
			}
		}
		TipObj tipObj = null;
		if (DisableTipCircleData.Count > 0)
		{
			tipObj = DisableTipCircleData[0];
			DisableTipCircleData.RemoveAt(0);
		}
		if (tipObj == null || tipObj.gameObject == null)
		{
			GameObject gameObject = Object.Instantiate(TipsObj.gameObject) as GameObject;
			tipObj = gameObject.GetComponent<TipObj>();
			gameObject.transform.parent = HandTipsObj.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
		}
		tipObj.Reset(targetObj, offset, TUTORIAL_STEP.INVALID, duration);
		EnableTipCircleData.Add(tipObj);
	}

	public bool IsHaveTipsCircle(GameObject checktarget)
	{
		for (int i = 0; i < EnableTipCircleData.Count; i++)
		{
			if (checktarget == EnableTipCircleData[i].TargetObj)
			{
				return true;
			}
		}
		return false;
	}

	public void AddHandTips(GameObject targetObj, Vector3 offset, TUTORIAL_STEP step, bool isChild, string tips, SCREEN_DIRECTION tipPos, Vector3 tipOffset, float duration = -1f, Transform parentObj = null, bool isWorldObj = false, bool isShowHand = true)
	{
		if (SingletonUnity<MissionTeamTipLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTeamTipLogic>.Instance.gameObject))
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.CloseHandTip();
		}
		for (int i = 0; i < EnableHandTipCircleData.Count; i++)
		{
			if (targetObj == EnableHandTipCircleData[i].TargetObj)
			{
				if (duration > 0f)
				{
					EnableHandTipCircleData[i].TargetTime = Time.time + duration;
				}
				return;
			}
		}
		TipObj tipObj = null;
		if (DisableHandTipCircleData.Count > 0)
		{
			tipObj = DisableHandTipCircleData[0];
			DisableHandTipCircleData.RemoveAt(0);
		}
		if (tipObj == null || tipObj.gameObject == null)
		{
			GameObject gameObject = Object.Instantiate(HandTipsObj.gameObject) as GameObject;
			tipObj = gameObject.GetComponent<TipObj>();
			gameObject.transform.parent = HandTipsObj.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
		}
		tipObj.Reset(targetObj, offset, tips, tipPos, tipOffset, step, duration, isChild, parentObj, isWorldObj, isShowHand);
		tipObj.transform.localScale = Vector3.one;
		EnableHandTipCircleData.Add(tipObj);
	}

	public void ClearHandTips()
	{
		if (EnableHandTipCircleData.Count > 0)
		{
			for (int num = EnableHandTipCircleData.Count - 1; num >= 0; num--)
			{
				RemoveTips(EnableHandTipCircleData[num]);
			}
		}
	}

	public void RemoveTips(TipObj tipData)
	{
		tipData.transform.parent = base.transform;
		tipData.transform.localPosition = Vector3.zero;
		NGUITools.SetActive(tipData.gameObject, state: false);
		tipData.TargetObj = null;
		tipData.TipText = string.Empty;
		if (EnableTipCircleData.Contains(tipData))
		{
			EnableTipCircleData.Remove(tipData);
			if (tipData != null && tipData.gameObject != null)
			{
				DisableTipCircleData.Add(tipData);
			}
		}
		else if (EnableHandTipCircleData.Contains(tipData))
		{
			EnableHandTipCircleData.Remove(tipData);
			if (tipData != null && tipData.gameObject != null)
			{
				DisableHandTipCircleData.Add(tipData);
			}
		}
		TutorialManager.ClearTutorialEvent(tipData.TutorialStep);
	}

	public void RemoveTips(GameObject targetObj)
	{
		TipObj tipObj = null;
		for (int num = EnableTipCircleData.Count - 1; num >= 0; num--)
		{
			if (EnableTipCircleData[num] == null)
			{
				EnableTipCircleData.RemoveAt(num);
			}
			else if (EnableTipCircleData[num].TargetObj == null || EnableTipCircleData[num].TargetObj == targetObj)
			{
				tipObj = EnableTipCircleData[num];
				EnableTipCircleData.RemoveAt(num);
				DisableTipCircleData.Add(tipObj);
				tipObj.transform.parent = base.transform;
				tipObj.transform.localPosition = Vector3.zero;
				NGUITools.SetActive(tipObj.gameObject, state: false);
				if (tipObj.TempRoot != null)
				{
					GameObject tempRoot = tipObj.TempRoot;
					tipObj.TempRoot = null;
					Object.Destroy(tempRoot);
				}
			}
		}
		for (int num2 = EnableHandTipCircleData.Count - 1; num2 >= 0; num2--)
		{
			if (EnableHandTipCircleData[num2] == null)
			{
				EnableHandTipCircleData.RemoveAt(num2);
			}
			else if (EnableHandTipCircleData[num2].TargetObj == null || EnableHandTipCircleData[num2].TargetObj == targetObj)
			{
				tipObj = EnableHandTipCircleData[num2];
				EnableHandTipCircleData.RemoveAt(num2);
				DisableHandTipCircleData.Add(tipObj);
				tipObj.transform.parent = base.transform;
				tipObj.transform.localPosition = Vector3.zero;
				NGUITools.SetActive(tipObj.gameObject, state: false);
				tipObj.TipText = string.Empty;
				if (tipObj.TempRoot != null)
				{
					GameObject tempRoot2 = tipObj.TempRoot;
					tipObj.TempRoot = null;
					Object.Destroy(tempRoot2);
				}
			}
		}
		if (tipObj != null)
		{
			TutorialManager.ClearTutorialEvent(tipObj.TutorialStep);
		}
	}

	private void Update()
	{
		for (int num = EnableTipCircleData.Count - 1; num >= 0; num--)
		{
			if (EnableTipCircleData[num].Duration > 0f && Time.time > EnableTipCircleData[num].TargetTime)
			{
				RemoveTips(EnableTipCircleData[num]);
			}
		}
		for (int num2 = EnableHandTipCircleData.Count - 1; num2 >= 0; num2--)
		{
			if (!EnableHandTipCircleData[num2].IsWorldObj)
			{
				if (!EnableHandTipCircleData[num2].IsChild)
				{
					if (UnityVersionUtil.IsActive(EnableHandTipCircleData[num2].gameObject) != UnityVersionUtil.IsActive(EnableHandTipCircleData[num2].TargetObj))
					{
						UnityVersionUtil.SetActiveRecursive(EnableHandTipCircleData[num2].gameObject, UnityVersionUtil.IsActive(EnableHandTipCircleData[num2].TargetObj));
					}
					EnableHandTipCircleData[num2].transform.position = EnableHandTipCircleData[num2].TargetObj.transform.position + EnableHandTipCircleData[num2].OffsetPos;
				}
			}
			else if (mainCamera != null)
			{
				Vector3 vector = mainCamera.WorldToViewportPoint(EnableHandTipCircleData[num2].TargetObj.transform.position + Vector3.up * 1.1f);
				float num3 = Mathf.RoundToInt(480f * ((float)Screen.width / (float)Screen.height));
				Vector3 vector2 = new Vector3(vector.x * num3 - num3 / 2f, vector.y * 480f - 240f, 0f);
				EnableHandTipCircleData[num2].transform.localPosition = vector2 + EnableHandTipCircleData[num2].OffsetPos;
			}
			if (EnableHandTipCircleData[num2].Duration > 0f && Time.time > EnableHandTipCircleData[num2].TargetTime)
			{
				RemoveTips(EnableHandTipCircleData[num2]);
			}
		}
	}
}
