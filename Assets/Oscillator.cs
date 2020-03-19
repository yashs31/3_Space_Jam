using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period=2f;
    [Range(0,1)] [SerializeField]float movementFactor;
    Vector3 startingPos;
    void Start()
    {
        startingPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        if(period<=Mathf.Epsilon)
        {
            return;
        }
        float cycles=Time.time/period;      //grows continually from 0
        const float tau=Mathf.PI*2;         //6.28 approx
        float rawSinWave=Mathf.Sin(cycles*tau); //ranges from -1 to 1
        movementFactor=rawSinWave/2f +0.5f;     
        Vector3 offset=movementFactor*movementVector;
        transform.position=startingPos+offset;
    }
}
