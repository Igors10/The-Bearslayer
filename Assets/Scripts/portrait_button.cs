using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class portrait_button : MonoBehaviour
{
    [SerializeField] GameObject rollout;
    [SerializeField] GameObject glowing;
    [SerializeField] TMP_Text attack_value;
    [SerializeField] TMP_Text defence_value;

    void Start()
    {
        attack_value.text = Samurai_stats.samurai_attack.ToString();
        defence_value.text = Samurai_stats.samurai_defence.ToString();
    }

    void OnMouseDown()
    {
        if (rollout.activeSelf) rollout.SetActive(false);
        else rollout.SetActive(true);
       // glowing.SetActive(false);
    }

    void OnMouseEnter()
    {
        
        glowing.SetActive(true);
    }

    void OnMouseExit()
    {
        
        glowing.SetActive(false);
    }
}
