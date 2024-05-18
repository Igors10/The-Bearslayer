using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_visuals : MonoBehaviour
{
    public GameObject glowing;
    public GameObject slot;
    [SerializeField] GameObject description;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseEnter()
    {
        description.SetActive(true);
        glowing.SetActive(true);
    }

    void OnMouseExit()
    {
        description.SetActive(false);
        glowing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
