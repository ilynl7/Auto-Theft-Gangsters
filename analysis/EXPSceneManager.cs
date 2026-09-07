using System.Collections.Generic;
using UnityEngine;

public class EXPSceneManager : SceneManager
{
	private List<Vector3> mPathPointList = new List<Vector3>();

	private MovePathPoint mMoveTargetPoint;

	private int mCurPointIndex;

	public override void Init(string id)
	{
		base.Init(id);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExpBattleInfoRoot, delegate
		{
			SingletonUnity<ExpBattleInfoRootLogic>.Instance.EnableReset();
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ExpBattleInfoRoot);
		});
	}

	public void OpenBlock()
	{
	}

	private void OnArriveMovePoint(Vector3 pos)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
		NetLogic.GetInstance().Send<Protocol.start_battle>();
	}

	public override void AutoFightAction()
	{
	}
}
