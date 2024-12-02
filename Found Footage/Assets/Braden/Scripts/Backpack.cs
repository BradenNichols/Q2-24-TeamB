using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public GameObject heldItem;

    // Start is called before the first frame update
    void Start()
    {
        // transform.parent = GameObject.Find("Floor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetItems()
    {
        List<GameObject> items = new();

        for (int i = 0; i < transform.childCount; i++)
            items.Add(transform.GetChild(i).gameObject);

        return items;
    }

    public void AddItem(GameObject item)
    {
        item.transform.parent = transform; // parent it to backpack
    }

    public void EquipItem(GameObject item)
    {
        if (heldItem == item)
        {
            // unequip
        } else
        {

        }
    }
}
