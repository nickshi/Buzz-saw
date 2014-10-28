using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	private const float		SkinWidth		= .02f;
	private const int		TotalHorizonRays	= 8;
	private const int		TotalVerticalRays	= 4;
	private static readonly float	SlopeLimitTarget	= Mathf.Tan( 75f * Mathf.Deg2Rad );
	public LayerMask		PlatformMask;
	public ControllerParameters2D	DefaultParameters;

	public ControllerState2D state {
		get; private set;
	}

	public Vector2 Velocity {
		get { return(_velocity); }
	}

	public bool CanJump {
		get { return(false); }
	}
	public bool HandleCollisions {
		get { return(false); }
	}
	public ControllerParameters2D Parameters {
		get { return(_overrideParameters ?? DefaultParameters); }
	};

	private Vector2			_velocity;
	private Transform		_transform;
	private Vector3			_localScale;
	private BoxCollider2D		_boxCollider;
	private float			_verticalDistanceBetweenRays;
	private float			_horizontalDistanceBetweenRays;
	private ControllerParameters2D	_overrideParameters;
	private Vector3			_raycastTopLeft;
	private Vector3			_raycastBottomLeft;
	private Vector3			_raycastBottomRight;

	public void Awake()
	{
		state		= new ControllerState2D();
		_transform	= transform;
		_localScale	= transform.localScale;
		_boxCollider	= GetComponent<BoxCollider2D> ();
	}


	public void AddForce( Vector2 force )
	{
		_velocity += force;
	}


	public void SetForce( Vector2 force )
	{
		_velocity = force;
	}


	public void SetHorizontalForce( float x )
	{
		_velocity.x = x;
	}


	public void SetVerticalForce( float y )
	{
		_velocity.y = y;
	}


	public void Jump()
	{
	}


	public void LateUpdate()
	{
		Move( Velocity * Time.deltaTime );
	}


	private void Move( Vector2 deltaMovement )
	{
		var wasGronded = state.IsCollidingBelow;
		state.Reset();

		if ( HandleCollisions )
		{
			HandlePlatforms();
			CalculateRayOrigins();

			if ( deltaMovement.y < 0 && wasGronded )
			{
				HandleVerticalSlope( ref deltaMovement );
			}

			if ( Mathf.Abs( deltaMovement.x ) > .001f )
				MoveHorizontally( ref deltaMovement );

			MoveVertically( ref deltaMovement );
		}

		_transform.Translate( deltaMovement, Space.World );

		/* TODO: Additional moing platform code */

		if ( Time.deltaTime > 0 )
			_velocity = deltaMovement / Time.deltaTime;

		_velocity.x	= Mathf.Min( _velocity.x, Parameters.MaxVelocity.x );
		_velocity.y	= Mathf.Min( _velocity.y, Parameters.MaxVelocity.y );

		if ( state.IsMovingUpSlope )
			_velocity.y = 0;
	}


	private void HandlePlatforms()
	{
	}


	private void CalculateRayOrigins()
	{
		var	size	= new Vector2( _boxCollider.size.x * Mathf.Abs( _localScale.x ), _boxCollider.size.y * Mathf.Abs( _localScale.y ) ) / 2;
		var	center	= new Vector2( _boxCollider.center.x * _localScale.x, _boxCollider.center.y * _localScale.y );
		_raycastTopLeft		= _transform.position + new Vector3( center.x - size.x + SkinWidth, center.y + size.y - SkinWidth );
		_raycastBottomRight	= _transform.position + new Vector3( center.x + size.x - SkinWidth, center.y - size.y + SkinWidth );
		_raycastBottomLeft	= _transform.position + new Vector3( center.x - size.x + SkinWidth, center.y - size.y + SkinWidth );
	}


	private void MoveHorizontally( ref Vector2 deltaMovement )
	{
		var isGoingRight = deltaMovement.x > 0;
	}


	private void MoveVertically( ref Vector2 deltaMovement )
	{
	}


	private void HandleVerticalSlope( ref Vector2 deltaMovement )
	{
	}


	private void HandleHorizontsSlope( ref Vector2 deltaMovement, float angle, bool isGoingHight )
	{
	}


	public void OnTriggerEnter2D( Collider2D other )
	{
	}


	public void OnTriggerExit2D( Collider2D other )
	{
	}
}
