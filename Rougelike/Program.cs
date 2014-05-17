using System;

namespace Rougelike
{

    class Program
    {

        static void Main(string[] args)
        {
            var game = new GameLogic.RLGame();

            game.SetUp();

            //place player

            //turn this into a TakeInitialTurn method? Or Setup?
            //I could put all the above setup code in there as well (although it's not likely to stay there).
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            bool gameOver = false;
            //listen for input and handle moves
            do
            {
                keyInfo = Console.ReadKey(true);

                gameOver = game.ProcessInput(keyInfo);
            } while (!gameOver);

            //end game
            
            Console.ReadLine();
        }
    }
}
