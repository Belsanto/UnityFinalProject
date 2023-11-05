using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, bool> itemsDictionary = new Dictionary<string, bool>();
    private static GameManager instance;

    // Arreglo de notes
    public bool[] Notes { get; set; }
    private int hitCount = 0;
    private bool isImmune = false;
    private float immunityTime = 2f;
    private float lastHitTime = 0f;

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

    private void Start()
    {
        Notes = new bool[6] { false, false, false, false, false, false };
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
        Notes = new bool[6] { false, false, false, false, false, false };
        itemsDictionary = new Dictionary<string, bool>();
        hitCount = 0;
    }

    // Función para establecer en true la posición indicada en el arreglo de notes
    public void FindNote(int position)
    {
        if (position >= 0 && position < Notes.Length)
        {
            Notes[position] = true;
        }
        else
        {
            Debug.Log("La posición especificada está fuera del rango del arreglo de notes.");
        }
    }
    
    public void ActivateNote(int posicion)
    {
        if (!GetNoteState(posicion))
        {
            FindNote(posicion);
        }
    }

    public bool GetNoteState(int posicion)
    {
        if (posicion >= 0 && posicion < Notes.Length)
        {
            return Notes[posicion];
        }
        else
        {
            Debug.Log("La posición especificada está fuera del rango del arreglo de notas.");
            return false;
        }
    }
    public int TotalNotes()
    {
        int count = 0;
        foreach (bool note in Notes)
        {
            if (note)
            {
                count++;
            }
        }
        return count;
    }

    public bool IsDeath()
    {
        return (hitCount >= 5);
    }
    public bool IncreaseHit()
    {
        if (Time.time - lastHitTime > immunityTime)
        {
            hitCount++;
            lastHitTime = Time.time;
            return true;
        }

        return false;
    }

}