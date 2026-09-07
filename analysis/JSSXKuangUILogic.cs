using UnityEngine;

public class JSSXKuangUILogic : MonoBehaviour
{
	public UISprite ATKsp;

	public UISprite HPsp;

	public UISprite DEFsp;

	public UISprite HITsp;

	public UISprite DGEsp;

	public UISprite CRIsp;

	public UISprite RESsp;

	public UISprite EXDsp;

	public UISprite EXRsp;

	public UILabel ATKValLabel;

	public UILabel HPValLabel;

	public UILabel DEFValLabel;

	public UILabel HITValLabel;

	public UILabel DGEValLabel;

	public UILabel CRIValLabel;

	public UILabel RESValLabel;

	public UILabel EXDValLabel;

	public UILabel EXRValLabel;

	public UILabel ATKLabel;

	public UILabel HPLabel;

	public UILabel DEFLabel;

	public UILabel HITLabel;

	public UILabel DGELabel;

	public UILabel CRILabel;

	public UILabel RESLabel;

	public UILabel EXDLabel;

	public UILabel EXRLabel;

	public void Reset(CharacterAttributeData data)
	{
		HPValLabel.text = data.MaxHP.ToString();
		ATKValLabel.text = data.ATK.ToString();
		CRIValLabel.text = data.CRI.ToString();
		HITValLabel.text = data.HIT.ToString();
		DEFValLabel.text = data.DEF.ToString();
		DGEValLabel.text = data.DGE.ToString();
		EXDValLabel.text = $"{data.EXD:P1}";
		EXRValLabel.text = $"{data.EXR:P1}";
		RESValLabel.text = data.RES.ToString();
		ATKLabel.text = GameDefine.GetAttributeName(1001);
		HPLabel.text = GameDefine.GetAttributeName(1002);
		DEFLabel.text = GameDefine.GetAttributeName(1003);
		HITLabel.text = GameDefine.GetAttributeName(1004);
		DGELabel.text = GameDefine.GetAttributeName(1005);
		CRILabel.text = GameDefine.GetAttributeName(1006);
		RESLabel.text = GameDefine.GetAttributeName(1007);
		EXDLabel.text = GameDefine.GetAttributeName(1008);
		EXRLabel.text = GameDefine.GetAttributeName(1009);
		ATKsp.spriteName = GameDefine.GetAttributeIcon(1001);
		HPsp.spriteName = GameDefine.GetAttributeIcon(1002);
		DEFsp.spriteName = GameDefine.GetAttributeIcon(1003);
		HITsp.spriteName = GameDefine.GetAttributeIcon(1004);
		DGEsp.spriteName = GameDefine.GetAttributeIcon(1005);
		CRIsp.spriteName = GameDefine.GetAttributeIcon(1006);
		RESsp.spriteName = GameDefine.GetAttributeIcon(1007);
		EXDsp.spriteName = GameDefine.GetAttributeIcon(1008);
		EXRsp.spriteName = GameDefine.GetAttributeIcon(1009);
	}

	public void Show(CharacterAttributeData data)
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		Reset(data);
	}

	public void Show()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
	}

	public void Hide()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
	}

	public void OnClickTipBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{100633}", "#{100634}", null);
		});
	}
}
