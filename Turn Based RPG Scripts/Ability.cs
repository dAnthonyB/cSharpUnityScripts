//Dennis Bruce
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._scripts
{
    [Serializable]
    public class Abilities
    {
        const int ABILITYLIMIT = 4;
        public List<Ability> abilityList;
        public int emptyAbilities;
        public Abilities(List<Ability> al)
        {
            abilityList = new List<Ability>(al);
            emptyAbilities = ABILITYLIMIT - abilityList.Capacity;
            for (int i = 0; i < emptyAbilities; i++)
            {
                abilityList.Add(new Ability());
            }
        }
        public Abilities(Abilities a)
        {
            abilityList = new List<Ability>(a.abilityList);
            emptyAbilities = a.emptyAbilities;
        }
        public void addAbility(Ability a)
        {
            if (emptyAbilities > 0)
            {
                abilityList[numAbilities() - 1] = a;
                emptyAbilities--;
            }
            else
                Debug.Log("Cannot add ability; need to make swap method");//add a swap ability method or something similar
        }
        public int numAbilities()
        {
            return ABILITYLIMIT - emptyAbilities;
        }

        public static int getAbilityLimit()
        {
            return ABILITYLIMIT;
        }
    }
    [Serializable]
    public class Ability
    {
        public string name;
        public int power;
        public int accuracy;
        public Effect effect; //so far no abilities have effects
        public Ability(){ name = "none"; power = 0; accuracy = 0; }

        public Ability(string name, int power, int accuracy)
        {
            this.name = name;
            this.power = power;
            this.accuracy = accuracy;
        }
        public Ability(string name, int power, int accuracy, Effect effect)
        {
            this.name = name;
            this.power = power;
            this.accuracy = accuracy;
            this.effect = effect;
        }
        
        public Ability(Ability a)
        {
            name = a.name;
            power = a.power;
            accuracy = a.accuracy;
        }
        public string toString()
        {
            return name;
        }
    }

    
}
