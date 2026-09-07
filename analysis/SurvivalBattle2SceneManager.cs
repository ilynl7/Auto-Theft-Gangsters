using SprotoType;
using UnityEngine;

public class SurvivalBattle2SceneManager : SurvivalBattleSceneManager
{
	public override void MoveToNextFloor()
	{
		Debug.Log("MoveToNextFloor");
		Debug.Log("CurrentMapInofData.Param1 !!!!!!!!!!!!!!" + base.CurrentMapInofData.Param1);
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101592}", surviveBattleData.FirstMaxScore), StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			enter_survive_batttle.request rpcReq = new enter_survive_batttle.request
			{
				id = base.CurrentMapInofData.Param1,
				floor = 0L,
				type = 1L
			};
			NetLogic.GetInstance().Send<Protocol.enter_survive_batttle>(rpcReq);
		});
	}

	public override void OnLoadingOver()
	{
		base.OnLoadingOver();
		NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101587}"));
	}
}
