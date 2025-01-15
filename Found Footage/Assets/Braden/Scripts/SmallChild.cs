using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallChild : BaseEnemy
{
    [Header("SmallMare AI")]
    public string aiState = "Patrol";

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        // TODO: add patrol and chase

        base.Update(); // pathing and such
    }
}
