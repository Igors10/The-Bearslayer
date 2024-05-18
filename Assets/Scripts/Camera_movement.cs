using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;
    public static bool freeze;

    public float left_camera_border;
    public float top_camera_border;
    public float bottom_camera_border;
    public float right_camera_border;

    private bool drag = false;

    bool dice_default = false;
    public bool camera_on_samurai = false;
    [SerializeField] bool dynamic_camera;

    void Start()
    {
        
        ResetCamera = Camera.main.transform.position;
    }

    public static void FocusOnSamurai()
    {
        //Camera.main.transform.position = Battle_manager.cells[(int)Battle_list.samurai_spawn.x, (int)Battle_list.samurai_spawn.y].transform.position;
    }

    private void LateUpdate()
    {
        if (camera_on_samurai && (Battle_manager.current_player == Battle_manager.characters[0]) && dynamic_camera)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(Battle_manager.current_player.transform.position.x, Battle_manager.current_player.transform.position.y, Battle_manager.current_player.transform.position.z - 10f), 0.2f);
            if (Camera.main.transform.position == new Vector3(Battle_manager.current_player.transform.position.x, Battle_manager.current_player.transform.position.y, Battle_manager.current_player.transform.position.z - 10f)) camera_on_samurai = false;
        }
        else if ((Battle_manager.current_player == Battle_manager.characters[0]) || dynamic_camera == false)
        {
            if (Input.GetMouseButton(1) && freeze == false)
            {
                Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
                if (drag == false)
                {
                    dice_default = true;

                    drag = true;
                    Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                drag = false;
                if (dice_default)
                {
                    dice_default = false;
                    // Put code for recreating default position here
                }
            }

            if (drag)
            {
                if (Origin.x - Difference.x > left_camera_border && Origin.x - Difference.x < right_camera_border && Origin.y - Difference.y > bottom_camera_border && Origin.y - Difference.y < top_camera_border) Camera.main.transform.position = Origin - Difference;
            }

            //if (Input.GetMouseButton(1))
            //  Camera.main.transform.position = ResetCamera;
        }
        else if (Battle_manager.characters[0].GetComponent<Samurai>().dead == false)
        {
            //Camera.main.transform.position = new Vector3(Battle_manager.current_player.transform.position.x, Battle_manager.current_player.transform.position.y, Battle_manager.current_player.transform.position.z - 10f);
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(Battle_manager.current_player.transform.position.x, Battle_manager.current_player.transform.position.y, Battle_manager.current_player.transform.position.z - 10f), 0.3f);
        }

    }
}
