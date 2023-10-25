using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;



    private void Start()
    {
        mainCamera = Camera.main;
    }



    private void Update()
    {
        
    }

    private void OnEnable()
    {
        transform.DOMoveY(-2f, 1f);
    }



}
