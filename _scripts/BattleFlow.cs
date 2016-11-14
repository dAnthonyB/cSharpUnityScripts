using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets._scripts
{
    public class BattleFlow : MonoBehaviour {

        public List<characterbehaviour> actors1;
        public List<characterbehaviour> actors2;
        
        //added for easy debug
        public List<Monster> masterML = DataClass.masterMonsterList;
        public PlayerCharacter Player1 = DataClass.player1;
        public PlayerCharacter Player2 = DataClass.player2;

        public characterbehaviour active_mem;
        public characterbehaviour enemy;
        
        public Text Attack_main;
        public Text Party_main;
        public Text Item_main;
        public Text Run_main;
        public Text Attack_abil1;
        public Text Attack_abil2;
        public Text Attack_abil3;
        public Text Attack_abil4;
        public Text Ret_button;
        public Text Right_damage;
        public Text Left_damage;
        public Text Win_text;
        public Button Ext;
        public Button Atk;
        public Button Prt;
        public Button Item;
        public Button Run;
        public Button Abil1;
        public Button Abil2;
        public Button Abil3;
        public Button Abil4;
        public Button Ret;
        public Button Leave;
        public Button Enter;

        public Text active_mon_health;
        public Text active_mon_name;
        public Text enemy_name;
        public Text enemy_health;

        private string activeMenu;

        protected Vector3 up_vec = new Vector3(0, 75, 0);
        protected Vector3 down_vec = new Vector3(0, -75, 0);

        private bool turn = true;
        private bool win = false;
        private bool lose = false;

        void Start () {
            active_mem = actors1[DataClass.player1.party.monsterList[0].idx];
            enemy = actors2[DataClass.player2.party.monsterList[0].idx];
            Win_text.text = "";
            Right_damage.text = "";
            Left_damage.text = "";
            baseMenu();
            active_mem.setAction("enter");
            enemy.setAction("enter");
	    }
        // Update is called once per frame
        void Update()
        {
            if (!win && !lose)
            {
                if (active_mem.owner.partyKOd())
                {
                    lose = true;
                }

                if (enemy.owner.partyKOd())
                {
                    win = true;
                }

                if (!turn)
                {
                    EnemyTurn();
                    turn = true;
                }
                setHud();
            
                switch (activeMenu)
                {
                    case "main":
                        Attack_main.text = "Attack";
                        Party_main.text = "Party";
                        Item_main.text = "Item";
                        Run_main.text = "Run";
                        break;
                    case "attack":
                        Left_damage.text = "";
                        Right_damage.text = "";
                        Attack_abil1.text = active_mem.mon.abilities.abilityList[0].name;
                        Attack_abil2.text = active_mem.mon.abilities.abilityList[1].name;
                        Attack_abil3.text = active_mem.mon.abilities.abilityList[2].name;
                        Attack_abil4.text = active_mem.mon.abilities.abilityList[3].name;
                        Ret_button.text = "Return";
                        break;
                }
            }
            if (win && !lose)
            {
                Win_text.text = "Player is Victorious";
            }
            else if (lose && !win)
            {
                Win_text.text = "Player Loses";
            }
            else if (lose && win)
                Win_text.text = "You tie";
        }
        //sets up base Menu
        void baseMenu()
        {
            activeMenu = "main";
            Atk.transform.Translate(up_vec);
            Prt.transform.Translate(up_vec);
            Item.transform.Translate(up_vec);
            Run.transform.Translate(up_vec);
        }

//Determines which menu to switch to from main menu
        public void MainMenuSwap(string name)
        {
            Atk.transform.Translate(down_vec);
            Prt.transform.Translate(down_vec);
            Item.transform.Translate(down_vec);
            Run.transform.Translate(down_vec);
            switch (name)
            {
                case "attack": attackMenu();  break;
                case "party": partyMenu();  break;
                case "item": break;
                case "run": break;
            }
        }

//Sets up menu after party is selected
        public void partyMenu()
        {
            Enter.transform.Translate(up_vec);
            Leave.transform.Translate(up_vec);
        }

//Determines which action to take from party menu
        public void partyMenuSwap(int idx)
        {
            active_mem.setAction("leave");
            if (idx < DataClass.player1.party.monsterList.Capacity)
                active_mem = actors1[DataClass.player1.party.monsterList[idx].idx];
            else
                Debug.Log("Party Swap Index out of range");
            active_mem.setAction("enter");
            Enter.transform.Translate(down_vec);
            Leave.transform.Translate(down_vec);

        }

//Determines which action to take from attack menu
        public void attackMenuSwap(int choice)
        {
            if(choice < 4)
            {
                if (!active_mem.mon.abilities.abilityList[choice].name.Equals("none"))
                {
                    moveAbilityText(false);
                    executeAttack(active_mem.mon.abilities.abilityList[choice]);
                    baseMenu();
                }
            }
            else
            {
                moveAbilityText(false);
                baseMenu();
            }
        }

//Sets up menu after attack is selected
        void attackMenu()
        {
            activeMenu = "attack";
            moveAbilityText(true);
            Ret_button.text = "Return";
        }

        //method that executes the follow of choosing an attack and doing said attack
        void executeAttack(Ability abil)
        {
            characterbehaviour attacker, defender;
            int power = attackPower(abil);
            if (power > 0)
            {
                if (turn)
                {
                    attacker = active_mem;
                    defender = enemy;
                    Left_damage.text = "" + power;
                }
                else
                {
                    attacker = enemy;
                    defender = active_mem;
                    Right_damage.text = "" + power;
                }
                attacker.setAction(abil.name);
                defender.setAction("damage");
                
                power = 0 - power;
                defender.mon.stats.changeStat("Current HP", power);

                checkFaint(defender);
            }
            turn = !turn;
        }

        void checkFaint(characterbehaviour defender)
        {
            if(defender.mon.KOd())
            {
                Ext.transform.Translate(down_vec);
                defender.setAction("faint");
            }
        }

        //Allows player to choose the ability to be used
        Ability chooseAbility(int choice)
        {
            Ability retAbil = null;
            if (turn)
            {
                retAbil = active_mem.mon.abilities.abilityList[choice];
            }
            else
            {
                int idx = Random.Range(0, enemy.mon.abilities.numAbilities());
                retAbil = enemy.mon.abilities.abilityList[idx];
            }
            return retAbil;
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
                return (ability.power * active_mem.mon.stats.getStatValue("Attack")*10) / enemy.mon.stats.getStatValue("Defense");
            else
                return (ability.power * enemy.mon.stats.getStatValue("Attack")*10) / active_mem.mon.stats.getStatValue("Defense");
        }

        public void EnemyTurn()
        {
            executeAttack(chooseAbility(0));
        }
        
        void setHud()
        {
            active_mon_name.text = active_mem.mon.name;
            active_mon_health.text = "HP: " + active_mem.mon.stats.getStatValue("Current HP").ToString() + "/" + active_mem.mon.stats.getStatValue("Max HP").ToString();
            enemy_name.text = enemy.mon.name;
            enemy_health.text = "HP: " + enemy.mon.stats.getStatValue("Current HP").ToString() + "/" + enemy.mon.stats.getStatValue("Max HP").ToString();
        }

        void moveAbilityText(bool up)
        {
            if (up)
            {
                Abil1.transform.Translate(up_vec);
                Abil2.transform.Translate(up_vec);
                Abil3.transform.Translate(up_vec);
                Abil4.transform.Translate(up_vec);
                Ret.transform.Translate(up_vec);
            }
            else
            {
                Abil1.transform.Translate(down_vec);
                Abil2.transform.Translate(down_vec);
                Abil3.transform.Translate(down_vec);
                Abil4.transform.Translate(down_vec);
                Ret.transform.Translate(down_vec);
            }
        }

        public void exitGame()
        {
            DataClass.saveData(DataClass.player1);
            DataClass.saveData(DataClass.player2);
            Application.Quit();
        }
    }
}

