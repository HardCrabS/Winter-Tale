using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] Slider energySlider;
        [SerializeField] AudioClip outOfEnergy;
        [SerializeField] float maxEnergyPoints = 100;
        [SerializeField] float regenPointsPerSecond = 1;

        public List<AbilityConfig> abilities = new List<AbilityConfig>();

        float currEnergyPoints;

        AudioSource audioSource;
        // Start is called before the first frame update
        void Start()
        {
            currEnergyPoints = maxEnergyPoints;
            audioSource = GetComponent<AudioSource>();

            AttachInitialAbilities();
        }

        private void Update()
        {
            RegenerateEnergyPoints();
        }
        private void AttachInitialAbilities()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i] != null)
                    abilities[i].AttachAbility(gameObject);
            }
        }
        public int GetNumberOfAbilities()
        {
            return abilities.Count;
        }
        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            if (abilities[abilityIndex] == null) return;

            float energyCost = abilities[abilityIndex].EnergyCost;
            if (energyCost <= currEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergy);
            }
        }
        void RegenerateEnergyPoints()
        {
            if (currEnergyPoints < maxEnergyPoints)
            {
                float pointsToAdd = regenPointsPerSecond * Time.deltaTime;
                currEnergyPoints = Mathf.Clamp(currEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
                UpdateEnergyBar();
            }
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currEnergyPoints;
        }
        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currEnergyPoints - amount;
            currEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        void UpdateEnergyBar()
        {
            energySlider.value = EnergyAsPercantage();
        }

        float EnergyAsPercantage()
        {
            return currEnergyPoints / maxEnergyPoints;
        }
    }
}