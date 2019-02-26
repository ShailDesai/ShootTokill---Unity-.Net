using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour {

    public Transform target;

    private bool lookedAtTarget;

    private void OnEnable()
    {
        lookedAtTarget = false;
    }

    private void Update()
    {
        if(!lookedAtTarget)
        {
            transform.LookAt(target.position);
            lookedAtTarget = true;
        }
    }

}
