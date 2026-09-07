using UnityEngine;

public class MissionLineLogic : MonoBehaviour
{
	public UILabel MissionNameLabel;

	public MissionData CurMissionData;

	public void Reset(MissionData data)
	{
		CurMissionData = data;
		MissionNameLabel.text = CurMissionData.MTipDescribeID;
	}

	public void OnClickMissionLine()
	{
		SingletonUnity<MissionPageRootLogic>.Instance.ResetMissionDataPage(this);
	}
}
