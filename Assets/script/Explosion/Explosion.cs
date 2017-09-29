using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float coefficient;   // 空気抵抗係数
    public float speed;         // 爆風の速さ

    void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Rigidbody>() == null)
        {
            return;
        }

        // 風速計算
        var velocity = (col.transform.position - transform.position).normalized * speed;

        // 風力与える
        col.GetComponent<Rigidbody>().AddForce(coefficient * velocity);//rigidbody.AddForce(coefficient * velocity);
    }
}