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

		if (velocity.y < 0)
			DescendSlope (ref velocity);
		
		if (velocity.x != 0)
			collisions.faceDir = (int) Mathf.Sign (velocity.x);

		HorizontalCollision (ref velocity);
		if (velocity.y != 0)
			VerticalCollisions (ref velocity);

		transform.Translate (velocity);
	}

	void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) 
		{
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
//			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			RaycastHit hit;

			Debug.DrawRay (rayOrigin, Vector3.up * directionY * rayLength, Color.red);

			if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask))
			{
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope) 
				{
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}

		if (collisions.climbingSlope) 
		{
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs (velocity.x) + skinWidth;
			Vector3 rayOrigin = (directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector3.up * velocity.y;
			RaycastHit hit;

			if (Physics.Raycast (rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask)) 
			{
				float slopeAngle = Vector3.Angle (hit.normal, Vector3.up);
				if (slopeAngle != collisions.slopeAngle) 
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void HorizontalCollision(ref Vector3 velocity)
	{
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		if (Mathf.Abs (velocity.x) < skinWidth) 
		{
			rayLength = 2 * skinWidth;
		}

		for (int i = 0; i < horizontalRayCount; i++) 
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			//RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			RaycastHit hit;

			Debug.DrawRay (rayOrigin, Vector3.right * directionX * rayLength, Color.red);

			if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask))
			{
				float slopeAngle = Vector3.Angle (hit.normal, Vector3.up);

				if (i == 0 && slopeAngle <= maxClimbAngle) 
				{
					if (collisions.descendingSlope)
					{
						collisions.descendingSlope = false;
						velocity = collisions.velocityPrev;
					}

					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAnglePrev) 
					{
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) 
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope) 
					{
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * Mathf.Abs(velocity.x));
					}
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
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
		float directionX = Mathf.Sign (velocity.x);
		Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit hit;

		if (Physics.Raycast (rayOrigin, -Vector2.up, out hit, Mathf.Infinity, collisionMask)) 
		{
			float slopeAngle = Vector3.Angle (hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) 
			{
				if (Mathf.Sign (hit.normal.x) == directionX) 
				{
//					print (hit.distance - skinWidth + " " + Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x));
					if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) 
					{
						float moveDistance = Mathf.Abs (velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}

	public struct CollisionInfo
	{
		public bool above, below, left, right, climbingSlope, descendingSlope;
		public int faceDir;
		public float slopeAngle, slopeAnglePrev;
		public Vector3 velocityPrev;

		public void Reset()
		{
			above = below = left = right = climbingSlope = descendingSlope = false;
			slopeAnglePrev = slopeAngle;
			slopeAngle = 0;
		}
	}
}
