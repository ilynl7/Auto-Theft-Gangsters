using System.Collections.Generic;
using SprotoType;

public class FriendInfo
{
	public enum FriendType
	{
		NORMAL,
		APPLY,
		APPLIED,
		ACCEPT,
		REFUSED,
		BEDELETED,
		ENEMY
	}

	public enum MailState
	{
		UNREAD,
		READ,
		UNGETITEM,
		GETITEM
	}

	public enum MailUpdateType
	{
		UPDATE,
		ADD,
		DEL
	}

	public enum MailSenderType
	{
		SYS,
		CONSIGN_CANCEL,
		CONSIGN_TIME,
		CONSIGN_BUY,
		CONSIGN_FINISH,
		USER
	}

	public class Mail
	{
		public MailSenderType senderType;

		public long key;

		public string title;

		public long time;

		public string text;

		public Dictionary<string, item> items;

		public bool read;

		public bool getItem;

		public int mailstate;

		public long expireday;

		public bool IsGetItem()
		{
			if (mailstate == 3)
			{
				return true;
			}
			return false;
		}

		public bool IsHaveItem()
		{
			if (items == null || items.Count == 0)
			{
				return false;
			}
			return true;
		}
	}

	private HaoYouRoot_TYPE mOpenType;

	private int mFriendCount;

	private int mEnemyCount;

	private int mFriendOnlineCount;

	private int tipsflag;

	private Dictionary<long, friend_info> mMainPlayerFriendDic = new Dictionary<long, friend_info>();

	private Dictionary<long, friend_info> mMainPlayerEnemyDic = new Dictionary<long, friend_info>();

	public Dictionary<long, friend_info> ApplyFriendDic = new Dictionary<long, friend_info>();

	private Dictionary<long, friend_info> mMainPlayerRandomFriendDic = new Dictionary<long, friend_info>();

	private Dictionary<long, friend_info> mMainPlayerSearchFriendDic = new Dictionary<long, friend_info>();

	public Dictionary<long, Mail> UserMailDic = new Dictionary<long, Mail>();

	public List<Mail> UserMailList = new List<Mail>();

	public bool MailListSortFlag;

	public MailUpdateType curUpdateType = MailUpdateType.ADD;

	public HaoYouRoot_TYPE OpenType => mOpenType;

	public int FriendCount
	{
		get
		{
			return mFriendCount;
		}
		set
		{
			mFriendCount = value;
		}
	}

	public int EnemyCount
	{
		get
		{
			return mEnemyCount;
		}
		set
		{
			mEnemyCount = value;
		}
	}

	public int FriendOnlineCount
	{
		get
		{
			return mFriendOnlineCount;
		}
		set
		{
			mFriendOnlineCount = value;
		}
	}

	public Dictionary<long, friend_info> MainPlayerFriendDic
	{
		get
		{
			return mMainPlayerFriendDic;
		}
		set
		{
			mMainPlayerFriendDic = value;
		}
	}

	public Dictionary<long, friend_info> MainPlayerEnemyDic
	{
		get
		{
			return mMainPlayerEnemyDic;
		}
		set
		{
			mMainPlayerEnemyDic = value;
		}
	}

	public Dictionary<long, friend_info> MainPlayerRandomFriendDic
	{
		get
		{
			return mMainPlayerRandomFriendDic;
		}
		set
		{
			mMainPlayerRandomFriendDic = value;
		}
	}

	public Dictionary<long, friend_info> MainPlayerSearchFriendDic
	{
		get
		{
			return mMainPlayerSearchFriendDic;
		}
		set
		{
			mMainPlayerSearchFriendDic = value;
		}
	}

	public FriendInfo()
	{
		Init();
	}

	public void SetFlagValue(int index, bool setOrClear)
	{
		if (setOrClear)
		{
			tipsflag |= 1 << index;
		}
		else
		{
			tipsflag &= ~(1 << index);
		}
	}

	public int GetFlagValue(int index)
	{
		return (tipsflag >> index) & 1;
	}

	public bool GetTips()
	{
		return tipsflag > 0;
	}

	public void FilterFriend(Dictionary<long, friend_info> dict)
	{
		mMainPlayerFriendDic.Clear();
		ApplyFriendDic.Clear();
		foreach (KeyValuePair<long, friend_info> item in dict)
		{
			if (item.Value.friendType == 2)
			{
				ApplyFriendDic.Add(item.Key, item.Value);
			}
			else if (item.Value.friendType != 1)
			{
				mMainPlayerFriendDic.Add(item.Key, item.Value);
			}
		}
		GetFriendCount();
		RefreshUI();
	}

	public void FilterEnemy(Dictionary<long, friend_info> dict)
	{
		mMainPlayerEnemyDic.Clear();
		foreach (KeyValuePair<long, friend_info> item in dict)
		{
			if (item.Value.friendType == 6)
			{
				long friendId = item.Value.friendId;
				item.Value.friendId = item.Value.timeInfo;
				item.Value.timeInfo = friendId;
				mMainPlayerEnemyDic.Add(item.Value.friendId, item.Value);
			}
		}
		GetEnemyCount();
		RefreshUI();
	}

