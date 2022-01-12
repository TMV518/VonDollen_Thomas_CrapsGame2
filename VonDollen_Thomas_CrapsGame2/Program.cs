using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

/* This program allows the user to simulate multiple craps games at once
 * After the games are played, the results are displayed to the user
 * through GUI pop-ups. The user is able to view the percentage of wins
 * and losses and the results of each game through GUI pop-ups.
 */

namespace VonDollen_Thomas_CrapsGame2
{
    class Game 
    {
        int gameNum;
        int numRolls;
        bool win;           //if false, that means game was lost
        string results = "";
        bool alreadyPrinted = false; //I call ToString() more than once, this prevents duplicate string concatenation

        public Game(int num, int rolls, bool win)
        {
            this.numRolls = rolls;
            this.gameNum = num;
            this.win = win;

        }

        public override string ToString() //puts together string used for each game window
        {
            
            if (!alreadyPrinted)
            {
                results += "Game number:\t" + gameNum + "\nNumber of rolls:\t" + numRolls;

                if (this.win) //if win == true
                {
                    results += "\n\nYou won this game";

                }
                else
                {
                    results += "\n\nYou lost this game";
                }
 

                alreadyPrinted = true;  //stops it from printing again
            }
            
            
            return results;
        }
    }

    class Program
    {
        

        static void Main(string[] args)
        {
            int gameNum = 0;
            

            ArrayList allGames = new ArrayList();

            Random rand = new Random();

            Console.WriteLine("What is your name? ");
            string name = Console.ReadLine();
            Console.WriteLine("How many games would you like to play? ");
            string numGames = Console.ReadLine();

            while (!int.TryParse(numGames, out int outVal) || int.Parse(numGames) <= 0) //checking if input is integer greater than 0
            {
                Console.WriteLine("Invalid input, please enter a number greater than 0: ");
                numGames = Console.ReadLine();
            }

            float totalGames = int.Parse(numGames); //used for calculating percentages later
            int games = int.Parse(numGames); //parsing numGames variable

            int numWins = 0;    //total number of wins and loses
            int numLoses = 0;

            int winStreak = 0;  //current winning and losing streaks
            int loseStreak = 0;

            int longestWin = 0; //longest winning and losing streaks
            int longestLose = 0;

            

            while (games > 0)               //All games run in this loop
            {
                bool win = false;
                int rolls = 0;              //number of rolls starts at 0 for each game

                int sum = Roll(ref rolls);  //First roll of game
                

                if (sum == 7 || sum == 11) //WIN
                {
                    Win(ref win);
                    
                }
                else if (sum == 2 || sum == 3 || sum == 12) //LOSE
                {
                    Lose(ref win);
                    
                }
                else
                {
                    int estabNum = sum;     //instsantiating established number
                    bool gameOver = false;

                    while (!gameOver)       //keep rolling until player rolls 7 or established number
                    {
                        sum = Roll(ref rolls);  //roll again

                        if (sum == 7) //LOSE
                        {
                            Lose(ref win);
                            
                            gameOver = true;
                        }
                        else if (sum == estabNum) //WIN
                        {
                            Win(ref win);
                            
                            gameOver = true;
                        }
                        else
                        {
                            //GO BACK UP TO TOP OF LOOP

                        }
                    }
                }

                Game thisGame = new Game(gameNum, rolls, win); //making Game object
                allGames.Add(thisGame);                   //saving it in an ArrayList

                games--;
            }

            /*foreach (object game in allGames)
            {

                Console.WriteLine(game.ToString());
            }*/

            float percentWins = (numWins / totalGames) * 100;
            float percentLoses = (numLoses / totalGames) * 100;

            //Result string used for result window
            string resultStr = "\nNumber of wins: \t\t" + numWins + "\nNumber of loses: \t\t" + numLoses
                + "\nPercent of wins: \t\t" + percentWins + "%" + "\nPercent of loses: \t\t" + percentLoses + "%"
                + "\nLongest winning streak: \t" + longestWin + "\nLongest losing streak: \t" + longestLose +
                "\n\n Press OK and return to the console to find results for each game";

            ResultWindow(numWins, numLoses, resultStr, name);   //method that creates window


            Console.WriteLine("\nType the number of a game to see its results (Enter to quit): ");
            string resultGame = Console.ReadLine();

            /*LOOP FOR INDIVIDUAL GAME STATS
             * Not the cleanest way, but it works*/
            while (!String.IsNullOrEmpty(resultGame) && int.TryParse(resultGame, out int output) && int.Parse(resultGame) <= allGames.Count && int.Parse(resultGame) > 0)
            {

                GameWindow(int.Parse(resultGame), allGames);

                Console.WriteLine("Type the number of a game to see its results (Enter to quit): ");
                resultGame = Console.ReadLine();

            }


            int Roll(ref int rolls) //initialize 2 random numbers and add them together
                                    //keep track of the number of rolls per game
            {
                rolls++;
                int die1 = rand.Next(1, 7);
                int die2 = rand.Next(1, 7);

                return die1 + die2;
            }

            void Win(ref bool win)  //what happens when the player wins
            {
                numWins++;
                winStreak++;

                if (longestLose < loseStreak)
                {
                    longestLose = loseStreak;
                }

                loseStreak = 0;
                gameNum++;

                win = true;
            }

            void Lose(ref bool win) //What happens when the player loses
            {
                numLoses++;
                loseStreak++;

                if (longestWin < winStreak)
                {
                    longestWin = winStreak;
                }

                winStreak = 0;
                gameNum++;
                win = false;
            }

        }


        static void ResultWindow(int wins, int loses, string results, string name) //creates window based on results
        {

            if (wins > loses)
            {
                MessageBox.Show("Congrats, " + name + ". You're a winner!\n" + results,
                "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (wins == loses)
            {
                MessageBox.Show("You've won just as many games as you've lost.\n" + results,
                    "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Sorry, " + name + ". You lost. Maybe fix your gambling problem...\n" + results,
                    "Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static void GameWindow(int game, ArrayList allGames)
        {
            
            MessageBox.Show(allGames[game - 1].ToString(), "Game " + game, MessageBoxButtons.OK);
        }
    }
}