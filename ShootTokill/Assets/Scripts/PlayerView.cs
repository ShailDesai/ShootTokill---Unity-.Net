using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
    {
    public float maxRotationDegreePerSecond =75f;
    public float gyroRotationSpeed = 70f;
#if UNITY_EDITOR
    public float mouseRotation = 100f;
   
#endif
   // private bool gyroEnable;
    //private Gyroscope gyro;
    //private Quaternion rot;

    [Range(0, 45)]
    public float maxPitchUpAngle = 45f;

    [Range(0, 45)]
    public float maxPitchDownAngle = 5f;

    private Rect LookRect;
    private int LookTouchID = -1;
    private Vector2 touchOrigin; //where touch event occur
   

    void Start()
    {
        LookRect = new Rect(0, 0, Screen.width / 2, Screen.height * 0.75f);

    #if UNITY_EDITOR
        Cursor.visible = false;
    #endif

    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            if (Input.gyro.enabled)//checking weather gyro is enabled
            {
                GyroInput();
                Debug.Log("I am here");
               

            }
            else
            {
               
                Debug.Log("I am in touch");
                TouchInput();
            }

    #if UNITY_EDITOR
            mouseInput();
    #endif

            // directionToTarget();
        }
    
    }


    private void GyroInput()
    {
        Vector3 rotation = Input.gyro.rotationRateUnbiased * gyroRotationSpeed;// using unbaised rotation for better accuracy

        RotateView(new Vector3(-rotation.x, -rotation.y, 0)); //rotate around x and y no z invert axi
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)//if there are touches
        {
            //Save touch input
            if (LookTouchID == -1)
            {//if  touch is not stored
                foreach (Touch touch in Input.touches)
                {
                    //Only register begin new touch
                    if (touch.phase != TouchPhase.Began)
                        continue;
                    //check inside the look rectengle
                    if (!LookRect.Contains(touch.position))
                        continue;

                    //Save touch and exit loop
                    LookTouchID = touch.fingerId;
                    touchOrigin = touch.position;//saving the touvh position  /origin
                    break;




                }
            }


            foreach (Touch touch in Input.touches)
                //check over saved id
            {
                if (touch.fingerId != LookTouchID)
                    continue;

                Vector3 touchDistance = touch.position - touchOrigin;

                

                //Limit rotation on touch
                Vector3 clamedRotation = Vector3.ClampMagnitude(new Vector3(-touchDistance.y, touchDistance.x), maxRotationDegreePerSecond);

                //rotate view
                RotateView(clamedRotation);
                //clear saved touch id
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    LookTouchID = -1;
                }
                //Exit the loop

                break;
            }
        }
    }

#if UNITY_EDITOR
    private void mouseInput()
    {
        Vector3 rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        RotateView(rotation * mouseRotation);
    }
#endif

    private void RotateView(Vector3 rotation)
    {
        //rotate the player foe scence view

       transform.Rotate(rotation * Time.deltaTime);

        if (Input.gyro.enabled)
        {

            Vector3 localEuler = transform.localEulerAngles;//transformation of euler angles
            transform.localRotation = Quaternion.Euler(localEuler.x, localEuler.y, 0);//rebuild local rotation
        }

        
        //limit the rotation pitch so that the player dont go out of bound
        float playerPitch = limitPitch();

        //clear roll
        transform.rotation = Quaternion.Euler(playerPitch, transform.eulerAngles.y, 0);

    }

    private float limitPitch ()
    {
        float playerPitch = transform.eulerAngles.x;

        float maxPitchUp = 360 - maxPitchUpAngle;
        float maxPitchDown = maxPitchDownAngle; 

        if(playerPitch > 180 && playerPitch <maxPitchUp)
        {
            playerPitch = maxPitchUp;

        }
        else if(playerPitch < 180 && playerPitch > maxPitchDown)
        {
            //limit pitch Down
            playerPitch = maxPitchDown;
        }
        return playerPitch;
    }

   // private void directionToTarget()
    //{



      //  Vector3 directionToTar = transform.position - target.transform.position;
        //float angle = Vector3.Angle(transform.forward, directionToTar);
        //if(Mathf.Abs(angle)<90)
       //{
         //   Debug.Log("target is in front");
        //}
    //}
}


