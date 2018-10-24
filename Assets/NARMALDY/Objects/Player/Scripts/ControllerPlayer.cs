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
	
    private void ChooseDir(float H, float V)
    {
        if (H == 0)
        {
            if (V > 0)
                Anim.Play("Walk0");
            else if (V < 0)
                Anim.Play("Walk4");
            else
                Anim.Play("Stand");
        }
        else if (H > 0)
        {
            if (V > 0)
                Anim.Play("Walk1");
            else if (V < 0)
                Anim.Play("Walk3");
            else
                Anim.Play("Walk2");
        }
        else if (H < 0)
        {
            if (V > 0)
                Anim.Play("Walk7");
            else if (V < 0)
                Anim.Play("Walk5");
            else
                Anim.Play("Walk6");
        }
    }



    void Update()
    {
        float H = Input.GetAxis("Horizontal"),
            V = Input.GetAxis("Vertical");
        ChooseDir(H, V);

        rb.MovePosition(new Vector2(
            transform.position.x + Input.GetAxis("Horizontal") * SpeedPlayer * Time.deltaTime,
            transform.position.y + Input.GetAxis("Vertical") * SpeedPlayer * Time.deltaTime));
        //transform.position = new Vector2(
        //    transform.position.x + Input.GetAxis("Horizontal") * SpeedPlayer * Time.deltaTime,
        //    transform.position.y + Input.GetAxis("Vertical") * SpeedPlayer * Time.deltaTime);
    }
}