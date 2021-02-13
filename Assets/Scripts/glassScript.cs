using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glassScript : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    SpriteRenderer MainSpriteRenderer;
    public Sprite half;
    public Sprite full;
    public Sprite defaultSprite; //空のグラスのこと？
    float elapsedtime= 0;
    float timeLimit = 1;
    private bool isEmpty = true;
    private bool isHalf = false;
    private bool isFull = false;
    public GameObject mistake;
    [SerializeField] //インスペクターウィンドウで編集できるようにする
    public int BeerType;
    private Vector3 defaultPos; //グラスの最初の位置
    float resetTime = 0f;
    const float resetLimit = 1.0f;　//const：定数を与える。プログラム処理中に変更しないため
    bool isReset = false;
    private GameDirector gameDirector; //スコアを表示する
    bool isMiss = false;
    float displayMissTime = 0f;
    float displayMissLimit = 1.0f; 
    GameObject miss;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        defaultPos = transform.position;
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        switch (BeerType)  
        {　  //switch-case文: 式の結果と値が一致した時に処理が行なわれる
            case 1:// ピルスナー　
                //Debug.Log(BeerType);
                half = Resources.Load<Sprite>("pilsner.half");　//Resourcesフォルダの中にあるSprite画像を読み込む
                full = Resources.Load<Sprite>("pilsner.full");
                break;　//処理を抜ける
            case 2:// フルーツビア
                half = Resources.Load<Sprite>("fruitbeer.half");
                full = Resources.Load<Sprite>("fruitbeer.full");
                break;
            case 3://　ラガー
                half = Resources.Load<Sprite>("lager.half");
                full = Resources.Load<Sprite>("lager.full");
                break;
            case 4:// エール
                half = Resources.Load<Sprite>("ale.half");
                full = Resources.Load<Sprite>("ale.full");
                break;
            case 5:// スタウト
                half = Resources.Load<Sprite>("stout.half");
                full = Resources.Load<Sprite>("stout.full");
                break;
            case 6:// ヴァイツェン
                half = Resources.Load<Sprite>("weizen.half");
                full = Resources.Load<Sprite>("weizen.full");
                break;
            default:
                break;
        }        
    }
        
    void OnMouseDown()
    {   
        if (isReset) return;　//Resetならメソッド内の処理を終了
        this.screenPoint = Camera.main.WorldToScreenPoint(transform.position); //オブジェクトのポジションからスクリーン座標を取りたい時に使用する
        this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)); 
        //ドラッグする時にオブジェクトがカーソルに自動でセンタリングしないようにする。マウスの位置とオブジェクトの基準点の差をoffsetに代入し、ドラッグが始まったらオフセットする。
    }
  
    void OnMouseDrag()
    {
        if (isReset) return;　
         if (!isHalf)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z); //ドラッグ時のマウス位置をスクリーン座標に変換する
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;　//上記にクリックした場所の差を足すことによって、オブジェクトを移動する座標位置を求める
            transform.position = currentPosition; //オブジェクトの位置を変更する
        }
    }

    void Update()
    {  
       if (isReset) //リセット中はマウスの操作をさせない。
        {
            resetTime += Time.deltaTime;　//1秒立ったらリセットを解除
            if (resetTime > resetLimit)
            {
                isReset = false;
            } 
        }    
       if (isHalf)
       {   
       elapsedtime += Time.deltaTime; //経過時間
        if (timeLimit < elapsedtime)
        {
            MainSpriteRenderer.sprite = full;
            isHalf = false;
            isFull = true;
        }
       }
       if (isMiss)
       {
           displayMissTime += Time.deltaTime;
           if (displayMissTime > displayMissLimit)
           {
               isMiss = false;
               Destroy(miss);
               displayMissTime = 0;
           }
       }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isEmpty)
        {
            string strBeerType = BeerType.ToString(); //ToString()でintなどを文字列に変換する。
            if (other.gameObject.name == strBeerType)　//サーバーに入ってきたグラスが正しいかどうか判定する。
            {
                MainSpriteRenderer.sprite = half;　//サーバーに置いたらグラスにビールが半分入る
                isEmpty = false;
                isHalf = true;
            } else {
                isReset = true;
                resetTime = .0f;
                miss = Instantiate(mistake) as GameObject;
                miss.transform.position = transform.position;
                transform.position = defaultPos;   
                isMiss = true;
            }        
        }
        
        if (isFull)
        {
            var go = other.gameObject; 
            if (other.gameObject.name.Contains("customer"))
            {
                 if (go.GetComponent<CustomerController1>().currentOrder + 1 == BeerType)//客の注文と合っているか確認
                {
                    float orderTime = go.GetComponent<CustomerController1>().orderTime;
                    int successLevel = 0;
                    if (orderTime < 5)
                    {
                        successLevel = 3;
                    }
                    else if (orderTime < 7)
                    {
                        successLevel = 2;
                    }
                    else {
                        successLevel = 1;
                    }
                    gameDirector.ScoreUp(successLevel);// 正解
                    go.GetComponent<CustomerController1>().displayResult(true, successLevel);
                    GameObject director = GameObject.Find("GameDirector");
                    director.GetComponent<GameDirector>().ScoreUp(successLevel);//正解した事をGameDirectorに伝える
                    
                }
                else
                {
                    int successLevel = 0;
                    go.GetComponent<CustomerController1>().displayResult(false, successLevel);
                    GameObject director = GameObject.Find("GameDirector");
                    director.GetComponent<GameDirector>().ScoreDown();
                    gameDirector.ScoreDown();// 不正解
                }
                //Destroy(go);
                // 初期化
                MainSpriteRenderer.sprite = defaultSprite;
                transform.position = defaultPos;
                isFull = false;
                isEmpty = true;
                isReset = true;
                resetTime = .0f;
                elapsedtime = .0f;
            }
        }
       
    }    
}
