using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class AbilityGiver : MonoBehaviour
    {
        [SerializeField] AbilityConfig abilityConfig;
        [SerializeField] int abilityKeyIndex;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerControl>() != null)
            {
                other.GetComponent<SpecialAbilities>().abilities[abilityKeyIndex] = abilityConfig;
                abilityConfig.AttachAbility(other.gameObject);
                Destroy(this);
            }
        }
    }
}