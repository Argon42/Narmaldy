using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIrat : MonoBehaviour
{
    [SerializeField]Transform Target;
    float x1, y1;
    [SerializeField] float speed;
    [SerializeField] float radius;
    float step, x2, y2;
    float[] dirs = new float[4];
    [SerializeField] Tilemap Ground;
    float width, higth,offsetY;

    [SerializeField]BoxCollider2D EnemyCol;
    private void Start()
    {
        width = EnemyCol.size.x / 2;
        higth = EnemyCol.size.y / 2;
        offsetY = EnemyCol.offset.y;
        x2 = transform.position.x;
        y2 = transform.position.y;
        x1 = x2;
        y1 = y2;
    }

    float distance(float x, float y)
    {
        return Mathf.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1));
    }
    bool isfloor(Vector3 DirForVector)
    {
        float x, y;
        x = transform.position.x;
        y = transform.position.y + offsetY;
        return (Ground.GetTile(Ground.WorldToCell(new Vector3(x - width, y - higth, 0) + DirForVector * step)) &&
                Ground.GetTile(Ground.WorldToCell(new Vector3(x - width, y + higth, 0) + DirForVector * step)) &&
                Ground.GetTile(Ground.WorldToCell(new Vector3(x + width, y - higth, 0) + DirForVector * step)) &&
                Ground.GetTile(Ground.WorldToCell(new Vector3(x + width, y + higth, 0) + DirForVector * step)));
    }
    void directions()
    {
        for (int i = 0; i < 4; i++)
            dirs[i] = 0;

        float x, y;
        x = transform.position.x;
        y = transform.position.y + offsetY;
        if (isfloor(Vector3Int.up))
            dirs[0] = distance(x, y + step) - distance(x, y);
        if (isfloor(Vector3Int.right))
            dirs[1] = distance(x + step, y) - distance(x, y);
        if (isfloor(Vector3Int.down))
            dirs[2] = distance(x, y - step) - distance(x, y);
        if (isfloor(Vector3Int.left))
            dirs[3] = distance(x - step, y) - distance(x, y);
    }
    void next()
    {
        float min=0;
        for (int i = 0; i < 4; i++)
            if (min > dirs[i])
                min = dirs[i];
        x2 = transform.position.x;
        y2 = transform.position.y;
        if (min == dirs[0])
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y + step));
        if (min == dirs[1])
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x + step, transform.position.y));
        if (min == dirs[2])
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y - step));
        if (min == dirs[3])
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x - step, transform.position.y));
    }
    bool ismoved()
    {
        if ((x2 - transform.position.x < step) && (y2 - transform.position.y < step))
            return false;
        else return true;
    }
    void raycollision()
    {
        float dx, dy, s, x, y;
        x = transform.position.x;
        y = transform.position.y;
        dx = Target.transform.position.x - transform.position.x;
        dy = Target.transform.position.y - transform.position.y;
        s = Mathf.Sqrt(dx * dx + dy * dy);
        for(float i=0; i<s/step; i++)
        {
            x += dx / s * step;
            y += dy / s * step;
            if (Ground.GetTile(Ground.WorldToCell(new Vector3(x, y, 0))) == null)
                return;
        }
        x1 = Target.transform.position.x;
        y1 = Target.transform.position.y;
    }
    float playerdistant()
    {
        float x = Target.transform.position.x - transform.position.x;
        float y = Target.transform.position.y - transform.position.y;
        return Mathf.Sqrt(x * x + y * y);
    }
    private void Update()
    {

        step = speed * Time.deltaTime;
        if(playerdistant()<=radius)
            raycollision();
        if (distance(transform.position.x, transform.position.y) > step)
        {
            directions();
            next();
        }
        
    }
}
