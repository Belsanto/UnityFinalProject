using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, bool> itemsDictionary = new Dictionary<string, bool>();
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("GameManagerSingleton");
                    instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AcquireItem(string itemType)
    {
        if (!itemsDictionary.ContainsKey(itemType))
        {
            itemsDictionary.Add(itemType, true);
        }
    }

    public bool ValidateItem(string itemType)
    {
        return itemsDictionary.ContainsKey(itemType);
    }

    public bool HasItem(string itemType)
    {
        return itemsDictionary.TryGetValue(itemType, out bool value) && value;
    }

    public void ResetItems()
    {
        itemsDictionary = new Dictionary<string, bool>();
    }
}