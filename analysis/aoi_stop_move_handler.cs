using Sproto;
using SprotoType;
using UnityEngine;

public class aoi_stop_move_handler
{
	public static SprotoTypeBase aoi_stop_move_request(SprotoTypeBase req)
	{
		aoi_stop_move.request request = req as aoi_stop_move.request;
		bool flag = false;
		if (request != null && request.character.id != Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.character.id);
			if (objCharacter != null)
			{
				flag = true;
				if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
				{
					ObjOtherPlayer objOtherPlayer = objCharacter as ObjOtherPlayer;
					if (objOtherPlayer != null)
					{
						MovePoint point = default(MovePoint);
						point.Index = 1;
						point.xPos = (float)request.character.movement.pos.x / 100f;
						point.zPos = (float)request.character.movement.pos.z / 100f;
						point.O = (float)request.character.movement.pos.o / 100f;
						objOtherPlayer.AutoMoveLogic.InterruptMove(point);
					}
				}
				else if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
				{
					ObjNPC objNPC = objCharacter as ObjNPC;
					MovePoint point2 = default(MovePoint);
					point2.Index = 1;
					point2.xPos = (float)request.character.movement.pos.x / 100f;
					point2.zPos = (float)request.character.movement.pos.z / 100f;
					objNPC.AutoMoveLogic.InterruptMove(point2);
				}
			}
			else
			{
				ObjInitPlayerData noLogicOtherPlayerData = Singleton<ObjManager>.Instance.GetNoLogicOtherPlayerData(request.character.id);
				if (noLogicOtherPlayerData != null)
				{
					flag = true;
					noLogicOtherPlayerData.mPos = new Vector3((float)request.character.movement.pos.x / 100f, (float)request.character.movement.pos.y / 100f, (float)request.character.movement.pos.z / 100f);
					noLogicOtherPlayerData.mDir = MathUtil.HeadingToVector3((float)request.character.movement.pos.o / 100f);
				}
			}
		}
		return null;
	}
}
