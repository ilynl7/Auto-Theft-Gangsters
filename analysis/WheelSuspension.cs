using UnityEngine;

public class WheelSuspension : MonoBehaviour
{
	public bool FrontFlag;

	public bool LeftFlag;

	private Transform dummyWheelTrs;

	public Transform wheelTrs;

	public CarControl vehicle;

	private Rigidbody vehicleRigidbody;

	private float fullCompressionSpringForce;

	private float suspensionTravel = 0.1f;

	private float wheelRadius = 0.5f;

	private float damperForece;

	private bool onGround = true;

	public RaycastHit outHit;

	private float damperForce = 30000f;

	private Vector3 originalPosition;

	private Vector3 meshOriginalPos;

	private bool mInitFlag;

	private LayerMask castLayer;

	private float wheelAngle;

	private float steerAngle;

	public float WheelRadius => wheelRadius;

	public bool OnGround => onGround;

	private void Start()
	{
		Init(-1f);
	}

	public void Init(float radius = -1f)
	{
		if (wheelTrs == null || mInitFlag)
		{
			return;
		}
		mInitFlag = true;
		onGround = true;
		dummyWheelTrs = base.transform;
		Transform parent = dummyWheelTrs.parent;
		while (parent != null)
		{
			if (parent.rigidbody != null)
			{
				vehicleRigidbody = parent.rigidbody;
				break;
			}
			parent = parent.parent;
		}
		if (vehicleRigidbody == null)
		{
			Debug.LogError("veichle no body");
		}
		fullCompressionSpringForce = vehicleRigidbody.mass * 0.25f * (0f - Physics.gravity.y);
		dummyWheelTrs.localPosition = new Vector3(dummyWheelTrs.localPosition.x, wheelTrs.localPosition.y, dummyWheelTrs.localPosition.z);
		originalPosition = dummyWheelTrs.localPosition;
		meshOriginalPos = wheelTrs.transform.localPosition;
		damperForce = fullCompressionSpringForce * 0.8f;
		if (radius < 0f)
		{
			MeshRenderer componentInChildren = wheelTrs.GetComponentInChildren<MeshRenderer>();
			if (componentInChildren != null)
			{
				wheelRadius = componentInChildren.bounds.extents.y / 1.5f * wheelTrs.localScale.y;
			}
			else
			{
				Transform transform = base.transform.parent.FindChild("houlun");
				if (transform != null)
				{
					componentInChildren = transform.gameObject.renderer.GetComponentInChildren<MeshRenderer>();
					if (componentInChildren != null)
					{
						wheelRadius = componentInChildren.bounds.extents.y * wheelTrs.localScale.y;
					}
					else
					{
						wheelRadius = 0.38f;
					}
				}
				else
				{
					wheelRadius = 0.38f;
				}
			}
		}
		else
		{
			wheelRadius = radius;
		}
		vehicle = vehicleRigidbody.gameObject.GetComponent<CarControl>();
		castLayer = 8388608;
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (!(wheelTrs == null))
		{
			if (LeftFlag)
			{
				wheelAngle += vehicle.CurSpeed / wheelRadius * Time.deltaTime * 57.29578f;
			}
			else
			{
				wheelAngle -= vehicle.CurSpeed / wheelRadius * Time.deltaTime * 57.29578f;
			}
			if (FrontFlag)
			{
				steerAngle = vehicle.CurSteerAngle;
			}
			else
			{
				steerAngle = 0f;
			}
			if (!LeftFlag)
			{
				steerAngle += 180f;
			}
			wheelTrs.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle, 0f));
		}
	}

	private void FixedUpdate()
	{
		if (wheelTrs == null)
		{
			return;
		}
		onGround = Physics.Raycast(dummyWheelTrs.position, -dummyWheelTrs.up, out outHit, suspensionTravel + wheelRadius, castLayer);
		if (onGround && outHit.collider.isTrigger)
		{
			onGround = false;
			float num = suspensionTravel + wheelRadius;
			RaycastHit[] array = Physics.RaycastAll(dummyWheelTrs.position, -dummyWheelTrs.up, suspensionTravel + wheelRadius);
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				if (!raycastHit.collider.isTrigger && raycastHit.distance <= num)
				{
					outHit = raycastHit;
					onGround = true;
					num = raycastHit.distance;
				}
			}
		}
		if (onGround)
		{
			Vector3 pointVelocity = vehicleRigidbody.GetPointVelocity(dummyWheelTrs.position);
			Vector3 lhs = dummyWheelTrs.InverseTransformDirection(pointVelocity);
			Vector3 rhs = dummyWheelTrs.InverseTransformDirection(outHit.normal);
			float num2 = Vector3.Dot(lhs, rhs) * damperForce;
			float num3 = 1f - (outHit.distance - wheelRadius) / suspensionTravel;
			float num4 = Mathf.Clamp01(1f - num3) * suspensionTravel;
			dummyWheelTrs.localPosition = new Vector3(originalPosition.x, originalPosition.y - num4, originalPosition.z);
			num3 = Mathf.Clamp(num3, -3f, 3f);
			Vector3 force = (fullCompressionSpringForce * num3 - num2) * dummyWheelTrs.up;
			vehicleRigidbody.AddForceAtPosition(force, dummyWheelTrs.position);
			wheelTrs.transform.localPosition = new Vector3(meshOriginalPos.x, Mathf.Lerp(wheelTrs.transform.localPosition.y, meshOriginalPos.y - num4, 0.5f), meshOriginalPos.z);
		}
	}
}
