using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour 
{
	public Vector2 			m_initialVelocity;

	public Transform		m_Shadow;
	public float			m_BounceDegradePercentage = 0.1f;
	public float			m_GroundBouncePercentage = 0.5f;
	public float			m_GroundBounceSpeed = 0.5f;
	public float			m_BounceMomentumDegradeRate = 0.7f;

	public float 			m_Gravity = 0.1f;
	public float			m_MaxHitHeight = 10.0f;
	public float			m_MinHitHeight = 5.0f;
	public float			m_MaxShadowDistance = 0.3f;

	private float 			m_currBallHeight = 0f;
	private float			m_bounceHeight = 0f;
	private float 			m_bounceMomentum = 0f;

	private bool			m_isBouncing = false;
	private int				m_bounceCount = 0;

	// Use this for initialization
	void Start () 
	{
		rigidbody2D.velocity = m_initialVelocity;
		m_currBallHeight = m_MaxHitHeight;
		m_bounceHeight = m_MaxHitHeight;
	}
	
	private void Bounce()
	{
		m_isBouncing = true;
		m_bounceCount++;

		rigidbody2D.velocity *= (1.0f - m_BounceDegradePercentage );

		float targetBounceHeight = m_bounceHeight * m_GroundBouncePercentage;
		m_bounceMomentum = Mathf.Lerp( m_currBallHeight, targetBounceHeight, m_GroundBounceSpeed );

		m_bounceHeight = targetBounceHeight;

	}

	// Update is called once per frame
	void Update () 
	{
		//add gravity
		m_currBallHeight = Mathf.Clamp( m_currBallHeight - m_Gravity, 0f, m_MaxHitHeight );

		if( m_currBallHeight <= 0f )
		{
			if( m_bounceHeight > 0f )
				Bounce();
		}

		if( m_bounceCount >= 2 )
		{
			SetBallColor( Color.red );
		}
		else
		{
			SetBallColor( Color.white );
		}

		if( m_bounceMomentum > 0f )
		{
			m_currBallHeight += m_bounceMomentum;
			m_bounceMomentum *= m_BounceMomentumDegradeRate;
		}

		//update shadow position
		Vector3 currShadowPos = m_Shadow.localPosition;
		currShadowPos.y = m_MaxShadowDistance * ( m_currBallHeight/m_MaxHitHeight ) * -1f;

		m_Shadow.localPosition = currShadowPos;
	}

	public void HitBall( float strength, float strengthPercentage, Vector2 direction )
	{
		rigidbody2D.velocity = direction * strength;

		float hitHeight = Mathf.Clamp( m_MaxHitHeight, m_MinHitHeight, m_MaxHitHeight );

		m_bounceHeight = hitHeight;
		m_bounceMomentum = Mathf.Lerp( m_currBallHeight, hitHeight, m_GroundBounceSpeed  );

		Debug.Log( m_bounceMomentum );

		m_bounceCount = 0;
	}

	private void SetBallColor( Color color )
	{
		SpriteRenderer spr = transform.GetComponent<SpriteRenderer>();
		spr.color = color;
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		rigidbody2D.velocity *= (1.0f - m_BounceDegradePercentage );
	}
}
