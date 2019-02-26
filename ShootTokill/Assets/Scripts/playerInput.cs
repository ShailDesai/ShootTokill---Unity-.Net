using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInput : MonoBehaviour {

    public LayerMask targetLayerMask;
    public AudioSource shootingClip;

    private Rect inputRect;

    private Camera cam;
   
    private float pitchVariance = 0.1f;
    private float orignalPitch;


    void Start()
    {
        inputRect = new Rect(Screen.width / 2, 0, Screen.width, Screen.height * 0.75f);

        cam = GetComponentInChildren<Camera>();

        //store orignal pitch
        orignalPitch = shootingClip.pitch;


      
    }
    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            TouchInput();
#if UNITY_EDITOR
            keyboardInput();
#endif
        }
    }

    private void TouchInput()
    {
        if(Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Began)
                    continue;

                if (!inputRect.Contains(touch.position))
                    continue;

                shoot();
            }
        }
    }

   private void keyboardInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }
    private void shoot()
    {
        //set random pitch and play audio. //same as target script
        shootingClip.pitch = Random.Range(orignalPitch - pitchVariance, orignalPitch + pitchVariance); //pick random from max and min range
        shootingClip.Play();

        //check it a target
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f , targetLayerMask))
        {
            Target target = hit.collider.GetComponentInParent<Target>();
            target.Hit(hit);
            GameObject.Find("TargetManager").GetComponent<TargetManager>().newenimies.Stop() ;


        }
    }
}
