using UnityEngine;

public class GuildBattlePoint : MonoBehaviour
{
	public int Id;

	public float Radius;

	public void Reset(long id, Vector3 pos, float radius, Color col)
	{
		base.transform.position = pos;
		base.transform.rotation = Quaternion.identity;
	}

	public void UpdateColor(Color col)
	{
	}
}
