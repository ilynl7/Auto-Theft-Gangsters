using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ObjPlayerCar : ObjCar
{
	public static int POLICECAR_DAMAGE = 50;

	public static int STATICBLOCK_DAMAGE = 25;

	public static int POLICENEAR_DAMAGE = 5;

	public static int PLAYERCAR_MAXHP = 500;

	private bool DriftFlag;

	private bool ExitDirftFlag;

	public float DriftAngleEnhance = 1.5f;

	public float DriftBrakeReduce = 2f;

	public float DriftStartMinSpeed = 10f;

	public float DriftStartMinAnglePercent = 0.2f;

	public float DriftExitMinSpeed = 10f;

	public float DriftExitMinAnglePercent = 0.2f;

	public float DriftMaxMeshAngle = 20f;

	public float DriftAngleSpeed = 60f;

	public float SkidMaskIntensityReduce = 0.6f;

	public float MaxZAngle = 5f;

	private float mMinDriftSpeedPercent;

	public Transform DummyPlayerPoint;

	public Transform DummyNPCPoint;

	public Vector3 DefaultNpcPos;

	public Quaternion DefaultNpcRotation;

	public Transform CarDoorView;

	public Transform StartCamView;

	public Transform WinCamView;

	private ParticleSystem mLSkidSmoke;

	private ParticleSystem mRSkidSmoke;

	protected LensFlareSensor BLLight;

	protected LensFlareSensor BRLight;

	public Vector3 defaultColliderSize = Vector3.zero;

	private Skidmarks mSkidmarks;

	private MountData mCurMountData;

	private GameObject mFrontTrigger;

	public ParticleSystem SmokeParticle;

	public ParticleSystem ExplosionParticle;

	private Transform DieCamView;

	private Transform PlayerDiePos;

	private SoundManager soundManager;

	private float creshCount;

	private Vector3 tempVector3;

	private int lastFLSkidmark;

	private int lastFRSkidmark;

	private int lastBLSkidmark;

	private int lastBRSkidmark;

	private float curDriftTargetMeshAngle;

	private float curDriftAngleSpeed;

	private float skidMaskIntensityPercent;

	private bool mFreezeCarFlag;

	private bool mStopCarFlag;

	private float ftime;

	private Vector3 mLastPosition = Vector3.zero;

	private float timeWait = 0.2f;

	private move.request request = new move.request();

	private position pos = new position();

	private int mCarEngineSoundId = 36;

	private AudioSource mCarEngineAudio;

	private int mCarBrakeSoundId = 38;

	private int mCarDriftSoundId = 39;

	private int mHitCarSoundId = 41;

	private float mHitCarSoundVolume = 1f;

	private float mBrakeSoundVolume = 1f;

	public LensFlareSensor LLight => BLLight;

	public LensFlareSensor RLight => BRLight;

	public MountData CurMountData => mCurMountData;

	public ObjPlayerCar()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR;
	}

	public void InitCar()
	{
		Init();
	}

	protected new void Init()
	{
		base.Init();
		DummyPlayerPoint = base.transform.FindChild("MeshRoot/Dummy_Player");
		DummyNPCPoint = base.transform.FindChild("MeshRoot/Dummy_NPC");
		if (DummyNPCPoint != null)
		{
			DefaultNpcPos = DummyNPCPoint.transform.localPosition;
			DefaultNpcRotation = DummyNPCPoint.transform.localRotation;
		}
		CarDoorView = base.transform.FindChild("MeshRoot/CarDoorView");
		StartCamView = base.transform.FindChild("MeshRoot/StartCamViewPoint");
		WinCamView = base.transform.FindChild("MeshRoot/WinCamViewPoint");
		Transform transform = base.transform.FindChild("MeshRoot/BLWheel/SkidSmoke");
		if (transform != null)
		{
			mLSkidSmoke = transform.gameObject.GetComponent<ParticleSystem>();
		}
		else
		{
			mLSkidSmoke = null;
		}
		Transform transform2 = base.transform.FindChild("MeshRoot/BRWheel/SkidSmoke");
		if (transform2 != null)
		{
			mRSkidSmoke = transform2.gameObject.GetComponent<ParticleSystem>();
		}
		else
		{
			mRSkidSmoke = null;
		}
		MeshRoot = base.transform.FindChild("MeshRoot").gameObject;
		if (mLSkidSmoke != null && UnityVersionUtil.IsactiveInHierarchy(mLSkidSmoke.gameObject))
		{
			mLSkidSmoke.enableEmission = false;
		}
		if (mRSkidSmoke != null && UnityVersionUtil.IsactiveInHierarchy(mRSkidSmoke.gameObject))
		{
			mRSkidSmoke.enableEmission = false;
		}
		if (mRSkidSmoke != null && UnityVersionUtil.IsactiveInHierarchy(mRSkidSmoke.gameObject))
		{
			mRSkidSmoke.enableEmission = false;
		}
		if (mSkidmarks == null)
		{
			if (SingletonUnity<Skidmarks>.Exists)
			{
				mSkidmarks = SingletonUnity<Skidmarks>.Instance;
			}
			else
			{
				Debug.Log("No Skidmarks!!!!!!!!!!!!!!!!!!!!");
			}
		}
		mMinDriftSpeedPercent = DriftStartMinSpeed / MaxSpeed;
		Transform transform3 = base.transform.FindChild("MeshRoot/BackLight/BLLight");
		if (transform3 != null)
		{
			BLLight = transform3.gameObject.GetComponent<LensFlareSensor>();
		}
		Transform transform4 = base.transform.FindChild("MeshRoot/BackLight/BRLight");
		if (transform4 != null)
		{
			BRLight = transform4.gameObject.GetComponent<LensFlareSensor>();
		}
		if (AttributeData == null)
		{
			AttributeData = new CharacterAttributeData();
		}
		mTransform = base.transform;
		Transform transform5 = base.transform.FindChild("MeshRoot/effect_Smoke");
		Transform transform6 = base.transform.FindChild("MeshRoot/effect_baoZha");
		if (transform5 != null)
		{
			SmokeParticle = transform5.gameObject.GetComponent<ParticleSystem>();
			SmokeParticle.Stop();
			UnityVersionUtil.SetActiveRecursive(SmokeParticle.gameObject, state: false);
		}
		if (transform6 != null)
		{
			ExplosionParticle = transform6.gameObject.GetComponent<ParticleSystem>();
			ExplosionParticle.Stop();
			UnityVersionUtil.SetActiveRecursive(ExplosionParticle.gameObject, state: false);
		}
		DieCamView = base.transform.FindChild("MeshRoot/DieCamView");
		PlayerDiePos = base.transform.FindChild("MeshRoot/PlayerDiePos");
	}

	public void OnMeshLoadDone(MountCarMeshRoot mountCarMeshRoot)
	{
		FLWheel.wheelTrs = mountCarMeshRoot.QLWheel;
		FLWheel.Init(mountCarMeshRoot.WheelRadius);
		FRWheel.wheelTrs = mountCarMeshRoot.QRWheel;
		FRWheel.Init(mountCarMeshRoot.WheelRadius);
		BLWheel.wheelTrs = mountCarMeshRoot.HLWheel;
		BLWheel.Init(mountCarMeshRoot.WheelRadius);
		BRWheel.wheelTrs = mountCarMeshRoot.HRWheel;
		BRWheel.Init(mountCarMeshRoot.WheelRadius);
		CarBodyRoot = mountCarMeshRoot.CarBodyRoot;
		GameObject gameObject = MeshRoot.transform.FindChild("cheshen").gameObject;
		gameObject.layer = LayerMask.NameToLayer("PlayerCar");
		gameObject.tag = "PlayerCar";
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		component.size = mountCarMeshRoot.ColliderSize;
		component.center = mountCarMeshRoot.ColliderCenter;
		defaultColliderSize = mountCarMeshRoot.ColliderSize;
		NGUITools.SetLayer(mountCarMeshRoot.gameObject, gameObject.layer);
		GameObject gameObject2 = MeshRoot.transform.FindChild("FrontCollision").gameObject;
		gameObject2.layer = gameObject.layer;
		gameObject2.tag = gameObject.tag;
		gameObject2.transform.localPosition = component.center + Vector3.forward * (component.size.z / 2f + 0.18f);
		GameObject gameObject3 = new GameObject("PlayerCarCollider");
		gameObject3.transform.parent = gameObject.transform.parent;
		gameObject3.transform.localPosition = gameObject.transform.localPosition;
		gameObject3.transform.localRotation = gameObject.transform.localRotation;
		BoxCollider boxCollider = gameObject3.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
		gameObject3.layer = gameObject.layer;
		gameObject3.tag = gameObject.tag;
		Singleton<ObjManager>.Instance.MainPlayer.DisableMainPlayer();
		if (mCurMountData != null)
		{
			if (BLLight != null)
			{
				BLLight.transform.localPosition = mCurMountData.LightPosL;
			}
			if (BRLight != null)
			{
				BRLight.transform.localPosition = mCurMountData.LightPosR;
			}
		}
		(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager as CarChaseSceneManager).OnPlayerCarMeshLoadDone();
	}

	private void Awake()
	{
		soundManager = SingletonDontDestoryUnity<SoundManager>.Instance;
	}

	private new void Update()
	{
		if (mFreezeCarFlag)
		{
			if (BLLight != null && !UnityVersionUtil.IsActive(BLLight.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(BLLight.gameObject, state: true);
			}
			if (BRLight != null && !UnityVersionUtil.IsActive(BRLight.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(BRLight.gameObject, state: true);
			}
			return;
		}
		if (mStopCarFlag)
		{
			if (base.CurSpeed > 0.5f)
			{
				mCarControl.OnPressBrakeBtn(isPress: true);
			}
			else
			{
				FreezeCar();
			}
			return;
		}
		base.Update();
		if (Vector3.Angle(base.transform.up, Vector3.up) > 75f || base.transform.position.y < -7f)
		{
			creshCount += Time.deltaTime;
			if (creshCount > 2f)
			{
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				base.rigidbody.velocity = Vector3.zero;
				base.rigidbody.angularVelocity = Vector3.zero;
				creshCount = 0f;
				if (base.transform.position.y < -7f)
				{
					base.transform.position = new Vector3(base.transform.position.x, SceneManager.GetHitHeight(base.transform.position) + 0.5f, base.transform.position.z);
				}
			}
		}
		else
		{
			creshCount = 0f;
		}
		if (mCarControl.IsBarking)
		{
			if (BLLight != null && !UnityVersionUtil.IsActive(BLLight.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(BLLight.gameObject, state: true);
				if (base.CurSpeed > 10f)
				{
					mBrakeSoundVolume = 0.2f + 0.8f * Mathf.Clamp01(base.CurSpeed / 60f);
					soundManager.PlaySoundEffect(mCarBrakeSoundId, mBrakeSoundVolume);
				}
			}
			if (BRLight != null && !UnityVersionUtil.IsActive(BRLight.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(BRLight.gameObject, state: true);
			}
		}
		else
		{
			if (BLLight != null && UnityVersionUtil.IsActive(BLLight.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(BLLight.gameObject, state: false);
				soundManager.StopSoundEffect(mCarBrakeSoundId);
			}
			if (BRLight != null && UnityVersionUtil.IsActive(BRLight.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(BRLight.gameObject, state: false);
			}
		}
		if (mCarControl.CurSpeed > 0f)
		{
			float num = Mathf.Lerp(0f - MaxZAngle, MaxZAngle, (mCarControl.SignCurSteerPercent + 1f) / 2f);
			if (CurMountData.IsMotorBool)
			{
				num = 0f - num;
			}
			num = Mathf.Lerp((!(CarBodyRoot.transform.localEulerAngles.z > 180f)) ? CarBodyRoot.transform.localEulerAngles.z : (CarBodyRoot.transform.localEulerAngles.z - 360f), num, Time.deltaTime * 4f);
			CarBodyRoot.transform.localEulerAngles = new Vector3(CarBodyRoot.transform.localEulerAngles.x, CarBodyRoot.transform.localEulerAngles.y, num);
		}
		else
		{
			float to = 0f;
			to = Mathf.Lerp((!(CarBodyRoot.transform.localEulerAngles.z > 180f)) ? CarBodyRoot.transform.localEulerAngles.z : (CarBodyRoot.transform.localEulerAngles.z - 360f), to, Time.deltaTime * 4f);
			CarBodyRoot.transform.localEulerAngles = new Vector3(CarBodyRoot.transform.localEulerAngles.x, CarBodyRoot.transform.localEulerAngles.y, to);
		}
		SynPlayerPosition();
		SetCarEngineAudio();
		if (SingletonUnity<CarBestTimeCountRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CarBestTimeCountRoot>.Instance.gameObject))
		{
			SingletonUnity<CarBestTimeCountRoot>.Instance.SetSpeedLable((int)((double)base.CurSpeed * 3.6));
		}
	}

	public void ResetPlayerCar(ObjCarInitData initData)
	{
		Reset();
		Transform transform = base.transform.FindChild("MeshRoot/PlayerCarCollider");
		if (transform != null)
		{
			mFrontTrigger = base.transform.FindChild("MeshRoot/PlayerCarCollider").gameObject;
		}
		base.CacheTransform.position = new Vector3(initData.Pos.x, SceneManager.GetHitHeight(initData.Pos.x, initData.Pos.z) + 0.1f, initData.Pos.z);
		base.CacheTransform.eulerAngles = initData.Angle;
		if (!base.rigidbody.isKinematic)
		{
			base.rigidbody.velocity = Vector3.zero;
			base.rigidbody.angularVelocity = Vector3.zero;
		}
		base.rigidbody.centerOfMass = Vector3.zero;
		base.rigidbody.mass = 6000f;
		if (initData.CarMountData != null)
		{
			mCarControl.maxSpeed = initData.CarMountData.MaxSp;
			mCarControl.maxSteerAngle = initData.CarMountData.MaxSteerAngle;
			mCarControl.maxAcceleration = initData.CarMountData.MaxAcce;
			mCarControl.brakeAcceleration = initData.CarMountData.BrakeAcce;
			mCurMountData = initData.CarMountData;
			AttributeData.MaxHP = initData.CarMountData.MaxHP;
			AttributeData.HP = AttributeData.MaxHP;
			AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
		}
		else
		{
			AttributeData.MaxHP = 900L;
			AttributeData.HP = 900L;
		}
		BoxCollider component = CarBodyRoot.GetComponent<BoxCollider>();
		if (component != null && defaultColliderSize.y < 0.1f)
		{
			defaultColliderSize = component.size;
		}
		DisableCar();
		ServerId = initData.ServerID;
	}

	public void BeforeLoadMeshReset(ObjCarInitData initData)
	{
		Reset();
		base.CacheTransform.position = initData.Pos;
		base.CacheTransform.eulerAngles = initData.Angle;
		if (!base.rigidbody.isKinematic)
		{
			base.rigidbody.velocity = Vector3.zero;
			base.rigidbody.angularVelocity = Vector3.zero;
		}
		base.rigidbody.centerOfMass = Vector3.zero;
		base.rigidbody.mass = 6000f;
		if (AttributeData == null)
		{
			AttributeData = new CharacterAttributeData();
		}
		if (initData.CarMountData != null)
		{
			mCurMountData = initData.CarMountData;
			AttributeData.MaxHP = initData.CarMountData.MaxHP;
			AttributeData.HP = AttributeData.MaxHP;
			AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
		}
		else
		{
			AttributeData.MaxHP = 900L;
			AttributeData.HP = 900L;
		}
		ServerId = initData.ServerID;
		if (!base.rigidbody.isKinematic)
		{
			base.rigidbody.velocity = Vector3.zero;
			base.rigidbody.angularVelocity = Vector3.zero;
		}
		base.rigidbody.useGravity = false;
		base.rigidbody.isKinematic = true;
		base.rigidbody.drag = 0.6f;
		base.rigidbody.angularDrag = 0.6f;
		NGUITools.SetLayer(base.gameObject, LayerMask.NameToLayer("Default"));
		mCarEngineAudio = null;
	}

	public void SetPath(CarPath path, int curIndex)
	{
		mPath = path;
		mCurPathIndex = curIndex;
	}

	public void OnPressAccelBtn(bool isPress)
	{
		mCarControl.OnPressAccelBtn(isPress);
	}

	public void OnPressBrakeBtn(bool isPress)
	{
		mCarControl.OnPressBrakeBtn(isPress);
	}

	public void OnPressLeftBtn(bool isPress)
	{
		mCarControl.OnPressLeftBtn(isPress);
	}

	public void OnPressRightBtn(bool isPress)
	{
		mCarControl.OnPressRightBtn(isPress);
	}

	public void OnCollisionEnter(Collision other)
	{
		if (base.enabled && other.relativeVelocity.sqrMagnitude > 100f && other.gameObject.layer != LayerMask.NameToLayer("ObjCharacter") && other.gameObject.layer != LayerMask.NameToLayer("Floor"))
		{
			EnableStrike(other.gameObject, other.contacts[0].point);
			mHitCarSoundVolume = 0.2f + 0.8f * Mathf.Clamp01(other.relativeVelocity.sqrMagnitude / 1800f);
			soundManager.PlaySoundEffect(mHitCarSoundId, mHitCarSoundVolume);
		}
	}

	public void OnCollisionExit(Collision other)
	{
		DisableStrike(other.gameObject);
	}

	private void ChangeHP(int newHP)
	{
		if (SingletonUnity<CarHPRootLogic>.Exists)
		{
			SingletonUnity<CarHPRootLogic>.Instance.ChangeHP(Mathf.Max(0, newHP));
			if (newHP <= 0)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.FailMission();
			}
		}
	}

	private new void FixedUpdate()
	{
		if (mFreezeCarFlag)
		{
			base.rigidbody.velocity = Vector3.zero;
			base.rigidbody.angularVelocity = Vector3.zero;
			return;
		}
		base.FixedUpdate();
		if (!DriftFlag && !ExitDirftFlag && base.CurSpeed > DriftStartMinSpeed && mCarControl.IsBarking && mCarControl.CurSteerPercent > DriftStartMinAnglePercent)
		{
			EnterDrift();
			DriftFlag = true;
		}
		if (DriftFlag && !ExitDirftFlag && mCarControl.OnTheGroundFlag)
		{
			if (base.CurSpeed < DriftExitMinSpeed || mCarControl.CurSteerPercent < DriftExitMinAnglePercent)
			{
				DriftFlag = false;
				ExitDirftFlag = true;
				ExitDrift();
			}
			else
			{
				float num = Mathf.Sin(mCarControl.CurSpeedPercent);
				curDriftAngleSpeed = num * DriftAngleSpeed * (float)((mCarControl.InputSteer > 0f) ? 1 : (-1));
				curDriftTargetMeshAngle = Mathf.Abs(num * DriftMaxMeshAngle * mCarControl.CurSteerPercent);
				float y = MeshRoot.transform.localEulerAngles.y;
				y = Mathf.Clamp(((!(y > 180f)) ? y : (y - 360f)) + curDriftAngleSpeed * Time.deltaTime, 0f - curDriftTargetMeshAngle, curDriftTargetMeshAngle);
				MeshRoot.transform.localEulerAngles = new Vector3(0f, y, 0f);
			}
		}
		if (ExitDirftFlag)
		{
			float y2 = MeshRoot.transform.localEulerAngles.y;
			y2 = ((!(y2 > 180f)) ? y2 : (y2 - 360f));
			float num2 = y2 + DriftAngleSpeed * 0.5f * (float)((!(y2 > 0f)) ? 1 : (-1)) * Time.deltaTime;
			if (num2 * y2 < 0f)
			{
				num2 = 0f;
				ExitDirftFlag = false;
			}
			MeshRoot.transform.localEulerAngles = new Vector3(0f, num2, 0f);
		}
		if (((mCarControl.IsBarking && base.CurSpeed > 10f) || DriftFlag) && mCarControl.OnTheGroundFlag)
		{
			skidMaskIntensityPercent = Mathf.Min(skidMaskIntensityPercent + Time.deltaTime * 3f, 1f);
			if (CurMountData.IsMotorBool)
			{
				SetSkidmark(ref lastFLSkidmark, FLWheel, FRWheel);
				SetSkidmark(ref lastBLSkidmark, BLWheel, BRWheel);
			}
			else
			{
				SetSkidmark(ref lastFLSkidmark, FLWheel);
				SetSkidmark(ref lastFRSkidmark, FRWheel);
				SetSkidmark(ref lastBLSkidmark, BLWheel);
				SetSkidmark(ref lastBRSkidmark, BRWheel);
			}
			if (GameSettingData.IsCarCopyEffectEnable[GameSettingData.GetPhoneClass()])
			{
				if (mLSkidSmoke != null && !mLSkidSmoke.enableEmission)
				{
					mLSkidSmoke.enableEmission = true;
				}
				if (mRSkidSmoke != null && !mRSkidSmoke.enableEmission)
				{
					mRSkidSmoke.enableEmission = true;
				}
			}
			return;
		}
		lastFLSkidmark = -1;
		lastFRSkidmark = -1;
		lastBLSkidmark = -1;
		lastBRSkidmark = -1;
		skidMaskIntensityPercent = 0f;
		if (!GameSettingData.IsCarCopyEffectEnable[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		if (mLSkidSmoke != null)
		{
			mLSkidSmoke.Play();
			if (mLSkidSmoke.enableEmission)
			{
				mLSkidSmoke.enableEmission = false;
			}
		}
		if (mRSkidSmoke != null)
		{
			mRSkidSmoke.Play();
			if (mRSkidSmoke.enableEmission)
			{
				mRSkidSmoke.enableEmission = false;
			}
		}
	}

	private void EnterDrift()
	{
		mCarControl.maxSteerAngle *= DriftAngleEnhance;
		mCarControl.brakeAcceleration /= DriftBrakeReduce;
		mBrakeSoundVolume = 0.2f + 0.8f * Mathf.Clamp01(base.CurSpeed / 60f);
		soundManager.PlaySoundEffect(mCarDriftSoundId, mBrakeSoundVolume);
	}

	private void ExitDrift()
	{
		mCarControl.maxSteerAngle /= DriftAngleEnhance;
		mCarControl.brakeAcceleration *= DriftBrakeReduce;
		soundManager.StopSoundEffect(mCarDriftSoundId);
	}

	private void SetSkidmark(ref int lastindex, WheelSuspension wheel)
	{
		if (GameSettingData.IsCarCopyEffectEnable[GameSettingData.GetPhoneClass()])
		{
			tempVector3 = wheel.outHit.point + base.rigidbody.velocity * Time.deltaTime * 2f;
			lastindex = mSkidmarks.AddSkidMark(tempVector3, wheel.outHit.normal, mCarControl.CurSpeedPercent * skidMaskIntensityPercent * SkidMaskIntensityReduce, lastindex, 0.4f);
		}
	}

	private void SetSkidmark(ref int lastIndex, WheelSuspension leftWheel, WheelSuspension rightWheel)
	{
		if (GameSettingData.IsCarCopyEffectEnable[GameSettingData.GetPhoneClass()])
		{
			tempVector3 = (leftWheel.outHit.point + rightWheel.outHit.point) / 2f + base.rigidbody.velocity * Time.deltaTime * 2f;
			lastIndex = mSkidmarks.AddSkidMark(tempVector3, leftWheel.outHit.normal, mCarControl.CurSpeedPercent * skidMaskIntensityPercent * SkidMaskIntensityReduce, lastIndex, 0.4f);
		}
	}

	public void FreezeCar()
	{
		mFreezeCarFlag = true;
		if (mCarEngineAudio != null)
		{
			soundManager.StopSoundEffect(mCarEngineSoundId);
		}
		mCarEngineAudio = null;
	}

	public void DisFreezeCar()
	{
		mFreezeCarFlag = false;
		soundManager.PlaySoundEffect(mCarEngineSoundId, 1f, OnPlaySound);
	}

	public void StopCar()
	{
		mStopCarFlag = true;
	}

	private void SynPlayerPosition()
	{
		if (base.enabled && !mFreezeCarFlag && Time.time > ftime && Vector3.SqrMagnitude(mLastPosition - base.Position) > 0.010000001f)
		{
			mLastPosition = base.CacheTransform.position;
			ftime = Time.time + timeWait;
			request.clear();
			pos.clear();
			pos.x = Mathf.CeilToInt(base.CacheTransform.position.x * 100f);
			pos.y = Mathf.CeilToInt(base.CacheTransform.position.y * 100f);
			pos.z = Mathf.CeilToInt(base.CacheTransform.position.z * 100f);
			pos.o = Mathf.CeilToInt(MathUtil.Heading(base.CacheTransform.forward) * 100f);
			request.pos = pos;
			request.moving = true;
			request.index = 1L;
			request.parm = (long)(AttributeData.CurSpeed * 100f);
			NetLogic.GetInstance().Send<Protocol.move>(request);
		}
	}

	public override void EnableCar(ObjCharacter insidePlayer)
	{
		base.EnableCar(insidePlayer);
		NGUITools.SetLayer(base.gameObject, LayerMask.NameToLayer("PlayerCar"));
		if (BLLight != null)
		{
			BLLight.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (BRLight != null)
		{
			BRLight.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		CarBodyRoot.gameObject.tag = "PlayerCar";
		base.gameObject.tag = "PlayerCar";
		if (mFrontTrigger != null)
		{
			mFrontTrigger.gameObject.tag = "PlayerCar";
		}
		soundManager.PlaySoundEffect(mCarEngineSoundId, 1f, OnPlaySound);
		BoxCollider component = CarBodyRoot.GetComponent<BoxCollider>();
		if (component != null)
		{
			component.size = new Vector3(defaultColliderSize.x, defaultColliderSize.y, defaultColliderSize.z);
		}
		insidePlayer.IsLocalDrivingCar = true;
		insidePlayer.CurPlayerCar = this;
		if (insidePlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && SingletonUnity<TouXiangKuangLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TouXiangKuangLogic>.Instance.gameObject))
		{
			SingletonUnity<TouXiangKuangLogic>.Instance.ChangeCarIcon(AttributeData.HP, AttributeData.MaxHP);
		}
	}

	private void OnPlaySound(AudioSource soundSounce)
	{
		mCarEngineAudio = soundSounce;
		mCarEngineAudio.pitch = GetTargetAudioPitch();
	}

	public void BaseDisableCar()
	{
		base.DisableCar();
	}

	public override void DisableCar()
	{
		if (InsidePlayer != null)
		{
			InsidePlayer.IsLocalDrivingCar = false;
			InsidePlayer.CurPlayerCar = null;
			if (SingletonUnity<TouXiangKuangLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TouXiangKuangLogic>.Instance.gameObject))
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.Init();
			}
		}
		base.DisableCar();
		NGUITools.SetLayer(base.gameObject, LayerMask.NameToLayer("Default"));
		CarBodyRoot.gameObject.tag = "Untagged";
		base.gameObject.tag = "Untagged";
		if (mFrontTrigger != null)
		{
			mFrontTrigger.gameObject.tag = "Untagged";
		}
		if (mCarEngineAudio != null)
		{
			soundManager.StopSoundEffect(mCarEngineSoundId);
		}
		mCarEngineAudio = null;
		BoxCollider component = CarBodyRoot.GetComponent<BoxCollider>();
		if (component != null)
		{
			component.size = new Vector3(defaultColliderSize.x, defaultColliderSize.y + FLWheel.WheelRadius * 1.5f, defaultColliderSize.z);
		}
	}

	private void SetCarEngineAudio()
	{
		if (mCarEngineAudio != null)
		{
			mCarEngineAudio.pitch = Mathf.Lerp(mCarEngineAudio.pitch, GetTargetAudioPitch(), 0.5f);
		}
	}

	private float GetTargetAudioPitch()
	{
		return (base.CarControl.CurSpeedPercent + 0.5f) / 1.5f;
	}

	private void OnDisable()
	{
		if (SingletonDontDestoryUnity<SoundManager>.Exists)
		{
			if (mCarEngineAudio != null)
			{
				SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(mCarEngineSoundId);
			}
			SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(mCarDriftSoundId);
			mCarEngineAudio = null;
		}
		ClearStrike();
	}

	public override void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE type)
	{
		if (!base.IsDie)
		{
			long num = AttributeData.HP - newHP;
			if (num > 0)
			{
				UpdateDamgeBoard(type, num);
				OnBeaton();
			}
			else
			{
				UpdateDamgeBoard(type, num);
			}
			if (newHP < 0)
			{
				newHP = 0L;
			}
			AttributeData.HP = newHP;
			if (AttributeData.HP < AttributeData.MaxHP / 2 && SmokeParticle != null && (!UnityVersionUtil.IsActive(SmokeParticle.gameObject) || !SmokeParticle.isPlaying))
			{
				UnityVersionUtil.SetActiveRecursive(SmokeParticle.gameObject, state: true);
				SmokeParticle.Play();
			}
			if (InsidePlayer != null && InsidePlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.ChangeHP(AttributeData.HP, AttributeData.MaxHP);
			}
			if (AttributeData.HP <= 0)
			{
				OnDie();
			}
		}
	}

	public override void ChangeHPVal(long newHP)
	{
		if (base.IsDie)
		{
			return;
		}
		if (AttributeData.HP != newHP)
		{
			AttributeData.HP = newHP;
			if (InsidePlayer != null && InsidePlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
			{
				SingletonUnity<TouXiangKuangLogic>.Instance.ChangeHP(AttributeData.HP, AttributeData.MaxHP);
			}
		}
		if (AttributeData.HP <= 0)
		{
			OnDie();
		}
	}

	public override void OnDie()
	{
		base.IsDie = true;
		if (ExplosionParticle != null)
		{
			UnityVersionUtil.SetActiveRecursive(ExplosionParticle.gameObject, state: true);
			ExplosionParticle.Play();
		}
		MeshRoot.animation.Play("GTACheBaoZa_Animation");
		vp_Timer.In(MeshRoot.animation["GTACheBaoZa_Animation"].length, delegate
		{
			base.rigidbody.useGravity = true;
			base.rigidbody.isKinematic = false;
		});
		SingletonUnity<CitySimController>.Instance.OnPlayerCarDie();
		if (InsidePlayer != null)
		{
			SingletonUnity<UIManager>.Instance.HideBaseUI();
			if (InsidePlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
			{
				ObjMainPlayer objMainPlayer = InsidePlayer as ObjMainPlayer;
				objMainPlayer.EnableMainPlayer();
				base.enabled = false;
				objMainPlayer.transform.parent = null;
				objMainPlayer.transform.position = PlayerDiePos.position;
				objMainPlayer.transform.rotation = PlayerDiePos.rotation;
				objMainPlayer.EnableNavMeshAgent();
				objMainPlayer.rigidbody.isKinematic = false;
				CameraController cameraController = objMainPlayer.CameraController;
				cameraController.LerpBackToPlayer(1f);
				local_character_attack.request request = new local_character_attack.request();
				request.characterId = objMainPlayer.ServerId;
				request.damage = objMainPlayer.AttributeData.HP;
				NetLogic.GetInstance().Send<Protocol.local_character_attack>(request);
				SingletonUnity<CitySimController>.Instance.ResetPlayerUI();
				SingletonUnity<CitySimController>.Instance.PlayerCar = null;
				objMainPlayer.OnDie();
				InsidePlayer = null;
			}
		}
		else if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.isCurMissionTypeEnable(MISSION_LOGICTYPE.DESTROY_CAR))
		{
			local_npc_die.request request2 = new local_npc_die.request();
			request2.type = 1L;
			NetLogic.GetInstance().Send<Protocol.local_npc_die>(request2);
		}
		DisableCar();
	}

	public void UpdateColor()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ColorData colorDataById = DataManager.GetColorDataById(mCurMountData.DefaultColorId);
		List<Material> list = new List<Material>();
		list.Add(CarBodyRoot.gameObject.renderer.sharedMaterial);
		for (int i = 0; i < CarBodyRoot.childCount; i++)
		{
			list.Add(CarBodyRoot.GetChild(i).gameObject.renderer.sharedMaterial);
		}
		Shader shader = Shader.Find(CarBodyRoot.gameObject.renderer.sharedMaterial.shader.name);
		if (shader != null)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j].shader = shader;
			}
		}
		else
		{
			Debug.Log("No Shader");
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].SetColor("_Color", colorDataById.CShaderColor);
			list[k].SetColor("_RimColor", colorDataById.CShaderRimColor);
			list[k].SetFloat("_ReflAmount", colorDataById.ShaderReflAmount);
			list[k].SetFloat("_RimPower", colorDataById.ShaderRimPower);
		}
	}
}
