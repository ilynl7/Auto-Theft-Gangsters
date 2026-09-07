using UnityEngine;

public class LockAreaObj : MonoBehaviour
{
	private SingleMapLockData mCurMapData;

	public void SetCurMapLockData(SingleMapLockData mapData)
	{
		mCurMapData = mapData;
	}

	private void OnCollisionEnter(Collision other)
	{
		ObjMainPlayer component = other.gameObject.GetComponent<ObjMainPlayer>();
		if (component != null)
		{
			if (mCurMapData != null)
			{
				if (mCurMapData.UnlockLevel > 1000)
				{
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101814}", mCurMapData.UnlockLevel));
				}
				else
				{
					NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101813}", mCurMapData.UnlockLevel));
				}
			}
		}
		else if (other.gameObject.layer == 29 && mCurMapData != null)
		{
			if (mCurMapData.UnlockLevel > 1000)
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101814}", mCurMapData.UnlockLevel));
			}
			else
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101813}", mCurMapData.UnlockLevel));
			}
		}
	}
}
