using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damage_number : MonoBehaviour
{
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    float transparency = 1f;
    [SerializeField] float R = 1f;
    [SerializeField] float G = 1f;
    [SerializeField] float B = 1f;
    [SerializeField] bool enemy = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transparency > 0f)
        {
            transparency -= 0.01f;
            if (transparency <= 1f) text.color = new Color(R, G, B, transparency);
            else text.color = new Color(R, G, B, 1f);
        }

    }

    public void Show(GameObject die, int number)
    {
        transparency = 1.5f;
        transform.position = die.transform.position;
        if (enemy) transform.Translate(0f, 1f, 0f);
        else transform.Translate(0.4f, 0.4f, 0f);
        text.text = number.ToString();
        if (number < 0)
        {
            R = 1f;
            G = 0.13f;
            B = 0.13f;
        }
        else
        {
            R = 0.2f;
            G = 0.8f;
            B = 0.2f;
        }
    }
}
