using UnityEngine;
using System.Collections;

public class SpikeballBehaviour : MonoBehaviour
{
    public float speed = 5f;

    private float worldRange;
    private bool goingRight;

	// Use this for initialization
	void Start ()
	{
        worldRange = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)).x - GetComponent<SpriteRenderer>().bounds.extents.x;
        goingRight = Random.value >= 0.5f ? true : false;
    }
	
	// Update is called once per frame
	void Update ()
	{
        Vector3 movement = new Vector3(speed * Time.deltaTime, 0f, 0f);
	    if (goingRight)
        {
            if (transform.position.x >= worldRange)
            {
                movement.x *= -1;
                goingRight = !goingRight;
            }
        }
        else
        {
            if (transform.position.x <= -worldRange)
            {
                goingRight = !goingRight;
            }
            else
            {
                movement.x *= -1;
            }
        }
        transform.position = transform.position + movement;
    }
}
