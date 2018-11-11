using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narmaldy;

public struct PrefersStruct
{
    public float Attack { get; private set; }
    public float Go { get; private set; }
    public float Walk { get; private set; }
    public float Run { get; private set; }
    public PrefersStruct(float a, float g, float w, float r)
    {
        Attack = a;
        Go = g;
        Walk = w;
        Run = r;
    }
    public PrefersStruct Mutation(float Ampl)
    {
        float NewAttack = Attack + Random.Range(-Ampl, Ampl);
        float NewGo = Go + Random.Range(-Ampl, Ampl);
        float NewWalk = Walk + Random.Range(-Ampl, Ampl);
        float NewRun = Run + Random.Range(-Ampl, Ampl);
        return new PrefersStruct(NewAttack,NewGo,NewWalk, NewRun);
    }
}

public class GiEnemy : Entity
{

    public PrefersStruct Prefers = new PrefersStruct(0, 0, 1, 0);
    List<Transform> Targets = new List<Transform>();
    float step, AttackRadius=0.5f;
    float x1, y1;
    float dx = 0, dy = 0, dxl, dyl;
    public float Radius { get; set; }
    public bool AttackReady { get; private set; }
    public int Damage { get; private set; }
    float timer = 30;

    Transform CurrentTarget;
    float TargetPriority;
    string TargetType;

