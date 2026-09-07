public class DailyExpData
{
	public string ID;

	public int WaveCount;

	public int GroupCount;

	public int WaveTime;

	public int GroupTime;

	public string Monsters;

	public string GroupNpcCount;

	private int[] WaveNums;

	public int GetWaveEnemyNum(int index)
	{
		if (WaveNums == null)
		{
			string[] array = GroupNpcCount.Split('#');
			WaveNums = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				WaveNums[i] = int.Parse(array[i]);
			}
		}
		if (index < WaveNums.Length)
		{
			return WaveNums[index];
		}
		return WaveNums[0];
	}
}