	public static List<friend_info> SortFrientList(List<friend_info> list)
	{
		list.Sort((friend_info x, friend_info y) => (x.state == y.state) ? (-(int)(x.combValue - y.combValue)) : (-(int)(x.state - y.state)));
		return list;
	}

	public static List<friend_info> SortEnemyList(List<friend_info> list)
	{
		list.Sort((friend_info x, friend_info y) => (x.state == y.state) ? (-(int)(x.friendScore - y.friendScore)) : (-(int)(x.state - y.state)));
		return list;
	}

	public HaoYouRoot_TYPE GetOpenType()
	{
		int flagValue = GetFlagValue(3);
		if (flagValue > 0)
		{
			return HaoYouRoot_TYPE.MAIL_LIST;
		}
		flagValue = GetFlagValue(2);
		if (flagValue > 0)
		{
			return HaoYouRoot_TYPE.APPLY_FRIEND;
		}
		return HaoYouRoot_TYPE.MAIL_LIST;
	}

	public bool IsAlreadyFriend(long id)
	{
		return mMainPlayerFriendDic.ContainsKey(id);
	}

	public bool IsAlreadyEnemy(long id)
	{
		return mMainPlayerEnemyDic.ContainsKey(id);
	}

	public bool RemoveFriend(long id)
	{
		bool flag = mMainPlayerFriendDic.Remove(id);
		if (flag)
		{
			mFriendCount--;
			RefreshUI();
		}
		return flag;
	}

	public bool RemoveEnemy(long id)
	{
		bool flag = mMainPlayerEnemyDic.Remove(id);
		if (flag)
		{
			mEnemyCount--;
			RefreshUI();
		}
		return flag;
	}

	public void GetFriendCount()
	{
		mFriendCount = 0;
		foreach (KeyValuePair<long, friend_info> item in mMainPlayerFriendDic)
		{
			mFriendCount++;
		}
	}

	public void GetEnemyCount()
	{
		mEnemyCount = 0;
		foreach (KeyValuePair<long, friend_info> item in mMainPlayerEnemyDic)
		{
			mEnemyCount++;
		}
	}

	public void AddFriend(friend_info friend)
	{
		if (friend.friendType == 6)
		{
			long friendId = friend.friendId;
			friend.friendId = friend.timeInfo;
			friend.timeInfo = friendId;
			AddEnemy(friend);
			return;
		}
		if (mMainPlayerFriendDic.ContainsKey(friend.friendId))
		{
			mMainPlayerFriendDic[friend.friendId] = friend;
		}
		else
		{
			mMainPlayerFriendDic.Add(friend.friendId, friend);
		}
		GetFriendCount();
		RefreshUI();
	}

	public void AddEnemy(friend_info enemyid)
	{
		if (mMainPlayerEnemyDic.ContainsKey(enemyid.friendId))
		{
			mMainPlayerEnemyDic[enemyid.friendId] = enemyid;
		}
		else
		{
			mMainPlayerEnemyDic.Add(enemyid.friendId, enemyid);
		}
		GetEnemyCount();
		RefreshUI();
	}

	public void AddApplyFriend(friend_info friend)
	{
		if (ApplyFriendDic.ContainsKey(friend.friendId))
		{
			ApplyFriendDic[friend.friendId] = friend;
		}
		else
		{
			ApplyFriendDic.Add(friend.friendId, friend);
		}
		RefreshUI();
	}

	public bool RemoveApply(long id)
	{
		bool flag = ApplyFriendDic.Remove(id);
		if (flag)
		{
			RefreshUI();
		}
		return flag;
	}

	public void RefreshUI()
	{
		if (SingletonUnity<SocialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SocialUIRootLogic>.Instance.UpdateSocialInfo();
		}
		SetFlagValue(2, IshavefriendApply());
		SetFlagValue(3, IsHaveMailTips());
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(IsHaveMailTips(), GameDefine.TIPS_TYPE.MAIL);
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(IshavefriendApply() || IsHaveMailTips(), GameDefine.TIPS_TYPE.SOCIAL);
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
	}

