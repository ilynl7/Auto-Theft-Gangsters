using Sproto;
using SprotoType;
using UnityEngine;

public class drop_item_info_handler
{
	public static SprotoTypeBase drop_item_info_request(SprotoTypeBase req)
	{
		if (req is drop_item_info.request request)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.serverId);
			if (objCharacter != null)
			{
				Singleton<ObjManager>.Instance.RemoveObj(request.serverId);
			}
			ObjInitDropItemData objInitDropItemData = new ObjInitDropItemData();
			objInitDropItemData.Pos = new VectorXZ((float)request.pos_x / 100f, (float)request.pos_z / 100f);
			objInitDropItemData.ownerServerId = request.ownServerId;
			objInitDropItemData.ServerID = request.serverId;
			objInitDropItemData.item = request.item;
			ItemData itemDataByID = DataManager.GetItemDataByID(request.item.itemId);
			objInitDropItemData.ItemType = itemDataByID.Type;
			Singleton<ObjManager>.Instance.CreateDropItem(objInitDropItemData);
		}
		return null;
	}
}
