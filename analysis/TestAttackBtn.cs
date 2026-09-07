using UnityEngine;

public class TestAttackBtn : MonoBehaviour
{
	public bool camboFlag;

	public string skillId;

	public void OnClick()
	{
		if (camboFlag)
		{
			if (!Singleton<ObjManager>.Instance.MainPlayer.SkillLogic.IsUsingSkill)
			{
				Singleton<ObjManager>.Instance.MainPlayer.UseComboSkill();
			}
		}
		else if (!Singleton<ObjManager>.Instance.MainPlayer.SkillLogic.IsUsingSkill)
		{
			Singleton<ObjManager>.Instance.MainPlayer.UseSkill(skillId);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
