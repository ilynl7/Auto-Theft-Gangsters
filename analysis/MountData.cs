using UnityEngine;

public class MountData
{
	public string ID;

	public string CarName;

	public string ItemID;

	public string Desc;

	public int Quality;

	public int Lv;

	public int Status1;

	public int Value1;

	public int Status2;

	public int Value2;

	public int Status3;

	public int Value3;

	public int Status4;

	public int Value4;

	public string ModelId;

	public int GetPath;

	public string GetDesc;

	public string CarIcon;

	public float ModelPosX;

	public float ModelPosY;

	public float ModelPosZ;

	public int MaxSpeed = 4000;

	public float MaxSteerAngle = 10f;

	public int MaxAcceleration = 2000;

	public int BrakeAcceleration = 2000;

	public float LightPosX;

	public float LightPosY;

	public float LightPosZ;

	public string ColorStr;

	public string DefaultColorId;

	public float NameHeight = 2f;

	public float ShadowHeight = 0.1f;

	public string StartTime = string.Empty;

	public string EndTime = string.Empty;

	public int NeedShow;

	public int MaxHP;

	public int IsShowPlayer;

	public string GTALinkCarId = string.Empty;

	public int CamDis = 650;

	public int CamHeight = 200;

	public int PriceType = -1;

	public int Price;

	public int ATK;

	public int IsMotor;

	private int[] mStartTimeList;

	private int[] mEndTimeList;

	public string MCarName => StrDictionary.GetDictionaryString(CarName);

	public float ATKValue => (float)ATK / 10000f;

	public bool IsMotorBool => IsMotor == 1;

	public float CamDisMeter => (float)CamDis / 100f;

	public float CamHeightMeter => (float)CamHeight / 100f;

	public int[] StartTimeList
	{
		get
		{
			if ((mStartTimeList == null || mStartTimeList.Length == 0) && !string.IsNullOrEmpty(StartTime))
			{
				string[] array = StartTime.Split('-');
				mStartTimeList = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mStartTimeList[i] = int.Parse(array[i]);
				}
			}
			return mStartTimeList;
		}
	}

	public int[] EndTimeList
	{
		get
		{
			if ((mEndTimeList == null || mEndTimeList.Length == 0) && !string.IsNullOrEmpty(EndTime))
			{
				string[] array = EndTime.Split('-');
				mEndTimeList = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					mEndTimeList[i] = int.Parse(array[i]);
				}
			}
			return mEndTimeList;
		}
	}

	public string[] ColorIDs
	{
		get
		{
			if (string.IsNullOrEmpty(ColorStr))
			{
				return null;
			}
			return ColorStr.Split('#');
		}
	}

	public float MaxSp => (float)MaxSpeed / 100f;

	public float MaxAcce => (float)MaxAcceleration / 100f;

	public float BrakeAcce => (float)BrakeAcceleration / 100f;

	public Vector3 ModelPos => new Vector3(ModelPosX, ModelPosY, ModelPosZ);

	public Vector3 LightPosR => new Vector3(LightPosX, LightPosY, LightPosZ);

	public Vector3 LightPosL => new Vector3(0f - LightPosX, LightPosY, LightPosZ);
}
