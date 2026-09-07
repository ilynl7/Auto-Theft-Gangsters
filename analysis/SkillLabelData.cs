using System.Collections.Generic;
using UnityEngine;

public class SkillLabelData
{
	public string ID = string.Empty;

	public string localization = string.Empty;

	public string value1 = string.Empty;

	private List<string> mValues;

	private string[] mValueArray;

	public string color = string.Empty;

	public int score;

	private List<int> labelCol;

	public List<string> ValueList
	{
		get
		{
			if (mValues == null)
			{
				mValues = new List<string>();
				string[] array = value1.Split('#');
				for (int i = 0; i < array.Length; i++)
				{
					mValues.Add(array[i]);
				}
			}
			return mValues;
		}
	}

	public string[] ValueArray
	{
		get
		{
			if (mValueArray == null)
			{
				mValueArray = value1.Split('#');
			}
			return mValueArray;
		}
	}

	public Color LabelColor
	{
		get
		{
			if (labelCol == null)
			{
				labelCol = new List<int>();
				string[] array = color.Split('#');
				for (int i = 0; i < array.Length; i++)
				{
					labelCol.Add(int.Parse(array[i]));
				}
			}
			return new Color((float)labelCol[0] / 255f, (float)labelCol[1] / 255f, (float)labelCol[2] / 255f, 1f);
		}
	}
}
