using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDie : MonoBehaviour {

   
    


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            Destroy(col.gameObject);
            GameManager gm = FindObjectOfType<GameManager>();
           
            
            gm.EndGame();

        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
