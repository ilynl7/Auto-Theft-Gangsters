using System.Collections.Generic;
using UnityEngine;

public class BossXueTiaoLogicNew : SingletonUnity<BossXueTiaoLogicNew>
{
	public UILabel Levellabel;

	public UILabel namelabel;

	private float mTargetProgress;

	private int mTargetWidth;

	private int mCurWidth;

	private int mPicWidth;

	private long mMaxHP;

	public UISprite HpLinePic;

	public UISprite HpBottomPic;

	public UISprite HpMiddlePic;

	public UILabel HPVal;

	private int picSpeed = 5;

	private int valSpeed;

	private long mCurHpLabelVal;

	private long mCurHPVal;

	private long mTargetHPVal;

	private bool reduceFlag;

	private List<ObjNPC> BossList = new List<ObjNPC>();

	private float mMinShowDisBossUI = 8f;

	private float mMaxShowDisBossUI = 9f;

	private ObjMainPlayer mMainPlayer;

	private float lastCheckTime;

	private ObjNPC minBoss;

	private float minDis;

	public void RegisterBoss(ObjNPC boss)
	{
		if (!BossList.Contains(boss))
		{
			BossList.Add(boss);
		}
	}

	public void RemoveBoss(ObjNPC boss)
	{
		if (BossList.Contains(boss))
		{
			BossList.Remove(boss);
		}
	}

	public void UpdateBossHp(ObjNPC curBoss, CharacterAttributeData attr)
	{
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (Time.time - lastCheckTime > 1f)
		{
			lastCheckTime = Time.time;
			minBoss = null;
			minDis = float.MaxValue;
			float num = 0f;
			for (int i = 0; i < BossList.Count; i++)
			{
				num = Vector3.Distance(mMainPlayer.Position, BossList[i].Position);
				if (num < minDis)
				{
					minBoss = BossList[i];
					minDis = num;
				}
			}
		}
		if (!(minBoss != null) || minBoss.ServerId != curBoss.ServerId)
		{
			return;
		}
		if (SingletonUnity<BossXueTiaoLogicNew>.Exists)
		{
			if (minDis < mMinShowDisBossUI && !UnityVersionUtil.IsActive(SingletonUnity<BossXueTiaoLogicNew>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BossXueTiaoUI, delegate
				{
					SingletonUnity<BossXueTiaoLogicNew>.Instance.resetHPinfo(attr);
				});
				SingletonUnity<UIManager>.Instance.CheckFunctionTop(isshowbossline: true);
			}
			else if (minDis > mMaxShowDisBossUI && UnityVersionUtil.IsActive(SingletonUnity<BossXueTiaoLogicNew>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BossXueTiaoUI);
				SingletonUnity<UIManager>.Instance.CheckFunctionTop(isshowbossline: false);
			}
		}
		else if (minDis < mMinShowDisBossUI)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BossXueTiaoUI, delegate
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.resetHPinfo(attr);
			});
			SingletonUnity<UIManager>.Instance.CheckFunctionTop(isshowbossline: true);
		}
	}

	public void resetHPinfo(CharacterAttributeData AttributeData)
	{
		mPicWidth = HpBottomPic.width;
		Levellabel.text = $"Lv.{AttributeData.Level}";
		namelabel.text = AttributeData.Name;
		mMaxHP = AttributeData.MaxHP;
		mCurHPVal = AttributeData.HP;
		valSpeed = (int)((float)picSpeed / (float)mPicWidth * (float)mMaxHP);
		mTargetProgress = ((float)mCurHPVal - 0.01f) / (float)mMaxHP;
		mTargetWidth = (int)(mTargetProgress * (float)mPicWidth);
		mCurWidth = mTargetWidth;
		HpLinePic.width = mTargetWidth;
		HpMiddlePic.width = mTargetWidth;
		mTargetHPVal = mCurHPVal;
		HPVal.text = mTargetHPVal.ToString();
	}

	public void ChangeHP(long curHP, ObjNPC curBoss)
	{
		if (minBoss != null && curBoss != null && minBoss.ServerId == curBoss.ServerId && curHP >= 0)
		{
			mTargetProgress = ((float)curHP - 0.01f) / (float)mMaxHP;
			HpMiddlePic.width = mTargetWidth;
			mTargetWidth = (int)(mTargetProgress * (float)mPicWidth);
			HpLinePic.width = mTargetWidth;
			mTargetHPVal = curHP;
			if (curHP > mCurHPVal)
			{
				reduceFlag = false;
			}
			else
			{
				reduceFlag = true;
			}
		}
	}

	private void UpdateSliderValue()
	{
		if (reduceFlag)
		{
			if (mCurWidth > mTargetWidth)
			{
				mCurWidth -= picSpeed;
				mCurHPVal -= valSpeed;
				if (mCurWidth < mTargetWidth)
				{
					mCurWidth = mTargetWidth;
					mCurHPVal = mTargetHPVal;
				}
			}
			else
			{
				mCurWidth = mTargetWidth;
				mCurHPVal = mTargetHPVal;
			}
		}
		else if (mCurWidth < mTargetWidth)
		{
			mCurWidth += picSpeed;
			mCurHPVal += valSpeed;
			if (mCurWidth > mTargetWidth)
			{
				mCurWidth = mTargetWidth;
				mCurHPVal = mTargetHPVal;
			}
		}
		else
		{
			mCurWidth = mTargetWidth;
			mCurHPVal = mTargetHPVal;
		}
		if (mCurWidth != HpMiddlePic.width)
		{
			HpMiddlePic.width = mCurWidth;
		}
		if (mCurHpLabelVal != mCurHPVal)
		{
			mCurHpLabelVal = mCurHPVal;
			HPVal.text = mCurHpLabelVal.ToString();
		}
	}

	private void Update()
	{
		UpdateSliderValue();
	}
}
