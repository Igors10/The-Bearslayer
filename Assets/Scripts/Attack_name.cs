using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_name : MonoBehaviour
{
    public float SpriteColor;
    // Start is called before the first frame update
    void Start()
    {
        SpriteColor = 0f;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, SpriteColor);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SpriteColor > 1f)
        {
            SpriteColor -= 0.01f;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        else if (SpriteColor > 0f)
        {
            SpriteColor -= 0.01f;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, SpriteColor);
        }
       
    }
}
