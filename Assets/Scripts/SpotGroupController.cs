using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotGroupController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject pg = Instantiate(Resources.Load("LightControlPanel")) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
