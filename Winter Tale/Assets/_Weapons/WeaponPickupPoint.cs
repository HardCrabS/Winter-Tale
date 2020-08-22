using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        void Start()
        {

        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                SpawnWeapon();
            }
        }

        private void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void SpawnWeapon()
        {
            var weapon = weaponConfig.WeaponPrefab;
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            bool isPlayer = other.GetComponent<PlayerControl>() != null;
            if (isPlayer)
            {
                var weaponSystem = other.GetComponent<WeaponSystem>();
                weaponSystem.PutWeaponInHands(weaponConfig);
                AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
