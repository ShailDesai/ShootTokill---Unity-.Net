using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScopeInput : MonoBehaviour {

    public GameObject disabled;

    private bool gyroscopeAvailaible;


    private void Awake()
    {
        gyroscopeAvailaible = SystemInfo.supportsGyroscope;
    }
    // Use this for initialization
    void Start () {
        if (!gyroscopeAvailaible)
        {
            Input.gyro.enabled = false;
            this.gameObject.SetActive(false);

        }
        else
        {

            Input.gyro.enabled = true; 
         
        }
		
	}

    public void Toggle()
    {
        if (Input.gyro.enabled)
        {
            disabled.SetActive(true);
            Input.gyro.enabled = false;
        }
        else
        {
            //if not enabled
            disabled.SetActive(false);
            Input.gyro.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
