using System;
using UnityEngine;

public class Rays : MonoBehaviour
{
    private Camera cam;
    public void Start()
    {
        cam=Camera.main;
        Debug.Log("cam width = "+cam.pixelWidth+" cam height = "+cam.pixelHeight);
        Debug.Log("screen width = "+Screen.width+" screen height = "+Screen.height);
        InvokeRepeating(nameof(Coord),5,10);   
    }

    private void Coord()
    {
        Debug.Log(cam.WorldToScreenPoint(transform.position));
        
    }
}