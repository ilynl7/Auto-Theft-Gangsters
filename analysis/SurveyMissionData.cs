public class SurveyMissionData
{
	public string ID = string.Empty;

	public string SceneID = string.Empty;

	public string ModelName = string.Empty;

	public float PosX;

	public float PosY;

	public float PosZ;

	public float PosO;

	public int Count = 1;

	public int SurveyTime = 1000;

	public int IsAutoFresh;

	public int AutoFreshTime;

	public int NeedNum;

	public string Text = string.Empty;

	public int IsAlwaysExist;

	public int DelayRecycleTime = -1;

	public float AutoFreshTimeSecond => (float)AutoFreshTime / 1000f;

	public float SurveyTimeSecond => (float)SurveyTime / 1000f;

	public string GetSurveyItemName()
	{
		return $"SurveyItem{ID}";
	}
}
