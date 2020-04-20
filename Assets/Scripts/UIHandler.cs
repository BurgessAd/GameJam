using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public Text reactorText;
    public ReactorComponent playerReactor;
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        //playerReactor = tilemap.GetComponent<TerrainGenerator>().playerReactor.GetComponent<ReactorComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerReactor == null)
        {
            playerReactor = tilemap.GetComponent<TerrainGenerator>().playerReactor.GetComponent<ReactorComponent>();
        }
        reactorText.text = $"Power: {(int)(playerReactor.power * 100f / playerReactor.maxPower)}% \nModerator: {(int)(playerReactor.moderator*100f/playerReactor.maxModerator)}%";
    }
}
