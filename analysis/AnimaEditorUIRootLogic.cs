using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimaEditorUIRootLogic : SingletonUnity<AnimaEditorUIRootLogic>
{
	public UISprite SkillBtnPic;

	public UIPopupList uiPopList;

	public UIPopupList EffPopList;

	public UILabel EffIdLabel;

	public UILabel SkillIdLabel;

	public UISprite ComboSkillCDPic;

	public UISprite MoveSkillCDPic;

	public UISprite NormalSkillCDPic;

	public UIInput CDInput;

	public UIInput TraceDistanceInput;

	public UIInput MoveTimeInput;

	public UIInput MoveDisInput;

	public UIInput EffTime1Input;

	public UIInput EffTime2Input;

	public UIInput EffTime3Input;

	public UIInput AnimaDurationInput;

	public UIInput CrossInTimeInput;

	public UIInput CrossOutTimeInput;

	public UIInput FxEffTimeInput;

	public UIInput FxEffDelayInput;

	public UIInput FxEffLinkNodeInput;

	public UIInput NPCIDInput;

	public UIInput FXPosX;

	public UIInput FXPosY;

	public UIInput FXPosZ;

	public UIInput FXAngleX;

	public UIInput FXAngleY;

	public UIInput FXAngleZ;

	public UIInput NPCHPInput;

	public UIToggle IsCtlPlayer;

	public UIPopupList B_ActPopList;

	public UIPopupList B_EffPopList;

	public UILabel B_ActIDLabel;

	public UILabel B_EffIDLabel;

	public UIInput B_AnimaDurationInput;

	public UIInput B_CrossInTimeInput;

	public UIInput B_CrossOutTimeInput;

	public UIInput B_FxEffTimeInput;

	public UIInput B_FxEffDelayInput;

	public UIInput B_FXPosX;

	public UIInput B_FXPosY;

	public UIInput B_FXPosZ;

	public UIInput B_FXAngleX;

	public UIInput B_FXAngleY;

	public UIInput B_FXAngleZ;

	public UIInput B_LinkNode;

	public UIPopupList CamRockIDPopList;

	public UILabel CamRockIDLabel;

	public UIInput CamRockTimeInput;

	public UIInput CamRockDelayInput;

	private ObjMainPlayer mainPlayer;

	private ObjNPC curNPC;

	private string curSkillID = string.Empty;

	private SkillData curSkillData;

	private ActionData curActionData;

	private int curEffID = -1;

	private Dictionary<string, ActionData> curAttackedActionDataDic = new Dictionary<string, ActionData>();

	private ActionData curAttackedActionData;

	private Dictionary<string, FxEffInfoData> curAttackedEffectDataDic = new Dictionary<string, FxEffInfoData>();

	private FxEffInfoData curAttackedEffectData;

	private List<FxEffInfoData> curFxEffInfoDataDic = new List<FxEffInfoData>();

	private FxEffInfoData curFxEffInfoData;

	private List<CharacterSkillData> mCurChaSkillData = new List<CharacterSkillData>();

	private Dictionary<string, SkillData> mCurSkillDataDic = new Dictionary<string, SkillData>();

	private List<CamRockData> mChangedCamRockData = new List<CamRockData>();

	private Dictionary<string, CamRockData> mCurCamRockDataDic = new Dictionary<string, CamRockData>();

	private CamRockData mCurCamRockData;

	private Dictionary<string, SkillData> ChangedSkillData = new Dictionary<string, SkillData>();

	private List<ActionData> ChangedActionData = new List<ActionData>();

	private List<FxEffInfoData> ChangedFxEffInfoData = new List<FxEffInfoData>();

	private new void Awake()
	{
		base.Awake();
		mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		SingletonUnity<MyEvent>.Instance.Register("OnNPCDie", this, "OnNPCDie");
	}

	public void OnNPCDie(object obj)
	{
		ObjNPC objNPC = obj as ObjNPC;
		if (objNPC == curNPC)
		{
			curNPC = null;
		}
	}

	public void Init()
	{
		mCurChaSkillData = mainPlayer.CharacterSkillData;
		List<string> list = new List<string>();
		for (int i = 0; i < mCurChaSkillData.Count; i++)
		{
			mCurSkillDataDic.Add(mCurChaSkillData[i].ID, DataManager.GetSkillDataById(mCurChaSkillData[i].ID));
			list.Add(mCurChaSkillData[i].ID);
		}
		uiPopList.items = list;
	}

	public void UseSkill_1_Onclick()
	{
		OnChangeSkillVal();
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			mainPlayer.UseComboSkill();
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	public void UseSkill_2_Onclick()
	{
		OnChangeSkillVal();
		if (IsCtlPlayer.value)
		{
			if (mainPlayer == null)
			{
				mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			}
			if (mainPlayer.IsHaveWeapon())
			{
				if (!string.IsNullOrEmpty(curSkillID))
				{
					mainPlayer.UseSkill(curSkillID);
				}
			}
			else
			{
				NoticeLogic.AddNotifyData("#{200062}");
			}
		}
		else if (!(curNPC == null))
		{
			curNPC.UseSkill(curSkillID);
		}
	}

	public void UseSkill_S_Onclick()
	{
		OnChangeSkillVal();
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			if (!string.IsNullOrEmpty(mCurChaSkillData[3].ID))
			{
				mainPlayer.UseSkill(mCurChaSkillData[3].ID);
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	public void OnSelectSkill(GameObject labelObj)
	{
		UILabel component = labelObj.GetComponent<UILabel>();
		if (!(component == null))
		{
			SelectSkill(component.text);
		}
	}

	public void SelectSkill(string id)
	{
		SkillIdLabel.text = id;
		curSkillID = id;
		curSkillData = mCurSkillDataDic[curSkillID];
		if (IsCtlPlayer.value)
		{
			curActionData = DataManager.GetActionDataByName(mainPlayer.GetActionName(curSkillData.ActionName));
		}
		else
		{
			curActionData = DataManager.GetActionDataByName(curNPC.GetActionName(curSkillData.ActionName));
		}
		curFxEffInfoDataDic.Clear();
		EffIdLabel.text = string.Empty;
		if (!string.IsNullOrEmpty(curActionData.FxEffID))
		{
			List<FxEffInfoData> fxEffInfoDataListById = DataManager.GetFxEffInfoDataListById(curActionData.FxEffID);
			for (int i = 0; i < fxEffInfoDataListById.Count; i++)
			{
				curFxEffInfoDataDic.Add(fxEffInfoDataListById[i]);
			}
		}
		SetSkillData();
		SkillBtnPic.spriteName = curSkillData.Icon;
		curAttackedActionDataDic.Clear();
		if (!string.IsNullOrEmpty(curSkillData.EffId_0))
		{
			EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(curSkillData.EffId_0);
			string text = string.Empty;
			if (IsCtlPlayer.value)
			{
				if (curNPC != null)
				{
					text = curNPC.GetActionName(effInfoDataById.HitAction);
				}
			}
			else
			{
				text = mainPlayer.GetActionName(effInfoDataById.HitAction);
			}
			ActionData actionDataByName = DataManager.GetActionDataByName(text);
			if (actionDataByName != null && !curAttackedActionDataDic.ContainsKey(actionDataByName.ID))
			{
				curAttackedActionDataDic.Add(actionDataByName.ID, actionDataByName);
			}
		}
		if (!string.IsNullOrEmpty(curSkillData.EffId_1))
		{
			EffInfoData effInfoDataById2 = DataManager.GetEffInfoDataById(curSkillData.EffId_1);
			string text2 = string.Empty;
			if (IsCtlPlayer.value)
			{
				if (curNPC != null)
				{
					text2 = curNPC.GetActionName(effInfoDataById2.HitAction);
				}
			}
			else
			{
				text2 = mainPlayer.GetActionName(effInfoDataById2.HitAction);
			}
			ActionData actionDataByName2 = DataManager.GetActionDataByName(text2);
			if (actionDataByName2 != null && !curAttackedActionDataDic.ContainsKey(actionDataByName2.ID))
			{
				curAttackedActionDataDic.Add(actionDataByName2.ID, actionDataByName2);
			}
		}
		if (!string.IsNullOrEmpty(curSkillData.EffId_2))
		{
			EffInfoData effInfoDataById3 = DataManager.GetEffInfoDataById(curSkillData.EffId_2);
			string text3 = string.Empty;
			if (IsCtlPlayer.value)
			{
				if (curNPC != null)
				{
					text3 = curNPC.GetActionName(effInfoDataById3.HitAction);
				}
			}
			else
			{
				text3 = mainPlayer.GetActionName(effInfoDataById3.HitAction);
			}
			ActionData actionDataByName3 = DataManager.GetActionDataByName(text3);
			if (actionDataByName3 != null && !curAttackedActionDataDic.ContainsKey(actionDataByName3.ID))
			{
				curAttackedActionDataDic.Add(actionDataByName3.ID, actionDataByName3);
			}
		}
		if (curAttackedActionDataDic.Count > 0)
		{
			List<string> items = new List<string>(curAttackedActionDataDic.Keys);
			B_ActPopList.items.Clear();
			B_ActPopList.items = items;
			B_ActIDLabel.text = B_ActPopList.items[0];
			SetAttackedAct(B_ActPopList.items[0]);
		}
		else
		{
			SetAttackedAct(string.Empty);
		}
		mCurCamRockDataDic.Clear();
		if (curSkillData.CamRockIDList != null && curSkillData.CamRockIDList.Length > 0)
		{
			CamRockIDPopList.items.Clear();
			CamRockIDPopList.items = new List<string>(curSkillData.CamRockIDList);
			CamRockIDLabel.text = CamRockIDPopList.items[0];
			for (int j = 0; j < curSkillData.CamRockIDList.Length; j++)
			{
				mCurCamRockDataDic.Add(curSkillData.CamRockIDList[j], DataManager.GetCamRockDataByID(curSkillData.CamRockIDList[j]));
			}
			SelectCamRockID(CamRockIDPopList.items[0]);
		}
	}

	public void OnSelectCamRockID(GameObject label)
	{
		UILabel component = label.GetComponent<UILabel>();
		SelectCamRockID(component.text);
	}

	private void SelectCamRockID(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			mCurCamRockData = mCurCamRockDataDic[id];
			CamRockTimeInput.value = $"{mCurCamRockData.NeedRockTime}";
			CamRockDelayInput.value = $"{mCurCamRockData.DelayTime}";
		}
		else
		{
			mCurCamRockData = null;
			CamRockTimeInput.value = string.Empty;
			CamRockDelayInput.value = string.Empty;
		}
	}

	public void OnSelectAttackedAct(GameObject label)
	{
		UILabel component = label.GetComponent<UILabel>();
		SetAttackedAct(component.text);
	}

	public void SetAttackedAct(string actId)
	{
		if (!string.IsNullOrEmpty(actId))
		{
			curAttackedActionData = curAttackedActionDataDic[actId];
			B_AnimaDurationInput.value = $"{curAttackedActionData.AnimDurationTime}";
			B_CrossInTimeInput.value = $"{curAttackedActionData.CrossInTime}";
			B_CrossOutTimeInput.value = $"{curAttackedActionData.CrossOutTime}";
			curAttackedEffectDataDic.Clear();
			B_EffIDLabel.text = string.Empty;
			if (!string.IsNullOrEmpty(curAttackedActionData.FxEffID))
			{
				List<FxEffInfoData> fxEffInfoDataListById = DataManager.GetFxEffInfoDataListById(curAttackedActionData.FxEffID);
				for (int i = 0; i < fxEffInfoDataListById.Count; i++)
				{
					curAttackedEffectDataDic.Add(fxEffInfoDataListById[i].EffName, fxEffInfoDataListById[i]);
				}
			}
			B_EffPopList.items.Clear();
			if (curAttackedEffectDataDic != null && curAttackedEffectDataDic.Count != 0)
			{
				List<string> list = new List<string>(curAttackedEffectDataDic.Keys);
				B_EffPopList.items = list;
				B_EffIDLabel.text = list[0];
				SetAttackedFxEff(list[0]);
			}
			else
			{
				SetAttackedFxEff(string.Empty);
			}
		}
		else
		{
			curAttackedActionData = null;
			B_AnimaDurationInput.value = string.Empty;
			B_CrossInTimeInput.value = string.Empty;
			B_CrossOutTimeInput.value = string.Empty;
		}
	}

	public void OnSelectAttackedEff(GameObject label)
	{
		UILabel component = label.GetComponent<UILabel>();
		SetAttackedFxEff(component.text);
	}

	public void SetAttackedFxEff(string fxEffId)
	{
		if (!string.IsNullOrEmpty(fxEffId))
		{
			curAttackedEffectData = curAttackedEffectDataDic[fxEffId];
			B_FxEffTimeInput.value = $"{curAttackedEffectData.EffDurationTime}";
			B_FxEffDelayInput.value = $"{curAttackedEffectData.EffDelayTime}";
			B_FXPosX.value = $"{curAttackedEffectData.PX}";
			B_FXPosY.value = $"{curAttackedEffectData.PY}";
			B_FXPosZ.value = $"{curAttackedEffectData.PZ}";
			B_FXAngleX.value = $"{curAttackedEffectData.AX}";
			B_FXAngleY.value = $"{curAttackedEffectData.AY}";
			B_FXAngleZ.value = $"{curAttackedEffectData.AZ}";
			B_LinkNode.value = curAttackedEffectData.EffLinkNode;
		}
		else
		{
			curAttackedEffectData = null;
			B_FxEffTimeInput.value = string.Empty;
			B_FxEffDelayInput.value = string.Empty;
			B_FXPosX.value = string.Empty;
			B_FXPosY.value = string.Empty;
			B_FXPosZ.value = string.Empty;
			B_FXAngleX.value = string.Empty;
			B_FXAngleY.value = string.Empty;
			B_FXAngleZ.value = string.Empty;
			B_LinkNode.value = string.Empty;
		}
	}

	public void SetSkillData()
	{
		if (curSkillData != null)
		{
			CDInput.value = $"{curSkillData.CD}";
			TraceDistanceInput.value = $"{curSkillData.TraceDistance}";
			MoveTimeInput.value = $"{curSkillData.MoveTime}";
			MoveDisInput.value = $"{curSkillData.MoveDistance}";
			EffTime1Input.value = $"{curSkillData.EffTime_0}";
			EffTime2Input.value = $"{curSkillData.EffTime_1}";
			EffTime3Input.value = $"{curSkillData.EffTime_2}";
		}
		if (curActionData != null)
		{
			AnimaDurationInput.value = $"{curActionData.AnimDurationTime}";
			CrossInTimeInput.value = $"{curActionData.CrossInTime}";
			CrossOutTimeInput.value = $"{curActionData.CrossOutTime}";
		}
		EffPopList.items.Clear();
		if (curFxEffInfoDataDic != null && curFxEffInfoDataDic.Count != 0)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < curFxEffInfoDataDic.Count; i++)
			{
				list.Add(i.ToString());
			}
			EffPopList.items = list;
			EffIdLabel.text = list[0];
			SelectFxEff(list[0]);
		}
		else
		{
			SelectFxEff(string.Empty);
		}
	}

	public void OnSelectFxEff(GameObject labelObj)
	{
		UILabel component = labelObj.GetComponent<UILabel>();
		if (!(component == null))
		{
			SelectFxEff(component.text);
		}
	}

	private void SelectFxEff(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			curEffID = int.Parse(id);
			curFxEffInfoData = curFxEffInfoDataDic[curEffID];
			FxEffTimeInput.value = $"{curFxEffInfoData.EffDurationTime}";
			FxEffDelayInput.value = $"{curFxEffInfoData.EffDelayTime}";
			FxEffLinkNodeInput.value = curFxEffInfoData.EffLinkNode;
			FXPosX.value = $"{curFxEffInfoData.PX}";
			FXPosY.value = $"{curFxEffInfoData.PY}";
			FXPosZ.value = $"{curFxEffInfoData.PZ}";
			FXAngleX.value = $"{curFxEffInfoData.AX}";
			FXAngleY.value = $"{curFxEffInfoData.AY}";
			FXAngleZ.value = $"{curFxEffInfoData.AZ}";
		}
		else
		{
			curFxEffInfoData = null;
			FxEffTimeInput.value = string.Empty;
			FxEffDelayInput.value = string.Empty;
			FxEffLinkNodeInput.value = string.Empty;
			FXPosX.value = string.Empty;
			FXPosY.value = string.Empty;
			FXPosZ.value = string.Empty;
			FXAngleX.value = string.Empty;
			FXAngleY.value = string.Empty;
			FXAngleZ.value = string.Empty;
		}
	}

	private void Update()
	{
		UpdateSKill();
	}

	private void UpdateSKill()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer != null)
		{
			ComboSkillCDPic.fillAmount = mainPlayer.GetComboSKillTimePercent();
			MoveSkillCDPic.fillAmount = mainPlayer.GetSkillTimePercent(mCurChaSkillData[3].ID);
			NormalSkillCDPic.fillAmount = mainPlayer.GetSkillTimePercent(curSkillID);
		}
	}

	public void OnChangeSkillVal()
	{
		if (curSkillData != null)
		{
			int result = 0;
			if (int.TryParse(CDInput.value, out result))
			{
				curSkillData.CD = result;
			}
			int result2 = 0;
			if (int.TryParse(TraceDistanceInput.value, out result2))
			{
				curSkillData.TraceDistance = result2;
			}
			int result3 = 0;
			if (int.TryParse(MoveTimeInput.value, out result3))
			{
				curSkillData.MoveTime = result3;
			}
			int result4 = 0;
			if (int.TryParse(MoveDisInput.value, out result4))
			{
				curSkillData.MoveDistance = result4;
			}
			int result5 = 0;
			if (int.TryParse(EffTime1Input.value, out result5))
			{
				curSkillData.EffTime_0 = result5;
			}
			int result6 = 0;
			if (int.TryParse(EffTime2Input.value, out result6))
			{
				curSkillData.EffTime_1 = result6;
			}
			int result7 = 0;
			if (int.TryParse(EffTime3Input.value, out result7))
			{
				curSkillData.EffTime_2 = result7;
			}
			if (!ChangedSkillData.ContainsKey(curSkillData.ID))
			{
				ChangedSkillData.Add(curSkillData.ID, curSkillData);
			}
		}
		if (curActionData != null)
		{
			int result8 = 0;
			if (int.TryParse(AnimaDurationInput.value, out result8))
			{
				curActionData.AnimDurationTime = result8;
			}
			int result9 = 0;
			if (int.TryParse(CrossInTimeInput.value, out result9))
			{
				curActionData.CrossInTime = result9;
			}
			int result10 = 0;
			if (int.TryParse(CrossOutTimeInput.value, out result10))
			{
				curActionData.CrossOutTime = result10;
			}
			if (!ChangedActionData.Contains(curActionData))
			{
				ChangedActionData.Add(curActionData);
			}
		}
		if (curFxEffInfoData != null)
		{
			int result11 = 0;
			if (int.TryParse(FxEffTimeInput.value, out result11))
			{
				curFxEffInfoData.EffDurationTime = result11;
			}
			int result12 = 0;
			if (int.TryParse(FxEffDelayInput.value, out result12))
			{
				curFxEffInfoData.EffDelayTime = result12;
			}
			curFxEffInfoData.EffLinkNode = FxEffLinkNodeInput.value;
			float result13 = 0f;
			if (float.TryParse(FXPosX.value, out result13))
			{
				curFxEffInfoData.PX = result13;
			}
			float result14 = 0f;
			if (float.TryParse(FXPosY.value, out result14))
			{
				curFxEffInfoData.PY = result14;
			}
			float result15 = 0f;
			if (float.TryParse(FXPosZ.value, out result15))
			{
				curFxEffInfoData.PZ = result15;
			}
			float result16 = 0f;
			if (float.TryParse(FXAngleX.value, out result16))
			{
				curFxEffInfoData.AX = result16;
			}
			float result17 = 0f;
			if (float.TryParse(FXAngleY.value, out result17))
			{
				curFxEffInfoData.AY = result17;
			}
			float result18 = 0f;
			if (float.TryParse(FXAngleZ.value, out result18))
			{
				curFxEffInfoData.AZ = result18;
			}
			if (!ChangedFxEffInfoData.Contains(curFxEffInfoData))
			{
				ChangedFxEffInfoData.Add(curFxEffInfoData);
			}
		}
		if (curAttackedActionData != null)
		{
			int result19 = 0;
			if (int.TryParse(B_AnimaDurationInput.value, out result19))
			{
				curAttackedActionData.AnimDurationTime = result19;
			}
			int result20 = 0;
			if (int.TryParse(B_CrossInTimeInput.value, out result20))
			{
				curAttackedActionData.CrossInTime = result20;
			}
			int result21 = 0;
			if (int.TryParse(B_CrossOutTimeInput.value, out result21))
			{
				curAttackedActionData.CrossOutTime = result21;
			}
			if (!ChangedActionData.Contains(curAttackedActionData))
			{
				ChangedActionData.Add(curAttackedActionData);
			}
		}
		if (curAttackedEffectData != null)
		{
			int result22 = 0;
			if (int.TryParse(B_FxEffTimeInput.value, out result22))
			{
				curAttackedEffectData.EffDurationTime = result22;
			}
			int result23 = 0;
			if (int.TryParse(B_FxEffDelayInput.value, out result23))
			{
				curAttackedEffectData.EffDelayTime = result23;
			}
			float result24 = 0f;
			if (float.TryParse(B_FXPosX.value, out result24))
			{
				curAttackedEffectData.PX = result24;
			}
			float result25 = 0f;
			if (float.TryParse(B_FXPosY.value, out result25))
			{
				curAttackedEffectData.PY = result25;
			}
			float result26 = 0f;
			if (float.TryParse(B_FXPosZ.value, out result26))
			{
				curAttackedEffectData.PZ = result26;
			}
			float result27 = 0f;
			if (float.TryParse(B_FXAngleX.value, out result27))
			{
				curAttackedEffectData.AX = result27;
			}
			float result28 = 0f;
			if (float.TryParse(B_FXAngleY.value, out result28))
			{
				curAttackedEffectData.AY = result28;
			}
			float result29 = 0f;
			if (float.TryParse(B_FXAngleZ.value, out result29))
			{
				curAttackedEffectData.AZ = result29;
			}
			curAttackedEffectData.EffLinkNode = B_LinkNode.value;
			if (!ChangedFxEffInfoData.Contains(curAttackedEffectData))
			{
				ChangedFxEffInfoData.Add(curAttackedEffectData);
			}
		}
		if (mCurCamRockData != null)
		{
			int result30 = 0;
			if (int.TryParse(CamRockTimeInput.value, out result30))
			{
				mCurCamRockData.NeedRockTime = result30;
			}
			int result31 = 0;
			if (int.TryParse(CamRockDelayInput.value, out result31))
			{
				mCurCamRockData.DelayTime = result31;
			}
			if (!mChangedCamRockData.Contains(mCurCamRockData))
			{
				mChangedCamRockData.Add(mCurCamRockData);
			}
		}
		if (curNPC != null)
		{
			curNPC.AttributeData.MaxHP = int.Parse(NPCHPInput.value);
			curNPC.AttributeData.HP = curNPC.AttributeData.MaxHP;
		}
	}

	public void SaveData()
	{
		OnChangeSkillVal();
		string text = Application.dataPath + "/AnimationEditorData";
		string text2 = "SkillData.csv";
		string text3 = "ActionData.csv";
		string text4 = "FxEffectData.csv";
		MyFileUtil.CheckPath(text);
		MyFileUtil.DeleteFile(text + "/" + text2);
		MyFileUtil.DeleteFile(text + "/" + text3);
		MyFileUtil.DeleteFile(text + "/" + text4);
		FileStream fileStream = new FileStream(text + "/" + text2, FileMode.OpenOrCreate);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		SkillData skillData = new SkillData();
		List<SkillData> list = new List<SkillData>(ChangedSkillData.Values);
		for (int i = 0; i < list.Count; i++)
		{
			SkillData skillData2 = list[i];
			string value = "*," + skillData2.ID + "," + skillData2.Name + "," + skillData2.Icon + "," + skillData2.Description + "," + IsEqual(skillData2.CastTime, skillData.CastTime) + "," + IsEqual(skillData2.CastAction, skillData.CastAction) + "," + skillData2.ActionName + "," + skillData2.CD + "," + IsEqual(skillData2.TraceDistance, skillData.TraceDistance) + "," + IsEqual(skillData2.ComboStart, skillData.ComboStart) + "," + IsEqual(skillData2.ComboValidTime, skillData.ComboValidTime) + "," + IsEqual(skillData2.NextSkill, skillData.NextSkill) + ",," + curSkillData.HoldTime + ",," + IsEqual(skillData2.MoveTime, skillData.MoveTime) + "," + IsEqual(skillData2.MoveDistance, skillData.MoveDistance) + "," + IsEqual(skillData2.MoveAngle, skillData.MoveAngle) + "," + skillData2.EffId_0 + "," + IsEqual(skillData2.EffTime_0, skillData.EffTime_0) + "," + skillData2.EffId_1 + "," + IsEqual(skillData2.EffTime_1, skillData.EffTime_1) + "," + skillData2.EffId_2 + "," + IsEqual(skillData2.EffTime_2, skillData.EffTime_2) + "," + skillData2.CamRockID;
			streamWriter.WriteLine(value);
		}
		streamWriter.Close();
		fileStream.Close();
		FileStream fileStream2 = new FileStream(text + "/" + text3, FileMode.OpenOrCreate);
		StreamWriter streamWriter2 = new StreamWriter(fileStream2);
		ActionData actionData = new ActionData();
		List<ActionData> changedActionData = ChangedActionData;
		for (int j = 0; j < changedActionData.Count; j++)
		{
			ActionData actionData2 = changedActionData[j];
			string value2 = "*," + actionData2.ID + "," + actionData2.AnimName + "," + IsEqual(actionData2.AnimWrapMode, actionData.AnimWrapMode) + "," + IsEqual(actionData2.AnimDurationTime, actionData.AnimDurationTime) + "," + IsEqual(actionData2.AnimCanBeBreak, actionData.AnimCanBeBreak) + "," + actionData2.NextActionName + "," + actionData2.FxEffID + "," + IsEqual(actionData2.CrossInTime, actionData.CrossInTime) + "," + IsEqual(actionData2.CrossOutTime, actionData.CrossOutTime);
			streamWriter2.WriteLine(value2);
		}
		streamWriter2.Close();
		fileStream2.Close();
		FileStream fileStream3 = new FileStream(text + "/" + text4, FileMode.OpenOrCreate);
		StreamWriter streamWriter3 = new StreamWriter(fileStream3);
		FxEffInfoData fxEffInfoData = new FxEffInfoData();
		List<FxEffInfoData> changedFxEffInfoData = ChangedFxEffInfoData;
		for (int k = 0; k < changedFxEffInfoData.Count; k++)
		{
			FxEffInfoData fxEffInfoData2 = changedFxEffInfoData[k];
			string value3 = "*," + fxEffInfoData2.ID + "," + fxEffInfoData2.EffName + "," + fxEffInfoData2.EffFilePath + "," + fxEffInfoData2.EffDurationTime + "," + IsEqual(fxEffInfoData2.EffDelayTime, fxEffInfoData.EffDelayTime) + "," + IsEqual(fxEffInfoData2.EffLinkNode, fxEffInfoData.EffLinkNode) + "," + IsEqual(fxEffInfoData2.PX, fxEffInfoData.PX) + "," + IsEqual(fxEffInfoData2.PY, fxEffInfoData.PY) + "," + IsEqual(fxEffInfoData2.PZ, fxEffInfoData.PZ) + "," + IsEqual(fxEffInfoData2.AX, fxEffInfoData.AX) + "," + IsEqual(fxEffInfoData2.AY, fxEffInfoData.AY) + "," + IsEqual(fxEffInfoData2.AZ, fxEffInfoData.AZ) + "," + IsEqual(fxEffInfoData2.AutoMove, fxEffInfoData.AutoMove);
			streamWriter3.WriteLine(value3);
		}
		streamWriter3.Close();
		fileStream3.Close();
		string text5 = "CamRockData.csv";
		MyFileUtil.DeleteFile(text + "/" + text5);
		FileStream fileStream4 = new FileStream(text + "/" + text5, FileMode.OpenOrCreate);
		StreamWriter streamWriter4 = new StreamWriter(fileStream4);
		CamRockData camRockData = new CamRockData();
		List<CamRockData> list2 = mChangedCamRockData;
		for (int l = 0; l < list2.Count; l++)
		{
			CamRockData camRockData2 = list2[l];
			string value4 = "*," + camRockData2.ID + "," + camRockData2.CurveName + "," + IsEqual(camRockData2.NeedRockTime, camRockData.NeedRockTime) + "," + IsEqual(camRockData2.DelayTime, camRockData.DelayTime);
			streamWriter4.WriteLine(value4);
		}
		streamWriter4.Close();
		fileStream4.Close();
		Debug.Log("Write~!!!!!!");
	}

	private string IsEqual(string src, string target)
	{
		return (!src.Equals(target)) ? src : string.Empty;
	}

	private string IsEqual(int src, int target)
	{
		return (src != target) ? src.ToString() : string.Empty;
	}

	private string IsEqual(float src, float target)
	{
		return (!(Mathf.Abs(src - target) < float.Epsilon)) ? src.ToString() : string.Empty;
	}

	public void CreateNPC()
	{
		if (curNPC != null)
		{
			return;
		}
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = Vector3.forward * 5f;
		NpcData npcDataByID = DataManager.GetNpcDataByID(NPCIDInput.value);
		if (npcDataByID != null)
		{
			objInitNpcData.HP = npcDataByID.Hp;
			objInitNpcData.MaxHP = npcDataByID.Hp;
			objInitNpcData.npcInfoData = npcDataByID;
			Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData, delegate(ObjNPC npc)
			{
				curNPC = npc;
			});
			NPCHPInput.value = npcDataByID.Hp.ToString();
			SelectSkill(curSkillID);
		}
	}

	public void OnToggleValChange()
	{
		if (IsCtlPlayer.value)
		{
			mCurChaSkillData = mainPlayer.CharacterSkillData;
			List<string> list = new List<string>();
			mCurSkillDataDic.Clear();
			for (int i = 0; i < mCurChaSkillData.Count; i++)
			{
				mCurSkillDataDic.Add(mCurChaSkillData[i].ID, DataManager.GetSkillDataById(mCurChaSkillData[i].ID));
				list.Add(mCurChaSkillData[i].ID);
			}
			uiPopList.items = list;
			SelectSkill(list[0]);
		}
		else if (!(curNPC == null))
		{
			List<string> list2 = new List<string>();
			for (int j = 0; j < curNPC.CharacterSkillData.Count; j++)
			{
				list2.Add(curNPC.CharacterSkillData[j].ID);
			}
			mCurSkillDataDic.Clear();
			for (int k = 0; k < list2.Count; k++)
			{
				mCurSkillDataDic.Add(list2[k], DataManager.GetSkillDataById(list2[k]));
			}
			uiPopList.items = list2;
			SelectSkill(list2[0]);
		}
	}
}
