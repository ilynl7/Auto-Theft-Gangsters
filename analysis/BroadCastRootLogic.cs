using System.Collections.Generic;
using UnityEngine;

public class BroadCastRootLogic : SingletonUnity<BroadCastRootLogic>
{
	private static List<string> mMessageList = new List<string>();

	public List<UILabel> DisableLabelList;

	private List<UILabel> EnableLabelList = new List<UILabel>();

	public UISprite CirclePic;

	private int curLabelIndex;

	private float MOVE_SPEED = 50f;

	private void OnEnable()
	{
		if (EnableLabelList.Count > 0)
		{
			for (int i = 0; i < EnableLabelList.Count; i++)
			{
				NGUITools.SetActive(EnableLabelList[i].gameObject, state: false);
			}
		}
		if (DisableLabelList.Count > 0)
		{
			for (int j = 0; j < DisableLabelList.Count; j++)
			{
				NGUITools.SetActive(DisableLabelList[j].gameObject, state: false);
			}
		}
		CirclePic.alpha = 0f;
	}

	public static void AddMessage(string str)
	{
		if (SingletonUnity<BroadCastRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<BroadCastRootLogic>.Instance.gameObject))
		{
			SingletonUnity<BroadCastRootLogic>.Instance.AddMSG(str);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BroadCastRoot, delegate
		{
			SingletonUnity<BroadCastRootLogic>.Instance.AddMSG(str);
		});
	}

	public void AddMSG(string str)
	{
		mMessageList.Add(str);
	}

	private UILabel GetCurEnableLabel()
	{
		if (DisableLabelList.Count > 0)
		{
			UILabel result = DisableLabelList[0];
			DisableLabelList.RemoveAt(0);
			CirclePic.alpha = 1f;
			return result;
		}
		return null;
	}

	private void RecycleLabel(UILabel curLabel)
	{
		EnableLabelList.Remove(curLabel);
		DisableLabelList.Add(curLabel);
		UnityVersionUtil.SetActiveRecursive(curLabel.gameObject, state: false);
	}

	private void Update()
	{
		if (EnableLabelList.Count > 0)
		{
			for (int num = EnableLabelList.Count - 1; num >= 0; num--)
			{
				EnableLabelList[num].transform.localPosition = EnableLabelList[num].transform.localPosition + Vector3.left * MOVE_SPEED * Time.deltaTime;
				if (EnableLabelList[num].transform.localPosition.x < (float)(-CirclePic.width / 2 - EnableLabelList[num].width))
				{
					RecycleLabel(EnableLabelList[num]);
				}
			}
		}
		if (mMessageList.Count > 0)
		{
			UILabel curEnableLabel = GetCurEnableLabel();
			if (curEnableLabel != null)
			{
				curEnableLabel.text = mMessageList[0];
				mMessageList.RemoveAt(0);
				UnityVersionUtil.SetActiveRecursive(curEnableLabel.gameObject, state: true);
				if (EnableLabelList.Count > 0)
				{
					curEnableLabel.transform.localPosition = Vector3.right * Mathf.Max(EnableLabelList[EnableLabelList.Count - 1].transform.localPosition.x + (float)EnableLabelList[EnableLabelList.Count - 1].width + 30f, CirclePic.width / 2);
				}
				else
				{
					curEnableLabel.transform.localPosition = Vector3.right * CirclePic.width / 2f;
				}
				EnableLabelList.Add(curEnableLabel);
			}
		}
		if (EnableLabelList.Count <= 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BroadCastRoot);
		}
	}

	private void OnDisable()
	{
		if (EnableLabelList == null || EnableLabelList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < EnableLabelList.Count; i++)
		{
			if (EnableLabelList[i] != null && EnableLabelList[i].transform.localPosition.x > 0f)
			{
				AddMSG(EnableLabelList[i].text);
			}
		}
	}
}
