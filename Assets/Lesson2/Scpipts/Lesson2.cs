using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lesson2 : MonoBehaviour
{
    [SerializeField] private string value;

    private List<string> Strings = new List<string>();

    [ContextMenu("Get All Items")]
    private void GetAllItems()
    {
        string msg = "List: ";

        for (int i = 0; i < Strings.Count; i++)
        {
            msg += $"\n {Strings[i]}";
        }

        Debug.Log(msg);
    }

    [ContextMenu("Get Item")]
    private void GetItem()
    {
        string item = Strings.Find(i => i == value);
        if (!string.IsNullOrEmpty(item))
        {
            Debug.Log(item);
        }
        else
        {
            Debug.Log($"{value} do not exist this list");
        }
    }

    [ContextMenu("Add Item")]
    private void AddItem()
    {
        Strings.Add(value);
    }

    [ContextMenu("Remove Item")]
    private void RemoveItem()
    {
        string item = Strings.Find(i => i == value);
        if (!string.IsNullOrEmpty(item))
        {
            Strings.Remove(value);
        }
        else
        {
            Debug.Log($"{value} do not exist this list");
        }
    }

    [ContextMenu("Sort List")]
    private void SortLIst()
    {
        Strings.Sort();

        GetAllItems();
    }

    [ContextMenu("Clear List")]
    private void ClearList()
    {
        Strings.Clear();

        GetAllItems();
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
