namespace DiceGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the game 21!");
            Console.WriteLine("");

            Random ran = new Random();
            // Variables
            int playerNumber = 0;
            int dealerNumber = 0;
            bool playerStopped = false;
            bool dealerStopped = false;
            bool isPlaying = true;
            int playerCash = 500;
            int moneyBetted = 0;

            while (isPlaying)
            {
                // Resetting the variables
                playerNumber = 0;
                dealerNumber = 0;
                playerStopped = false;
                dealerStopped = false;
                moneyBetted = 0;

                // betting money
                Console.WriteLine("You have: " + playerCash + " money, how much would you like to bet?");
                while (moneyBetted == 0)
                {
                    BetMoney(ref moneyBetted, ref playerCash);
                }

                // players turn
                while (playerNumber < 22 && dealerNumber < 22)
                {
                    if (playerStopped && dealerStopped)
                    {
                        Winner(ref dealerStopped, ref playerStopped, ref playerNumber, ref dealerNumber, ref playerCash, ref moneyBetted);
                        PlayAgain(ref isPlaying);
                        break;
                    }

                    // Players Choice
                    if (playerStopped == false)
                    {
                        Console.WriteLine("Hit or stay?");
                        string answer = Console.ReadLine();

                        if (answer.ToLower().Trim() == "hit")
                        {
                            int diceRoll = ran.Next(1, 7);
                            PlayerRoll(ref diceRoll, ref playerStopped, ref playerNumber);

                            if (playerStopped == false && dealerNumber < 18 && !dealerStopped)
                            {
                                diceRoll = ran.Next(1, 7);
                                dealerNumber += diceRoll;
                                Console.WriteLine("Dealer Hit: " + diceRoll + ", his points is: " + dealerNumber);

                                if (dealerNumber >= 18 && dealerNumber <= 21)
                                {
                                    dealerStopped = true;
                                    Console.WriteLine("The dealer has stopped with points: " + dealerNumber);
                                }
                            }
                        }
                        else if (answer.ToLower().Trim() == "stay")
                        {
                            playerStopped = true;
                            Console.WriteLine("You have chosen to stay.");
                        }
                    }

                    if (playerStopped && !dealerStopped && dealerNumber < 18)
                    {
                        Thread.Sleep(1000);
                        int diceRoll = ran.Next(1, 7);
                        dealerNumber += diceRoll;
                        Console.WriteLine("Dealer Hit: " + diceRoll + ", his points is: " + dealerNumber);

                        if (dealerNumber >= 18 && dealerNumber <= 21)
                        {
                            dealerStopped = true;
                            Console.WriteLine("Dealer has stopped with points: " + dealerNumber);
                        }
                        else if (dealerNumber > 21)
                        {
                            Console.WriteLine("Dealer went over 21, you won!");
                            playerCash += (moneyBetted * 2);
                            PlayAgain(ref isPlaying);
                            break;
                        }
                    }

                    // If someone exactly gets 21
                    if (playerNumber == 21)
                    {
                        Console.WriteLine("\n you got 21, you won! (+" + (moneyBetted * 2) + " money)");
                        playerCash += (moneyBetted * 2);
                        PlayAgain(ref isPlaying);
                        break;
                    }
                    else if (dealerNumber == 21)
                    {
                        Console.WriteLine("\n Dealer got 21, he won! (-" + moneyBetted + " money)");
                        PlayAgain(ref isPlaying);
                        break;
                    }
                }

                // if someone goes over 21
                if (playerNumber > 21)
                {
                    Console.WriteLine("you went over 21, dealer won! (-" + moneyBetted + " money)");
                    PlayAgain(ref isPlaying);
                }
                else if (dealerNumber > 21)
                {
                    Console.WriteLine("dealer went over 21, you won! (+" + moneyBetted * 2 + " money)");
                    playerCash += (moneyBetted * 2);
                    PlayAgain(ref isPlaying);
                }
            }

            // Players dice functions
            static void PlayerRoll(ref int dice, ref bool stopPlayer, ref int playerNumber)
            {
                if (!stopPlayer)
                {
                    playerNumber += dice;
                    Console.WriteLine("You Hit: " + dice + ", your points is: " + playerNumber);
                }
                else
                {
                    Console.WriteLine("Your Points is: " + playerNumber + " and you have stayed.");
                }
            }

            // Who wins
            static void Winner(ref bool stopDealer, ref bool stopPlayer, ref int playerNumber, ref int dealerNumber, ref int playerMoney, ref int moneyBet)
            {
                Console.WriteLine("\n Both have stayed.");
                Console.WriteLine("You got: " + playerNumber + "\nDealer got: " + dealerNumber);

                if (dealerNumber > playerNumber)
                {
                    Console.WriteLine("Dealer won, you lost: " + moneyBet + " money.");
                }
                else if (playerNumber > dealerNumber)
                {
                    Console.WriteLine("You won and you got: " + (moneyBet * 2) + " money.");
                    playerMoney += (moneyBet * 2);
                }
                else
                {
                    Console.WriteLine("It was a draw, you got your money back.");
                    playerMoney += moneyBet;
                }
                moneyBet = 0;
            }

            // Play again function
            static void PlayAgain(ref bool isPlaying)
            {
                Console.WriteLine("Would you like to play again? [Yes, No]");
                string ans = Console.ReadLine();

                if (ans.ToLower().Trim() == "yes")
                {
                    isPlaying = true;
                }
                else if (ans.ToLower().Trim() == "no")
                {
                    isPlaying = false;
                }
            }

            // Betting money function
            static void BetMoney(ref int moneyBetted, ref int playerCash)
            {
                while (moneyBetted == 0)
                {
                    string reply = Console.ReadLine();

                    if (int.Parse(reply) <= playerCash)
                    {
                        moneyBetted = int.Parse(reply);
                        playerCash -= moneyBetted;
                        Console.WriteLine("You have bet: " + moneyBetted + " money.");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough money.");
                    }
                }
            }
        }
    }
}