using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class RaycastController : MonoBehaviour 
{
	public const float skinWidth = 0.015f;
    public int horizontalRayCount = 16, verticalRayCount = 16; 
    public int horizontalRowRayCount, verticalRowRayCount;
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

		raycastOrigins.bottom1 = new Vector3 (bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigins.bottom2 = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.bottom3 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigins.bottom4 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.top1 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        raycastOrigins.top2 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigins.top3 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.top4 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
        raycastOrigins.front1 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.front2 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
        raycastOrigins.front3 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.front4 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigins.back1 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigins.back2 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        raycastOrigins.back3 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigins.back4 = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.right1 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        raycastOrigins.right2 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
        raycastOrigins.right3 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.right4 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigins.left1 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.left2 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        raycastOrigins.left3 = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.left4 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
    }

	public void CalculateRaySpacing()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 4, int.MaxValue);
        verticalRayCount = Mathf.Clamp (verticalRayCount, 4, int.MaxValue);

        horizontalRowRayCount = (int)Mathf.Sqrt(horizontalRayCount);
        verticalRowRayCount = (int)Mathf.Sqrt(verticalRayCount);

        horizontalRaySpacing = bounds.size.y / (horizontalRowRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRowRayCount - 1);
	}

	public struct RaycastOrigins
	{
        // front/back: z, right/left: x
        // count corners: top left - 1, top right - 2, bottom right - 3, bottom left - 4
        public Vector3 top1, top2, top3, top4, bottom1, bottom2, bottom3, bottom4,
        front1, front2, front3, front4, back1, back2, back3, back4, left1, left2, left3, left4,
            right1, right2, right3, right4;
	}

    //void Update()
    //{
    //    UpdateRaycastOrigins();
    //    Debug.DrawRay(raycastOrigins.bottom1, Vector3.down, Color.red);
    //    Debug.DrawRay(raycastOrigins.bottom2, Vector3.down, Color.red);
    //    Debug.DrawRay(raycastOrigins.bottom3, Vector3.down, Color.red);
    //    Debug.DrawRay(raycastOrigins.bottom4, Vector3.down, Color.red);
    //    Debug.DrawRay(raycastOrigins.top1, Vector3.up, Color.red);
    //    Debug.DrawRay(raycastOrigins.top2, Vector3.up, Color.red);
    //    Debug.DrawRay(raycastOrigins.top3, Vector3.up, Color.red);
    //    Debug.DrawRay(raycastOrigins.top4, Vector3.up, Color.red);
    //    Debug.DrawRay(raycastOrigins.front1, Vector3.forward, Color.red);
    //    Debug.DrawRay(raycastOrigins.front2, Vector3.forward, Color.red);
    //    Debug.DrawRay(raycastOrigins.front3, Vector3.forward, Color.red);
    //    Debug.DrawRay(raycastOrigins.front4, Vector3.forward, Color.red);
    //    Debug.DrawRay(raycastOrigins.back1, Vector3.back, Color.red);
    //    Debug.DrawRay(raycastOrigins.back2, Vector3.back, Color.red);
    //    Debug.DrawRay(raycastOrigins.back3, Vector3.back, Color.red);
    //    Debug.DrawRay(raycastOrigins.back4, Vector3.back, Color.red);
    //    Debug.DrawRay(raycastOrigins.right1, Vector3.right, Color.red);
    //    Debug.DrawRay(raycastOrigins.right2, Vector3.right, Color.red);
    //    Debug.DrawRay(raycastOrigins.right3, Vector3.right, Color.red);
    //    Debug.DrawRay(raycastOrigins.right4, Vector3.right, Color.red);
    //    Debug.DrawRay(raycastOrigins.left1, Vector3.left, Color.red);
    //    Debug.DrawRay(raycastOrigins.left2, Vector3.left, Color.red);
    //    Debug.DrawRay(raycastOrigins.left3, Vector3.left, Color.red);
    //    Debug.DrawRay(raycastOrigins.left4, Vector3.left, Color.red);
    //}


}
