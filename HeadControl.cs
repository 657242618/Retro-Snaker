using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                   //使用UI中的Text
using UnityEngine.SceneManagement;      //重新游戏

public class HeadControl : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public GameObject imgMenu;          //暂停菜单

    public float moveSpeed;
    public float rotateSpeed;

    public GameObject snack_Body;
    private GameObject firstBody;
    private GameObject temptBody;


    //随机生成食物
    private float cube_foodMax_X = 15;
    private float cube_foodMin_X = -15;
    private float cube_foodMax_Z = 15;
    private float cube_foodMin_Z = -15;
    public GameObject cube_food;

    private GameObject cc;

    void RandomFood()
    {
        //随机出现食物
        float cube_food_PositionX = Random.Range(cube_foodMin_X, cube_foodMax_X);
        float cube_food_PositionZ = Random.Range(cube_foodMin_Z, cube_foodMax_Z);
        cc = Instantiate(cube_food, new Vector3(cube_food_PositionX, 1f, cube_food_PositionZ), Quaternion.identity) as GameObject;
        //
    }
    // Use this for initialization
    void Start()
    {
        RandomFood();
    }

    // Update is called once per frame
    void Update()
    {

        WalkForward();

        /*       if (timer > time)
                {
                    Vector3 old = this.transform.position;

                    if (firstBody != null)
                    {

                        firstBody.transform.position = old;  
                        firstBody.GetComponent<BodyMove>().moveTo(old);
                    }
                    timer = 0;
                }
                else
                {
                    timer += Time.deltaTime;
                }
        */

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 1, 0) * -rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 1, 0) * rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, 1) * -moveSpeed * Time.deltaTime);
        }
        /*        if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(new Vector3(1, 0, 0) * -moveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
                }
        */
    }


    void WalkForward()
    {
        transform.position = transform.position + moveSpeed * transform.forward * Time.deltaTime;  //下一帧位置=现位置+移动速度*面向*时间

    }

    void CreatBody()
    {
        //新建一节身体，snack_Body参数为身体prefabs
        GameObject newBody = Instantiate(snack_Body, transform.position, Quaternion.identity) as GameObject;
        if (firstBody == null)
        {
            //如果是第一节身体
            firstBody = newBody;
            //将头的transform传给第一节身体
            firstBody.GetComponent<FollowMovement>().TargetTF = this.transform;//将头部的transform传给第一节点  
        }
        else
        {
            temptBody = newBody;
            temptBody.GetComponent<FollowMovement>().nextBody = firstBody;
            firstBody.GetComponent<FollowMovement>().TargetTF = temptBody.transform;
            firstBody = temptBody;
            firstBody.GetComponent<FollowMovement>().TargetTF = this.transform;
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "food")
        {

            //重置食物  
            Destroy(cc);
            CreatBody();
            scoreText.text = (++score).ToString();
            RandomFood();

        }
    }

    public void OnPause()
    {
        Time.timeScale = 0;      //暂停游戏
        imgMenu.SetActive(true); //显示暂停界面
    }

    public void OnResume()
    {
        Time.timeScale = 1;      //返回游戏
        imgMenu.SetActive(false);//关闭暂停界面
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//重新游戏
        Time.timeScale = 1;
    }
}
    
