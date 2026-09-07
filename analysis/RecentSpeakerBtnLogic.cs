using UnityEngine;

public class RecentSpeakerBtnLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UIWidget BottomSprite;

	public RecentSpeaker SpeakerInfo;

	public BoxCollider Collider;

	public UISprite TipPic;

	public UISprite SelfTipPic;

	public void Reset(RecentSpeaker speakerInfo)
	{
		NameLabel.text = speakerInfo.Name;
		BottomSprite.width = NameLabel.width + 20;
		SpeakerInfo = speakerInfo;
		Collider.size = new Vector3(BottomSprite.width, BottomSprite.height, 1f);
		SelfTipPic.transform.localPosition = new Vector3(NameLabel.width / 2, BottomSprite.height / 2, 0f);
		if (speakerInfo.NewMessageFlag)
		{
			UnityVersionUtil.SetActiveRecursive(TipPic.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(SelfTipPic.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TipPic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(SelfTipPic.gameObject, state: false);
		}
	}

	public void OnClickBtn()
	{
		if (SingletonUnity<ChatUIRootLogic>.Exists)
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickRecentSpeaker(SpeakerInfo);
		}
	}
}
