using System.Collections.Generic;
using SprotoType;

public class PopTipsRoot : SingletonUnity<PopTipsRoot>
{
	private static List<int> typeList = new List<int>();

	private static List<object> typeParmList = new List<object>();

	public UILabel contentLabel;

	public int curType;

	public UITweener tweener;

	public static void ShowPop(int type, object p)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.CurrentMapInofData != null && sceneManager.CurrentMapInofData.MapType == MAPTYPE.BAR_FIGHT_COPY)
		{
			return;
		}
		typeList.Add(type);
		typeParmList.Add(p);
		if (!SingletonUnity<PopTipsRoot>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<PopTipsRoot>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopTipsRoot, delegate
			{
				SingletonUnity<PopTipsRoot>.Instance.PlayNext();
			});
		}
	}

	public void PlayNext()
	{
		if (typeList.Count > 0)
		{
			curType = typeList[0];
		}
		tweener.ResetToBeginning();
		tweener.PlayForward();
		contentLabel.text = StrDictionary.GetDictionaryString("#{102052}");
	}

	public void PlayeFinish()
	{
		typeList.RemoveAt(0);
		typeParmList.RemoveAt(0);
		curType = -1;
		if (typeList.Count <= 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTipsRoot);
		}
		else
		{
			PlayNext();
		}
	}

	public void OnClickNo()
	{
		PlayeFinish();
	}

	public void OnClickYes()
	{
		if (curType == 0)
		{
			string iD = (string)typeParmList[0];
			enter_bar_fight.request request = new enter_bar_fight.request();
			request.ID = iD;
			WaitResponseUIRootLogic.OpenWaitBox(207, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.enter_bar_fight>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_4", "start");
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTipsRoot);
	}
}
