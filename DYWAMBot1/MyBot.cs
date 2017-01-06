﻿using Discord;
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
                "!helloAlt",
                "!meme ",
                "!sharpshooter ",
                "!diceRoll ",
                "!vote "
            };

            memes = new string[]
            {
                "images/meme1.jpg"
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
                await e.Channel.SendTTSMessage("Gin alt! Get down! @ pause. @ pause. @ pause. @.... " + rand.Next(1, 5) + " shots hit!");
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