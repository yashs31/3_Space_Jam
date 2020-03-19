using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{   
    [SerializeField] float rcsThrust=100f;
    [SerializeField] float mainThrust=20f;
    [SerializeField] float levelLoadDelay=1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool isTransitioning=false;
    Boolean collisionsDisabled=false;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }
    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled=!collisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isTransitioning|| collisionsDisabled)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                    break;
            case "Finish":
                StartSuccessProcess();//method called after 1s
                break;
            default:
                //kill
                StartDeathProcess();
                break;
        }
    }

    private void StartSuccessProcess()
    {
        isTransitioning=true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    private void StartDeathProcess()
    {
        isTransitioning=true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex=SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex=currentSceneIndex+1;
        if(nextSceneIndex==SceneManager.sceneCountInBuildSettings)  //if scene is last scene in build settings
        {
            nextSceneIndex=0;
        }
        SceneManager.LoadScene(1);
    }

    private void RespondToRotateInput()
    {   
       rigidBody.angularVelocity=Vector3.zero;  //remove rotation due to physics
        
        float rotationThisFrame=rcsThrust*Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {   
            
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {   
            transform.Rotate(-Vector3.forward*rotationThisFrame);
        }

        
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);    //relative force means if ship is tilted,force is in y of ship not actual y
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        print("play");
        mainEngineParticles.Play();
        
    }
}
