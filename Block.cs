using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isBlack = false;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    public void Blacken()
    {
        isBlack = true;
    }
    public void ChangeColor(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        Blacken();
    }
    public void Shrink()
    {
        animator.SetTrigger("Shrink");

    }

}
