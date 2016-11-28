//Dennis Bruce
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets
{
    [Serializable]
    public class Party
    {
        const int MAXSIZE = 4;
        public List<Monster> monsterList;
        public Party()
        {
            monsterList = new List<Monster>();
        }
        public Party(List<Monster> mL)
        {
            monsterList = new List<Monster>(mL);
        }
        public static int getMaxPartySize()
        {
            return MAXSIZE;
        }
        public void addMonster(Monster m)
        {
            //checks if party is full
            if (monsterList.Count == MAXSIZE)
                removeMonster(chooseMonster()); //lets the player choose which monster to remove
            monsterList.Add(new Monster(m)); //adds the monster to the list
        }
        public void removeMonster(Monster m)
        {
            if (monsterList.Contains(m))
            {
                monsterList.Remove(m);
            }
            else
            {
                Debug.Log("Can't find monster in party");
            }
        }
        public void reviveMonster(Monster m, int healthPercent)
        {
            if (m.KOd())
            {
                m.stats.changeCurrentStat("HP", m.stats.getStatValue("HP") * (healthPercent / 100));
            }
            else
            {
                Debug.Log("monster is not knocked out");
            }
        }
        //checks if all monsters are knocked out
        public bool checkIfLoss()
        {
            for(int i = 0; i < monsterList.Count; i++)
            {
                if (!monsterList[i].KOd())
                    return false;
            }
            return true;
        }
        public string toString()
        {
            string party = "party = ";
            for (int i = 0; i < MAXSIZE; i++)
                party += "[monster" + i + monsterList[i] + "] , ";
            return party;
        }
        public void swapMonsters(Monster switchIn)
        {
            removeMonster(chooseMonster());
            addMonster(switchIn);
        }
        public Monster getMonster(string name)
        {
            return monsterList[getMonsterIdx(name)];
        }
        public int getMonsterIdx(string name)
        {
            for (int m = 0; m < monsterList.Count; m++)
            {
                if (name.Equals(monsterList[m].name))
                    return m;
            }
            Debug.Log("couldn't find monster");
            return -1;
        }
        public void resetParty()
        {
            for(int i = 0; i < monsterList.Count; i++)
            {
                monsterList[i].stats.setCurrentStatsToMax();
                monsterList[i].removeEffects();
            }
        }
        //incomplete
        //method that lets the player choose a monster from their party
        public Monster chooseMonster()
        {
            return new Monster();
        }
    }
}
