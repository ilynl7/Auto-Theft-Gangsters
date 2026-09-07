using UnityEngine;

public class FakeMissionTutorial : SingletonUnity<FakeMissionTutorial>
{
	private float StartTime;

	private bool isHaveHand;

	public GameObject Handobj;

	private void OnEnable()
	{
		StartTime = Time.time;
		isHaveHand = true;
	}

	public void OnClickMissionBtn()
	{
		(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager as NewTutorialSceneManager).MissionMoveToPoint();
		StartTime = Time.time;
		UnityVersionUtil.SetActiveRecursive(Handobj.gameObject, state: false);
		isHaveHand = false;
	}

	private void Update()
	{
		if (!isHaveHand && Time.time - StartTime > 10f)
		{
			UnityVersionUtil.SetActiveRecursive(Handobj.gameObject, state: true);
			isHaveHand = true;
		}
	}
}
