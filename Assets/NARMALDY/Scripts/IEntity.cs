using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narmaldy
{
    public abstract class Entity : MonoBehaviour
    {
        public int Heals { get; set; } 
        public int Mana { get; set; }
        public float Speed { get; set; }
    }

    public class Player : Entity
    {
        [SerializeField] GameObject HitAreaPrefab;
        public void Attak()
        {
            GameObject HitArea = Instantiate(HitAreaPrefab, transform.position, Quaternion.identity);

        }
    }

    public abstract class Item
    {
        public int Damage { get; set; }
        public int Cost { get; set; }
        public abstract void Attack(Entity entity);
    }
}
