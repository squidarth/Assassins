using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class StaticItemManager : MonoBehaviour
{
    public static List<ItemObject> AllGameItems;
    // Start is called before the first frame update
    public static int ToIndex(ItemObject itemObject)
    {
        return AllGameItems.IndexOf(itemObject);
    }

    public static ItemObject FromIndex(int index)
    {
        return AllGameItems[index];
    }

    void Start()
    {
        StaticItemManager.AllGameItems = Resources.LoadAll("", typeof(ItemObject)).Cast<ItemObject>().ToList();
        foreach (var item in StaticItemManager.AllGameItems)
        {
            Debug.Log(item.type);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
