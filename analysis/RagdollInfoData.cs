using System;
using UnityEngine;

[Serializable]
public class RagdollInfoData
{
	public string Path;

	public int ColliderType;

	public Vector3 ColliderCenter;

	public float ColliderRadius;

	public float Height;

	public int Direction;

	public Vector3 size;

	public float Mass;

	public float Drag;

	public float AngularDrag;

	public bool UseGravity;

	public bool IsKinematic;

	public bool HasJoint;

	public string ConnectBodyPath;

	public Vector3 Anchor;

	public Vector3 Axis;

	public Vector3 SwingAxis;

	public MySoftJointLimit LowTwistLimit;

	public MySoftJointLimit HighTwistLimit;

	public MySoftJointLimit Swing1Limit;

	public MySoftJointLimit Swing2Limit;
}
