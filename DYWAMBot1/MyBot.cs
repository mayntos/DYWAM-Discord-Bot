using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DYWAMBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;
        Random rand;

        string[] memes;
        string[] randomGreetings;
        string[] randomGreetings2;
        string[] commandList;
        ArrayList betters = new ArrayList();

        // ArrayList<chatter> bettingList = new ArrayList<chatter>();

        Boolean roll1 = false;
        Boolean roll2 = false;
        string roller1 = "";
        string roller2 = "";

        /**
         * CONSTRUCTOR
         * Connects the bot to the channel and utilizes LogLevel to track connection issues.
         */
        public MyBot()
        {
            rand = new Random();


            commandList = new string[]
            {
                "!hello ",
                "!helloAlt ",
                "!meme ",
                "!sharpshooter ",
                "!roll ",
                "!diceRoll ",
                "!vote "
            };

            memes = new string[]
            {
                "images/meme1.jpg",
                "images/meme2.jpg",
                "images/meme3.jpg",
                "images/meme4.jpg",
                "images/meme5.jpg",
                "images/meme6.jpg"
            };

            randomGreetings = new string[]
            {
                "what's good, b.",
                "ayo!",
                "dam...Look who joined the chat...",
                "jesus christ. it's jason borne.",
               
            };

            randomGreetings2 = new string[]
            {
                "Hey Felix!",
                "How about that paper",
                "Did I ask?",
                "It's not my shift.",
            };




            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });


            //function which sets the String to tell bot that you're initiating a command. 
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;

            });


            commands = discord.GetService<CommandService>();

            registerHelp();
            registerMemeCommand();
            registerHelloCommand();
            registerHelloCommand2();
            registerJhinUlt();
            registerDieRoll();
            registerDiceRoll();
            registerVote();


            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjUwNzMzMjg0OTIzOTMyNjcz.CxZMPA.FPYnjWsiIPbyH3FhGZ50hZ32Kag", TokenType.Bot);
            });


        }
        //-----------------------------------------------------------------BEGINNING OF COMMANDS-----------------------------------------------------------------
        //After creating a command, do not forget to call it in MyBot's constructor. 
        /**
         * Creates !meme command. Picks from random Array of images.
         */

        private void registerHelp()
        {
            commands.CreateCommand("help")
            .Description("Displays all available commands.")
            .Do(async (e) =>
            {
               await e.Channel.SendMessage(String.Join("// ", commandList));

            });
        }
        private void registerMemeCommand()
        {
            commands.CreateCommand("meme")
                //when you call this command in Discord, it calls this function. Once. 
                .Description("Displays an image from DYWAM's meme library.")
                .Do(async (e) =>
                {
                    int randomMemeIndex = rand.Next(memes.Length);
                    string memeToPost = memes[randomMemeIndex];
                    await e.Channel.SendFile(memeToPost);
                });
        }

        /**
         * Creates !hello command. Picks from random Array of greetings. 
         */
        private void registerHelloCommand()
        {
            commands.CreateCommand("hello")
            .Description("Displays a greeting.")
            .Do(async (e) =>
            {
                int randomGreetingIndex = rand.Next(randomGreetings.Length);
                string greetingToPost = randomGreetings[randomGreetingIndex];
                await e.Channel.SendMessage(greetingToPost);
            });
        }

        /**
         * Creates !hello command. Picks from random Array2 of greetings. 
         */
        private void registerHelloCommand2()
        {
            commands.CreateCommand("helloAlt")
            .Description("Displays a greeting.")
            .Do(async (e) =>
            {
                int randomGreetingIndex = rand.Next(randomGreetings2.Length);
                string greetingToPost = randomGreetings2[randomGreetingIndex];
                await e.Channel.SendMessage(greetingToPost);
            });
        }

        /**
         * Creates !sharpshooter command. Jhin Ult, get down :^)
         */
        private void registerJhinUlt()
        {
            commands.CreateCommand("sharpshooter")
            .Description("Don't miss.")
            .Do(async (e) =>
            {
                await e.Channel.SendTTSMessage("Gin alt! Get down! @. @. @. @.... " + rand.Next(1, 5) + " shots hit!");
            });
           
        }

        /**
         * Creates !roll command. Requires one call to prompt a die toss.
         */
        private void registerDieRoll()
        {
            commands.CreateCommand("roll")
            .Description("Requires one call to prompt a die toss.")
            .Do(async (e) =>
            {
               string roller = e.User.Name;
                await e.Channel.SendMessage(roller + " rolled a " + rand.Next(1, 7) + ". ");
            });
        }

        /**
         * Creates !diceRoll command. Requires two successive calls of !diceRoll to play game. 
         */ 
        private void registerDiceRoll()
        {
            commands.CreateCommand("diceRoll")
            .Description("Requires two successive dice rolls to initiate a dice toss.")
            .Do(async (e) =>
            {
                if(roll1 == false)
                {
                    roll1 = true;
                    roller1 = e.User.Name;
                }
                else if(roll1 == true && roll2 == false)
                {
                    roll2 = true;
                    roller2 = e.User.Name;
                    await e.Channel.SendMessage(roller1 + " rolled a " + rand.Next(1, 7) + ". " + roller2 + " rolled a " + rand.Next(1, 7) + ".");
                    roll1 = false;
                    roll2 = false;
                    roller1 = "";
                    roller2 = "";
                }
            });
        }

        /**
         * Creates a !ranked command. Creates a "chatter" object which enters the user into a ranked queue. Ranked queue allows for wagering and tally count. 
         */
        private void registerRanked()
        {
            commands.CreateCommand("ranked")
            .Description("allows a user to enter the ranked queue")
            .Do(async (e) =>
            {
                if(betters.Contains(e.User.Name))
                {
                    await e.Channel.SendMessage(e.User.Name + " has already been added to the betting list.");
                    return;
                }
                
                betters.Add(e.User.Name);
                chatter p1 = new chatter(e.User.Name);
                await e.Channel.SendMessage(e.User.Name + " has been added to the betting list.");
                
            });
        }
      


        private void registerVote()
        {
            commands.CreateCommand("vote")
            .Description("Displays a Strawpoll link")
            .Do(async (e) =>
            {
                await e.Channel.SendMessage(e.User.Name + " wants to take a vote! " + "http://www.strawpoll.me/");
            });
        }


        //---------------------------------------------------------------------END OF COMMANDS---------------------------------------------------------------------

        /**
         * Prints error and sends to x.LogHandler(?)
         */
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}