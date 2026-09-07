using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ChatLogic : MonoBehaviour
{
	public ChatType CurrentChatType;

	public static Dictionary<long, friend_info> FriendList = new Dictionary<long, friend_info>();

	public static ChatLogic Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static void AddFriend(friend_info friend)
	{
		if (!FriendList.ContainsKey(friend.characterId))
		{
			FriendList.Add(friend.characterId, friend);
		}
	}

	private void Start()
	{
		base.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
	}

	private void Update()
	{
	}

	public void OnClickExit()
	{
	}
}
