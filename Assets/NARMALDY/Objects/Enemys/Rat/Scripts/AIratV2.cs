using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIratV2 : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float VisibleRange = 5;
    [SerializeField] float RangeToAttak = 0.5f;
    [SerializeField] float Cooldown = 0.5f;
    bool AttackReady = true;

    [SerializeField] Transform Player;
    [SerializeField] CapsuleCollider2D EnemyCol;
    Collider2D[] collider2D3;

    Animator Anim;
    Rigidbody2D rb;

    Vector3 Target;

    /// <summary>
    /// Функция определения направления движения по 8 направлениям
    /// </summary>
    /// <param name="H">Горизонтальная составляющая</param>
    /// <param name="V">Вертикальная составляющая</param>
    /// <returns>-1 находится на месте,0 - вверх,1 вверх направо, 2 направо ...</returns>
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

    private void Start()
    {
        collider2D3 = GetComponents<Collider2D>();
        Target = transform.position;
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator CoolDownForAttak(float delay)
    {
        AttackReady = false;
        Debug.Log("attack");
        Anim.SetInteger("Dir", -2);
        yield return new WaitForSeconds(delay);
        AttackReady = true;
    }

    public void Update()
    {

        float step = speed * Time.deltaTime;

        float DistanseToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (DistanseToPlayer < VisibleRange)
        {
            EnemyCol.enabled = false;
            RaycastHit2D hit2D = Physics2D.Raycast(
                transform.position,
                Player.transform.position - transform.position,
                DistanseToPlayer);
            EnemyCol.enabled = true;

            if (hit2D)
            {
                if (hit2D.collider.tag == "Player")
                {
                    Target = hit2D.collider.transform.position;

                }
            }
        }


        if (Vector2.Distance(Target, transform.position) > RangeToAttak)
        {
            Vector3 Offset = (Target - transform.position).normalized * speed * Time.deltaTime;
            //Vector3 LastPos = transform.position;
            rb.MovePosition(transform.position + Offset );
            //Offset = transform.position - LastPos;
            Anim.SetInteger("Dir", ChooseDir(Mathf.Sign(Offset.x), Mathf.Sign(Offset.y)));

        }
        else
        {
            if (Vector2.Distance(Player.position, transform.position) <= RangeToAttak)
            {
                if (AttackReady)
                    StartCoroutine(CoolDownForAttak(Cooldown));
            }
            else
            {
                Anim.SetInteger("Dir", -1);
            }
        }
        GameObject.Find("Debub").transform.position = Target;
    }
}