using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class timerevent
{
    [SerializeField]
    protected float rate;

    [SerializeField]
    protected float LerpTime;

    protected float curenttime;

    [SerializeField]
    protected Color col;

    // Use this for initialization

    private void Awake()
    {

    }

    void Start() {

    }

    private void Update()
    {
        
    }
    
    public float GetRate()
    {
        return rate;
    }

    public Color GetColor()
    {
        return col;
    }

    public float GetLerpTime ()
    {
        return LerpTime;
    }

    public void SetLerpTime(float var)
    {
        LerpTime = var;
    }

    //クイックソート
    public static float[] QSort(float[] num, int left, int right)
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
    private static void Swap(float[] x, int i, int j)
    {
        float temp;

        temp = x[i];
        x[i] = x[j];
        x[j] = temp;
    }

}
