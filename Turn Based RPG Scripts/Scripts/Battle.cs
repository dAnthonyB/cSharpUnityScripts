using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Battle : MonoBehaviour
    {
        public PlayerCharacter player1 = DataClass.player1;
        public PlayerCharacter player2 = DataClass.player2;

        public Monster player1ActiveMonster;
        public Monster player2ActiveMonster;
        public int numberOfChoices = 5;
        public Button[] choices;
        public Text winnerOrLoser;
        public Text Right_damage;
        public Text Left_damage;
        public Text active_mon_health;
        public Text active_mon_name;
        public Text enemy_name;
        public Text enemy_health;

        public string activeMenu;

        public bool turn = true;
        public bool[] winner = { false, false };

        void Start()
        {
            choices = GetComponentsInChildren<Button>();
            player1ActiveMonster = player1.party.monsterList[0];
            player2ActiveMonster = player2.party.monsterList[0];
            winnerOrLoser.text = "";
            Right_damage.text = "";
            Left_damage.text = "";
            changeMenu(activeMenu);
            setHudValues();
        }
        
        void Update()
        {
            if (turn == false && !winner[0] && !winner[1])
                EnemyTurn();
            else if (winner[0] && !winner[1])
            {
                endBattle();
                winnerOrLoser.text = player1.name + " is Victorious";
            }
                
            else if (!winner[0] && winner[1])
            {
                endBattle();
                winnerOrLoser.text = player1.name + " Loses";
            }
                
            else if (winner[0] && winner[1])
            {
                endBattle();
                winnerOrLoser.text = "You tie";
            }
                
            else
                winnerOrLoser.text = "BATTLE!"; 
        }

        //changes the current menu, removes any listeners on buttons and adds new listeners
        void changeMenu(string menuName)
        {
            activeMenu = menuName;
            int i;
            switch (activeMenu)
            {
                case "main":
                    string[] mainText = { "Attack", "Party", "Item", "Run" };
                    for (i = 0; i < mainText.Length; i++)
                        choices[i].GetComponentInChildren<Text>().text = mainText[i];
                    for (i = mainText.Length; i < choices.Length; i++)
                        choices[i].GetComponentInChildren<Text>().text = "";
                    break;
                case "attack":
                    for (i = 0; i < AbilityList.getAbilityLimit(); i++)
                        choices[i].GetComponentInChildren<Text>().text = player1ActiveMonster.abilities.abilityList[i].name;
                    choices[AbilityList.getAbilityLimit()].GetComponentInChildren<Text>().text = "Return";
                    break;
                case "party":
                    for (i = 0; i < player1.party.monsterList.Count; i++)
                        choices[i].GetComponentInChildren<Text>().text = player1.party.monsterList[i].name;
                    for (; i < Party.getMaxPartySize(); i++)
                        choices[i].GetComponentInChildren<Text>().text = "none";
                    choices[Party.getMaxPartySize()].GetComponentInChildren<Text>().text = "Return";
                    break;
                case "item":
                    for (i = 0; i < player1.itemList.Count; i++)
                        choices[i].GetComponentInChildren<Text>().text = player1.itemList[i].name;
                    for (; i < Item.maxNumItems; i++)
                    {
                        choices[i].GetComponentInChildren<Text>().text = "none";
                    }
                    choices[i].GetComponentInChildren<Text>().text = "Return";
                    break;
            }
            //adds new listeners to buttons
            for (i = 0; i < choices.Length; i++)
            {
                string x = choices[i].GetComponentInChildren<Text>().text;
                choices[i].onClick.RemoveAllListeners();
                choices[i].onClick.AddListener(() => actionOnClick(x));
            }
        }
        
        //Determines which menu to switch to from main menu
        void actionOnClick(string action)
        {
            if (turn && !winner[0] && !winner[1])
            {
                switch (action)
                {
                    case "":
                        return;
                    case "none":
                        return;
                    case "Run":
                        //need to implement
                        return;
                    case "Return":
                        changeMenu("main");
                        return;
                    case "Attack":
                        changeMenu("attack");
                        return;
                    case "Party":
                        changeMenu("party");
                        return;
                    case "Item":
                        changeMenu("item");
                        return;
                }
                if (activeMenu.Equals("party"))
                {
                    for (int i = 0; i < player1.party.monsterList.Count; i++)
                    {
                        if (player1.party.monsterList[i].name.Equals(action))
                        {
                            switchActiveMonster(action);
                            return;
                        }
                    }
                }
                if (activeMenu.Equals("attack"))
                {
                    for (int i = 0; i < player1ActiveMonster.abilities.abilityList.Count; i++)
                    {
                        if (player1ActiveMonster.abilities.abilityList[i].name.Equals(action))
                        {
                            executeAttack(player1ActiveMonster.abilities.abilityList[i]);
                            return;
                        }
                    }
                }
                if (activeMenu.Equals("item"))
                {
                    for (int i = 0; i < player1.itemList.Count; i++)
                    {
                        if (player1.itemList[i].name.Equals(action))
                        {
                            useItem(action);
                            return;
                        }
                    }
                }
            }
        }
        
        //switches player's monster
        void switchActiveMonster(string monsterName)
        {
            if (!player1.party.getMonster(monsterName).KOd())
            {
                player1ActiveMonster = player1.party.getMonster(monsterName);
                DataClass.output(player1.name + " switched to " + monsterName);
                endTurn();
            }
            else
                DataClass.output("monster is ko'd; can't switch.");
        }

        //either player can use an item
        void useItem(string action)
        {
            if (turn)
            {
                if (player1.hasItemAt(action) > -1)
                {
                    if (player1.getItem(action).numOwned > 0)
                    {
                        DataClass.output("using " + action);
                        player1ActiveMonster.addEffect(new Effect(player1.getItem(action).effect));
                        player1.getItem(action).numOwned--;
                        endTurn();
                        return;
                    }
                    DataClass.output("You don't have any " + action + "s");
                }
            }
            else
            {
                if (player2.hasItemAt(action) > -1)
                {
                    if (player2.getItem(action).numOwned > 0)
                    {
                        player2ActiveMonster.addEffect(player2.getItem(action).effect);
                        endTurn();
                    }
                }
            }
        }
        
        //method that executes the follow of choosing an attack and doing said attack
        void executeAttack(Ability abil)
        {
            Monster attacker, defender;
            int power = attackPower(abil);
            if (turn)
            {
                defender = player2ActiveMonster;
                attacker = player1ActiveMonster;
                Left_damage.text = "" + power;
            }
            else
            {
                defender = player1ActiveMonster;
                attacker = player2ActiveMonster;
                Right_damage.text = "" + power;
            }
            if (!attacker.KOd())
            {
                if (power > 0)
                {
                    defender.stats.changeCurrentStat("HP", -power);
                    checkKO();
                }
                else
                    power = 0;
                DataClass.output(attacker.name + "'s " + abil.name + " did " + power.ToString() + " damage to " + defender.name);
                endTurn();
            }
        }

        //Given a ceratin Ability, the power and whether the move hits is determined
        int attackPower(Ability ability)
        {
            if ((ability.accuracy) < Random.Range(1, 100))
            {
                if (turn)
                    Left_damage.text = "missed";
                else
                    Right_damage.text = "missed";
                return 0;
            }
            
            if (turn)
            {
                double multiplier = player1ActiveMonster.type.powerModifierAgainst(player2ActiveMonster.type);
                return (int)(ability.power * player1ActiveMonster.stats.getStatValue("Attack") * 10 * multiplier) / player2ActiveMonster.stats.getStatValue("Defense");
            }
            else if (!turn)
            {
                double multiplier = player2ActiveMonster.type.powerModifierAgainst(player1ActiveMonster.type);
                return (int)(ability.power * player2ActiveMonster.stats.getStatValue("Attack") * 10 * multiplier) / player1ActiveMonster.stats.getStatValue("Defense");
            }
            else
                Debug.Log("shouldn't be here; called from attackPower()");
            return -1;
        }

        //checks if defending monster has been KO'd
        void checkKO()
        {
            PlayerCharacter owner;
            Monster defender;
            if (turn)
            {
                owner = player2;
                defender = player2ActiveMonster;
            }
            else
            {
                owner = player1;
                defender = player1ActiveMonster;
            }
            if (defender.KOd())
            {
                DataClass.output(defender.name + " got KOd");
                if (owner.party.checkIfLoss())
                {
                    if (owner.number == 1)
                    {
                        winner[1] = true;
                        return;
                    }
                    else
                    {
                        winner[0] = true;
                        return;
                    }
                }
                else
                {
                    if (owner.number == 2)
                    {
                        for (int i = 0; i < owner.party.monsterList.Count; i++)
                        {
                            if (!owner.party.monsterList[i].KOd())
                            {
                                player2ActiveMonster = owner.party.monsterList[i];
                                return;
                            }
                        }
                    }
                    else
                    {
                        for(int i = 0; i < owner.party.monsterList.Count; i++)
                        {
                            if (!owner.party.monsterList[i].KOd())
                            {
                                player1ActiveMonster = owner.party.monsterList[i];
                                return;
                            }
                        }
                        changeMenu ("party");
                    }
                } 
            }
        }
        //choses random ablility from enemy monster
        public void EnemyTurn()
        {
            int temp = Random.Range(0, player2ActiveMonster.abilities.numAbilities()-1);
            executeAttack(player2ActiveMonster.abilities.abilityList[temp]);
            setHudValues();
        }
        void setHudValues()
        {
            active_mon_name.text = player1ActiveMonster.name;
            active_mon_health.text = "HP: " + player1ActiveMonster.stats.getCurrentStatValue("HP").ToString() + "/" + player1ActiveMonster.stats.getStatValue("HP").ToString();
            enemy_name.text = player2ActiveMonster.name;
            enemy_health.text = "HP: " + player2ActiveMonster.stats.getCurrentStatValue("HP").ToString() + "/" + player2ActiveMonster.stats.getStatValue("HP").ToString();
        }
        void endTurn()
        {
            setHudValues();
            changeMenu("main");
            turn = !turn;
            player1ActiveMonster.applyEffects();
            player2ActiveMonster.applyEffects();
        }
        void endBattle()
        {
           player1.party.resetParty();
           player2.party.resetParty();
        }
    }
}

