using UnityEngine;
using System.Collections;

public class Controller3D : RaycastController 
{
	float maxClimbAngle = 65;
    float maxDescendAngle = 45;

    public CollisionInfo collisions;

	public override void Start () 
	{
		base.Start ();
		collisions.faceDir = 1;
	}
		
	public void Move(Vector3 velocity)
	{
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityPrev = velocity;

		//if (velocity.y < 0)
			//DescendSlope (ref velocity);

		HorizontalXCollision (ref velocity);
        HorizontalZCollision (ref velocity);
		if (Mathf.Abs(velocity.y) > 0.001)
			VerticalCollisions (ref velocity);

        SetFaceDir(velocity);

        transform.Translate (velocity);
	}

    void SetFaceDir(Vector3 velocity)
    {
        // 0 is 12 o' clock
        if (Mathf.Abs(velocity.z) > 0.001)
        {
            if (Mathf.Abs(velocity.x) > 0.001)
                collisions.faceDir = 1;
            else if (Mathf.Abs(velocity.x) < -0.001)
                collisions.faceDir = 7;
            else
                collisions.faceDir = 0;
        }
        else if (Mathf.Abs(velocity.z) < -0.001)
        {
            if (Mathf.Abs(velocity.x) > 0.001)
                collisions.faceDir = 3;
            else if (Mathf.Abs(velocity.x) < -0.001)
                collisions.faceDir = 5;
            else
                collisions.faceDir = 4;
        }
        else
        {
            if (Mathf.Abs(velocity.x) > 0.001)
                collisions.faceDir = 2;
            else if (Mathf.Abs(velocity.x) < -0.001)
                collisions.faceDir = 6;
        }
        Debug.Log(velocity);
        Debug.Log(collisions.faceDir);
    }

