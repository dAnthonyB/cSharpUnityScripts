//Dennis Bruce

using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets._scripts
{
    [Serializable]
    public class Stats
    {
        public static string[] statNames = {"Max HP", "Current HP", "Attack", "Defense", "Speed"};
        public List<Stat> statList = new List<Stat>();
        public Stats() {
            statList.Add(new Stat("Max HP", 20, 5));
            statList.Add(new Stat("Current HP", 20, 5));
            statList.Add(new Stat("Attack", 20));
            statList.Add(new Stat("Defense", 20));
            statList.Add(new Stat("Speed", 20));
        }
        public Stats(int maxHP, int curHP, int atk, int def, int spd) {
            statList.Add(new Stat("Max HP", maxHP/5, 5));
            statList.Add(new Stat("Current HP", curHP));
            statList.Add(new Stat("Attack", atk));
            statList.Add(new Stat("Defense", def));
            statList.Add(new Stat("Speed", spd));
        }
        public Stats(Stats s)
        {
            statList = s.statList;
        }
        public int totalStatPoints()
        {
            return (getStatValue("Max HP") / 5) + getStatValue("Attack")
                  + getStatValue("Defense") + getStatValue("Speed");
        }
        public int getStatIdx(string s)
        {
            for (int i = 0; i < statList.Capacity; i++)
            {
                if (statList[i].getName().Equals(s))
                {
                    return i;
                }
            }
            Debug.Log("could not find stat index");
            return -1;
        }
        //adds value to the named stat (i.e. negative values for reducing stat)
        public void changeStat(string name, int value)
        {
            statList[getStatIdx(name)].changeValue(value);
        }
        public int getStatValue(string name)
        {
            return statList[getStatIdx(name)].getValue();
        }
        public int getNumStats()
        {
            return statList.Capacity;
        }
    }

    [Serializable]
    public class Stat
    {
        public string name;
        public int value;
        public int weight;
        public Stat()
        {
            name = "Default Stat";
            value = 0;
            weight = 0;
        }
        public Stat(string name, int value)
        {
            this.name = name;
            this.value = value;
            weight = 1;
        }
        public Stat(string name, int value, int weight)
        {
            this.name = name;
            this.value = value*weight;
            this.weight = weight;
        }
        public string getName()
        {
            return name;
        }
        public int getValue()
        {
            return value;
        }
        public void changeValue(int v)
        {
            value += v;
        }
    }
}