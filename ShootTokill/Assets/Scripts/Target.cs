using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public TargetZone[] targetZones;
    public GameObject geometryContainer;
    public GameObject destructionParticleContainer;
    public AudioSource hitClip;//audio source 
   
    private TargetManager targetMananger;
    private Movetowards1 moveTowards;
    private RotateTowards rotateTowards;
    private GameObject player;
    private ParticleSystem[] destructionParticle;
    private float pointsValueLoss;
    private Vector3 startPosition;
    private float pitchVariance = 0.2f;
    private float orignalPitch;
    private PointsDisplay pointsDisplay;


    public TargetManager targetManager
    {
        set { targetMananger = value; }
    }

    public GameObject Player
    {
        set
        {
            player = value;

        }


    }

    public void initTaregt()
    {
        moveTowards = GetComponent<Movetowards1>();
        rotateTowards = GetComponent<RotateTowards>();
        destructionParticle = destructionParticleContainer.GetComponentsInChildren<ParticleSystem>();


        //store orignal pitch ;
        orignalPitch = hitClip.pitch;


        pointsValueLoss = targetMananger.pointsValueLoss;
        //set transformation of target
        moveTowards.target = player.transform;
        rotateTowards.target = player.transform;

        pointsDisplay = targetMananger.PointsDisplay;

        //to enable script by setting it to true
        moveTowards.enabled = true;
        rotateTowards.enabled = true;
    }

    public void Reset()
    {

        destructionParticleContainer.SetActive(false);
        geometryContainer.SetActive(true);
        //Add to inactive targets list
        targetMananger.InactiveTargets.Enqueue(this);

        gameObject.SetActive(false);

    }

    public void Activate()
    {
        //store starting position for points value
        startPosition = transform.position;
        gameObject.SetActive(true);
    }

   public void Hit(RaycastHit hit)
    {
        int points = GetPoints(hit.collider);
        pointsDisplay.SetText(points);

        GameManager.instance.AddPoints(points);
        StartCoroutine(Destroy());

    }
    public IEnumerator Destroy()
    {
        //set random pitch and play audio based on pitch range
        hitClip.pitch = Random.Range(orignalPitch - pitchVariance, orignalPitch + pitchVariance);
        hitClip.Play();
        geometryContainer.SetActive(false);

        //disable geometry
        destructionParticleContainer.SetActive(true);

        float maxParticleDuration = 0;

        foreach(ParticleSystem particles in destructionParticle)
        {
            maxParticleDuration = Mathf.Max(maxParticleDuration, particles.main.duration);
            particles.Play();

        }

        pointsDisplay.transform.position = moveTowards.StoppingPosition;// point display is not use anywhere 
        pointsDisplay.transform.LookAt(player.transform);

        yield return new WaitForSeconds(maxParticleDuration);

        Reset();

        yield return new WaitForEndOfFrame();
    }

    private int GetPoints (Collider hitTargetZone)
    {
        foreach(TargetZone targetZone in targetZones)
        {
            if (targetZone.collider != hitTargetZone)
                continue;

            return CalculatePointLosses(targetZone.points);

        }
        return 0;
    }

    private int CalculatePointLosses(int pointsBase)
    {
        float startDistanceToTarget = Vector3.Distance(startPosition, moveTowards.StoppingPosition);
        float CurrentDistanceToTarget = Vector3.Distance(transform.position, moveTowards.StoppingPosition);

        float distancePercentage = (startDistanceToTarget * CurrentDistanceToTarget) / 100;
        distancePercentage = Mathf.Max(0, distancePercentage);

        float maxPoints = pointsBase;
        float minPoints = maxPoints - (pointsBase * pointsValueLoss);

        float pointsValue = Mathf.Lerp(minPoints, maxPoints, distancePercentage);
        pointsValue = Mathf.Max(0, pointsValue);

        //round to whole numbers
        return Mathf.RoundToInt(pointsValue);


    }


    [System.Serializable]
    public struct TargetZone
    {
        public Collider collider;
        public int points;
    }
}




