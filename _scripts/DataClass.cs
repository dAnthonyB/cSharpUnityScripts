using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Assets._scripts
{
    public class DataClass : MonoBehaviour
    {
        public static DataClass data;
        public static List<Ability> masterAbilityList = new List<Ability>(); //List of ALL abilities saved (including ones not in scene)
        public static List<Monster> masterMonsterList = new List<Monster>(); //List of ALL monsters saved (including ones not in scene)
        public static bool[] monsterTaken;
        public static PlayerCharacter player1 = new PlayerCharacter();
        public static PlayerCharacter player2 = new PlayerCharacter();
        string monsterFile = "Assets/monsters.txt"; //make sure there is a file in the assests folder with info for monsters
        string abilityFile = "Assets/abilityList.txt"; //make sure there is a file in the assests folder with info for abilities
        // Use this for initialization
        void Awake()
        {
            if (data == null) {
                DontDestroyOnLoad(gameObject);
                data = this;
                loadMasterLists();
                monsterTaken = new bool[masterMonsterList.Count];
                for (int i = 0; i < monsterTaken.Length; i++)
                    monsterTaken[i] = false;
                setUpPredefinedParty();
                /*
                player1 = loadData("Player1");
                player1.playerName = "Player1";
                player2 = loadData("Player1");
                player2.playerName = "Player2";*/
                player1.number = 1;
                player2.number = 2;

                
            }
            else if (data != this)
            {
                Destroy(gameObject);
            }
            
        }
        //Update needed to satisfy sub-class requirements
        void Update()
        {
        }
        public static PlayerCharacter getPlayer(int playerNumber)
        {
            if (playerNumber == 1)
                return player1;
            else
                return player2;
        }
        void loadMasterLists()
        {
            //sets up the master list of abilities
            string[] abilities = File.ReadAllLines(abilityFile);
            for(int i=0; i< abilities.Length; i += 3)
            {
                masterAbilityList.Add(new Ability(abilities[i], Int32.Parse(abilities[i+1]), Int32.Parse(abilities[i+2])));
            }

            //sets up monster list
            string[] s = File.ReadAllLines(monsterFile);
            for (int i = 0; i < s.Length; i += 9)
            {
                string[] abilityNames = new string[Abilities.getAbilityLimit()];
                //gets the names of the monster's abilities
                for (int j = 0; j < Abilities.getAbilityLimit(); j++)
                    abilityNames[j] = s[i + j + 5];

                List<Ability> abilityList = new List<Ability>();

                //puts the abilities into the monsters ability list
                for (int k = 0; k < Abilities.getAbilityLimit(); k++)
                {                    
                    if (!abilityNames[k].Equals("none"))
                    {
                        for(int m = 0; m < masterAbilityList.Count; m++)
                        {
                            if (abilityNames[k].Equals(masterAbilityList[m].name))
                                abilityList.Add(masterAbilityList[m]);
                        }
                    }                        
                }
                //creates new stat object using file data
                Stats stats = new Stats(int.Parse(s[i + 1]), int.Parse(s[i + 1]), int.Parse(s[i + 2]), 
                                        int.Parse(s[i + 3]), int.Parse(s[i + 4]));
                Abilities abil = new Abilities(abilityList);
                Monster mon = new Monster(s[i], stats, abil, i / 9);
                masterMonsterList.Add(mon);
            }

        }
        public static void saveData(PlayerCharacter player)
        {
            string filepath = "Assets\\" + player.name + "Data.dat";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filepath, FileMode.OpenOrCreate);
            
            SaveData data = new SaveData();
            data.p = player;
            bf.Serialize(file, data);
            file.Close();

        }
        public static PlayerCharacter loadData(string playerName)
        {
            string filepath = "Assets\\" + playerName + "Data.dat";
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
        public static Party createParty(string[] monsterNames)
        {
            List<Monster> mon = new List<Monster>();
            for(int i =0; i< monsterNames.Length; i++)
            {
                for(int j = 0; j<masterMonsterList.Count; j++)
                {
                    if (monsterNames[i].Equals(masterMonsterList[j].name))
                        mon.Add(masterMonsterList[j]);
                }
            }
            return new Party(mon);
        }
        public static Party createSequentialParty()
        {
            List<Monster> mon = new List<Monster>();
            for (int i = 0; i < masterMonsterList.Count && i < Party.getMaxPartySize(); i++)
            {
                mon.Add(masterMonsterList[i]);
            }
            return new Party(mon);
        }
        public static Monster randomMonster()
        {
            int randInt; 
            System.Random random = new System.Random();
            if (masterMonsterList.Count > 0)
                randInt = random.Next(0, masterMonsterList.Count - 1);
            else
                return null;
            return masterMonsterList[randInt];
        }
        public static Monster getMonster(string name)
        {
            for (int m = 0; m < masterMonsterList.Count; m++) {
                if (name.Equals(masterMonsterList[m].name))
                    return masterMonsterList[m];
            }
            return null;
        }
        public void setUpPredefinedParty()
        {
            player1.name = "Player1";
            player2.name = "Player2";

            Monster temp;
            temp = new Monster(masterMonsterList[0]);
            temp.levelUp(30);
            player1.party.addMonster(temp);
            //player1.party.addMonster(masterMonsterList[1]);
            //player1.party.addMonster(masterMonsterList[2]);
            temp = new Monster(masterMonsterList[1]);
            temp.levelUp(30);
            player2.party.addMonster(temp);
            //player2.party.addMonster(masterMonsterList[3]);
            //player2.party.addMonster(masterMonsterList[0]);
            //player2.party.addMonster(masterMonsterList[2]);
        }
        void Destroy()
        {

        }
    }
    
    [Serializable]
    class SaveData
    {
        public PlayerCharacter p;
    }
}