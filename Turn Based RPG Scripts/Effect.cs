using System;

namespace Assets._scripts
{
    [Serializable]
    public class Effect
    {
        public string statToChange;
        public string name;
        public int value;
        public int duration;
        
        public Effect()
        {
            name = "(DEFAULT)";
            value = -10;
            duration = 1;
            statToChange = "Current HP";
        }
        public Effect(string name, int value, string statToChange)
        {
            this.name = name;
            this.value = value;
            duration = 1;
            this.statToChange = statToChange;
        }
        public Effect(string name, int value, int duration, string statToChange)
        {
            this.name = name;
            this.value = value;
            this.duration = duration;
            this.statToChange = statToChange;
        }
        public void affectStat(Monster monster)
        {
            monster.stats.changeStat(statToChange, value);
        }

    }
}
