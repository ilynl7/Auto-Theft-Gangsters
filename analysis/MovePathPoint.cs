using System;
using UnityEngine;

public class MovePathPoint : MonoBehaviour
{
	public delegate void OnArrivePointDelegate(Vector3 pos);

	public float ActiveRadius = 3f;

	private bool mbValid = true;

	private float mfLastInvaildTime;

	private Transform mMainPlayerTransform;

	private Transform mTeleportTransform;

	public OnArrivePointDelegate OnArrivePoint;

	public void RegisterOnArrivePathPoint(OnArrivePointDelegate func)
	{
		OnArrivePoint = (OnArrivePointDelegate)Delegate.Combine(OnArrivePoint, func);
	}

	public void DeRegisterOnArrivePathPoint(OnArrivePointDelegate func)
	{
		OnArrivePoint = (OnArrivePointDelegate)Delegate.Remove(OnArrivePoint, func);
	}

	private void Start()
	{
		mTeleportTransform = base.transform;
	}

	private void FixedUpdate()
	{
		if (!mbValid)
		{
			if (Time.time - mfLastInvaildTime < 3f)
			{
				return;
			}
			mbValid = true;
		}
		if (null == mMainPlayerTransform)
		{
			if (null != Singleton<ObjManager>.Instance.MainPlayer)
			{
				mMainPlayerTransform = Singleton<ObjManager>.Instance.MainPlayer.CacheTransform;
			}
			if (null == mMainPlayerTransform)
			{
				return;
			}
		}
		if (null != Singleton<ObjManager>.Instance.MainPlayer && Vector3.Distance(mMainPlayerTransform.position, mTeleportTransform.position) <= ActiveRadius)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (!sceneManager.IsSurviveBattleScene())
			{
				UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
			}
			if (OnArrivePoint != null)
			{
				OnArrivePoint(base.transform.position);
			}
		}
	}
}
