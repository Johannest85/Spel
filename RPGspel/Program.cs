using System;
class Client
{
    static void Main(string[] args)
    {
        Elf elf = new Elf();
        Console.WriteLine($"Elf stats - Strength: {elf.strength}, Intelligence: {elf.intelligence}, Agility: {elf.agility}, MaxHP: {elf.maxHP}");
    }
}

class Race
{
    
   
    public int strength{ get; }
    public int intelligence { get; }
    public int agility { get; }
    public int maxHP { get; }
    protected string[] Celebration;

    protected Race ( int strength, int intelligence, int agility, int maxHP , string[] celebration)
    {
        
        this.strength = strength;
        this.intelligence = intelligence;
        this.agility = agility;
        this.maxHP = maxHP;
        Celebration = celebration;
    }




}


class Elf : Race
{
    public Elf() : base(4, 6, 7, 30, new string[] { "jippi" })
    {
        
    }

}

abstract class Charachter
{
    public string Name { get; }
    public Race Race { get; }
    private int hp;
    public int HP 
    { 
        get { return hp; } 
        set 
        { 
            if (value < 0 || value > Race.maxHP)
            { }
            hp = value;
        } 
    }

    private double xp;
    public double XP 
    { 
        get { return xp; } 
        set 
        { 
            if (value < 0 || value > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(XP), "XP must be between 0 and 10");
            }
            xp = value;
        } 
    }

    protected Charachter(string name, Race race, double xp)
    {
        Name = name;
        Race = race;
        XP = xp;
        hp = race.maxHP;
    }

    public abstract double OnAttack(Random rng);
    public abstract double OnDefense(Random rng);

    public override string ToString()
    {
        return $"Name: {Name}, Race: {Race.GetType().Name}, HP: {HP}, XP: {XP:F2}";
    }

}
