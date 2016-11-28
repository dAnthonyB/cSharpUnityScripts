using System;
using System.Collections.Generic;
namespace Assets
{
    [Serializable]
    public class PlayerCharacter
    {
        public Party party = new Party();
        public List<Monster> ownedMonsters = new List<Monster>();
        public List<Item> itemList = new List<Item>();
        public int number;
        public string name;
        public int money = 0;
        public int score = 0;
        public PlayerCharacter(){}
        public string toString()
        {
            string player = "player = [name = "+name+"], " +party;
            return player;
        }
        //use method for capturing monsters
        public void addMonster(Monster m)
        {
            ownedMonsters.Add(m);
            if(party.monsterList.Capacity < Party.getMaxPartySize())
                party.addMonster(m);
            if(party.monsterList.Count == Party.getMaxPartySize())
                ownedMonsters.Add(new Monster(m));
            else
                party.addMonster(new Monster(m));
        }
        public void addItem(string name)
        {
            int idx = hasItemAt(name);
            if (idx == -1)
            {
                itemList.Add(new Item(Item.getItemFromMaster(name)));
                itemList[itemList.Count-1].numOwned++;
            }
            else if (idx >= 0)
            {
                itemList[idx].numOwned++; 
            }
        }
        //returns true and lowers item count if player has item 
        //returns false if the player doesn't have the item
        public bool removeItem(string name)
        {
            int idx = hasItemAt(name);
            if (idx == -1)
            {
                return false;
            }
            else if (itemList[idx].numOwned <= 1)
            {
                itemList.RemoveAt(idx);
            }
            else
            {
                itemList[idx].numOwned--;
            }
            return true;
                
        }
        public int hasItemAt(string name)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].name.Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }
        public Item getItem(string name)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].name.Equals(name))
                {
                    return itemList[i];
                }
            }
            return null;
        }
    }
}
