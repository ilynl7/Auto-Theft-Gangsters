using SprotoType;

public class SurviveBattleResultRootLogic : SingletonUnity<SurviveBattleResultRootLogic>
{
	public UILabel ResultLabel;

	public void Reset(survive_battle_finish.request request)
	{
		ResultLabel.text = StrDictionary.GetServerDictionaryString(request.info);
	}

	public void OnClickOkBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}

	private void OnEnable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
		}
	}

	private void OnDisable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = false;
		}
	}
}
