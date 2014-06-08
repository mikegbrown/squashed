using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{

	public enum PlayerNumber
	{
		One,
		Two
	}

	public PlayerNumber 		m_PlayerNumber = PlayerNumber.One;
	public Transform			m_BallTransform;
	public float 				m_MoveSpeed = 10.0f;
	public float				m_ChargingMoveSpeed = 3.0f;
	public float				m_MinSwingStrength = 2.0f;
	public float				m_MaxSwingStrength = 10.0f;
	public float				m_SwingStrengthGain = 0.5f;
	public float				m_BallHitDistance = 1.0f;

	private bool				m_isCharging = false;
	private float				m_currentSwingStrength = 0.0f;

	private string 				m_movementHoriztaonInputName = "P1_MovementHorizontal";
	private string 				m_movementVerticalInputName = "P1_MovementVertical";
	private string 				m_diveInputName = "P1_Dive";
	private string 				m_swingInputName = "P1_Swing";

	private Vector2 			m_moveInput;
	private Vector2				m_lastMoveInput;

	// Use this for initialization
	void Start () 
	{
		SetControls();
		Physics2D.IgnoreCollision( transform.collider2D, m_BallTransform.collider2D );
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		ProcessInput();

		UpdateMovement();
		if( m_isCharging )
		{
			UpdateCharging();
		}
	}

	private void UpdateCharging()
	{
		m_currentSwingStrength = Mathf.Clamp( m_currentSwingStrength + m_SwingStrengthGain, m_MinSwingStrength, m_MaxSwingStrength );
	}

	private void UpdateMovement()
	{
		float tempMovespeed = m_isCharging ? m_ChargingMoveSpeed : m_MoveSpeed;

		rigidbody2D.velocity = m_moveInput * tempMovespeed;
	}

	private void ProcessInput()
	{
		m_moveInput = new Vector2( Input.GetAxis( m_movementHoriztaonInputName ), Input.GetAxis( m_movementVerticalInputName ) );
		if( m_moveInput != Vector2.zero )
		{
			m_lastMoveInput = m_moveInput;
		}

		if( Input.GetButtonDown( m_swingInputName ) )
		{
			//start charging swing;
			m_isCharging = true;
		}
		if( Input.GetButtonUp( m_swingInputName ) )
		{
			Swing();

			m_isCharging = false;
			m_currentSwingStrength = m_MinSwingStrength;
		}

		if( Input.GetButtonDown ( m_diveInputName ) )
		{
			//dive
		}
	}

	private void Swing()
	{
		float distance = Vector3.Distance( transform.position, m_BallTransform.position );

		Debug.Log(string.Format( "Distance: {0} Strength: {1}", distance, m_currentSwingStrength ) );

		//play swing animation...
		if(  distance < m_BallHitDistance )
		{
			BallController ball = m_BallTransform.GetComponent<BallController>();
			ball.HitBall( m_currentSwingStrength, m_currentSwingStrength/m_MaxSwingStrength, m_lastMoveInput );
		}
	}

	private void SetControls()
	{
		string playerNum = m_PlayerNumber == PlayerNumber.One ? "P1" : "P2";

		m_movementHoriztaonInputName = string.Format("{0}_MovementHorizontal", playerNum );
		m_movementVerticalInputName = string.Format("{0}_MovementVertical", playerNum );
		m_diveInputName = string.Format("{0}_Dive", playerNum );
		m_swingInputName = string.Format("{0}_Swing", playerNum );
	}
}
