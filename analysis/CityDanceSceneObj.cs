using UnityEngine;

public class CityDanceSceneObj : MonoBehaviour
{
	public GameObject[] NPCDancerRoot;

	private void Start()
	{
		AnimationState animationState = null;
		for (int i = 0; i < NPCDancerRoot.Length; i++)
		{
			ActionData actionDataByName = DataManager.GetActionDataByName("baiRen_XD_tiaoWu_fuFei");
			NPCDancerRoot[i].animation.playAutomatically = false;
			animationState = NPCDancerRoot[i].animation[NPCDancerRoot[i].animation.clip.name];
			animationState.speed = animationState.length / actionDataByName.AnimDurationTimeSecond;
			animationState.normalizedTime = Time.realtimeSinceStartup % actionDataByName.AnimDurationTimeSecond / actionDataByName.AnimDurationTimeSecond;
			NPCDancerRoot[i].animation.CrossFade(animationState.name);
		}
	}

	private void OnEnable()
	{
		MapInfoData currentMapInofData = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		SoundData soundDataById = DataManager.GetSoundDataById(108);
		if (soundDataById != null)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlayBGMusic(soundDataById.Id, soundDataById.FadeOutTime, soundDataById.FadeInTime);
		}
	}
}
