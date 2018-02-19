using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationGatherer : MonoBehaviour {
    public GameObject selectedTarget;
    public Vector3 selectedTargetPos;
    public int selectedTargetArm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GetTargetInfo(GameObject target, Vector3 targetPos, int arm)
    {
        print("This is the target" + target.name);
        selectedTarget = target;
        selectedTargetPos = targetPos;
        selectedTargetArm = arm;
    }
    public void ClearInfo()
    {
        selectedTarget = null;
        selectedTargetPos = new Vector3(0,0,0);
        selectedTargetArm = 0;
        

    }
    public void DoDamage(int num)
    {        
        selectedTarget.GetComponent<Player>().ChangeHealth(num);
        selectedTarget.GetComponent<Animator>().SetTrigger("hurtTrigger");

    }
        
}
