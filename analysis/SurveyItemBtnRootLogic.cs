using UnityEngine;

public class SurveyItemBtnRootLogic : SingletonUnity<SurveyItemBtnRootLogic>
{
	public GameObject BtnRoot;

	private SurveyItemObj mCurSurveyItemObj;

	public void Reset(SurveyItemObj curObj)
	{
		mCurSurveyItemObj = curObj;
	}

	public void OnClickSurveyBtn()
	{
		if ((SingletonUnity<SurveyProgressLineLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SurveyProgressLineLogic>.Instance.gameObject)) || !(mCurSurveyItemObj != null))
		{
			return;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer.IsLocalDrivingCar)
		{
			if (!SingletonUnity<CitySimController>.Exists)
			{
				return;
			}
			SingletonUnity<CitySimController>.Instance.RobCar(delegate
			{
				mainPlayer.MoveTo(mCurSurveyItemObj.transform.position, 1f, delegate
				{
					mainPlayer.FaceToPub(mCurSurveyItemObj.transform.position);
					Singleton<SurveyItemManager>.Instance.StartSurveyItem(mCurSurveyItemObj);
				});
			});
		}
		else
		{
			mainPlayer.MoveTo(mCurSurveyItemObj.transform.position, 1f, delegate
			{
				mainPlayer.FaceToPub(mCurSurveyItemObj.transform.position);
				Singleton<SurveyItemManager>.Instance.StartSurveyItem(mCurSurveyItemObj);
			});
		}
	}
}
