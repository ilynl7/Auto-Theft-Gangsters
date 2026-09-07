using UnityEngine;

public class ExitGameRoot : SingletonUnity<ExitGameRoot>
{
	public Transform upTra;

	public Transform downTra;

	public void Reset()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.IsFullScreenSmallReady() && !LocalDataSaveManager.AdFree)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.ShowFullScreenExitSmall();
			upTra.localPosition = new Vector3(0f, 167f, 0f);
			downTra.localPosition = new Vector3(0f, -182f, 0f);
		}
		else
		{
			upTra.localPosition = new Vector3(0f, 40f, 0f);
			downTra.localPosition = new Vector3(0f, -30f, 0f);
		}
	}

	public void CloseAd()
	{
		upTra.localPosition = new Vector3(0f, 40f, 0f);
		downTra.localPosition = new Vector3(0f, -30f, 0f);
	}

	public void OnClickExitBtn()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ExitGameRoot);
		Application.Quit();
	}

	public void OnClickMoreGame()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.ShowMoreGames();
	}

	public void OnClickNoBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ExitGameRoot);
		if (SingletonDontDestoryUnity<GameManager>.Instance.IsFullScreenSmallShowing())
		{
			SingletonDontDestoryUnity<GameManager>.Instance.HideFullScreenSmall();
		}
	}
}
