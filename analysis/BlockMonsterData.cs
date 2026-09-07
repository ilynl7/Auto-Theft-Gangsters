public class BlockMonsterData
{
	public MonsterData Data;

	public long ServerId = -1L;

	public BlockMonsterData(MonsterData data, long serverId = -1)
	{
		Data = data;
		ServerId = serverId;
	}
}
