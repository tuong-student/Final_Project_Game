using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOOD.ModifiableStats;

namespace Game
{
    public class HealthSystem : MonoBehaviour
    {
        private ModifiableStats<float> _health;

        public void SetHealth(float health)
        {

        }

        public void MinusHealth(float amount)
        {
            // _health.modifiers.Add() ;
        }
    }

}
