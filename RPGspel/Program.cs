using System.Security;

class Client
{
    static void Main(string[] args)
    {
        Character player = new Jedi("Yoda", new Alien());
        Character player2 = new Jedi("Yoda2", new Elf());
        Character player3 = new Warrior("Conan", new Orc());
        Character player4 = new Mage("Gandalf", new Fairy());
        Character player5 = new Archer("Legolas", new Elf());
        List<Character> participants = new List<Character> { player, player2, player3, player4, player5 };
        Tournament tournament = new Tournament(participants, 0.5, 80, 0);
        tournament.StartTournament();   

    }
}


