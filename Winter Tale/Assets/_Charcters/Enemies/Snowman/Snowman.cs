using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Snowman : Character
    {
        [SerializeField] GameObject regularSnowman;
        [SerializeField] GameObject snowParticle;
        public override void Kill()
        {
            base.Kill();
            transform.GetChild(0).gameObject.SetActive(false);
            GameObject snowman = Instantiate(regularSnowman, transform.position, transform.rotation);

            float width = Random.Range(0.5f, 1.2f);
            float height = Random.Range(0.5f, 1.3f);
            snowman.transform.localScale = new Vector3(width, height, width);

            var part = Instantiate(snowParticle, transform.position, snowParticle.transform.rotation);
            Destroy(part, 20);
        }
    }
}