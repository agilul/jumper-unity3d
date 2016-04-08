using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private GameObject player;

	// Use this for initialization
	void Start ()
	{
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    void OnBecameInvisible()
    {
        if (player && transform.position.y < player.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
