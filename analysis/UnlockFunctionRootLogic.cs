using DG.Tweening;
using UnityEngine;

public class UnlockFunctionRootLogic : SingletonUnity<UnlockFunctionRootLogic>
{
	public UILabel FunctionNameLabel;

	public UISprite FunctionIconPic;

	public GameObject OtherRoot;

	public Transform IconRoot;

	private FunctionData mCurFuncData;

	private vp_Timer.Handle curTimeHandle = new vp_Timer.Handle();

	public void ResetUnlockFunction(string functionId)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChatRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.HitOtherPlayerRoot);
		mCurFuncData = DataManager.GetFunctionDataById(functionId);
		if (mCurFuncData.UnlockType == 1)
		{
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				FunctionBtnRootLogic instance = SingletonUnity<FunctionBtnRootLogic>.Instance;
				bool openRight = false;
				UISprite uISprite = null;
				switch ((FUNCTION_TYPE)int.Parse(functionId))
				{
				case FUNCTION_TYPE.ACTIVITY:
					uISprite = instance.ActivityBtnIcon;
					break;
				case FUNCTION_TYPE.RANK:
					uISprite = instance.RankFuncBtn.IconSp;
					break;
				case FUNCTION_TYPE.GIFT:
					uISprite = instance.GiftFuncBtn.IconSp;
					break;
				case FUNCTION_TYPE.SHOP:
					uISprite = instance.ShopFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.LOTTO:
					uISprite = instance.SlotFuncBtn.IconSp;
					break;
				case FUNCTION_TYPE.FIRST_PAY:
					uISprite = instance.FirstSaleFuncBtn.IconSp;
					break;
				case FUNCTION_TYPE.CAR:
					uISprite = instance.CarFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.TITLE:
					uISprite = instance.TitleFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.ENHANCE:
					uISprite = instance.EnhanceFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.SKILL:
					uISprite = instance.SkillFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.CHARACTER:
					uISprite = instance.CharacterBtnIcon;
					openRight = true;
					break;
				case FUNCTION_TYPE.BAG:
					uISprite = instance.BagFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.GUILD:
					uISprite = instance.GuildFuncBtn.IconSp;
					openRight = true;
					break;
				case FUNCTION_TYPE.BIGSALES:
					uISprite = instance.BigSaleFuncBtn.IconSp;
					break;
				case FUNCTION_TYPE.MYSTERYSHOP:
					uISprite = instance.MysteryFuncBtn.IconSp;
					break;
				case FUNCTION_TYPE.AUTO_FIGHT:
					uISprite = instance.AutoBtnSprite;
					break;
				}
				if (uISprite != null)
				{
					ResetIconFunction(uISprite, mCurFuncData.MName, openRight);
				}
				else
				{
					SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.UnlockFunctionRoot);
					SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
				}
			}
		}
		else if (mCurFuncData.UnlockType != 0)
		{
		}
		if (mCurFuncData.SideMissionIdList == null)
		{
			return;
		}
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		for (int i = 0; i < mCurFuncData.SideMissionIdList.Length; i++)
		{
			if (missionManager.IsMissionAcceptable(mCurFuncData.SideMissionIdList[i]))
			{
				missionManager.AcceptMission(mCurFuncData.SideMissionIdList[i]);
			}
		}
	}

	public void ShowAcceptMissionEffect(string missionid)
	{
		ActivityMapData activityMapDataByActID = DataManager.GetActivityMapDataByActID(11, missionid);
		if (activityMapDataByActID != null)
		{
			ResetMissionIconFunction(activityMapDataByActID.Icon, SingletonUnity<MissionTeamTipLogic>.Instance.MissionBtn.transform.position);
			SingletonUnity<MissionTeamTipLogic>.Instance.ShowNewMissionFlag();
		}
		else
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.UnlockFunctionRoot);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
		}
	}

	public void ResetIconFunction(UISprite functionSourceIcon, string functionName, bool openRight)
	{
		FunctionNameLabel.text = functionName;
		FunctionIconPic.spriteName = functionSourceIcon.spriteName;
		FunctionIconPic.width = functionSourceIcon.width;
		FunctionIconPic.height = functionSourceIcon.height;
		curTimeHandle.Cancel();
		vp_Timer.In(2f, delegate
		{
			FunctionBtnRootLogic instance = SingletonUnity<FunctionBtnRootLogic>.Instance;
			instance.refershBtn();
			if (openRight)
			{
				if (!instance.IsOpenRightBtn)
				{
					instance.OnClickMenuBtn();
				}
				else
				{
					instance.ResetRightBtn();
				}
			}
			functionSourceIcon.alpha = 0f;
			if (FunctionIconPic != null)
			{
				FunctionIconPic.transform.parent = functionSourceIcon.transform;
				FunctionIconPic.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InCubic).OnComplete(delegate
				{
					NGUITools.SetActive(FunctionIconPic.gameObject, state: false);
					FunctionIconPic.transform.parent = IconRoot;
					FunctionIconPic.transform.localPosition = Vector3.zero;
					FunctionIconPic.transform.localRotation = Quaternion.identity;
					NGUITools.SetActive(functionSourceIcon.gameObject, state: true);
					functionSourceIcon.alpha = 1f;
					SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.UnlockFunctionRoot);
					if (mCurFuncData.FirstOpen == 1)
					{
						switch ((FUNCTION_TYPE)int.Parse(mCurFuncData.ID))
						{
						case FUNCTION_TYPE.SKILL:
							SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.SKILL);
							if (!Singleton<ObjManager>.Instance.MainPlayer.CheckSkillCanUpdate())
							{
							}
							break;
						case FUNCTION_TYPE.CAR:
							break;
						case FUNCTION_TYPE.TITLE:
						case FUNCTION_TYPE.ENHANCE:
							break;
						}
					}
					else
					{
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
					}
				});
				FunctionIconPic.transform.DOScale(Vector3.one * 0.633f, 0.5f);
			}
			NGUITools.SetActive(OtherRoot, state: false);
		}, curTimeHandle);
	}

	private void OnEnable()
	{
		Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
	}

	private void OnDisable()
	{
		curTimeHandle.Cancel();
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = false;
		}
	}

	public void ResetNoIconFunction(string functionName, Vector3 targetPos)
	{
		FunctionNameLabel.text = functionName;
		FunctionIconPic.enabled = false;
		vp_Timer.In(2f, delegate
		{
			FunctionIconPic.transform.parent = SingletonUnity<UIManager>.Instance.transform;
			FunctionIconPic.transform.DOMove(targetPos, 0.5f).SetEase(Ease.InCubic).OnComplete(delegate
			{
				Object.Destroy(FunctionIconPic.gameObject);
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
			});
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.UnlockFunctionRoot);
		});
	}

	public void ResetMissionIconFunction(string IconName, Vector3 targetPos)
	{
		FunctionNameLabel.text = string.Empty;
		FunctionIconPic.spriteName = IconName;
		FunctionIconPic.MakePixelPerfect();
		vp_Timer.In(2f, delegate
		{
			FunctionIconPic.transform.parent = SingletonUnity<UIManager>.Instance.transform;
			FunctionIconPic.transform.DOMove(targetPos, 0.5f).SetEase(Ease.InCubic).OnComplete(delegate
			{
				Object.Destroy(FunctionIconPic.gameObject);
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckShowUnlockFunction();
			});
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.UnlockFunctionRoot);
		});
	}
}
