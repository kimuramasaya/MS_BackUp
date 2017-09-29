using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public timerevent[] time_event; //色変更event
    private int colocunter;             //カラーリスト配列
    private bool ColorChangeFlag = false;//カラー変更フラグ


    [SerializeField]
    Color current_col;              //現在の色
    private float timecurent;
    [SerializeField]
    private Image UIobj;          //イメージデータ

    [SerializeField]
    private bool startflag = true;  //true : move now _ false : move stop

    [SerializeField]
    private float countTime;        //リミットtime

    private float currenttime;      //現在割合

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
                Debug.LogError("UIGageTimeEven : Rate又はLerpTimeの値が違います。\n詳しくは開発担当者に聞いてください。木村");
                Debug.Break();
                return;
            }
        }
    }

    private void Awake()
    {
        ErrorCheck();
        currenttime = 0.0f;
        timecurent = 0.0f;
        ColorChangeFlag = false;

        UIobj.color = current_col;

        colocunter = time_event.Length - 1;
    }

    //クラスチェックの後交換
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

    // Use this for initialization
    void Start()
    {
        float[] worker = new float[time_event.Length];
        for(int i = 0; i<time_event.Length;i++ )
        {
            worker.SetValue(time_event[i].GetRate(), i);
        }
        worker = timerevent.QSort(worker, 0, time_event.Length - 1);
        ClassArraySwapCheck_AfterSwap(worker);
    }

    // Update is called once per frame
    void Update()
    {
        if (startflag)
        {

            currenttime = UIobj.fillAmount -= 1.0f / countTime * Time.deltaTime;

            if (currenttime <= time_event[colocunter].GetRate())
            {
                ColorChangeFlag = true;
            }

            if (ColorChangeFlag)
            {
                timecurent += (1.0f - timecurent) * time_event[colocunter].GetLerpTime();
                UIobj.color = current_col= Color.Lerp(current_col, time_event[colocunter].GetColor(), /*Time.deltaTime*/timecurent);
                if (time_event[colocunter].GetColor() == current_col)
                {
                    colocunter--;
                    timecurent = 0.0f;
                    if (colocunter < 0)
                    {
                        colocunter = 0;
                    }
                    ColorChangeFlag = false;

                }
            }
            //エラーチェック
            if (!startflag)
            {
                Debug.LogWarning("UIGage : startflag is false !!\n ゲージが更新されません!!\n");
            }

        }
    }

    private void OnValidate()
    {
        if (time_event.Length <= 0)
        {
            Debug.LogWarning("UIGage : TimeEvent sizeが0以下の値で設定されています。\n使用する場合は値を変えてください");
        }

        if (countTime < 0 || countTime > int.MaxValue) Debug.LogWarning("UIGage : countTimeが限界地です!!");
        countTime = Mathf.Clamp(countTime, 0, int.MaxValue);

        for(int i = 0; i< time_event.Length; i++)
        {
            if (time_event[i].GetLerpTime() <= 0.0f )
            {
                Debug.LogWarning("UIGage : Time_event.LerpTimeが0以下です!!\nゲージの色の更新ができません!!\n");
            }
        }
        
    }


}