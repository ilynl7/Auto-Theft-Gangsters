using UnityEngine;

public class Obj : MonoBehaviour
{
	protected long mServerId;

	protected CharacterModelData mCharacterModelData;

	protected Transform mTransform;

	protected Vector3 mPosition;

	protected Quaternion mRotation;

	protected Vector3 mScale;

	protected GameDefine.OBJ_TYPE mObjType;

	public virtual long ServerId
	{
		get
		{
			return mServerId;
		}
		set
		{
			mServerId = value;
		}
	}

	public string ModelId => mCharacterModelData.ID;

	public CharacterModelData CurrentCharacterModelData => mCharacterModelData;

	public virtual float ModelHeight => mCharacterModelData.ModelHeight;

	public string IndexName
	{
		get
		{
			return mCharacterModelData.IndexName;
		}
		set
		{
			mCharacterModelData.IndexName = value;
		}
	}

	public Transform CacheTransform
	{
		get
		{
			if (mTransform == null)
			{
				mTransform = base.transform;
			}
			return mTransform;
		}
	}

	public Vector3 Position
	{
		get
		{
			return CacheTransform.position;
		}
		set
		{
			CacheTransform.localPosition = value;
		}
	}

	public Quaternion Rotation => CacheTransform.localRotation;

	public Vector3 Scale
	{
		get
		{
			return CacheTransform.localScale;
		}
		set
		{
			CacheTransform.localScale = value;
		}
	}

	public GameDefine.OBJ_TYPE ObjType => mObjType;

	public virtual void Init()
	{
	}
}
