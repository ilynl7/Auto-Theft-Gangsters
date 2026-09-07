using UnityEngine;

public class TutorialPlayerEnterCheck : MonoBehaviour
{
	private float mLastEnterTime;

	private float ShowInterVal = 10f;

	private NewTutorialSceneManager tutorialSceneManager;

	private void Start()
	{
		tutorialSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager as NewTutorialSceneManager;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (tutorialSceneManager != null && tutorialSceneManager.IsNeedCheckMainPlayerActiveRange())
		{
			ObjMainPlayer component = other.gameObject.GetComponent<ObjMainPlayer>();
			if (component != null && Time.time - mLastEnterTime > ShowInterVal)
			{
				StoryDialogRootLogic.ShowStory("108", null);
				component.StopMove();
				mLastEnterTime = Time.time;
			}
		}
	}
}
