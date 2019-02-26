using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movetowards1 : MonoBehaviour {

    // Use this for initialization
    public Transform target;
    public float moveSpeed = 2f;
    public float stoppingDistance = 5f;
    public GUIManager guiManager;
    private TargetManager targetManager;


    private bool isMoving;
    private Vector3 stoppingPosition;

    public Vector3 StoppingPosition
    {
        get { return stoppingPosition; }
    }

    void OnEnable()
    {
        isMoving = true;
        stoppingPosition = GetStoppingPosition();
    }
    void OnDisable()
    {
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, stoppingPosition) >= 0)
            {
                MoveTowardsTarget();
            }
            else
            {
                transform.position = StoppingPosition;
                isMoving = false;
            }
        }

    }

    private Vector3 GetStoppingPosition()
    {
        Ray ray = new Ray(target.position, transform.position);
        return ray.GetPoint(stoppingDistance);
    }
    private void MoveTowardsTarget()
    {   
        transform.position = Vector3.MoveTowards(transform.position, stoppingPosition, moveSpeed * Time.deltaTime);
        if (target.position == transform.position)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            GUIManager gu = FindObjectOfType<GUIManager>();
         


            
            GameObject.Find("TargetManager").GetComponent<TargetManager>().newenimies.Stop() ;
            GameObject.Find("TargetManager").GetComponent<TargetManager>().enimieappeared.Stop();// using audio source to stop playing

            gu.ShowGAmeOver();
            gm.EndGame();
            //gm.StartNewGame();
        }
    }
}

