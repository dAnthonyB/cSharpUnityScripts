//Dennis Bruce
using System;
namespace Assets
{
    [Serializable]
    public class Item
    {
        public string name;
        public Effect effect;
        public int shopPrice;
        public int numOwned = 0;
        public static int maxNumItems = 4;
        public Item()
        {
            const int POTIONHEALAMOUNT = 100;
            name = "potion";
            effect = new Effect("heal", POTIONHEALAMOUNT, "HP");
            shopPrice = 100;
        }
        public Item(Item item)
        {
            name = item.name;
            effect = new Effect(item.effect);
            shopPrice = item.shopPrice;
            numOwned = 0;
        }
        public Item(string name, Effect effect, int shopPrice)
        {
            this.name = name;
            this.effect = effect;
            this.shopPrice = shopPrice;
        }
        public Item(string name, string effect, string shopPrice)
        {
            this.name = name;
            this.effect = Effect.getEffect(effect);
            this.shopPrice = int.Parse(shopPrice);
        }
        public static Item getItemFromMaster(string name)
        {
            for (int i = 0; i < DataClass.masterItemList.Count; i++)
            {
                if (DataClass.masterItemList[i].name == name)
                {
                    return DataClass.masterItemList[i];
                }
            }
            return null;
        }
    }
}
