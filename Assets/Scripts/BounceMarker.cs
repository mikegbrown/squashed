using UnityEngine;
using System.Collections;

public class BounceMarker : MonoBehaviour 
{

	public float m_FadeRate = 0.1f;

	private SpriteRenderer m_spriteRenderer;
	// Use this for initialization
	void Start () {
		m_spriteRenderer = transform.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Color tempColor = m_spriteRenderer.color;
		tempColor.a -= m_FadeRate;

		if( tempColor.a <= 0f )
		{
			Destroy( gameObject );
		}
	
		m_spriteRenderer.color = tempColor;
	}
}
