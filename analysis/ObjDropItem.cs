using SprotoType;
using UnityEngine;

public class ObjDropItem : Obj
{
	private const float CheckPickInterval = 0.5f;

	private const float CheckPickRange = 5f;

	private const float ItemActiveTime = 10f;

	private const string DROP_MONEYICON = "DiaoLuo_Coin_5";

	private GameDefine.ITEM_TYPE mItemType = GameDefine.ITEM_TYPE.INVALID;

	private string mItemId = string.Empty;

	private int mItemNum = -1;

	private long mOwnerServerId = -1L;

	private ObjMainPlayer mMainPlayer;

	public float MoveTime = 0.6f;

	private float mDropCheckTime;

	private float mPickUpTime;

	private float mDropTime;

	private bool mPickingUpFlag;

	private Vector3 mItemStartMovePos = Vector3.zero;

	private vp_Timer.Handle handle = new vp_Timer.Handle();

	public GameDefine.ITEM_TYPE ItemType
	{
		get
		{
			return mItemType;
		}
		set
		{
			mItemType = value;
		}
	}

	public string ItemId
	{
		get
		{
			return mItemId;
		}
		set
		{
			mItemId = value;
		}
	}

	public int ItemNum
	{
		get
		{
			return mItemNum;
		}
		set
		{
			mItemNum = value;
		}
	}

	public long OwnerServerId
	{
		get
		{
			return mOwnerServerId;
		}
		set
		{
			mOwnerServerId = value;
		}
	}

	private ObjDropItem()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_DROP_ITEM;
	}

	public void Init(ObjInitDropItemData initData)
	{
		if (mTransform == null)
		{
			mTransform = base.transform;
		}
		ItemType = initData.ItemType;
		ItemId = initData.item.itemId;
		ItemNum = (int)initData.item.itemCount;
		ServerId = initData.ServerID;
		OwnerServerId = initData.ownerServerId;
		if (OwnerServerId == Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			mTransform.position = initData.Pos + Vector3.up * 0.3f;
			SetItemLabel();
			SetItemSprite();
			mDropCheckTime = Time.realtimeSinceStartup;
			mDropTime = Time.realtimeSinceStartup;
			mPickingUpFlag = false;
			vp_Timer.In(0.5f, delegate
			{
				if (!(mTransform == null))
				{
					mPickUpTime = Time.realtimeSinceStartup;
					mPickingUpFlag = true;
					mItemStartMovePos = mTransform.position;
				}
			}, handle);
		}
		else
		{
			mMainPlayer = null;
		}
	}

	public void SetItemLabel()
	{
	}

	public void SetItemSprite()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (mPickingUpFlag)
		{
			if (Time.realtimeSinceStartup - mPickUpTime > MoveTime - 0.1f)
			{
				PickedUpItem();
				UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
				Singleton<ObjManager>.Instance.RecycleDropItem(this);
			}
			else
			{
				UpdateMove();
			}
		}
	}

	private void PickedUpItem()
	{
		if (ItemType == GameDefine.ITEM_TYPE.ADD_COIN)
		{
			item item = new item();
			item.itemId = ItemId;
			item.itemCount = ItemNum;
			SimpleRewardRootLogic.AddReward(item);
		}
	}

	private void UpdateMove()
	{
		if (mMainPlayer != null && !mMainPlayer.IsDie)
		{
			float num = (Time.realtimeSinceStartup - mPickUpTime) / MoveTime;
			num *= num;
			mTransform.position = Vector3.Lerp(mItemStartMovePos, mMainPlayer.Position + Vector3.up * 0.8f, num);
		}
	}

	private void OnDisable()
	{
		handle.Cancel();
	}
}
