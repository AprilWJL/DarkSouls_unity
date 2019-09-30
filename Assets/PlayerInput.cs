using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public float Dup;      //上下信号
    public float Dright;   //左右信号

    public float Dmag;      //速度
    public Vector3 Dvec;    //旋转

    public bool inputEnable = true;     //输入开关

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));
        Dvec = Dright * transform.right + Dup * transform.forward;
    }
}
