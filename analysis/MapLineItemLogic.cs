using SprotoType;
using UnityEngine;

public class MapLineItemLogic : MonoBehaviour
{
	public UILabel Line1Label;

	public UILabel Line2Label;

	public UILabel Line1StateLable;

	public UILabel Line2StateLable;

	public UISprite Btn1Sp;

	public UISprite Btn2Sp;

	public GameObject RightObj;

	private int mRealIndex = -1;

	private int LindeIndex1 = -1;

	private int LindeIndex2 = -1;

	public string GetStrState(int state)
	{
		return state switch
		{
			0 => StrDictionary.GetDictionaryString("#{101810}"), 
			1 => StrDictionary.GetDictionaryString("#{101811}"), 
			_ => StrDictionary.GetDictionaryString("#{101804}"), 
		};
	}

	public void UpdateLineState(int index1, int state1, int index2, int state2, int realIndex)
	{
		Line1Label.text = string.Format("{0}{1}", StrDictionary.GetDictionaryString("#{101802}"), index1 + 1);
		if (state1 > -1)
		{
			Line1StateLable.text = GetStrState(state1);
		}
		else
		{
			Line1StateLable.text = string.Empty;
		}
		LindeIndex1 = index1;
		LindeIndex2 = index2;
		if (index2 > -1)
		{
			if (state2 > -1)
			{
				Line2StateLable.text = GetStrState(state2);
			}
			else
			{
				Line2StateLable.text = string.Empty;
			}
			Line2Label.text = string.Format("{0}{1}", StrDictionary.GetDictionaryString("#{101802}"), index2 + 1);
		}
		mRealIndex = realIndex;
		int num = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex - 1;
		if (index1 != num && state1 < 2)
		{
			Btn1Sp.alpha = 1f;
		}
		else
		{
			Btn1Sp.alpha = 0f;
		}
		if (index2 != num && state2 < 2)
		{
			Btn2Sp.alpha = 1f;
		}
		else
		{
			Btn2Sp.alpha = 0f;
		}
		NGUITools.SetActive(RightObj, index2 > -1);
	}

	public void OnClickChangeLineIndex1()
	{
		change_scene_line.request request = new change_scene_line.request();
		request.line_index = LindeIndex1 + 1;
		NetLogic.GetInstance().Send<Protocol.change_scene_line>(request);
	}

	public void OnClikcChangeLineIndex2()
	{
		change_scene_line.request request = new change_scene_line.request();
		request.line_index = LindeIndex2 + 1;
		NetLogic.GetInstance().Send<Protocol.change_scene_line>(request);
	}
}
