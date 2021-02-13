using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomerController1 : MonoBehaviour
{
    public GameObject[] orders;
    public GameObject customer;
    float span = 3.0f;
    float delta = 0;
    GameObject[] realOrders;
    bool isOrderFinished = false; 
    public int currentOrder;
    GameObject currentOrderImage;
    float judgeSpan = 1.0f;
    float judgeDelta = 0;
    bool isJudge = false;
    bool isSuccess;
    public GameObject[] judgments;
    GameObject judge;
    public float orderTime = 0.0f;
    bool isOrder;
    void Start()
    {
       realOrders = new GameObject[6];
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
      if (this.delta > this.span)
      {
          List<int> checkCustomers = new List<int>();　//Listの宣言
          for (var i = 0; i < realOrders.Length; ++i) 
            {　
             if (realOrders[i] == null) 
              checkCustomers.Add(i);
            }        
      }  
      {
          this.delta += Time.deltaTime;
          if (!isOrderFinished && this.delta > span) 
          {
            currentOrder = Random.Range(0, orders.Length);
            currentOrderImage = Instantiate(orders[currentOrder]) as GameObject;　//プレハブをランダムに生成する
            orderTime = 0;
            isOrder = true;
            Vector2 customer = transform.position; //客の位置を取得する
            currentOrderImage.transform.position = new Vector3(transform.position.x + 2.3f, transform.position.y, transform.position.z); //生成したプレハブを客の隣に配置する
            isOrderFinished = true;
          }
      }  
      if (isJudge)
      {
        this.judgeDelta += Time.deltaTime;
        if (this.judgeDelta > this.judgeSpan)
        {
        Destroy(this.gameObject);
        Destroy(judge);
        isJudge = false;
        judgeDelta = 0;
        }
      }
      if (isOrder)
      {
        orderTime += Time.deltaTime;
      }
    }
    public void displayResult(bool isSuccess, int successLevel)
    {
      isJudge = true;
      if (isSuccess)
      {
        judge = Instantiate(judgments[successLevel]) as GameObject;
        Vector2 customer = transform.position; //客の位置を取得する
        judge.transform.position = new Vector3(transform.position.x + 2.2f, transform.position.y, transform.position.z);
        Destroy(currentOrderImage);
        isSuccess = true;
      }
      else
      {
        judge = Instantiate(judgments[successLevel]) as GameObject;
        Vector2 customer = transform.position; //客の位置を取得する
        judge.transform.position = new Vector3(transform.position.x + 2.2f, transform.position.y, transform.position.z);
        Destroy(currentOrderImage);
        isSuccess = false;
      }
    }
} 