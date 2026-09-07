using System.Collections.Generic;
using Sproto;
using SprotoType;

public class ret_chat_handler
{
	public static SprotoTypeBase ret_chat_request(SprotoTypeBase req)
	{
		if (req is ret_chat.request { HasChat_list: not false } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			List<chat_item> chat_list = request.chat_list;
			for (int i = 0; i < chat_list.Count; i++)
			{
				if (chat_list[i].chattype != 6 && chat_list[i].chattype != 7)
				{
					playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
				}
				else if (chat_list[i].chattype == 7)
				{
					string empty = string.Empty;
					string empty2 = string.Empty;
					ItemData itemData = null;
					string text = string.Empty;
					if (chat_list[i].HasIntdata && chat_list[i].intdata.Count > 0)
					{
						switch ((GameDefine.BROAD_CAST_TYPE)chat_list[i].intdata[0])
						{
						case GameDefine.BROAD_CAST_TYPE.SLOT:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							empty2 = chat_list[i].stringdata[1];
							itemData = DataManager.GetItemDataByID(empty2);
							if (GameManager.IsSupportCurDataVersion56())
							{
								if (itemData.Type == GameDefine.ITEM_TYPE.EXCHANGE)
								{
									MountData mountDataById2 = DataManager.GetMountDataById(itemData.Function.ToString());
									if (mountDataById2 != null)
									{
										text = StrDictionary.GetDictionaryString("#{101914}", empty, GameDefine.GetYellowColor(mountDataById2.MCarName));
									}
								}
								else if (itemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
								{
									text = StrDictionary.GetDictionaryString("#{101915}", empty, GameDefine.GetYellowColor(itemData.MName));
								}
								else if (itemData.Type == GameDefine.ITEM_TYPE.BADGE)
								{
									text = StrDictionary.GetDictionaryString("#{101916}", empty, GameDefine.GetYellowColor(itemData.MName));
								}
							}
							if (string.IsNullOrEmpty(text))
							{
								text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(itemData.MName));
							}
							break;
						case GameDefine.BROAD_CAST_TYPE.PACK_ITEM:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							empty2 = chat_list[i].stringdata[1];
							itemData = DataManager.GetItemDataByID(empty2);
							if (GameManager.IsSupportCurDataVersion56())
							{
								if (itemData.Type == GameDefine.ITEM_TYPE.EXCHANGE)
								{
									MountData mountDataById = DataManager.GetMountDataById(itemData.Function.ToString());
									if (mountDataById != null)
									{
										text = StrDictionary.GetDictionaryString("#{101914}", empty, GameDefine.GetYellowColor(mountDataById.MCarName));
									}
								}
								else if (itemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
								{
									text = StrDictionary.GetDictionaryString("#{101915}", empty, GameDefine.GetYellowColor(itemData.MName));
								}
								else if (itemData.Type == GameDefine.ITEM_TYPE.BADGE)
								{
									text = StrDictionary.GetDictionaryString("#{101916}", empty, GameDefine.GetYellowColor(itemData.MName));
								}
							}
							if (string.IsNullOrEmpty(text))
							{
								text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(itemData.MName));
							}
							break;
						case GameDefine.BROAD_CAST_TYPE.RARE_ITEM:
						{
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							empty2 = chat_list[i].stringdata[2];
							NpcData npcDataByID3 = DataManager.GetNpcDataByID(chat_list[i].stringdata[1]);
							itemData = DataManager.GetItemDataByID(empty2);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(npcDataByID3.MName), GameDefine.GetYellowColor(itemData.MName));
							break;
						}
						case GameDefine.BROAD_CAST_TYPE.WILD_BOSS:
						{
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							empty2 = chat_list[i].stringdata[1];
							NpcData npcDataByID2 = DataManager.GetNpcDataByID(empty2);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(npcDataByID2.MName));
							break;
						}
						case GameDefine.BROAD_CAST_TYPE.RANK_PVP:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.REFINE:
						{
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							int num2 = (int)chat_list[i].intdata[1];
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(StrDictionary.GetDictionaryString(GameDefine.RefinePartName[num2])));
							break;
						}
						case GameDefine.BROAD_CAST_TYPE.REFINE_2:
						{
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							int num = (int)chat_list[i].intdata[1];
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(StrDictionary.GetDictionaryString(GameDefine.RefinePartName[num])), (int)chat_list[i].intdata[2]);
							break;
						}
						case GameDefine.BROAD_CAST_TYPE.BADGE:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							empty2 = chat_list[i].stringdata[1];
							itemData = DataManager.GetItemDataByID(empty2);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(itemData.MName));
							break;
						case GameDefine.BROAD_CAST_TYPE.SURVIVE:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.TITLE:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.GUILD_LEVEL:
						{
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							int value = (int)chat_list[i].intdata[1];
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty, GameDefine.GetYellowColor(value));
							break;
						}
						case GameDefine.BROAD_CAST_TYPE.DANCE:
							empty = GameDefine.GetYellowColor(StrDictionary.GetDictionaryString("#{101515}"));
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.SURVIVE_2:
							empty = GameDefine.GetYellowColor(StrDictionary.GetDictionaryString("#{101517}"));
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.WILD_BOSS_2:
							empty = GameDefine.GetYellowColor(StrDictionary.GetDictionaryString("#{101519}"));
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.GUILD_BOSS_2:
							empty = GameDefine.GetYellowColor(StrDictionary.GetDictionaryString("#{100748}"));
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.BAR_FIGHT:
							empty = GameDefine.GetYellowColor(StrDictionary.GetDictionaryString("#{101516}"));
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.RAID_BOSS:
						{
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							empty2 = chat_list[i].stringdata[1];
							NpcData npcDataByID = DataManager.GetNpcDataByID(empty2);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, GameDefine.GetYellowColor(npcDataByID.MName), empty);
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = text;
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						}
						case GameDefine.BROAD_CAST_TYPE.GUILD_BATTLE:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						case GameDefine.BROAD_CAST_TYPE.FIRST_GUILD_DANCE:
							empty = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							text = StrDictionary.GetDictionaryString(chat_list[i].chatInfo, empty);
							break;
						}
					}
					else
					{
						text = StrDictionary.GetServerDictionaryString(chat_list[i].chatInfo);
					}
					if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene() && !string.IsNullOrEmpty(text))
					{
						BroadCastRootLogic.AddMessage(text);
					}
				}
				else
				{
					if (chat_list[i].chattype != 6)
					{
						continue;
					}
					GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
					GameDefine.ACTIVITY_TYPE aCTIVITY_TYPE = (GameDefine.ACTIVITY_TYPE)chat_list[i].intdata[0];
					int num3 = (int)chat_list[i].intdata[1];
					if (!chat_list[i].HasIntdata)
					{
						continue;
					}
					switch (num3)
					{
					case 1:
						switch (aCTIVITY_TYPE)
						{
						case GameDefine.ACTIVITY_TYPE.ESCORT:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{101513}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 5L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{101514}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 10L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.WILD_BOSS:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{101519}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 8L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.CITY_DANCE:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{101515}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 6L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.BAR_FIGHT:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{101516}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 7L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{101517}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 9L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.GUILD_BOSS:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{100748}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 4L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.GUILD_DANCE:
							chat_list[i].chattype = 4L;
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString("#{100264}*#{105100}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 11L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							break;
						case GameDefine.ACTIVITY_TYPE.GUILD_DONMINE:
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = StrDictionary.GetDictionaryString("#{106034}");
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 13L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
							{
								chat_list[i].chattype = 4L;
								playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							}
							break;
						case GameDefine.ACTIVITY_TYPE.GUILD_DONMINE_RES:
						{
							chat_list[i].chattype = 0L;
							GuildCaptureData guildCaptureDataByID = DataManager.GetGuildCaptureDataByID(chat_list[i].stringdata[1]);
							MapInfoData mapInfoDataByID3 = DataManager.GetMapInfoDataByID(guildCaptureDataByID.MapID);
							string yellowColor = GameDefine.GetYellowColor(chat_list[i].stringdata[0]);
							chat_list[i].chatInfo = StrDictionary.GetDictionaryString("#{106035}", yellowColor, StrDictionary.GetDictionaryString(mapInfoDataByID3.Name));
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 14L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
							{
								chat_list[i].chattype = 4L;
								playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							}
							break;
						}
						case GameDefine.ACTIVITY_TYPE.KILL_PLAYER:
							chat_list[i].chattype = 0L;
							if (chat_list[i].stringdata.Count >= 4 && !string.IsNullOrEmpty(chat_list[i].stringdata[3]))
							{
								MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(chat_list[i].stringdata[2]);
								chat_list[i].chatInfo = StrDictionary.GetDictionaryString("#{103309}", chat_list[i].stringdata[0], StrDictionary.GetDictionaryString(mapInfoDataByID.Name), chat_list[i].stringdata[3], chat_list[i].stringdata[1]);
							}
							else
							{
								MapInfoData mapInfoDataByID2 = DataManager.GetMapInfoDataByID(chat_list[i].stringdata[2]);
								chat_list[i].chatInfo = StrDictionary.GetDictionaryString("#{103310}", chat_list[i].stringdata[0], StrDictionary.GetDictionaryString(mapInfoDataByID2.Name), chat_list[i].stringdata[1]);
							}
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = -1L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
							{
								chat_list[i].chattype = 4L;
								playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
							}
							break;
						}
						break;
					case 2:
						if (aCTIVITY_TYPE == GameDefine.ACTIVITY_TYPE.BAR_FIGHT)
						{
							chat_list[i].chattype = 0L;
							chat_list[i].chatInfo = "#{100264}*#{101516}";
							chat_list[i].chatInfo = StrDictionary.GetServerDictionaryString(chat_list[i].chatInfo);
							chat_list[i].chatInfo2 = chat_list[i].chatInfo;
							chat_list[i].linktype = 7L;
							playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
						}
						break;
					}
					instance.PlayerData.ActivityData.UpdateActivity(chat_list[i]);
					if (instance.SceneManager != null)
					{
						instance.SceneManager.CheckSceneActivity();
					}
				}
			}
		}
		return null;
	}
}
