using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyLesson2
{
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
            if (Strings.Contains(value))
            {
                Debug.Log(value);
            }
            else
            {
                Debug.Log($"{value} do not exist this list");
            }
        }

        [ContextMenu("Add Item")]
        private void AddItem()
        {
            if (value != string.Empty)
            {
                Strings.Add(value);
            }
            else
            {
                Debug.Log($"Fild value is emptty");
            }
        }

        [ContextMenu("Remove Item")]
        private void RemoveItem()
        {
            if (Strings.Remove(value))
            {
                Debug.Log($"{value} is deleted");
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
}