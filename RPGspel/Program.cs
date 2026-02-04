using System.Security;

class Client
{
    static void Main(string[] args)
    {
        Character player = new Jedi("Yoda", new Alien());
        Character player2 = new Jedi("Yoda2", new Elf());
        
        Console.WriteLine($"{player.ToString()}");
        Console.WriteLine($"{player2.ToString()}");

        Tournament tournament = new Tournament(new List<Character> { player, player2 });
        tournament.StartTournament();   

    }
}


