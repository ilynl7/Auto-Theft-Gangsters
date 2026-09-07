using SprotoType;
using UnityEngine;

public class ScuffleAreaInfoRoot : SingletonUnity<ScuffleAreaInfoRoot>
{
	public UILabel TypeLabel;

	public UISprite BtnPic;

	private int floorId;

	private ScuffleData curData;

	public TweenPosition TwPosition;

	public void Reset(int floorid, ScuffleData data)
	{
		curData = data;
		floorId = floorid;
		if (floorid == 0)
		{
			TypeLabel.text = StrDictionary.GetDictionaryString("#{102019}");
			BtnPic.spriteName = "CZ_tuBiao_Up";
			BtnPic.color = new Color(1f, 73f / 85f, 0f, 1f);
			TwPosition.enabled = true;
		}
		else
		{
			TypeLabel.text = StrDictionary.GetDictionaryString("#{102020}");
			BtnPic.spriteName = "CZ_tuBiao_Down";
			BtnPic.color = Color.white;
			TwPosition.enabled = false;
		}
	}

	public void OnClickBtn()
	{
		if (floorId == 0)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_scuffle_batttle.request request = new enter_scuffle_batttle.request();
			request.ID = curData.ID;
			request.floor = 1L;
			NetLogic.GetInstance().Send<Protocol.enter_scuffle_batttle>(request);
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_scuffle_batttle.request request2 = new enter_scuffle_batttle.request();
			request2.ID = curData.ID;
			request2.floor = 0L;
			NetLogic.GetInstance().Send<Protocol.enter_scuffle_batttle>(request2);
		}
	}
}
