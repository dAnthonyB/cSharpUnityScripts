//Dennis Bruce
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Assets
{
    public class DataClass : MonoBehaviour
    {
        public static DataClass data;

        public static List<Ability> masterAbilityList = new List<Ability>(); //List of ALL abilities saved (including ones not in scene)
        public string abilityFile = "Assets/Lists/abilityList.txt"; //make sure there is a file in the assests folder with info for abilities

        public static List<Monster> masterMonsterList = new List<Monster>(); //List of ALL monsters saved (including ones not in scene)
        public string monsterFile = "Assets/Lists/monsterList.txt"; //make sure there is a file in the assests folder with info for monsters

        public static List<Item> masterItemList = new List<Item>(); //List of ALL items saved (including ones not in scene)
        public string itemFile = "Assets/Lists/itemList.txt"; //make sure there is a file in the assests folder with info for items

        public static List<Effect> masterEffectList = new List<Effect>(); //List of ALL items saved (including ones not in scene)
        public string effectFile = "Assets/Lists/effectList.txt"; //make sure there is a file in the assests folder with info for items

        public static PlayerCharacter player1 = new PlayerCharacter();
        public string player1Name = "Player1";

        public static PlayerCharacter player2 = new PlayerCharacter();
        public string player2Name = "Player2";

        public bool loadPlayer1 = false; //set to true if you have a save file
        public bool loadPlayer2 = false; //set to true if you have a save file

        public static List<string> textString = new List<string>(); //list of strings to be output 

        //variables used for debugging
        public List<int> idxsForParty1, idxsForParty2;
        public List<string> ts; //used to manually add text from unity editor
        // Use this for initialization
        void Awake()
        {
            if (data == null)
            {
                DontDestroyOnLoad(gameObject);
                data = this;
                loadMasterLists();
                if(loadPlayer1)
                    player1 = loadData(player1Name);
                if(loadPlayer2)
                    player2 = loadData(player2Name);
                setUpPredefinedParty(loadPlayer1, loadPlayer2);

                player1.number = 1;
                player2.number = 2;
                textString = new List<string>(ts);
            }
            else if (data != this)
            {
                Destroy(gameObject);
            }
        }
        void Destroy() { }
        void Update() {
            //used to update list manually in unity editor
            if (textString.Count < ts.Count)
            {
                textString.Add(ts[textString.Count - 1]);
            } 
        }
        //returns the player object associated with the argument's number
        public static PlayerCharacter getPlayer(int playerNumber)
        {
            if (playerNumber == 1)
                return player1;
            else
                return player2;
        }
        void loadMasterAbilityList()
        {
            //sets up the master list of abilities
            string[] abilities = File.ReadAllLines(abilityFile);
            for(int i=0; i< abilities.Length; i += 3)
            {
                masterAbilityList.Add(new Ability(abilities[i], Int32.Parse(abilities[i+1]), Int32.Parse(abilities[i+2])));
            }
        }
        void loadMasterMonsterList()
        {
            string[] s = File.ReadAllLines(monsterFile);
            int numArgs = 10;
            for (int i = 0; i < s.Length; i += numArgs)
            {
                //grabs stat values
                int offset1;
                int[] statValues = new int[StatList.statNames.Length];
                for (offset1 = 1; offset1 <= statValues.Length; offset1++)
                    statValues[offset1-1] = int.Parse(s[i + offset1]);
                Type type = new Type(s[i + offset1]);
                offset1++;
                //gets the names of the monster's abilities
                int offset2 = offset1;
                string[] abilityNames = new string[AbilityList.getAbilityLimit()];
                for (; offset2 < offset1+AbilityList.getAbilityLimit(); offset2++)
                    abilityNames[offset2-offset1] = s[i + offset2];
                
                //puts the abilities into the monsters ability list
                List<Ability> abilityList = new List<Ability>();
                for (int j = 0; j < AbilityList.getAbilityLimit(); j++)
                {
                    if (!abilityNames[j].Equals("none"))
                    {
                        for (int m = 0; m < masterAbilityList.Count; m++)
                        {
                            if (abilityNames[j].Equals(masterAbilityList[m].name))
                                abilityList.Add(masterAbilityList[m]);
                        }
                    }
                }
                StatList stats = new StatList(statValues);
                AbilityList abilities = new AbilityList(abilityList);
                Monster mon = new Monster(s[i], stats, type, abilities, i / numArgs);
                masterMonsterList.Add(mon);
            }
        }
        private void loadMasterEffectList()
        {
            string[] effects = File.ReadAllLines(effectFile);
            int numArgs = 5;
            for (int i = 0; i < effects.Length; i += numArgs)
            {
                masterEffectList.Add(new Effect(effects[i], effects[i + 1], effects[i + 2], effects[i + 3], effects[i + 4]));
            }
        }
        void loadMasterItemList()
        {
            string[] items = File.ReadAllLines(itemFile);
            int numArgs = 3;
            for (int i = 0; i < items.Length; i += numArgs)
            {
                masterItemList.Add(new Item(items[i], items[i + 1], items[i + 2]));
            }
        }
        void loadMasterLists()
        {
            loadMasterAbilityList();
            loadMasterMonsterList();
            loadMasterEffectList();
            loadMasterItemList();
        }
        //saves a player to file
        public static void saveData(PlayerCharacter player)
        {
            string filepath = "Assets/" + player.name + "Data.dat";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filepath, FileMode.OpenOrCreate);
            
            SaveData data = new SaveData();
            data.p = player;
            bf.Serialize(file, data);
            file.Close();
        }
        //loads a player's data from file
        public static PlayerCharacter loadData(string playerName)
        {
            string filepath = "Assets/" + playerName + "Data.dat";
            if (File.Exists(filepath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filepath, FileMode.Open);
                SaveData data = (SaveData)bf.Deserialize(file);
                file.Close();
                return data.p;
            }
            PlayerCharacter p = new PlayerCharacter();
            return p;
        }
        //get the Monster with base stats with name matching the argument's value
        public static Monster getMonster(string name)
        {
            for (int m = 0; m < masterMonsterList.Count; m++) {
                if (name.Equals(masterMonsterList[m].name))
                    return masterMonsterList[m];
            }
            return null;
        }
       
        //Debug functions
        public static Party createParty(string[] monsterNames)
        {
            List<Monster> mon = new List<Monster>();
            for (int i = 0; i < monsterNames.Length; i++)
            {
                for (int j = 0; j < masterMonsterList.Count; j++)
                {
                    if (monsterNames[i].Equals(masterMonsterList[j].name))
                        mon.Add(masterMonsterList[j]);
                }
            }
            return new Party(mon);
        }
        public static Monster randomMonster(int level)
        {
            int randInt;
            System.Random random = new System.Random();
            if (masterMonsterList.Count > 0)
                randInt = random.Next(0, masterMonsterList.Count - 1);
            else
                return null;
            Monster temp = masterMonsterList[randInt];
            temp.levelUp(level);
            return temp;
        }
        //change to set up a party to debug instead of loading from file
        public void setUpPredefinedParty(bool loadP1, bool loadP2)
        {
            //sets up player 1's party
            if (!loadP1)
            {
                player1.name = player1Name;
                for (int i = 0; i < idxsForParty1.Capacity; i++)
                {
                    Monster temp = new Monster(masterMonsterList[idxsForParty1[i]]);
                    temp.levelUp(30);
                    player1.party.addMonster(temp);
                }
                player1.itemList.Add(new Item(masterItemList[0]));
                player1.itemList[0].numOwned++;
            }
            //sets up player 2's party
            if (!loadP2)
            {
                player2.name = player2Name;
                for (int i = 0; i < idxsForParty2.Capacity; i++)
                {
                    Monster temp = new Monster(masterMonsterList[idxsForParty2[i]]);
                    temp.levelUp(30);
                    player2.party.addMonster(temp);
                }
            }
        }
        public static void output(string line)
        {
            textString.Add(line);
        }
    }
    
    [Serializable]
    class SaveData
    {
        public PlayerCharacter p;
    }
}