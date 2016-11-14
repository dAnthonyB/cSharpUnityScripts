using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._scripts
{
    [Serializable]
    public class Party
    {
        const int MAXSIZE = 6;
        public List<Monster> monsterList;
        public int numKOd = 0;

        public Party() { monsterList = new List<Monster>(); }
        public Party(List<Monster> mL)
        {
            monsterList = new List<Monster>(mL);
        }
        public Party(Boolean random)
        {
            if(random == true) { 
                for(int i = 0; i < MAXSIZE; i++)
                {
                    monsterList.Add(DataClass.randomMonster());
                    monsterList[i].idx = i;
                }
                    
            }
            else
            {
                for (int i = 0; i < MAXSIZE; i++)
                {
                    monsterList.Add(new Monster());
                    monsterList[i].idx = i;
                }
            }
        }

        public static int getMaxPartySize()
        {
            return MAXSIZE;
        }
        public void addMonster(Monster m)
        {
            if (monsterList.Count < MAXSIZE)
            {
                monsterList.Add(new Monster(m));
            }
            else
            {
                //swapMonsters(m);
            }

        }
        public void removeMonster(Monster m)
        {
            if (monsterList.Contains(m))
            {
                monsterList.Remove(m);
            }
            else
            {
                //error handling
            }
            
        }
        public void reviveMonster(Monster m, int healthPercent)
        {
            if (m.KOd())
            {
                m.stats.changeStat("Current HP", m.stats.getStatValue("Max HP") * (healthPercent / 100));
                numKOd--;
            }
            else
            {
                //error handling
            }
        }
        public void KOMonster(Monster m)
        {
            if (!m.KOd())
                numKOd++;
            else
            {
                //error handling
            }
        }
        public bool checkIfLoss()
        {
            if (numKOd >= monsterList.Count)
                return true;
            else
                return false;
        }
        public string toString()
        {
            string party = "party = ";
            for (int i = 0; i < MAXSIZE; i++)
                party += "[monster" + i + monsterList[i] + "] , ";
            party += "[numKOd = " + numKOd;
            return party;
        }
    }
}
