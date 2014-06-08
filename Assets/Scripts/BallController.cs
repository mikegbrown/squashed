using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour 
{
	public Vector2 			m_initialVelocity;

	public Transform		m_Shadow;
	public float			m_BounceDegradePercentage = 0.1f;
	public float			m_GroundBouncePercentage = 0.5f;
	public float			m_GroundBounceSpeed = 0.5f;

	public float 			m_Gravity = 0.1f;
	public float			m_MaxHitHeight = 10.0f;
	public float			m_MinHitHeight = 5.0f;
	public float			m_MaxShadowDistance = 0.3f;

	private float 			m_currBallHeight = 0f;
	private float			m_bounceHeight = 0f;

	private bool			m_isBouncing = false;

	// Use this for initialization
	void Start () 
	{
		rigidbody2D.velocity = m_initialVelocity;
		m_currBallHeight = m_MaxHitHeight;
		m_bounceHeight = m_MaxHitHeight;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( m_currBallHeight > 0f && !m_isBouncing )
		{
			m_currBallHeight -= m_Gravity;
		}
		else
		{
			if( m_isBouncing == false )
			{
				rigidbody2D.velocity *= (1.0f - m_BounceDegradePercentage );
			}

			m_currBallHeight = Mathf.Lerp( m_currBallHeight, m_bounceHeight * m_GroundBouncePercentage, m_GroundBounceSpeed );
			m_isBouncing = true;

			if( m_currBallHeight > (m_bounceHeight * m_GroundBouncePercentage ) || Mathf.Approximately(m_currBallHeight, m_bounceHeight * m_GroundBouncePercentage ))
		   	{
				m_isBouncing = false;
				m_bounceHeight = m_currBallHeight;
			}
		}

		Vector3 currShadowPos = m_Shadow.localPosition;
		currShadowPos.y = m_MaxShadowDistance * ( m_currBallHeight/m_MaxHitHeight ) * -1f;

		m_Shadow.localPosition = currShadowPos;
	}

	public void HitBall( float strength, float strengthPercentage, Vector2 direction )
	{
		rigidbody2D.velocity = direction * strength;
		m_currBallHeight = m_MaxHitHeight;
		m_bounceHeight = m_currBallHeight;
		m_isBouncing = false;
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		rigidbody2D.velocity *= (1.0f - m_BounceDegradePercentage );
	}
}
