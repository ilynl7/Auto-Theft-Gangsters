using UnityEngine;

public class LensFlareSensor : MonoBehaviour
{
	public Camera cam;

	public float maxDistance;

	public AnimationCurve decayDistance;

	public bool angleValid;

	public float maxAngle;

	public AnimationCurve decayAngle;

	private LensFlare flare;

	public float maxBrightness = 3f;

	public void SetMaxBrightness(float bright)
	{
		maxBrightness = bright;
	}

	private void Start()
	{
		if (cam == null)
		{
			cam = Camera.main;
		}
		flare = GetComponent<LensFlare>();
	}

	private void FixedUpdate()
	{
		if (cam == null)
		{
			cam = Camera.main;
			if (cam == null)
			{
				return;
			}
		}
		Vector3 from = cam.transform.position - base.transform.position;
		float magnitude = from.magnitude;
		float num = decayDistance.Evaluate(magnitude / maxDistance);
		float num2 = 1f;
		if (angleValid)
		{
			float num3 = Vector3.Angle(from, base.transform.forward);
			num2 = decayAngle.Evaluate(num3 / maxAngle);
		}
		float num4 = num * num2;
		if (flare != null)
		{
			flare.brightness = maxBrightness * num4;
		}
		else if (base.light != null)
		{
			base.light.intensity = maxBrightness * num4;
		}
	}
}
