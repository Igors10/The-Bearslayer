using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying_die : MonoBehaviour
{
    [SerializeField] float death_speed;
    [SerializeField] bool UI = false;
    float death_color = 1f;
   

    void FixedUpdate()
    {
        transform.Translate(0f, death_speed, 0f);
        death_color -= 0.025f;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, death_color);
        //else GetComponent<Image>().color = new Color(1f, 1f, 1f, death_color);
        if (death_color <= 0) Destroy(this.gameObject);
    }
}
