//Dennis Bruce
using System;
namespace Assets
{
    [Serializable]
    public class Effect
    {
        public string name;
        public int value;
        public int duration;
        public string statToChange;
        public bool percentage;
        
        public Effect()
        {
            name = "(DEFAULT)";
            value = -10;
            duration = 1;
            statToChange = "Current HP";
        }
        public Effect(Effect effect)
        {
            name = effect.name;
            value = effect.value;
            duration = effect.duration;
            statToChange = effect.statToChange;
            percentage = effect.percentage;
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
        public Effect(string name, string value, string duration, string statToChange, string percentage)
        {
            this.name = name;
            this.value = int.Parse(value);
            this.duration = int.Parse(duration);
            this.statToChange = statToChange;
            if (percentage[0].Equals("t") || percentage[0].Equals("T"))
                this.percentage = true;
            else
                this.percentage = false;
        }
        public void affectStat(Monster monster)
        {
            if (percentage)
            {
                int percentValue = (value / 100) * monster.stats.statList[monster.stats.getStatIdx(statToChange)].value;
                monster.stats.changeCurrentStat(statToChange, percentValue);
            }
            else
                monster.stats.changeCurrentStat(statToChange, value);
        }
        public static Effect getEffect(string name)
        {
            for(int i = 0; i < DataClass.masterEffectList.Count; i++)
            {
                if (name.Equals(DataClass.masterEffectList[i].name))
                {
                    return DataClass.masterEffectList[i];
                }
            }
            return null;
        }
    }
}
