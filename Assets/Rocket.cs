﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{   
    [SerializeField] float rcsThrust=100f;
    [SerializeField] float mainThrust=20f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                    print("launch pad collided");
                    break;
            case "Fuel":
                    print("Fuel");
                    break;  
            default:
                print("dead");
                //kill
                break;              
        }
    }

    private void Rotate()
    {   
        rigidBody.freezeRotation=true;          //manual rotation control
        
        float rotationThisFrame=rcsThrust*Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {   
            
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {   
            transform.Rotate(-Vector3.forward*rotationThisFrame);
        }

        rigidBody.freezeRotation=false;         //resume physics control
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up*mainThrust);    //relative force means if ship is tilted,force is in y of ship not actual y
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
