using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hint : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text actual_text;
    [SerializeField] GameObject textbox;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        
        transform.position = mousePosition;
        
    }

    public void Text(string text, bool active)
    {
        if (active) textbox.SetActive(true);
        else
        {
            textbox.SetActive(false);
            return;
        }

        actual_text.text = text;
    }

}
