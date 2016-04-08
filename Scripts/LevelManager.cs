using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject[] platformPrefabs;
    public GameObject[] coinPrefabs;
    public GameObject springPrefab;
    public GameObject[] powerupPrefabs;
    public GameObject[] enemyPrefabs;

    public Text scoreText;

    public float minDistance = 2f;
    public float maxDistance = 2.7f;

    private GameObject player;
    private GameObject tempObj;
    private SpriteRenderer lastPlatform = null;
    private float lastHeight;
    private float platformRange;

    private int highest;
    public static int bonusScore;
    private float prevHighest; // previous height score
    private int prevBonusScore; // previous bonus score

	// Use this for initialization
	void Start ()
	{
        player = (GameObject)Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

        tempObj = (GameObject)Instantiate(platformPrefabs[0], new Vector3(0f, -2f, 0f), Quaternion.identity);
        lastPlatform = tempObj.GetComponent<SpriteRenderer>();
        lastHeight = tempObj.transform.position.y;

        platformRange = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)).x - lastPlatform.bounds.extents.x;

        highest = 0;
        bonusScore = 0;
        prevHighest = 0f;
        prevBonusScore = 0;
        
        CreatePlatforms();
    }
	
	// Update is called once per frame
	void Update ()
	{
        bool updateScore = false;

        if (player.transform.position.y > prevHighest)
        {
            prevHighest = player.transform.position.y;
            highest = Mathf.RoundToInt(prevHighest * 10);
            updateScore = true;
        }

        if (bonusScore > prevBonusScore)
        {
            prevBonusScore = bonusScore;
            updateScore = true;
        }

        if (updateScore)
        {
            scoreText.text = (highest + bonusScore).ToString();
        }

        CreatePlatforms();
	}

    private void CreatePlatforms()
    {
        while (lastPlatform.isVisible)
        {
            int index = Random.Range(0, platformPrefabs.Length);
            float randomX = Random.Range(-platformRange, platformRange);
            float randomY = lastHeight + Random.Range(minDistance, maxDistance);
            tempObj = (GameObject)Instantiate(platformPrefabs[index], new Vector3(randomX, randomY, 0f), Quaternion.identity);
            lastPlatform = tempObj.GetComponent<SpriteRenderer>();
            lastHeight = tempObj.transform.position.y;

            // spawn stuff on large platforms
            if (index == 0)
            {
                int randomInt = Random.Range(0, 10);
                switch (randomInt)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        Instantiate(coinPrefabs[Random.Range(0, coinPrefabs.Length)], tempObj.transform.position + new Vector3(0f, 1.1f, 0f), Quaternion.identity);
                        break;

                    case 4:
                    case 5:
                        (Instantiate(springPrefab, tempObj.transform.position + new Vector3(0f, 0.47f, 0f), Quaternion.identity) as GameObject).transform.parent = tempObj.transform;
                        break;

                    case 6:
                    case 7:
                    case 8:
                        Instantiate(powerupPrefabs[Random.Range(0, powerupPrefabs.Length)], tempObj.transform.position + new Vector3(0f, 1.1f, 0f), Quaternion.identity);
                        break;
                    case 9:
                        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], tempObj.transform.position + new Vector3(0f, 1.3f, 0f), Quaternion.identity);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
