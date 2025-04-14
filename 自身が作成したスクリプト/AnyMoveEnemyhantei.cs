using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyMoveEnemyhantei : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ground")
        {              
            GameObject.Find(transform.root.name).SendMessage("ReturnX"); //Ú’n‚µ‚Ä‚È‚¢       
        }
        else if (c.gameObject.tag == "Box")
        {
            GameObject.Find(transform.root.name).SendMessage("ReturnX"); //Ú’n‚µ‚Ä‚È‚¢       
        }
    }
}
