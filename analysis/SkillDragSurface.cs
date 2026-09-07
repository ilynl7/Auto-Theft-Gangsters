using UnityEngine;

public class SkillDragSurface : MonoBehaviour
{
	public int index;

	public void SelectSkill(CharacterSkillData skillData)
	{
		SingletonUnity<SkillInfoRootLogic>.Instance.SelectSkill(skillData.ID, index);
	}
}
