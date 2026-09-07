using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneAnimationObjData
{
	public Animation AnimationObj;

	public Animation SubAnimationObj;

	public List<string> AnimationNameList;

	public List<string> SubAniamtionNameList;

	public List<float> AnimationSpeedList;

	public Transform SubObjRoot;

	public FakeObjLogic SubFakeObj;

	public string SubModelId;

	public CharacterModelData SubModelData;
}
