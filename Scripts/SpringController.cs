using UnityEngine;

public class SpringController : MonoBehaviour
{
    public float strength = 20f;
    
    private Animator animator;
    private new Collider2D collider;

	// Use this for initialization
	void Start ()
	{
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void JumpedOn()
    {
        animator.SetTrigger("JumpedOn");
        collider.enabled = false;
    }

    private void EnableCollider()
    {
        collider.enabled = true;
    }
}
