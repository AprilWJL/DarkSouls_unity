using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [Header("===== Keys settings  =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA = "left shift";
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJUp;       //右摇杆（上下左右）控制摄像机
    public string keyJDown;
    public string keyJRight;
    public string keyJLeft;

    [Header("===== Output signals =====")]
    public float Dup;      //上下信号
    public float Dright;   //左右信号
    public float Dmag;      //速度
    public Vector3 Dvec;    //旋转

    public float Jup;     //右摇杆
    public float Jright;

    //pressing signal
    public bool run;        //跑步信号
    //trigger once signal
    public bool jump;       //跳跃信号
    public bool lastJump;
    public bool attack;       //跳跃信号
    public bool lastAttack;
    //double trigger

    [Header("===== Others =====")]
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

        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0f) - (Input.GetKey(keyJDown) ? 1.0f : 0f);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0f) - (Input.GetKey(keyJLeft) ? 1.0f : 0f);

        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;


        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(keyA);

        bool newJump = Input.GetKey(keyB);
        //jump = newJump;
        if (newJump != lastJump && newJump == true)
        {
            jump = true;
            //print("jump");
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;

        bool newAttack = Input.GetKey(keyC);
        //jump = newJump;
        if (newAttack != lastAttack && newAttack == true)
        {
            attack = true;
            //print("jump");
        }
        else
        {
            attack = false;
        }
        lastAttack = newAttack;
    }

    /// <summary>
    /// 平面坐标转圆形坐标
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}
