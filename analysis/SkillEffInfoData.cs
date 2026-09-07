using System.Collections.Generic;
using SprotoType;

public class SkillEffInfoData
{
	public string skillId;

	public string EffInfoId;

	public float DelayTime;

	public long TargetId;

	public List<attack_list> AttackList;

	public bool IsServerAttack;

	public SkillEffInfoData(string skId, string effId, float t, long tarId, List<attack_list> attackList = null, bool isServerAttack = false)
	{
		skillId = skId;
		EffInfoId = effId;
		DelayTime = t;
		TargetId = tarId;
		AttackList = attackList;
		IsServerAttack = isServerAttack;
	}
}
