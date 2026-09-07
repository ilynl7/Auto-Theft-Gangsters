using UnityEngine;

public class MapBtnLine : MonoBehaviour
{
	public bool RightBtnFlag = true;

	public UILabel NameLabel;

	public UISprite PicSprite;

	private MapPoint mMapPoint;

	private int mBtnIndex;

	public void ResetRightBtn(MapPoint mapPoint)
	{
		RightBtnFlag = true;
		mMapPoint = mapPoint;
		NameLabel.text = mMapPoint.PointName;
		switch (mMapPoint.PointType)
		{
		case MAP_POINT_TYPE.MONSTER:
			PicSprite.spriteName = "CZ_diTU_guai";
			break;
		case MAP_POINT_TYPE.NPC:
			PicSprite.spriteName = "CZ_diTU_anQuanQu";
			break;
		case MAP_POINT_TYPE.TELEPORT:
			PicSprite.spriteName = "CZ_diTU_chuanSongDian";
			break;
		}
	}

	public void ResetLeftBtn(int index)
	{
		NameLabel.text = "Line   " + (index + 1);
		mBtnIndex = index;
	}

	public void OnClickBtn()
	{
		if (RightBtnFlag)
		{
			SingletonUnity<MapUIRootLogic>.Instance.OnClickRightNPCBtn(mMapPoint);
		}
		else
		{
			SingletonUnity<MapUIRootLogic>.Instance.OnClickLeftMapBtn(mBtnIndex);
		}
	}
}
