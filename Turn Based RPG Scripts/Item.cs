using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Assets._scripts
{
    [Serializable]
    public class Item
    {
        public string name;
        public Effect effect;
        public int shopPrice;
        public Item()
        {
            const int POTIONHEALAMOUNT = 100;
            name = "potion";
            effect = new Effect("heal", POTIONHEALAMOUNT, "Current HP");
            shopPrice = 100;
        }
        public Item(string name, Effect effect, int shopPrice)
        {
            this.name = name;
            this.effect = effect;
            this.shopPrice = shopPrice;
        }/*
        public Item(string type)
        {
            if (type.Equals("potion"))
            {
                
            }else if (type.Equals("revive"))
            {

            }
        }*/
    }
}
