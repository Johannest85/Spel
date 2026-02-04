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
class Tournament
{
  private List<Character> participants;

    public Tournament(List<Character> participants) // Konstruktor som tar in deltagare
    {
        this.participants = participants;
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
        //Dela upp karaktärerna 2 och 2 i en turnering (random parning)
        ShuffleParticipants();
        for (int i = 0; i < participants.Count; i += 2)
        {
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
                //Hantera bye (automatiskt vidare till nästa runda)
            }
        }
    }

    private void Fight(Character char1, Character char2)
    {
        Character attacker = char1; // 0 för char1, 1 för char2
        Character defender = char2;
        while (true)
        {
            bool isDraw = false; // Variabel för att hålla koll på oavgjort (om skada = 0 för båda)
            // - en attackerar, den andra försvarar varannan gång
            // char1 attackerar, char2 försvarar
            // - skriv ut karaktärerenas actions tillsammans med uppdaterad status
            int damage = Math.Max(0, (int)Math.Ceiling(attacker.OnAttack().attackPoints - defender.OnDefense().defensePoints));
            Console.WriteLine(attacker.OnAttack().message);
            Console.WriteLine(defender.OnDefense().message);    
            if (damage == 0 && isDraw)
            {
                Console.WriteLine("The fight ended in a draw!");
                char1.XP += 0.25;
                char2.XP += 0.25;
                return;
            }
            else if (damage == 0) isDraw = true; // Om det är första gången skadan är 0 så sätter vi isDraw till true, om det händer igen så är det oavgjort
            Console.WriteLine($"{attacker.Name} attacks {defender.Name} with {damage} damage!");
            defender.HP -= Math.Max(0, damage);
            if (defender.HP <= 0) 
            {
                Console.WriteLine($"{defender.Name} has been defeated!");
                attacker.XP += 0.50;
                string celebration = attacker.Race.Celebrations[new Random().Next(attacker.Race.Celebrations.Count)]; // Slumpmässigt firande från rasen
                Console.WriteLine($"{celebration}!");
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
        
            // - Endast försvarare kan ta skada i rundan
            // - Skada = attackpoäng - försvarspoäng (om skada < 0 så blir den 0)
            // - vinnande karaktär får 0.50 xp och firar med ett slumpmässigt firande från sin ras
            // - om det blir oavgjort (vad menas här, är det om skada = 0?) så får båda karaktärerna 0.25 xp och ingen firar
            // - Efter vunnen match så får
        // - en attackerar, den andra försvarar varannan gång
    }
    
//     - skriv ut karaktärerenas actions tillsammans med uppdaterad status
//     - Endast försvarare kan ta skada i rundan
//     - Skada = attackpoäng - försvarspoäng (om skada < 0 så blir den 0)
//     - vinnande karaktär får 0.50 xp och firar med ett slumpmässigt firande från sin ras
//     - om det blir oavgjort (vad menas här, är det om skada = 0?) så får båda karaktärerna 0.25 xp och ingen firar
//     - Efter vunnen match så får man tillbaka hp efter regler som vi satt upp själva (ex 80% av förlorad hp)

}


/*
    private List<Character> participants;

    public Tournament(List<Character> participants) // Konstruktor som tar in deltagare
    {
        this.participants = participants;
    }

    public void StartTournament()
    {
        Random rand = new Random();
        List<Character> currentRound = new List<Character>(participants);

        // Loop för att köra turneringen tills en vinnare är utsedd
        while (currentRound.Count > 1)
        {
            List<Character> nextRound = new List<Character>();
            for (int i = 0; i < currentRound.Count; i += 2)
            {
                if (i + 1 < currentRound.Count)
                {
                    Character winner = Fight(currentRound[i], currentRound[i + 1], rand);
                    nextRound.Add(winner);
                }
                else
                {
                    nextRound.Add(currentRound[i]); // Bye
                }
            }
            currentRound = nextRound;
        }

        Console.WriteLine($"The tournament winner is {currentRound[0].Name}!");
    }
    private Character Fight(Character char1, Character char2, Random rand)
    {
        Console.WriteLine($"\nFight between {char1.Name} and {char2.Name} begins!");

        while (char1.HP > 0 && char2.HP > 0)
        {
            AttackRound(char1, char2);
            if (char2.HP <= 0) break;
            AttackRound(char2, char1);
        }

        Character winner, loser;
        if (char1.HP > 0)
        {
            winner = char1;
            loser = char2;
        }
        else
        {
            winner = char2;
            loser = char1;
        }

        winner.XP += 0.50;
        string celebration = winner
    }
}
*/