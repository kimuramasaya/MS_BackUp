using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class timer : MonoBehaviour
{

    public timerevent[] time_event;

    private float AccumulatedTime;
    private float currenttime;

    [SerializeField]
    private float LimitTime = 0.0f;

    [SerializeField]
    private int MinorityNumber = 0;

    [SerializeField]
    Color current_col;//今の色

    bool ColorChangeFlag = false;

    int colorcunter;

    private float time;

    float timecurent;

    private void Awake()
    {
        ErrorCheck();
        currenttime = 0.0f;
        AccumulatedTime = 0.0f;
        time = 0.0f;
        colorcunter = time_event.Length - 1;
        timecurent = 0.0f;
    }

    // Use this for initialization
    void Start()
    {
        float[] worker = new float[time_event.Length];
        for (int i = 0; i < time_event.Length; i++)
        {
            worker.SetValue(time_event[i].GetRate(), i);
        }

        worker = timerevent.QSort(worker, 0, time_event.Length - 1);

        ClassArraySwapCheck_AfterSwap(worker);
    }

    // Update is called once per frame
    void Update()
    {
        //時間加算
        AccumulatedTime += Time.deltaTime;

        //現在の時間算出
        currenttime = LimitTime - AccumulatedTime;

        float bai = currenttime * Mathf.Pow(10, MinorityNumber);
        //少数切り捨て
        time = Mathf.Floor(bai);
        time = time / Mathf.Pow(10, MinorityNumber);

        //タイマーリセット
        if (currenttime <= 0)
        {
            AccumulatedTime = 0;
        }

        if (currenttime / LimitTime <= time_event[colorcunter].GetRate())
        {
            ColorChangeFlag = true;
        }
    }
    
    //UI描画
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        GUIStyleState styleState = new GUIStyleState();

        if(ColorChangeFlag)
        {
            timecurent += (1.0f - timecurent) * time_event[colorcunter].GetLerpTime();
            //カラー変更
            styleState.textColor = current_col = Color.Lerp(current_col, time_event[colorcunter].GetColor(), /*Time.deltaTime*/timecurent);
            
            if(time_event[colorcunter].GetColor() == current_col)
            {
                timecurent = 0.0f;
                colorcunter--;
                if(colorcunter < 0)
                {
                    colorcunter = 0;
                }
                ColorChangeFlag = false;
            }
        }
        else
        {
            styleState.textColor = current_col;
        }
        

        guiStyle.normal = styleState;
        guiStyle.fontSize = 80;


        GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.2f, Screen.width, Screen.height), time.ToString(), guiStyle);

    }

    //インスペクター入力制限関数
    private void OnValidate()
    {
        if (LimitTime <= 0 || LimitTime > 99) Debug.LogError("Timer : LimitTimeの値が違います。\n詳しくは開発担当者に聞いてください。木村");
        LimitTime = Mathf.Clamp(LimitTime, 1, 99);
        if (MinorityNumber < 0 || MinorityNumber > 5) Debug.LogError("Timer : MinorityNumberの値が違います。\n詳しくは開発担当者に聞いてください。木村");
        MinorityNumber = Mathf.Clamp(MinorityNumber, 0, 5);

        if (time_event.Length <= 0)
        {
            Debug.LogWarning("timer : TimeEvent sizeが0以下の値で設定されています。\n使用する場合は値を変えてください");
        }

        for (int i = 0; i < time_event.Length; i++)
        {
            if (time_event[i].GetLerpTime() <= 0.0f)
            {
                Debug.LogWarning("timer : Time_event.LerpTimeが0以下です!!\nゲージの色の更新ができません!!\n");
            }
        }
    }

    //エラーチェック
    void ErrorCheck()
    {
        if (time_event.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < time_event.Length; i++)
        {
            if (time_event[i].GetRate() < 0.0f || time_event[i].GetRate() > 1.0f ||
                time_event[i].GetLerpTime() < 0.0f || time_event[i].GetLerpTime() > 1.0f)
            {
                Debug.LogError("TimeEven : Rate又はLerpTimeの値が違います。\n詳しくは開発担当者に聞いてください。木村");
                Debug.Break();
                return;
            }
        }
    }



    void ClassArraySwapCheck_AfterSwap(float[] rateList)
    {
        for (int i = 0; i < time_event.Length; i++)
        {
            for (int n = 0; n < rateList.Length; n++)
            {
                if (time_event[i].GetRate() == rateList[n])
                {
                    ClassSwap(i, n);
                }
            }

        }
    }

    //クラスの入れ替え
    void ClassSwap(int x, int y)
    {
        timerevent workbox = null;

        workbox = time_event[x];
        time_event[x] = time_event[y];
        time_event[y] = workbox;
    }
}
