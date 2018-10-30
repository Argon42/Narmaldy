using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour {

    Animator Anim;
    Rigidbody2D rb;
    [SerializeField] public float SpeedPlayer;
    
	void Start () {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private int ChooseDir(float H, float V)
    {
        if (H == 0)
        {
            if (V > 0)
                return 0;
            else if (V < 0)
                return 4;
            else
                return -1;
        }
        else if (H > 0)
        {
            if (V > 0)
                return 1;
            else if (V < 0)
                return 3;
            else
                return 2;
        }
        else if (H < 0)
        {
            if (V > 0)
                return 7;
            else if (V < 0)
                return 5;
            else
                return 6;
        }
        return -1;
    }



    void Update()
    {
        float H = Input.GetAxis("Horizontal"),
            V = Input.GetAxis("Vertical");
        Anim.SetInteger("Dir",ChooseDir(H, V));

        rb.MovePosition(new Vector2(
            transform.position.x + Input.GetAxis("Horizontal") * SpeedPlayer * Time.deltaTime,
            transform.position.y + Input.GetAxis("Vertical") * SpeedPlayer * Time.deltaTime));
        //transform.position = new Vector2(
        //    transform.position.x + Input.GetAxis("Horizontal") * SpeedPlayer * Time.deltaTime,
        //    transform.position.y + Input.GetAxis("Vertical") * SpeedPlayer * Time.deltaTime);
    }
}