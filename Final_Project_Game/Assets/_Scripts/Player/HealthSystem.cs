using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeawispHunter.RolePlay.Attributes;

namespace Game
{
    public class HealthSystem : MonoBehaviour
    {
        private ModifiableValue<float> _health;

        public void SetHealth(float health)
        {

        }

        public void MinusHealth(float amount)
        {
            // _health.modifiers.Add() ;
        }
    }

}
