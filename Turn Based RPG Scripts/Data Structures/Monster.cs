//Dennis Bruce
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets
{
    [Serializable]
    public class Monster
    {
        public string name;
        public int level = 1;
        public int idx; //index on the masterMonsterList in DataClass
        public string ownerName;
        public const int STATPOINTSPERLEVEL = 30;
        public List<Effect> currentEffects = new List<Effect>();
        public List<int> currentEffectDurations = new List<int>();
        public AbilityList abilities;
        public StatList stats;
        public Type type = new Type();
        public Monster()
        {
            name = "Default";
        }
        public Monster(Monster m)
        {
            name = m.name;
            abilities = new AbilityList(m.abilities);
            stats = new StatList(m.stats);
            level = m.level;
            idx = m.idx;
            type = new Type(m.type);
        }
        
        public Monster(string monsterName)
        {
            for(int i =0; i < DataClass.masterMonsterList.Count; i++)
            {
                if (DataClass.masterMonsterList[i].name.Equals(monsterName))
                {
                    name = DataClass.masterMonsterList[i].name;
                    abilities = new AbilityList(DataClass.masterMonsterList[i].abilities);
                    stats = DataClass.masterMonsterList[i].stats;
                }
            }
        }
        public Monster(string name, StatList stats, Type type, AbilityList abilities, int idx)
        {
            this.name = name;
            this.stats = new StatList(stats);
            this.idx = idx;
            this.abilities = new AbilityList(abilities);
            this.type = new Type(type);           
        }
        public bool KOd() {
            if(stats.getCurrentStatValue("HP") < 0)
            {
                stats.currentStatList[0].value = 0;
                return true;
            }
            if (stats.getCurrentStatValue("HP") == 0)
                return true;
            return false;
        }
        public void addEffect(Effect e)
        {
            //add limit to the number of effects?
            Effect temp = new Effect(e);
            Debug.Log(temp.duration);
            currentEffectDurations.Add(temp.duration);
            currentEffects.Add(temp);
            
        }
        public void applyEffects()
        {
            for(int i = 0; i < currentEffects.Count; i++)
            {
                if (currentEffectDurations[i] > 0)
                {
                    currentEffects[i].affectStat(this);
                    string healOrDamage = "";
                    if (currentEffects[i].value > 0)
                        healOrDamage = "healed";
                    if (currentEffects[i].value > 0)
                        healOrDamage = "damaged";
                    currentEffectDurations[i]--;
                    DataClass.output(currentEffects[i].name + " has " + healOrDamage + " " + name + "'s " + currentEffects[i].statToChange + " by " + currentEffects[i].value);
                }
                if(currentEffectDurations[i] == 0)
                {
                    currentEffects.RemoveAt(i);
                    currentEffectDurations.RemoveAt(i);
                }
            }
        }
        //function for leveling up monsters
        public void levelUp(int toLevel)
        {
            double totalStatPoints = DataClass.masterMonsterList[idx].stats.totalStatPoints();
            List<double> statWeights = new List<double>();
            statWeights.Add(0);
            for (int i = 1; i < stats.getNumStats(); i++)
            {
                statWeights.Add((DataClass.masterMonsterList[idx].stats.getStatValue(StatList.statNames[i]) / stats.statList[i].weight) 
                                / totalStatPoints + statWeights[i - 1]);
            }
            while (level < toLevel)
            {
                double temp;
                for (int i = 0; i < STATPOINTSPERLEVEL; i++)
                {
                    temp = UnityEngine.Random.value;
                    if (0 <= temp && temp <= statWeights[1])
                        stats.changeStat("HP", 10);
                    else if (statWeights[0] < temp && temp <= statWeights[2])
                        stats.changeStat("Attack", 1);
                    else if (statWeights[1] < temp && temp <= statWeights[3])
                        stats.changeStat("Defense", 1);
                    else
                        stats.changeStat("Speed", 1);

                }
                level++;
            }
            stats.setCurrentStatsToMax();
        }
        public void removeEffects()
        {
            currentEffects = new List<Effect>();
            currentEffectDurations = new List<int>();
        }
        public string toString()
        {
            string monster = "";
            for(int i = 0; i < abilities.abilityList.Count; i++)
            {
                monster += "Ability"+i+" = ["+abilities.abilityList[i].name;
            }
            return monster;
        }
    }
}
