/*
Skapa ett gäng karaktärer
om det är ojämnt antal så får den sista karaktären en "bye" och går vidare automatiskt
varja strid ska gå i rundor
    - en attackerar, den andra försvarar varannan gång
    - skriv ut karaktärerenas actions tillsammans med uppdaterad status
    - Endast försvarare kan ta skada i rundan
    - Skada = attackpoäng - försvarspoäng (om skada < 0 så blir den 0)
    - vinnande karaktär får 0.50 xp och firar med ett slumpmässigt firande från sin ras
    - om det blir oavgjort (vad menas här, är det om skada = 0?) så får båda karaktärerna 0.25 xp och ingen firar
    - Efter vunnen match så får man tillbaka hp efter regler som vi satt upp själva (ex 80% av förlorad hp)
    parametrar för turneringen ska inte vara hårdkodade! dvs vi behöver sätta in dem i konstruktorn

*/
using System.Numerics;

class Tournament
{
    private List<Character> participants;
    double xpGain; // Standard XP gain för vinnare
    int HealingPercentage; // Procent av förlorad HP som återställs efter varje runda   
    int drawLimit;

    private bool fightIsDraw;

    public Tournament(List<Character> participants, double xpGain, int HealingPercentage, int drawLimit) // Konstruktor som tar in deltagare
    {
        this.participants = participants;
        this.xpGain = xpGain;
        this.HealingPercentage = HealingPercentage;
        this.drawLimit = drawLimit;
    }  

    private void ShuffleParticipants() //Fisher-Yates shuffle (https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
    {
        Random rand = new Random();
        int n = participants.Count;
        for (int i = participants.Count-1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (participants[i], participants[j]) = (participants[j], participants[i]);
        }
    }

    public void StartTournament()
    {
        Console.WriteLine("Tournament begins!");
        int roundNo = 1;
        while (participants.Count > 1 && !(fightIsDraw && participants.Count == 2))
        {
            Console.WriteLine($"\n--- Round {roundNo} ---");
            Console.WriteLine($"Participants: {participants.Count}");
            foreach (var p in participants)
            {
                Console.WriteLine($"{p.ToString()}");
            }
            StartRound();
            roundNo++;
            //Efter varje runda kan vi ta bort de karaktärer som har förlorat
            participants = participants.Where(c => c.HP > 0).ToList();
            //Återställ HP för kvarvarande karaktärer
            if (participants.Count != 1 && HealingPercentage > 0 && !(fightIsDraw && participants.Count == 2))
            {
                Console.WriteLine("\n-- HP Recovery --");
                foreach (var p in participants)
                {
                    int lostHP = p.MaxHP - p.HP;
                    int recoverHP = (int)(lostHP * (HealingPercentage / 100.0));
                    p.HP += recoverHP;
                    Console.WriteLine($"{p.Name} recovers {recoverHP} HP and now has {p.HP} HP.");
                }
            }
        }
        if (fightIsDraw) Console.WriteLine($"\nTournament ends in a draw between {participants[0].Name} and {participants[1].Name}!");
        else Console.WriteLine($"\nTournament ends! The winner is {participants[0].Name}!");
    }

    private void StartRound()
    {
         //Dela upp karaktärerna 2 och 2 i en turnering (random parning)
        ShuffleParticipants();
        for (int i = 0; i < participants.Count; i += 2)
        {
            Console.WriteLine($"\n-- Fight {(i / 2) + 1} --");

            if (i + 1 < participants.Count)
            {
                Character char1 = participants[i];
                Character char2 = participants[i + 1];
                Console.WriteLine($"\nFight between {char1.Name} and {char2.Name} begins!");
                Fight(char1, char2);
                //Här kan vi kalla på en Fight-metod som hanterar striden mellan char1 och char2
            }
            else
            {
                Console.WriteLine($"\n{participants[i].Name} gets a bye to the next round!");
                fightIsDraw = false; // En bye kan inte vara oavgjord så vi sätter den till false här
            }
        }
    }

    private void Fight(Character char1, Character char2)
    {
        Character attacker = char1; // 0 för char1, 1 för char2
        Character defender = char2;
        int lastDamage = -1; // För se om det är oavgjort
        for (int round = 0; round < 100; round++) // Max 100 ronder för att undvika oändliga loopar
        {
            int damage = Math.Max(0, Math.Min((int)Math.Ceiling(attacker.OnAttack().attackPoints - defender.OnDefense().defensePoints), defender.HP)); // Beräkna skada men maximalt försvararens HP för att inte bli negativt
            Console.Write($"{attacker.OnAttack().message}    {defender.OnDefense().message}   Damage dealt: {damage}   ");
            
            defender.HP -= Math.Max(0, damage);
            if ((damage == 0 && lastDamage == 0) || (drawLimit <= round && drawLimit > 0))
            {
                Console.WriteLine("\n The fight ended in a draw!");
                if (char1.XP < 10-(xpGain/2)) char1.XP += xpGain / 2;
                else char1.XP = 10;
                if (char2.XP < 10-(xpGain/2)) char2.XP += xpGain / 2;
                else char2.XP = 10;
                fightIsDraw = true;
                return;
            }
            lastDamage = damage;
            if (defender.HP <= 0) 
            {
                Console.WriteLine($"\n{defender.Name} has been defeated!");
                if (attacker.XP < 10-xpGain) attacker.XP += xpGain;
                else attacker.XP = 10;
                string celebration = attacker.Race.Celebrations[new Random().Next(attacker.Race.Celebrations.Count)]; // Slumpmässigt firande från rasen
                Console.WriteLine($"{attacker.Name}: {celebration}!");
                fightIsDraw = false;
                return; // Avsluta metoden när en karaktär har förlorat
            }
            Console.WriteLine($"{defender.Name} has {defender.HP} HP left.");
            if (attacker == char1) 
            {
                attacker = char2;
                defender = char1;
            }
            else
            {
                attacker = char1;
                defender = char2;
            }
        }
    }
}