	public bool IshavefriendApply()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL_APPLY))
		{
			return false;
		}
		return ApplyFriendDic.Count > 0;
	}

	public void UpdateFriendInfo(friend_info friend)
	{
		if (IsAlreadyFriend(friend.friendId))
		{
			RemoveFriend(friend.friendId);
		}
		mMainPlayerFriendDic.Add(friend.friendId, friend);
		GetFriendCount();
		RefreshUI();
	}

	public void UpdateEnemyInfo(friend_info enemy)
	{
		if (IsAlreadyEnemy(enemy.friendId))
		{
			RemoveEnemy(enemy.friendId);
		}
		mMainPlayerEnemyDic.Add(enemy.friendId, enemy);
		GetEnemyCount();
		RefreshUI();
	}

	public void FilterRandomFriendDic(List<friend_info> list)
	{
		mMainPlayerRandomFriendDic.Clear();
		foreach (friend_info item in list)
		{
			if (item.friendType != 1)
			{
				mMainPlayerRandomFriendDic.Add(item.characterId, item);
			}
		}
	}

	public void FilterSearchFriend(List<friend_info> list)
	{
		mMainPlayerSearchFriendDic.Clear();
		foreach (friend_info item in list)
		{
			if (item.friendType != 1)
			{
				mMainPlayerSearchFriendDic.Add(item.characterId, item);
			}
		}
	}

	public friend_info GetFriendById(long id)
	{
		if (mMainPlayerFriendDic.ContainsKey(id))
		{
			return mMainPlayerFriendDic[id];
		}
		return null;
	}

	public friend_info GetEnemyById(long id)
	{
		if (mMainPlayerEnemyDic.ContainsKey(id))
		{
			return mMainPlayerEnemyDic[id];
		}
		return null;
	}

	public bool IsCanAddFriend()
	{
		return FriendCount < GameDefine.MAX_FRIENT_COUNT;
	}

	public bool IsCanAddEnemy()
	{
		return EnemyCount < GameDefine.MAX_ENEMY_COUNT;
	}

	public void Init()
	{
		mFriendCount = 0;
		UserMailDic.Clear();
		UserMailList.Clear();
		MailListSortFlag = false;
	}

	public void SortMailList()
	{
		if (!MailListSortFlag)
		{
			MailListSortFlag = true;
			UserMailList.Sort((Mail x, Mail y) => (int)((x.read != y.read) ? (x.read ? 1 : (-1)) : (y.time - x.time)));
		}
	}

	public void ClearMailData()
	{
		UserMailDic.Clear();
		UserMailList.Clear();
		MailListSortFlag = false;
		RefreshUI();
	}

	public void UpdateMailData(mail_update.request data)
	{
		curUpdateType = MailUpdateType.UPDATE;
		Mail mail = null;
		if (UserMailDic.ContainsKey(data.mailId))
		{
			mail = UserMailDic[data.mailId];
			curUpdateType = MailUpdateType.UPDATE;
		}
		else
		{
			mail = new Mail();
			curUpdateType = MailUpdateType.ADD;
		}
		mail.key = data.mailId;
		mail.senderType = (MailSenderType)data.sendertype;
		mail.text = data.context;
		mail.time = data.sortTime;
		mail.title = ((!data.HasTitle) ? "no title!!!" : data.title);
		mail.read = (int)data.readTime > 0;
		mail.expireday = ((!data.HasExpireday) ? (-1) : data.expireday);
		if (data.HasItems)
		{
			mail.items = data.items;
		}
		else
		{
			mail.items = null;
		}
		mail.mailstate = (int)data.mailState;
		UserMailDic[mail.key] = mail;
		if (curUpdateType == MailUpdateType.UPDATE)
		{
			MailListSortFlag = false;
			for (int i = 0; i < UserMailList.Count; i++)
			{
				if (mail.key == UserMailList[i].key)
				{
					UserMailList[i] = mail;
					break;
				}
			}
		}
		else if (curUpdateType == MailUpdateType.ADD)
		{
			UserMailList.Insert(0, mail);
		}
		RefreshUI();
	}

	public void DelMail(long key)
	{
		if (UserMailDic.ContainsKey(key))
		{
			UserMailDic.Remove(key);
			for (int i = 0; i < UserMailList.Count; i++)
			{
				if (key == UserMailList[i].key)
				{
					UserMailList.RemoveAt(i);
					break;
				}
			}
		}
		RefreshUI();
	}

	public void ReadMail(long key)
	{
		if (!UserMailDic.ContainsKey(key))
		{
			return;
		}
		UserMailDic[key].read = true;
		if (UserMailDic[key].mailstate == 0)
		{
			UserMailDic[key].mailstate = 1;
		}
		MailListSortFlag = false;
		for (int i = 0; i < UserMailList.Count; i++)
		{
			if (key == UserMailList[i].key)
			{
				UserMailList[i] = UserMailDic[key];
				break;
			}
		}
		RefreshUI();
	}

	public void GetMailItem(long key)
	{
		if (!UserMailDic.ContainsKey(key))
		{
			return;
		}
		UserMailDic[key].getItem = true;
		UserMailDic[key].mailstate = 3;
		for (int i = 0; i < UserMailList.Count; i++)
		{
			if (key == UserMailList[i].key)
			{
				UserMailList[i] = UserMailDic[key];
				break;
			}
		}
		MailListSortFlag = false;
		RefreshUI();
	}

	public bool IsHaveMail()
	{
		return UserMailList.Count != 0;
	}

	public bool IsHaveMailTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL_MAIL))
		{
			return false;
		}
		foreach (KeyValuePair<long, Mail> item in UserMailDic)
		{
			if (!item.Value.read)
			{
				return true;
			}
			if (item.Value.IsHaveItem() && !item.Value.IsGetItem())
			{
				return true;
			}
		}
		return false;
	}
}
