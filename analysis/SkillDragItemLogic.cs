using UnityEngine;

public class SkillDragItemLogic : UIDragItemEx
{
	public CharacterSkillData mCharacterSkillData;

	private GameObject dragObj;

	protected override void Start()
	{
		base.Start();
	}

	private void OnEnable()
	{
		dragObj = null;
	}

	private void OnDisable()
	{
		if (dragObj != null)
		{
			NGUITools.Destroy(base.gameObject);
		}
	}

	protected override void OnDragDropStart(GameObject OrginDrag)
	{
		SkillInfoBtnLogic component = OrginDrag.GetComponent<SkillInfoBtnLogic>();
		mCharacterSkillData = component.CharacterSkillData;
		if (mCharacterSkillData != null)
		{
			dragObj = OrginDrag;
			base.OnDragDropStart(OrginDrag);
		}
		else
		{
			base.OnDragDropRelease(OrginDrag);
		}
	}

	protected override void OnDragDropRelease(GameObject surface)
	{
		SkillDragSurface component = surface.GetComponent<SkillDragSurface>();
		if (component != null && mCharacterSkillData != null)
		{
			component.SelectSkill(mCharacterSkillData);
		}
		dragObj = null;
		base.OnDragDropRelease(surface);
	}
}
