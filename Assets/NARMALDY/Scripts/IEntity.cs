using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narmaldy
{
    public abstract class Entity : MonoBehaviour
    {
        private int _HP;

        public virtual int Mana { get; set; }
        public virtual float Speed { get; set; }
        public int Heals
        {
            get
            {
                return _HP;
            }

            set
            {
                _HP = value;
                if (_HP <= 0)
                    Death();
            }
        }

        public virtual void Death()
        {
            Debug.Log("Death");
            Destroy(gameObject);
        }
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
