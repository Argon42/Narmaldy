using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatorRed : MonoBehaviour
{
    [SerializeField] GameObject PrefabBomj;
    [SerializeField] int CountInPack = 3;
    PrefersStruct DefaultPref = new PrefersStruct(3, 1, 1, 1);
    List<GameObject> Bomjs;

    private void CreatePackBomjs()
    {
        Bomjs = new List<GameObject>();
        for(int i=0; i<CountInPack; i++)
        {
            PrefersStruct prefers = DefaultPref.Mutation(5);
            Vector3 SpawnTarget = new Vector3(transform.position.x + Random.Range(-1.0f, 1), transform.position.y + Random.Range(-1.0f, 1), 0);
            GameObject Temp = Instantiate(PrefabBomj, SpawnTarget, Quaternion.identity);
            Temp.transform.SetParent(transform);
            Temp.GetComponent<GiEnemy>().Prefers = prefers;
            Bomjs.Add(Temp);
        }
    }
    
    private void Start()
    {
        CreatePackBomjs();
    }

    private void Update()
    {
        if (transform.childCount == 1)
        {
            DefaultPref = transform.GetChild(0).GetComponent<GiEnemy>().Prefers;
            Debug.Log(DefaultPref.Attack + " " + DefaultPref.Walk + " " + DefaultPref.Go + " " + DefaultPref.Run);
        }
    }
}
