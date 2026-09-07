using SprotoType;
using UnityEngine;

public class ObjInitDropItemData
{
	public long ServerID = -1L;

	public VectorXZ Pos = VectorXZ.zero;

	public GameDefine.ITEM_TYPE ItemType = GameDefine.ITEM_TYPE.INVALID;

	public item item;

	public long ownerServerId = -1L;
}
