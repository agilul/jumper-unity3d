using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Sprite broken;
    public Material primary;
    public Material secondary;
    public ParticleSystem effect;

    public bool breakable = false;
    public bool isBroken = false;
    public int size = 4;

    private new SpriteRenderer renderer;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    public void LandedOn()
    {
        if (breakable)
        {
            if (isBroken)
            {
                ParticleSystem ps = (ParticleSystem)Instantiate(effect, transform.position, Quaternion.identity);
                ps.GetComponent<Renderer>().material = primary;
                ps.maxParticles = size;
                Destroy(ps.gameObject, ps.startLifetime);

                ps = (ParticleSystem)Instantiate(effect, transform.position, Quaternion.identity);
                ps.GetComponent<Renderer>().material = secondary;
                ps.maxParticles = size / 2;
                Destroy(ps.gameObject, ps.startLifetime);

                Destroy(gameObject);
            }
            else
            {
                renderer.sprite = broken;
                isBroken = true;
            }
        }
    }
}
