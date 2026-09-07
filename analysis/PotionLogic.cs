using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PotionLogic : SingletonUnity<PotionLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UISprite PotionShowSprite;

	public UILabel PotionNumLabel;

	public UIScrollView scrollview;

	public UIGrid Grid;

	public GameObject PotionListObj;

	public GameObject PotionBlackObj;

	public List<PotionItem> PotionItemList = new List<PotionItem>();

	private PlayerData playerData;

	private GameItem currentUseItem;

	private bool isSelect;

	private float pressTime;

	public UISprite DragCdSprite;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	protected override void Awake()
	{
		base.Awake();
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
	}

	public void OpenPotionList(List<GameItem> list)
	{
		if (list == null || list.Count <= 0)
		{
			GameMoneyHelper.ShowBuyPotion();
			return;
		}
		UnityVersionUtil.SetActiveRecursive(PotionListObj, state: true);
		UnityVersionUtil.SetActiveRecursive(PotionBlackObj, state: true);
		int num = list.Count - PotionItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(PotionItemList[0].gameObject) as GameObject;
				if (gameObject != null)
				{
					Grid.AddChild(gameObject.transform);
					gameObject.transform.localScale = Vector3.one;
					gameObject.name = $"jiaXue_{PotionItemList.Count}";
					PotionItem component = gameObject.GetComponent<PotionItem>();
					if (component != null)
					{
						PotionItemList.Add(component);
					}
				}
			}
		}
		for (int j = 0; j < PotionItemList.Count; j++)
		{
			NGUITools.SetActive(PotionItemList[j].gameObject, j < list.Count);
		}
		for (int k = 0; k < list.Count; k++)
		{
			PotionItemList[k].Init(list[k]);
		}
		Grid.Reposition();
	}

	private void OnEnable()
	{
		NGUITools.SetActive(PotionListObj, state: false);
		NGUITools.SetActive(PotionBlackObj, state: false);
		UIUpdateEvent.SyncBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.SyncBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
		if (playerData == null)
		{
			playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		currentUseItem = playerData.GetPotionItem();
		isSelect = false;
		Reset();
		UpdateAutoSelectDrag();
	}

	private void OnDisable()
	{
		UIUpdateEvent.SyncBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.SyncBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdatePotion));
	}

	public void UpdatePotion()
	{
		currentUseItem = playerData.GetPotionItem();
		Reset();
		UpdateAutoSelectDrag();
	}

	private void UpdateAutoSelectDrag()
	{
		if (currentUseItem == null || currentUseItem.IsEmpty())
		{
			GameItem gameItem = Singleton<ObjManager>.Instance.MainPlayer.SelectDragItem();
			if (gameItem != null)
			{
				ClickSelectItem(gameItem);
			}
		}
	}

	public void ClickUsePotion()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.DRUG_TIP_START || TutorialManager.CurStep == TUTORIAL_STEP.DRUG_USE_TIP_START)
		{
			CheckTutorialEvent();
		}
		if (!isSelect)
		{
			if (UnityVersionUtil.IsActive(PotionListObj))
			{
				ClosePotionList();
			}
			else if (currentUseItem != null && !currentUseItem.IsEmpty())
			{
				Singleton<ObjManager>.Instance.MainPlayer.UseDrag(currentUseItem);
			}
			else if (!UnityVersionUtil.IsActive(PotionListObj))
			{
				List<GameItem> targetPotionItemLevel = ItemContainerTool.GetTargetPotionItemLevel(playerData.ItemBackPack, playerData.Level);
				OpenPotionList(targetPotionItemLevel);
			}
		}
	}

	public void OnPress()
	{
		pressTime = Time.realtimeSinceStartup;
	}

	public void OnRelease()
	{
		float num = Time.realtimeSinceStartup - pressTime;
		isSelect = num > 0.8f;
		if (isSelect)
		{
			ClickShowList();
		}
	}

	public void ClickShowList()
	{
		if (!UnityVersionUtil.IsActive(PotionListObj))
		{
			List<GameItem> targetPotionItemLevel = ItemContainerTool.GetTargetPotionItemLevel(playerData.ItemBackPack, playerData.Level);
			OpenPotionList(targetPotionItemLevel);
		}
		else
		{
			ClosePotionList();
		}
	}

	public void ClosePotionList()
	{
		UnityVersionUtil.SetActiveRecursive(PotionListObj, state: false);
		NGUITools.SetActive(PotionBlackObj, state: false);
	}

	public void ClickSelectItem(GameItem item)
	{
		if (item != null)
		{
			if (currentUseItem == null || currentUseItem.IndexId != item.IndexId)
			{
				currentUseItem = item;
				playerData.CurSelectPotionIndex = currentUseItem.IndexId;
				Reset();
				change_potion.request request = new change_potion.request();
				request.indexId = item.IndexId;
				NetLogic.GetInstance().Send<Protocol.change_potion>(request);
			}
			ClosePotionList();
		}
	}

	public static void AutoUpdateSelectItem(GameItem item)
	{
		if (SingletonUnity<PotionLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PotionLogic>.Instance.gameObject))
		{
			SingletonUnity<PotionLogic>.Instance.ClickSelectItem(item);
		}
	}

	public void Reset()
	{
		if (currentUseItem != null && !currentUseItem.IsEmpty())
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(currentUseItem.ItemId);
			PotionShowSprite.spriteName = itemDataByID.BackPackIcon + "_Min";
			PotionNumLabel.text = currentUseItem.StackNum.ToString();
			if (TutorialManager.CurStep == TUTORIAL_STEP.DRUG_TIP_START)
			{
				TutorialManager.MoveNext();
			}
		}
		else
		{
			PotionShowSprite.spriteName = "CZ_jiaXue_tuBiao_0";
			PotionNumLabel.text = string.Empty;
			if (TutorialManager.IsTutorialCanShow() && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload && SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.USE_DRUG_TIP))
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.DRUG_TIP_START);
			}
		}
		NGUITools.SetActive(PotionListObj, state: false);
		NGUITools.SetActive(PotionBlackObj, state: false);
	}

	private void Update()
	{
		DragCdSprite.fillAmount = playerData.GetDragTime01();
	}
}
