//Dennis Bruce
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets
{
    [Serializable]
    public class StatList
    {
        public static string[] statNames = {"HP", "Attack", "Defense", "Speed"};
        public static int[] statWeights = { 10, 1, 1, 1 };
        public List<Stat> statList = new List<Stat>();
        public List<Stat> currentStatList = new List<Stat>();
        public StatList() {
            for (int i = 0; i < statNames.Length; i++)
            {
                Stat temp = new Stat(statNames[i], 20 * statWeights[i], statWeights[i]);
                statList.Add(temp);
                currentStatList.Add(temp);
            }
        }
        public StatList(int[] statValues) {
            for(int i = 0; i < statNames.Length; i++)
            {
                statList.Add(new Stat(statNames[i], statValues[i], statWeights[i]));
                currentStatList.Add(new Stat(statNames[i], statValues[i], statWeights[i]));
            }
        }
        public StatList(StatList s)
        {
            for(int i = 0; i < s.statList.Count; i++)
            {
                statList.Add(new Stat(s.statList[i]));
                currentStatList.Add(new Stat(s.statList[i]));
            }
        }
        public int totalStatPoints()
        {
            int total = 0;
            for(int i = 0; i < statNames.Length; i++)
            {
                total += getStatValue(statNames[i]) / statWeights[i];
            }
            return total;
        }
        public double statPercentage(int idx)
        {
            return (statList[idx].value/statList[idx].weight) / totalStatPoints();
        }
        public int getStatIdx(string s)
        {
            for (int i = 0; i < statList.Count; i++)
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
        public void changeCurrentStat(string name, int value)
        {
            currentStatList[getStatIdx(name)].changeValue(value);
        }
        public int getStatValue(string name)
        {
            return statList[getStatIdx(name)].getValue();
        }
        public int getCurrentStatValue(string name)
        {
            return currentStatList[getStatIdx(name)].getValue();
        }
        public int getNumStats()
        {
            return statList.Count;
        }
        public void setCurrentStatsToMax()
        {
            for(int i = 0; i < currentStatList.Count; i++)
            {
                    currentStatList[i].value = statList[i].value;
            }
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
        public Stat(Stat s)
        {
            name = s.name;
            value = s.value;
            weight = s.weight;
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
            this.value = value;
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