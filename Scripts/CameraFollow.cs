using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float deadzone = 0.15f;
    public float speed = 5f;

    private GameObject player;

	// Use this for initialization
	void Start()
	{
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
	{

	}

    void LateUpdate()
    {
        // remember to set player's rigidbody to Interpolate
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (player && !player.GetComponent<PlayerController>().isDead)
        {
            Vector3 pos = transform.position;
            pos.y = player.transform.position.y;

            if (Vector3.Distance(transform.position, pos) > deadzone)
            {
                transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
            }
        }
    }
}
