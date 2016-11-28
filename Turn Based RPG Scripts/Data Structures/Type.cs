using System;

public class Type{
    public static string[] typeNames = { "Normal", "Fire", "Water", "Earth" };
    public string name;

    public Type()
    {
        name = "Normal";
    }
    public Type(string name)
    {
        this.name = name;
    }
    public Type(Type t)
    {
        name = t.name;
    }
    public double powerModifierAgainst(Type defender)
    {
        if (reducedPowerAgainst(defender))
            return .5;
        else if (increasedPowerAgainst(defender))
            return 2;
        else
            return 1;
    }
    public bool reducedPowerAgainst(Type defender)
    {
        if (name.Equals("Fire") && defender.name.Equals("Water"))
            return true;
        else if (name.Equals("Water") && defender.name.Equals("Earth"))
            return true;
        else if (name.Equals("Earth") && defender.name.Equals("Fire"))
            return true;
        else
            return false;
    }
    public bool increasedPowerAgainst(Type defender)
    {
        if (name.Equals("Fire") && defender.name.Equals("Earth"))
            return true;
        else if (name.Equals("Water") && defender.name.Equals("Fire"))
            return true;
        else if (name.Equals("Earth") && defender.name.Equals("Water"))
            return true;
        else
            return false;
    }
}
