using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
   
    public void Rotate()
    {
        StartCoroutine(RotateCamera());
    }
    IEnumerator RotateCamera()
    {
        for (int i = 0; i < 55; i++)
        {
            yield return new WaitForSeconds(0);
            transform.RotateAround(Vector3.zero, Vector3.up, 3);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
