using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 10f;
    public bool isDead = false;
    
    public float jetpackTime = 2f;
    public float jetpackStrength = 30f;

    public int wingsUses = 3;
    public float wingsStrength = 15f;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Animator animator;
    private float movement;

    // jetpack variables
    private float jetpackCount;
    private GameObject jetpackObject;
    private GameObject jetpackLeftFlame;
    private GameObject jetpackRightFlame;

    // wings variables
    private int wingsCount;
    private GameObject wingsObject;

    // bubble variables
    private GameObject bubbleObject;

	// Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        movement = 0f;
        isDead = false;

        jetpackCount = 0f;
        jetpackObject = GameObject.Find("/" + gameObject.name + "/Jetpack");
        jetpackLeftFlame = GameObject.Find("/" + gameObject.name + "/Jetpack/FlameL");
        jetpackRightFlame = GameObject.Find("/" + gameObject.name + "/Jetpack/FlameR");
        jetpackObject.SetActive(false);

        wingsCount = 0;
        wingsObject = GameObject.Find("/" + gameObject.name + "/Wings");
        wingsObject.SetActive(false);

        bubbleObject = GameObject.Find("/" + gameObject.name + "/Bubble");
        bubbleObject.SetActive(false);
    }
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.position = new Vector2(0f, 0f);
        }

        movement = 0f;
	    if (Input.GetButton("Fire1") && !isDead)
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                movement = -speed;
            }
            else
            {
                movement = speed;
            }
        }

        if (wingsCount > 0 && Input.GetMouseButtonDown(1) && !isDead)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, wingsStrength);
            animator.SetTrigger("useWings");
            wingsCount--;
        }

        if (rigidbody.velocity.y < -30f)
        {
            animator.SetTrigger("dead");
            isDead = true;
        }
    }

    void FixedUpdate()
    {
        if (jetpackCount > 0f)
        {
            rigidbody.velocity = new Vector2(movement, jetpackStrength);
            jetpackCount -= Time.deltaTime;
        }
        else
        {
            rigidbody.velocity = new Vector2(movement, rigidbody.velocity.y);
        }

        if (jetpackObject.activeSelf)
        {
            if (rigidbody.velocity.y < 1f)
            {
                jetpackObject.SetActive(false);
            }
            else
            {
                float scale = Mathf.InverseLerp(1f, jetpackStrength, rigidbody.velocity.y);
                jetpackLeftFlame.transform.localScale = new Vector3(Mathf.Min(scale * 2f, 1f), scale, 1f);
                jetpackRightFlame.transform.localScale = new Vector3(Mathf.Min(scale * 2f, 1f), scale, 1f);
            }
        }

        if (wingsObject.activeSelf && wingsCount <= 0 && rigidbody.velocity.y < 1f)
        {
            wingsObject.SetActive(false);
        }

        animator.SetFloat("velocity", rigidbody.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.tag == "Platform")
        {
            if (rigidbody.velocity.y <= 0f && Mathf.Abs(collider.bounds.min.y - other.bounds.max.y) < 0.5f)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jump);
                other.GetComponent<PlatformController>().LandedOn();
            }
        }
        else if (other.tag == "Jetpack")
        {
            Destroy(other.gameObject);
            jetpackCount = jetpackTime;
            jetpackObject.SetActive(true);
        }
        else if (other.tag == "Wings")
        {
            Destroy(other.gameObject);
            wingsCount = wingsUses;
            wingsObject.SetActive(true);
        }
        else if (other.tag == "Bubble")
        {
            Destroy(other.gameObject);
            bubbleObject.SetActive(true);
        }
        else if (other.tag == "Enemy")
        {
            if (jetpackObject.activeSelf)
            {
                return; // ignore enemy collision when jetpacking
            }
            if (bubbleObject.activeSelf)
            {
                bubbleObject.SetActive(false); // bubble saved us
                return;
            }

            animator.SetTrigger("dead");
            rigidbody.freezeRotation = false;
            isDead = true;
            if (Camera.main.WorldToViewportPoint(transform.position).x < 0.5f)
            {
                rigidbody.AddForceAtPosition(new Vector2(1f, 0f), transform.position + new Vector3(0f, 1f, 0f), ForceMode2D.Impulse);
            }
            else
            {
                rigidbody.AddForceAtPosition(new Vector2(-1f, 0f), transform.position + new Vector3(0f, 1f, 0f), ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;

        if (other.tag == "Spring")
        {
            if (rigidbody.velocity.y <= 0f && collider.bounds.min.y > other.bounds.min.y)
            {
                SpringController sc = other.GetComponent<SpringController>();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, sc.strength);
                sc.JumpedOn();
            }
        }
    }
}
