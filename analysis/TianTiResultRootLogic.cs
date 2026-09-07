using SprotoType;
using UnityEngine;

public class TianTiResultRootLogic : SingletonUnity<TianTiResultRootLogic>
{
	public UILabel titleLabel;

	public UILabel RecordnameLabel;

	public UILabel bestLabel;

	public UILabel ranknameLabel;

	public UILabel rankLabel;

	public UILabel deltaLabel;

	public Transform rewardOffset;

	public UISprite midline;

	public ShowRewardItems showrewarditem;

	public void Reset(tiantti_result.request request)
	{
		if (request.win)
		{
			if (request.HasBestRankPos)
			{
				titleLabel.text = StrDictionary.GetDictionaryString("#{101009}");
				NGUITools.SetActive(RecordnameLabel.gameObject, state: true);
				RecordnameLabel.text = StrDictionary.GetDictionaryString("#{101009}");
				bestLabel.text = $"{request.bestRankPos}";
			}
			else
			{
				NGUITools.SetActive(RecordnameLabel.gameObject, state: false);
				titleLabel.text = StrDictionary.GetDictionaryString("#{101012}");
			}
			NGUITools.SetActive(ranknameLabel.gameObject, state: true);
			ranknameLabel.text = StrDictionary.GetDictionaryString("#{101010}");
			rankLabel.text = $"{request.rankPos2}";
			deltaLabel.text = $"(   {request.rankPos1 - request.rankPos2})";
			NGUITools.SetActive(deltaLabel.gameObject, request.rankPos1 - request.rankPos2 > 0);
			midline.enabled = true;
			rewardOffset.localPosition = Vector3.zero;
		}
		else
		{
			titleLabel.text = StrDictionary.GetDictionaryString("#{101013}");
			NGUITools.SetActive(RecordnameLabel.gameObject, state: false);
			NGUITools.SetActive(ranknameLabel.gameObject, state: false);
			midline.enabled = false;
			rewardOffset.localPosition = new Vector3(0f, 36f, 0f);
		}
		if (request.HasItems)
		{
			showrewarditem.ShowRewards(request.items);
		}
	}

	public void OnClickExitBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}
}
