using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 10f;

        float damage;
        int shooterLayer;

        public void SetDamage(float _damage)
        {
            damage = _damage;
        }
        public void SetShooterLayer(int layer)
        {
            shooterLayer = layer;
        }
        public float GetDefauliSpeed()
        {
            return speed;
        }

        //void OnCollisionEnter(Collision col)
        //{
        //    if (col.gameObject.GetComponent<>() != null && col.gameObject.layer != shooterLayer)
        //    {
        //        col.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        //        Destroy(gameObject);
        //    }
        //}
    }
}