using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_border : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] float xmove;
    [SerializeField] float ymove;
    [SerializeField] GameObject main_camera;


    void OnMouseOver()
    {
        main_camera.transform.Translate(xmove, ymove, 0f);
    }
}
