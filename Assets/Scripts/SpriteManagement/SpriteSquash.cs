using UnityEngine;
using System.Collections;

public class SpriteSquash : MonoBehaviour 
{
	public enum SquashDirection
	{
		Xaxis,
		Yaxis
	}

	public float m_SquashPercentage = 0.5f;
	public float m_SquashRate = 0.1f;
	public float m_ReboundRate = 0.2f;
	public SquashDirection m_SquashDirection;

	private float m_startScale;
	private bool m_isSquashed = false;

	// Use this for initialization
	void Start () 
	{
		switch (m_SquashDirection) 
		{
			case SquashDirection.Xaxis:
				m_startScale = transform.localScale.x;
				break;
			case SquashDirection.Yaxis:
				m_startScale = transform.localScale.y;
				break;
		}
	}

	float GetCurrentScale ()
	{
		float rtrn = 0f;

		switch( m_SquashDirection )
		{
		case SquashDirection.Xaxis:
			rtrn =  transform.localScale.x;
			break;
		case SquashDirection.Yaxis:
			rtrn = transform.localScale.y;
			break;
		}

		return rtrn;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 scale = transform.localScale;
		float currScale = GetCurrentScale();
		float scaleChange = m_startScale;

		if( Input.GetKey( KeyCode.Space ) )
	   	{
			float targetScale = m_startScale * m_SquashPercentage;
			scaleChange = Mathf.Lerp( currScale, targetScale, m_SquashRate );

			m_isSquashed = true;
		}
		else
		{
			if( currScale >= m_startScale )
			{
				scaleChange = m_startScale;
				m_isSquashed = false;
			}

			if( m_isSquashed )
			{
				scaleChange = Mathf.Lerp( currScale, m_startScale, m_ReboundRate );
			}
		}

		switch( m_SquashDirection )
		{
			case SquashDirection.Xaxis:
				scale.x = scaleChange;
				break;
			case SquashDirection.Yaxis:
				scale.y = scaleChange;
				break;
		}

		transform.localScale = scale;
	}
}
