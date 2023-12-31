using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMovement : MonoBehaviour
{
    public Transform capsuleTranform;

    public static CapsuleMovement instance;

    public Rigidbody theRB;

    public Animator myAnim;

    [Range(0,100)] public int hp = 100;
    
    [Range(0,20)] public float moveSpeed = 6;

    void Start()
    {
        instance = this;
    }
    
    void Update()
    {
        theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
    
        

    }
}
