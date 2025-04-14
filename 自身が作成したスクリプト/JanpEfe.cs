using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JanpEfe : MonoBehaviour
{

    // フェードアウトするまでの時間(0.5sec)
    public float fadeTime = 0.5f;
    private float time;
    private SpriteRenderer render;


    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < fadeTime)
        {
            float alpha = 1.0f - time / fadeTime;
            Color color = render.color;
            color.a = alpha;
            render.color = color;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
