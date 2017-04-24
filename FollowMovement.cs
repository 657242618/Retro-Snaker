using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowMovement : MonoBehaviour
{
    public GameObject nextBody;

    public float RecordGap;         //目标移动多远记录一次距离,数值越小距离越近，路径越接近头的运动轨迹
    public float WalkSpeed;         //走速度
    public float RunSpeed;          //跑速度
    public float SpeedLerpRant;     //速度变化的缓动率
    public int StartRunCount;       //距离目标多远后开始跑（单位是List的Item数）

    public Transform TargetTF;      //要跟随的目标

    public bool Running
    {   
        //访问器
        get { return PosList.Count > StartRunCount; }
        set { }
    }


    private Rigidbody Rig;
    private List<Vector3> PosList = new List<Vector3>();



    void Awake()
    {
        Rig = GetComponent<Rigidbody>();
    }


        void FixedUpdate()
        {

            if (TargetTF)
            {
                //清除已经到达的点
                if (PosList.Count > 0 && Vector3.Distance(transform.position, PosList[0]) < RecordGap)
                {
                    PosList.RemoveAt(0);
                }
                if (PosList.Count > 0)
                {
                    //添加当前Target位置
                    if (Vector3.Distance(TargetTF.position, PosList[PosList.Count - 1]) > RecordGap)
                    {
                        PosList.Add(TargetTF.position);
                    }
                    //处理移动
                    if (PosList.Count > 0)
                    {//更新velocity
                        Rig.velocity = Vector3.Lerp(Rig.velocity, new Vector3(PosList[0].x - transform.position.x, 0, PosList[0].z - transform.position.z).normalized * (Running ? RunSpeed : WalkSpeed), 1);
                        //Lerp（）得到两个向量的线性差值p，p=form+(to-form)*t   t[0,1]
                }
                else
                {
                    
                    Rig.velocity = Vector3.Lerp(Rig.velocity, Vector3.zero, SpeedLerpRant);
                }
            }
            else
            {
                //生成新节点的时候后续节点停止运动
                Rig.Sleep();
                //记录新的点
                PosList.Add(TargetTF.position);
            }
        }


    }

}