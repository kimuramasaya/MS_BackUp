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

        worker = QSort(worker, 0, time_event.Length - 1);
        for (int i = 0; i < worker.Length; i++)
        {
            Debug.Log(worker[i]);
        }
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
            Debug.LogWarning("TimeEvent : sizeが0以下の値で設定されています。\n使用する場合は値を変えてください");
        }
    }

    /*public timerevent GetTimeEventClassArray(int idx)
    {
        return time_event[idx];
    }*/

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

    //クイックソート
    float[] QSort(float[] num, int left, int right)
    {
        int i, j;
        float pivot;

        i = left;                      /* ソートする配列の一番小さい要素の添字 */
        j = right;                     /* ソートする配列の一番大きい要素の添字 */

        pivot = num[(left + right) / 2]; /* 基準値を配列の中央付近にとる */

        while (true)
        {                    /* 無限ループ */

            while (num[i] < pivot)       /* pivot より大きい値が */
                i++;                   /* 出るまで i を増加させる */

            while (pivot < num[j])       /* pivot より小さい値が */
                j--;                   /*  出るまで j を減少させる */
            if (i >= j)                /* i >= j なら */
                break;                 /* 無限ループから抜ける */

            Swap(num, i, j);             /* x[i] と x[j]を交換 */
            i++;                       /* 次のデータ */
            j--;
        }

        if (left < i - 1)              /* 基準値の左に 2 以上要素があれば */
            QSort(num, left, i - 1);     /* 左の配列を Q ソートする */
        if (j + 1 < right)            /* 基準値の右に 2 以上要素があれば */
            QSort(num, j + 1, right);    /* 右の配列を Q ソートする */

        return num;
    }

    /* 配列の要素を交換する */
    void Swap(float[] x, int i, int j)
    {
        float temp;

        temp = x[i];
        x[i] = x[j];
        x[j] = temp;
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
