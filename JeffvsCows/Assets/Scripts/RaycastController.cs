using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class RaycastController : MonoBehaviour 
{
	public const float skinWidth = 0.015f;
	public int horizontalRayCount = 4, verticalRayCount = 4;
	public LayerMask collisionMask;

	[HideInInspector]
	public float horizontalRaySpacing, verticalRaySpacing;

	[HideInInspector]
	public new BoxCollider collider;
	public RaycastOrigins raycastOrigins;

	// Use this for initialization
	public virtual void Start () 
	{
		collider = GetComponent<BoxCollider> ();
		CalculateRaySpacing ();
	}

	public void UpdateRaycastOrigins()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector3 (bounds.min.x, bounds.min.y, 0);
		raycastOrigins.bottomRight = new Vector3 (bounds.max.x, bounds.min.y, 0);
		raycastOrigins.topLeft = new Vector3 (bounds.min.x, bounds.max.y, 0);
		raycastOrigins.topRight = new Vector3 (bounds.max.x, bounds.max.y, 0);
	}

	public void CalculateRaySpacing()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins
	{
		public Vector3 topLeft, topRight, bottomLeft, bottomRight;
	}
}
