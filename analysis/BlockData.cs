using System;
using System.Collections.Generic;

[Serializable]
public class BlockData
{
	public List<int> PointIndex = new List<int>();

	public List<BlockMonsterData> BlockMonsterList = new List<BlockMonsterData>();

	public List<BlockCarData> BlockCarDataList = new List<BlockCarData>();
}
