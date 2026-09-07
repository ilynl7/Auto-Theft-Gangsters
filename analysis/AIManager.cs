using System.Collections.Generic;

public class AIManager : SingletonUnity<AIManager>
{
	private List<ObjNPC> mAIList;

	public List<ObjNPC> AIList
	{
		get
		{
			return mAIList;
		}
		set
		{
			mAIList = value;
		}
	}

	private void Start()
	{
		Init();
	}

	public void Init()
	{
	}

	public ObjNPC ObjCharacterToObjNPC(ObjCharacter obj)
	{
		return obj.gameObject.GetComponent<ObjNPC>();
	}

	public void CheckAIState(AILogic ai)
	{
	}

	private void Update()
	{
	}
}
