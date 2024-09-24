using System;
using System.Threading;

namespace DiceGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till spelet 21!");
            Console.WriteLine("");

            Random ran = new Random();

            // Variabler
            int playerNumber = 0;
            int dealerNumber = 0;
            bool playerStopped = false;
            bool dealerStopped = false;
            bool isPlaying = true;
            int playerCash = 500;
            int moneyBetted = 0;

            while (isPlaying)
            {
                // Återställ variablerna för ny omgång
                playerNumber = 0;
                dealerNumber = 0;
                playerStopped = false;
                dealerStopped = false;
                moneyBetted = 0;

                // Satsa pengar
                Console.WriteLine("Du har: " + playerCash + " pengar, hur mycket vill du satsa?");
                while (moneyBetted == 0)
                {
                    BetMoney(ref moneyBetted, ref playerCash);
                }

                // Spelarens tur
                while (playerNumber < 22 && dealerNumber < 22)
                {
                    // Om både spelaren och dealern har stannat
                    if (playerStopped && dealerStopped)
                    {
                        Winner(ref dealerStopped, ref playerStopped, ref playerNumber, ref dealerNumber, ref playerCash, ref moneyBetted);
                        PlayAgain(ref isPlaying);
                        break;
                    }

                    // Spelarens val
                    if (!playerStopped)
                    {
                        Console.WriteLine("Vill du slå eller stanna?");
                        string answer = Console.ReadLine();

                        if (answer.ToLower() == "slå")
                        {
                            int diceRoll = ran.Next(1, 7);
                            PlayerRoll(ref diceRoll, ref playerStopped, ref playerNumber);

                            if (playerStopped == false && dealerNumber < 18 && !dealerStopped)
                            {
                                diceRoll = ran.Next(1, 7);
                                dealerNumber += diceRoll;
                                Console.WriteLine("Dealern slog: " + diceRoll + ", hans poäng är: " + dealerNumber);

                                // Kontrollera om dealern ska stanna
                                if (dealerNumber >= 18 && dealerNumber <= 21)
                                {
                                    dealerStopped = true;
                                    Console.WriteLine("Dealern har stannat med poäng: " + dealerNumber);
                                }
                            }
                        }
                        else if (answer.ToLower() == "stanna")
                        {
                            playerStopped = true;
                            Console.WriteLine("Du har valt att stanna.");
                        }
                    }

                    if (playerStopped && !dealerStopped && dealerNumber < 18)
                    {
                        Thread.Sleep(1000);
                        int diceRoll = ran.Next(1, 7);
                        dealerNumber += diceRoll;
                        Console.WriteLine("Dealern slog: " + diceRoll + ", hans poäng är: " + dealerNumber);

                        if (dealerNumber >= 18 && dealerNumber <= 21)
                        {
                            dealerStopped = true;
                            Console.WriteLine("Dealern har stannat med poäng: " + dealerNumber);
                        }
                        else if (dealerNumber > 21)
                        {
                            Console.WriteLine("Dealern gick över 21, du vann!");
                            playerCash += (moneyBetted * 2);
                            PlayAgain(ref isPlaying);
                            break;
                        }
                    }

                    // Om någon fick exakt 21
                    if (playerNumber == 21)
                    {
                        Console.WriteLine("\n Du fick 21, du vann! (+" + (moneyBetted * 2) + " pengar)");
                        playerCash += (moneyBetted * 2);
                        PlayAgain(ref isPlaying);
                        break;
                    }
                    else if (dealerNumber == 21)
                    {
                        Console.WriteLine("\n Dealern fick 21, han vann! (-" + moneyBetted + " pengar)");
                        PlayAgain(ref isPlaying);
                        break;
                    }
                }

                // Om någon går över 21
                if (playerNumber > 21)
                {
                    Console.WriteLine("Du gick över 21, dealern vann! (-" + moneyBetted + " pengar)");
                    PlayAgain(ref isPlaying);
                }
                else if (dealerNumber > 21)
                {
                    Console.WriteLine("Dealern gick över 21, du vann! (+" + moneyBetted * 2 + " pengar)");
                    playerCash += (moneyBetted * 2);
                    PlayAgain(ref isPlaying);
                }
            }

            // Spelar tärning funktionen
            static void PlayerRoll(ref int dice, ref bool stopPlayer, ref int playerNumber)
            {
                if (!stopPlayer)
                {
                    playerNumber += dice;
                    Console.WriteLine("Du slog: " + dice + ", din poäng är: " + playerNumber);
                }
                else
                {
                    Console.WriteLine("Din poäng är: " + playerNumber + " och du har stannat.");
                }
            }

            // Vem som vinner
            static void Winner(ref bool stopDealer, ref bool stopPlayer, ref int playerNumber, ref int dealerNumber, ref int playerMoney, ref int moneyBet)
            {
                Console.WriteLine("\n Båda har stannat.");
                Console.WriteLine("Du fick: " + playerNumber + "\nDealern fick: " + dealerNumber);

                if (dealerNumber > playerNumber)
                {
                    Console.WriteLine("Dealern vann, du förlorade: " + moneyBet + " pengar.");
                }
                else if (playerNumber > dealerNumber)
                {
                    Console.WriteLine("Du vann och du fick: " + (moneyBet * 2) + " pengar.");
                    playerMoney += (moneyBet * 2);
                }
                else
                {
                    Console.WriteLine("Det blev lika, du fick tillbaks dina pengar.");
                    playerMoney += moneyBet;
                }
                moneyBet = 0;
            }

            // Spela igen funktionen
            static void PlayAgain(ref bool isPlaying)
            {
                Console.WriteLine("Vill du spela igen? [Ja, Nej]");
                string ans = Console.ReadLine();

                if (ans.ToLower() == "ja")
                {
                    isPlaying = true;
                }
                else if (ans.ToLower() == "nej")
                {
                    isPlaying = false;
                }
            }

            // Satsa pengar funktionen
            static void BetMoney(ref int moneyBetted, ref int playerCash)
            {
                while (moneyBetted == 0)
                {
                    string reply = Console.ReadLine();

                    if (int.Parse(reply) <= playerCash)
                    {
                        moneyBetted = int.Parse(reply);
                        playerCash -= moneyBetted;
                        Console.WriteLine("Du har satsat: " + moneyBetted + " pengar.");
                    }
                    else
                    {
                        Console.WriteLine("Du har inte så mycket pengar.");
                    }
                }
            }
        }
    }
}