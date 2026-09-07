public class UIUpdateEvent
{
	public delegate void UpdateNoParamEvent();

	public delegate void OnStoryShowOverDelegate(string storyId);

	public static UpdateNoParamEvent UpdateMoneyEvent;

	public static UpdateNoParamEvent UpdateBackPackEvent;

	public static UpdateNoParamEvent UpdateBadgeBackPackEvent;

	public static UpdateNoParamEvent UpdateBadgeEquipPackEvent;

	public static UpdateNoParamEvent SyncBackPackEvent;

	public static OnStoryShowOverDelegate OnStoryShowOver;

	public static UpdateNoParamEvent LevelUpEvent;

	public static DelegateDefine.NoParamDelegate OnUIPageLoadFinished;

	public static DelegateDefine.NoParamDelegate OnChangeTeam;

	public static DelegateDefine.NoParamDelegate OnChangeGuild;

	public static UpdateNoParamEvent OnReshowBase;
}