    void Start ()
    {
        Mana = 100;
        AttackReady = true;
        Damage = 10;
        Heals = 100;
        Speed = 1;
        Radius = 5;
        TargetType = "None";
    }
    float distance(Transform Target)
    {
        float x = transform.position.x - Target.transform.position.x;
        float y = transform.position.y - Target.transform.position.y;
        return Mathf.Sqrt(x * x + y * y);
    }
    bool Raycollision(Transform Target)
    {
        if (Target.tag == "Wall")
            return false;
        RaycastHit2D[] Hit2D = Physics2D.RaycastAll(transform.position, Target.transform.position - transform.position, distance(Target));
        foreach(RaycastHit2D Hit in Hit2D)
        if (Hit)
            if (Hit.collider.transform == Target )
                return true;
            else if(Hit.collider.transform.tag == "Wall")
                return false;
        return true;    
    }
    bool Raycollision(Vector3 Target)
    {
        Vector3 Offset = Vector3.up * GetComponent<CircleCollider2D>().offset.y;
        RaycastHit2D[] Hit2D = Physics2D.RaycastAll(transform.position + Offset, Target - transform.position, Vector3.Distance(transform.position,Target));
        foreach (RaycastHit2D Hit in Hit2D)
            if (Hit)
                if (Hit.collider.transform.position == Target)
                    return true;
                else if (Hit.collider.transform.tag == "Wall")
                    return false;
        return true;
    }
    void Move(Transform Target)
    {
        if (Raycollision(Target))
        {
            Vector3 Offset = (Target.position - transform.position).normalized * step;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + Offset);
        }
    }
    void Move(float x, float y)
    {
        Vector3 Target = new Vector3(x, y, 0);
        if (Raycollision(Target))
        {
            Vector3 Offset = (Target - transform.position).normalized * step;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + Offset);
        }
    }
    void RandomPoint()
    {
        x1 = Random.Range(-Radius, Radius) + transform.position.x;
        y1 = Random.Range(-Radius, Radius) + transform.position.y;
        for (int i = 0; i < 10 && !Raycollision(new Vector3(x1, y1, 0)); i++)
        {
            x1 = Random.Range(-Radius, Radius) + transform.position.x;
            y1 = Random.Range(-Radius, Radius) + transform.position.y;
        }
    }

    float prioritylisting(Transform Target)
    {
        float g= -distance(Target), w = Prefers.Walk, r = g, a = g;
        if(Target.transform.tag=="Player")
        {
            g += Prefers.Go;
            a += Mathf.Abs(Prefers.Attack);
            r += Prefers.Run;
        }
        if (Target.transform.tag == "Enemy")
        {
            g += Prefers.Go;
            if (Prefers.Attack<0)
                a += Mathf.Abs(Prefers.Attack);
            r += Prefers.Run;
        }
        if (g >= a && g >= r && g >= w)
            return g;
        if (a >= g && a >= r && a >= w)
            return a;
        if (r >= g && r >= a && r >= w)
            return r;
        return w;
    }
    void Targeting(Transform Target)
    {
        float g = -distance(Target), w = Prefers.Walk, r = g, a = g;
        if (Target.transform.tag == "Player")
        {
            g += Prefers.Go;
            a += Mathf.Abs(Prefers.Attack);
            r += Prefers.Run;
        }
        if (Target.transform.tag == "Enemy")
        {
            g += Prefers.Go;
            if (Prefers.Attack < 0)
                a += Mathf.Abs(Prefers.Attack);
            r += Prefers.Run;
        }
        if (g >= a && g >= r && g >= w)
            TargetType = "Go";
        else if (a >= g && a >= r && a >= w)
            TargetType = "Attack";
        else if (r >= g && r >= a && r >= w)
            TargetType = "Run";
        else
        {
            TargetType = "Walk";
            CurrentTarget = null;
            RandomPoint();
        }
    }
    void Targetprefers()
    {
        List<float> Priority = new List<float>();
        foreach(Transform Target in Targets)
            Priority.Add(prioritylisting(Target));
        if(Targets==null)
        {
            TargetType = "Walk";
            CurrentTarget = null;
            RandomPoint();
            return;
        }

        float max = 0;
        foreach (float i in Priority)
            if (max < i)
                max = i;
        if (max <= TargetPriority)
            return;
        int n = Priority.IndexOf(max);
        CurrentTarget = Targets[n];
        Targeting(CurrentTarget);
        TargetPriority = max;
    }

    IEnumerator CoolDownForAttak(float delay)
    {
        AttackReady = false;
        CurrentTarget.GetComponent<Entity>().Heals -= Damage;
        timer += Damage;
        Debug.Log("Attack lol");
        yield return new WaitForSeconds(delay);
        AttackReady = true;
    }
    void Walk()
    {
        Move(x1, y1);
        if (Mathf.Abs(x1 - transform.position.x) < 2 && Mathf.Abs(y1 - transform.position.y) < 2)
        {
            CurrentTarget = null;
            TargetType = "None";
            TargetPriority = 0;
        }
    }
    void Go()
    {
        Move(CurrentTarget);
        if (distance(CurrentTarget) < step)
        {
            CurrentTarget = null;
            TargetType = "None";
            TargetPriority = 0;
        }
    }
    void Attack()
    {
        try
        {
            CurrentTarget.GetComponent<Entity>();
        }
        catch (MissingReferenceException)
        {
            CurrentTarget = null;
            TargetType = "None";
            TargetPriority = 0;
            throw;
        }
        if (distance(CurrentTarget) > AttackRadius)
            Move(CurrentTarget);
        else
            if (AttackReady)
                StartCoroutine(CoolDownForAttak(2));
        
    }
    void Run()
    {
        if(distance(CurrentTarget)<Radius)
        {
            x1 = transform.position.x+(transform.position.x - CurrentTarget.transform.position.x) * Time.deltaTime;
            y1 = transform.position.y+(transform.position.y - CurrentTarget.transform.position.y) * Time.deltaTime;
            Move(x1, y1);
        }
        else
        {
            CurrentTarget = null;
            TargetType = "None";
            TargetPriority = 0;
        }
    }
    void Execute()
    {
        if (TargetType == "Walk")
        {
            Walk();
        }
        if (TargetType == "Run")
            Run();
        if (TargetType == "Attack")
            Attack();
        if (TargetType == "Go")
            Go();
        if (CurrentTarget != null)
        {
            if (distance(CurrentTarget) > Radius)
            {
                CurrentTarget = null;
                TargetType = "None";
            }
        }
        if(TargetType == "None")
        {
            RandomPoint();
            TargetType = "Walk";
            Walk();
        }
    }
    void IsMoved()
    {
        if( dx==dxl && dy==dyl && TargetType == "Walk")
        {
            Debug.Log(((dx == dxl) && (dy == dyl)) + " \t" + dx + " " + dxl + " \t" + dy + " " + dyl );
            RandomPoint();
            Walk();
        }
    }
    void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(gameObject);
        step = Speed * Time.deltaTime;
        Targets = new List<Transform>();
        RaycastHit2D[] Hits = Physics2D.CircleCastAll(transform.position, Radius, Vector2.zero);
        foreach(RaycastHit2D Hit in Hits)
            if (Raycollision(Hit.collider.transform) && Hit.collider.transform != transform)
                Targets.Add(Hit.collider.transform);
        Targetprefers();
        if (TargetType=="Walk" && !Raycollision(new Vector3(x1, y1, 0)))
        {
            TargetType = "None";
            Targetprefers();
            
        }
        Execute();
    }
}
