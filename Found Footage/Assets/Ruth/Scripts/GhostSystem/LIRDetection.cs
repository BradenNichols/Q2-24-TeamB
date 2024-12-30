using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIRDetection : MonoBehaviour
{
    //if you added another option next to Melee you can choose between 2 in the inspector(Tutorial vibes)
    public enum Lady {Melee}
    public Lady Filler;
    Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        //finds the player 
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isfront()
    {
        //gives a yay or nay if enemy is infront of player
        Vector3 directionOfPlayer = transform.position - Player.position;
        float angle = Vector3.Angle(transform.forward, directionOfPlayer);

        if(Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            Debug.DrawLine(transform.position, Player.position, Color.red);
            return true;
        }

        return false;
    }
}
