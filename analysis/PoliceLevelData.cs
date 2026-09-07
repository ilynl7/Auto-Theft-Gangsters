using System.Collections.Generic;

public class PoliceLevelData
{
	public string ID = string.Empty;

	public int Star1Scores;

	public int Star2Scores;

	public int Star3Scores;

	public int MaxScores;

	public int Star1FlashTime;

	public int Star2FlashTime;

	public int Star3FlashTime;

	public int Star1Num;

	public int Star2Num;

	public int Star3Num;

	public string Star1PoliceId = string.Empty;

	public string Star2PoliceId = string.Empty;

	public string Star3PoliceId = string.Empty;

	private List<int> policeScores;

	private List<float> policeFlashTime;

	private List<int> policeNum;

	private List<string>[] policeIdList;

	public List<int> PoliceScoresList
	{
		get
		{
			if (policeScores == null)
			{
				policeScores = new List<int>();
				policeScores.Add(Star1Scores);
				policeScores.Add(Star2Scores);
				policeScores.Add(Star3Scores);
			}
			return policeScores;
		}
	}

	public List<float> PoliceFlashTimeList
	{
		get
		{
			if (policeFlashTime == null)
			{
				policeFlashTime = new List<float>();
				policeFlashTime.Add((float)Star1FlashTime / 1000f);
				policeFlashTime.Add((float)Star2FlashTime / 1000f);
				policeFlashTime.Add((float)Star3FlashTime / 1000f);
			}
			return policeFlashTime;
		}
	}

	public List<int> PoliceNumList
	{
		get
		{
			if (policeNum == null)
			{
				policeNum = new List<int>();
				policeNum.Add(Star1Num);
				policeNum.Add(Star2Num);
				policeNum.Add(Star3Num);
			}
			return policeNum;
		}
	}

	public List<string>[] PoliceIdList
	{
		get
		{
			if (policeIdList == null)
			{
				string[] array = Star1PoliceId.Split('|');
				string[] array2 = Star2PoliceId.Split('|');
				string[] array3 = Star3PoliceId.Split('|');
				List<string> list = new List<string>();
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(array[i]);
				}
				List<string> list2 = new List<string>();
				for (int j = 0; j < array2.Length; j++)
				{
					list2.Add(array2[j]);
				}
				List<string> list3 = new List<string>();
				for (int k = 0; k < array3.Length; k++)
				{
					list3.Add(array3[k]);
				}
				policeIdList = new List<string>[3] { list, list2, list3 };
			}
			return policeIdList;
		}
	}
}
