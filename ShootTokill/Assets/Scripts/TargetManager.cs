using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{

    public GameObject targetPrefab;
    public float spawnDelay = 2f;
    public float timeBetweenSpawnsMin = 1f;
    public float timeBetweenSpawnMax = 5f;
    public float spawnRadius = 10f;
    public float maxSpawnHeight = 40f;
    public int maxNumTargets = 20;
    public AudioSource newenimies;
    public AudioSource enimieappeared;


    [Range(0, 1), Tooltip("how much % of point value is removed when target is at stopping distance 0= 0% , 1 = 100%)")]
    public float pointsValueLoss;

    public PointsDisplay PointsDisplay;

    public float xMinRange = -25.0f;
    public float xMaxRange = 25.0f;
    //public float yMinRange = 8.0f;
    //public float yMaxRange = 25.0f;
    //public float zMinRange = -25.0f;
    //public float zMaxRange = 25.0f;
    Camera cam;

    private List<Target> spawnedTargets = new List<Target>();
    private Queue<Target> inactiveTargets = new Queue<Target>();

    public Queue<Target> InactiveTargets
    {

        get { return inactiveTargets; }
    }

    private void Awake()
    {
        //diable on game start 
        this.enabled = false;
    }

    private void OnEnable()
    {
        //InitTargets(); written before game manager was assign
        StartCoroutine(SpawnTarget());
       // cam = GetComponent<Camera>();


    }


    private void OnDisable()
    {
        StopCoroutine(SpawnTarget());
    }
    

    public void InitTargets()
    {
        //Temp:store pLayer before assigning GUI just to check
       // GameObject player = GameObject.FindGameObjectWithTag("Player") as GameObject;


        GameObject targetParent = new GameObject();
        targetParent.name = "Targets";
        //

        for (int i = 0; i < maxNumTargets; i++) // Setting upto maximum number of count so target remain under control
        {
            Target targetInstance = (Instantiate(targetPrefab) as GameObject).GetComponent<Target>(); // intantiating the target using assigned gameobject name TArget through unity UI

            targetInstance.targetManager = this; 

            targetInstance.transform.parent = targetParent.transform;

            targetInstance.Player = GameManager.instance.player;

            targetInstance.initTaregt();

            spawnedTargets.Add(targetInstance);

            //newenimies.Play();



        }

        //newenimies.Stop();

        ResetAllTargets();
    }

     private IEnumerator SpawnTarget()
    {
        yield return new WaitForSeconds(spawnDelay);


        while (this.isActiveAndEnabled)
        {
            if (inactiveTargets.Count > 0)
            {
                Target target = inactiveTargets.Dequeue();

                Vector3 position;
               // int randomPosition;

                do
                {
                    //position.x = Random.Range(xMinRange, xMaxRange);
                    //position.y = Random.Range(yMinRange, yMaxRange);
                    //position.z = Random.Range(zMinRange, zMaxRange);

                   // randomPosition = Mathf.RoundToInt(Random.Range(xMaxRange, xMinRange));

                    position = (transform.position + (Random.onUnitSphere)  * spawnRadius);
                }
                while (position.y < transform.position.y || position.y > maxSpawnHeight);


                target.transform.position = position;


                //Vector3 screenPos = cam.WorldToScreenPoint(position);
                  //  Debug.Log("target is " + screenPos.x + " pixels from the left");



                target.Activate();
                newenimies.Play();
                enimieappeared.Play();

                //Vector3  directionToTarget = camera.transform.position- transform.position.x
            }


            //Get Random wait Time
            float waitTime = UnityEngine.Random.Range (timeBetweenSpawnsMin, timeBetweenSpawnMax);
            yield return new WaitForSeconds(waitTime);


        }
    }
    private void ResetAllTargets()
    {
        inactiveTargets.Clear();

        foreach (Target target in spawnedTargets)
        {
            //Reset the target
            target.Reset();

        }
    }
}
