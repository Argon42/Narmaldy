using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narmaldy;

public class ControllerPlayer : Entity {

    Animator Anim;
    Rigidbody2D rb;
    
	void Start () {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Heals = 200;
        Speed = 1;
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
            transform.position.x + Input.GetAxis("Horizontal") * Speed * Time.deltaTime,
            transform.position.y + Input.GetAxis("Vertical") * Speed * Time.deltaTime));
        //transform.position = new Vector2(
        //    transform.position.x + Input.GetAxis("Horizontal") * SpeedPlayer * Time.deltaTime,
        //    transform.position.y + Input.GetAxis("Vertical") * SpeedPlayer * Time.deltaTime);
    }
}