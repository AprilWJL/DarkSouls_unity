using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 2.4f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 2.0f;   //向上跳跃冲量
    public float rollVelocity = 1.0f;   //向上翻滚冲量

    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;      //平面
    private Vector3 thrustVec;      //冲量

    public bool lockPlanar = false; //锁死平面移动速度

	// Use this for initialization
	void Awake () {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float targetRunMulti = (pi.run ? 2.0f : 1.0f);
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.5f));     //移动动画控制

        if (rigid.velocity.magnitude > 0f)    //判断是否翻滚
        {
            anim.SetTrigger("roll");
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }

        if (pi.Dmag > 0.1f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.4f);  //做差值，增加切换旋转动画时的平滑度
            model.transform.forward = targetForward;      //旋转
        }
        if (lockPlanar == false)
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);

        }
	}

    private void FixedUpdate()
    {
        //rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;   //用刚体来进行移动
        thrustVec = Vector3.zero;
    }

    /// <summary>
    /// Message接收
    /// </summary>
    public void OnJumpEnter()
    {
        thrustVec = new Vector3(0, jumpVelocity, 0);       //新增跳跃冲量
        pi.inputEnable = false;     //跳跃时不能移动  
        lockPlanar = true;          //锁死平面移动
        //print("onjumpenter");
    }

    public void IsGround()
    {        
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlanar = false;
    }

    public void OnFallEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);       
        pi.inputEnable = false;     
        lockPlanar = true;          //锁死平面移动
    }

    public void OnJabEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;          //锁死平面移动
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");

    }
}