    void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRowRayCount; i++) 
		{
            for (int j = 0; j < verticalRowRayCount; j++)
            {
                Vector3 rayOrigin = (Mathf.Abs(directionY - -1) < 0.001) ? raycastOrigins.bottom1 : raycastOrigins.top1;
                rayOrigin -= Vector3.right * (verticalRaySpacing * j + velocity.x) - Vector3.forward * (verticalRaySpacing * i + velocity.z);

                Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);

                if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out RaycastHit hit, rayLength, collisionMask))
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    collisions.below = (Mathf.Abs(directionY - -1) < 0.001);
                    collisions.above = (Mathf.Abs(directionY - 1) < 0.001);
                }
            }
		}

		//if (collisions.climbingSlope) 
		//{
		//	float directionX = Mathf.Sign (velocity.x);
		//	rayLength = Mathf.Abs (velocity.x) + skinWidth;
		//	Vector3 rayOrigin = (directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector3.up * velocity.y;
		//	RaycastHit hit;

		//	if (Physics.Raycast (rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask)) 
		//	{
		//		float slopeAngle = Vector3.Angle (hit.normal, Vector3.up);
		//		if (slopeAngle != collisions.slopeAngle) 
		//		{
		//			velocity.x = (hit.distance - skinWidth) * directionX;
		//			collisions.slopeAngle = slopeAngle;
		//		}
		//	}
		//}
	}

	void HorizontalXCollision(ref Vector3 velocity)
	{
        float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		if (Mathf.Abs (velocity.x) < skinWidth) 
		{
			rayLength = 2 * skinWidth;
		}

        for (int i = 0; i < horizontalRowRayCount; i++)
        {
            for (int j = 0; j < horizontalRowRayCount; j++)
            {
                Vector3 rayOrigin = (Mathf.Abs(directionX - -1) < 0.001) ? raycastOrigins.left1 : raycastOrigins.right1;
                rayOrigin += Vector3.down * (horizontalRaySpacing * j); 
                if (Mathf.Abs(directionX - -1) < 0.001)
                    rayOrigin -= Vector3.forward * (horizontalRaySpacing * i);
                else
                    rayOrigin -= Vector3.back * (horizontalRaySpacing * i);

                Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength * 10, Color.red);

                if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out RaycastHit hit, rayLength, collisionMask))
                {
                    print("detected object in x direction");
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    //float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                    //if (i == 0 && slopeAngle <= maxClimbAngle)
                    //{
                    //    if (collisions.descendingSlope)
                    //    {
                    //        collisions.descendingSlope = false;
                    //        velocity = collisions.velocityPrev;
                    //    }

                    //    float distanceToSlopeStart = 0;
                    //    if (slopeAngle != collisions.slopeAnglePrev)
                    //    {
                    //        distanceToSlopeStart = hit.distance - skinWidth;
                    //        velocity.x -= distanceToSlopeStart * directionX;
                    //    }
                    //    ClimbSlope(ref velocity, slopeAngle);
                    //    velocity.x += distanceToSlopeStart * directionX;
                    //}

                    //if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) 
                    //{
                    //  velocity.x = (hit.distance - skinWidth) * directionX;
                    //  rayLength = hit.distance;

                    //  if (collisions.climbingSlope) 
                    //  {
                    //      velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * Mathf.Abs(velocity.x));
                    //  }
                    //  collisions.left = directionX == -1;
                    //  collisions.right = directionX == 1;
                    //}
                }

            }
        }
	}

    void HorizontalZCollision(ref Vector3 velocity)
    {
        float directionZ = Mathf.Sign(velocity.z);
        float rayLength = Mathf.Abs(velocity.z) + skinWidth;

        if (Mathf.Abs(velocity.z) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRowRayCount; i++)
        {
            for (int j = 0; j < horizontalRowRayCount; j++)
            {
                Vector3 rayOrigin = (Mathf.Abs(directionZ - -1) < 0.001) ? raycastOrigins.back1 : raycastOrigins.front1;
                rayOrigin += Vector3.down * (horizontalRaySpacing * j);
                if (Mathf.Abs(directionZ - -1) < 0.001)
                    rayOrigin += Vector3.right * (horizontalRaySpacing * i);
                else
                    rayOrigin -= Vector3.left * (horizontalRaySpacing * i);

                Debug.DrawRay(rayOrigin, Vector3.forward * directionZ * rayLength * 10, Color.red);

                if (Physics.Raycast(rayOrigin, Vector3.forward * directionZ, out RaycastHit hit, rayLength, collisionMask))
                {
                    print("detected object in z direction");
                    velocity.z = (hit.distance - skinWidth) * directionZ;
                    rayLength = hit.distance;
                }

            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
	{
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		if (velocity.y <= climbVelocityY) 
		{
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity)
	{
//		float directionX = Mathf.Sign (velocity.x);
//		Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
//		RaycastHit hit;

//		if (Physics.Raycast (rayOrigin, -Vector2.up, out hit, Mathf.Infinity, collisionMask)) 
//		{
//			float slopeAngle = Vector3.Angle (hit.normal, Vector2.up);
//			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) 
//			{
//				if (Mathf.Sign (hit.normal.x) == directionX) 
//				{
////					print (hit.distance - skinWidth + " " + Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x));
		//			if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) 
		//			{
		//				float moveDistance = Mathf.Abs (velocity.x);
		//				float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		//				velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
		//				velocity.y -= descendVelocityY;

		//				collisions.slopeAngle = slopeAngle;
		//				collisions.descendingSlope = true;
		//				collisions.below = true;
		//			}
		//		}
		//	}
		//}
	}

	public struct CollisionInfo
	{
		public bool above, below, left, right, front, back, climbingSlope, descendingSlope;
		public int faceDir;
		public float slopeAngle, slopeAnglePrev;
		public Vector3 velocityPrev;

		public void Reset()
		{
			above = below = left = right = front = back = climbingSlope = descendingSlope = false;
			slopeAnglePrev = slopeAngle;
			slopeAngle = 0;
            faceDir = 4;
		}
	}
}
