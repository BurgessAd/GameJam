using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reactor : MonoBehaviour
{
    public float power;
    public float health;
    public float moderator;
    public List<GameObject> robots;
    public GameObject robot;
    public GameObject healthBar;
    public GameObject powerBar;
    public GameObject moderatorBar;
    public float maxPower = 100;
    public float maxModerator = 100;
    public float maxHealth = 100;
    public int numRobots;
    private int count =0;
    public bool isPlayer = false;
    public static List<GameObject> reactors = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        healthBar.GetComponent<SpriteRenderer>().color = Color.green;
        if (isPlayer)
        {
            powerBar.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            powerBar.transform.localScale = Vector3.zero;
            moderatorBar.transform.localScale = Vector3.zero;
        }

        for(int i = 0; i < numRobots; i++)
        {
            robots.Add(Instantiate(robot, gameObject.transform.position + Random.onUnitSphere * 2, Quaternion.identity));
        }
        


        power = 100;
        health = 100;
        moderator = 100;

    }

    void Setup(int _numRobots, float _power, float _health, float _moderator)
    {
        numRobots = _numRobots;
        power = _power;
        health = _health;
        moderator = _moderator;
    }


    public static void ClearList()
    {
        for (int i = 0; i < reactors.Count; i++)
        {
            Destroy(reactors[i]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        count++;
        healthBar.transform.localScale = new Vector3(health / 100f, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        if (isPlayer)
        {
            powerBar.transform.localScale = new Vector3(power / 100f, powerBar.transform.localScale.y, powerBar.transform.localScale.z);
            moderatorBar.transform.localScale = new Vector3(moderator / 100f, moderatorBar.transform.localScale.y, moderatorBar.transform.localScale.z);
        }
        
        count++;
        if (isPlayer&&count % 50 == 0)
        {
            slowUpdate();
        }

    }


    void fillReactor(float _power, float _moderator)
    {
        if (power + _power <= maxPower)
        {
            power += _power;
        }
        else
        {
            power = maxPower;
        }
        if (moderator + _moderator <= maxModerator)
        {
            moderator += _moderator;
        }
        else
        {
            moderator = maxModerator;
        }
    }


    void slowUpdate()
    {
        power--;
        moderator -= power / 100f;
    }



}
