using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._scripts
{
    [Serializable]
    public class Monster
    {
        public string name;
        public int level = 1;
        public int idx; //index on the masterMonsterList in DataClass
        public const int STATPOINTSPERLEVEL = 30;
        public Abilities abilities;
        public Stats stats;
        public Monster()
        {
            name = "Default";
        }
        public Monster(Monster m)
        {
            name = m.name;
            abilities = new Abilities(m.abilities);
            stats = new Stats(m.stats);
            level = m.level;
            idx = m.idx;
        }
        
        public Monster(string monsterName)
        {
            for(int i =0; i < DataClass.masterMonsterList.Count; i++)
            {
                if (DataClass.masterMonsterList[i].name.Equals(monsterName))
                {
                    name = DataClass.masterMonsterList[i].name;
                    abilities = new Abilities(DataClass.masterMonsterList[i].abilities);
                    stats = DataClass.masterMonsterList[i].stats;

                }
            }
        }
        public Monster(string name, Stats stats, Abilities abilities, int idx)
        {
            this.name = name;
            this.stats = new Stats(stats);
            this.idx = idx;
            this.abilities = abilities;
            
        }
        public bool KOd() {
            if(stats.getStatValue("Current HP") <= 0)
            {
                stats.changeStat("Current HP", stats.getStatValue("Current HP") * -1);
                return true;
            }
            return false;
        }

        //function for leveling up monsters
        public void levelUp(int toLevel)
        {
            for (int j = level; j < toLevel; j++)
            {
                double[] statWeights = new double[stats.getNumStats()];
                double totalStatPoints = DataClass.getMonster(name).stats.totalStatPoints();
                statWeights[0] = (stats.getStatValue("Max HP") / 5) / totalStatPoints;
                statWeights[1] = stats.getStatValue("Attack") / totalStatPoints + statWeights[0];
                statWeights[2] = stats.getStatValue("Defense") / totalStatPoints + statWeights[1];
                statWeights[3] = stats.getStatValue("Speed") / totalStatPoints + statWeights[2];

               
                double temp;
                for (int i = 0; i < STATPOINTSPERLEVEL; i++)
                {
                    System.Random rand = new System.Random(i);
                    temp = rand.NextDouble();
                    if (0 <= temp && temp <= statWeights[0])
                    {
                        stats.changeStat("Max HP", 5);
                        stats.changeStat("Current HP", 5);
                    }
                    else if (statWeights[0] < temp && temp <= statWeights[1])
                        stats.changeStat("Attack", 1);
                    else if (statWeights[1] < temp && temp <= statWeights[2])
                        stats.changeStat("Defense", 1);
                    else
                        stats.changeStat("Speed", 1);
                }
                level += 1;
            }
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
