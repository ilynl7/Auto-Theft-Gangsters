using UnityEngine;

public class CarStaticCrashObj : MonoBehaviour
{
	public float DisactiveTime = 5f;

	private float mTimeCount;

	private bool EnableFlag;

	private void Awake()
	{
		Reset();
	}

	private void Update()
	{
		if (EnableFlag)
		{
			mTimeCount += Time.deltaTime;
			if (mTimeCount > DisactiveTime)
			{
				DisactiveSelf();
			}
		}
	}

	private void DisactiveSelf()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
	}

	private void Reset()
	{
		base.rigidbody.isKinematic = true;
		base.rigidbody.useGravity = false;
		EnableFlag = false;
	}

	private void EnableRigidBody()
	{
		base.rigidbody.isKinematic = false;
		base.rigidbody.useGravity = true;
		EnableFlag = true;
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			base.rigidbody.centerOfMass = component.center - Vector3.forward * component.height / 4f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!EnableFlag && (other.CompareTag("PlayerCar") || other.CompareTag("PoliceCar")) && !(other.attachedRigidbody == null) && other.attachedRigidbody != null && other.attachedRigidbody.velocity.sqrMagnitude > 100f)
		{
			EnableRigidBody();
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!EnableFlag && (other.gameObject.CompareTag("PlayerCar") || other.gameObject.CompareTag("PoliceCar")) && !(other.rigidbody == null) && other.rigidbody != null && other.rigidbody.velocity.sqrMagnitude > 100f)
		{
			EnableRigidBody();
		}
	}
}
