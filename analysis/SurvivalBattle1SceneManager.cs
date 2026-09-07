using SprotoType;

public class SurvivalBattle1SceneManager : SurvivalBattleSceneManager
{
	public override void MoveToNextFloor()
	{
		if (mPlayerScores >= surviveBattleData.SecondMinScore)
		{
			enter_survive_batttle.request request = new enter_survive_batttle.request();
			request.id = base.CurrentMapInofData.Param1;
			request.floor = 1L;
			NetLogic.GetInstance().Send<Protocol.enter_survive_batttle>(request);
		}
		else
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101590}", surviveBattleData.SecondMinScore));
		}
	}

	public override void UpdatePlayerScore(long playerScores)
	{
		mPlayerScores = playerScores;
		if (SingletonUnity<SurviveBattleFloorInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SurviveBattleFloorInfoRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SurviveBattleFloorInfoRootLogic>.Instance.UpdateBtnEnable(mPlayerScores >= surviveBattleData.SecondMinScore);
		}
		if (!mHasShowMessageBox && mPlayerScores >= surviveBattleData.FirstMaxScore)
		{
			mHasShowMessageBox = true;
			MessageBoxLogic.OpenOKCancelBox("#{103001}", "#{100127}", delegate
			{
				MoveToNextFloor();
			});
		}
	}
}
