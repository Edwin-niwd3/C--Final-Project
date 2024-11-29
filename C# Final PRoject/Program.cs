using System;
using System.Dynamic;

namespace MyApp
{
  class Game()
  {
    private bool catCheck {get; set;}
    private char playerChoice {get;set;}
    //This is going to be a 2d array
    //Turns out 2d Arrays have to be the same size for each portion, which sucks this will be an extremely dumb design...
    private string[,] Lines = {
      //When the string says: Ask choice <- then we will put it to the user to allow them to choose their option
      //When a NAN is present, the scene has finished, move on
      //This first part of the array will be all the first scene prolouge lines, so this would be [0][0-23]
      {"Press Any key to start!", "The night sky seems to extend indefinitly, with no end in sight", "\'ring ring\'", "William:\nHey man, how's it been?", "William:\nI'm calling because I wanted to catch up with whats been going on.", "William:\nRecently, I just got promoted at my job, and I'm making some good money!", "Oliver:\nOh yeah? Congrats. Sadly  I just got let go from my previous job as a teacher at La Verne High, I was an English Teacher", "William:\nOh damn. Really man? That sucks to hear.", "Oliver:\nYeah they said ChatGPT could do a better job than I could, and let me go", "William:\n Ah well, hopefully you can get another job soon. But for some good news, I just proposed to my girlfriend and she said yes!", "Oliver:\nHey thats amazing man! I'm glad to hear that things are looking up for you.", "Oliver:\nWell, it'sa been nice talking man, talk to you later.", "William:\n Yeah man, see ya.", "Oliver:\n...", "Oliver:\n...", "Oliver:\n God I wish things could have worked out for me too. Why does it have to be this way...", "Some time passes...", "Oliver:\nOh no... How am I gonna make this month's rent?!? I can't believe I still haven't found a job...", "A card slips beneath his door", "Oliver:\nWhats this? La Verne Casino.... Maybe I can make some quick cash", "You grab your essentials and makes his way to his car", "Oliver:\nIs this really a good idea? Should I really be trying this?", "Oliver:\nNah, this is a great idea! I'll become a fu**ing millionaire and NEVER have to worry about rent again!!!"},
      //The prolouge has finished, this is now Oliver at the Casino scenes, which are [1][0-23]
      {"The bright flashing lights blind you as you approach the casino", "Oliver:\nI guess this is the casino...", "Oliver:\nHey, this is La Verne Casino right?", "Bouncer:\n...", "Oliver:\nI guess it's not... maybe I should just go home...", "Bouncer:\nPass.", "Oliver:\nO-oh...ok (Should I really be doing this?)", "Attendant:\nHello there, and welcome to La Verne Casino. What would you like to play?", "Ask choice", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN"},
      //These are all the lines that can be said by NPC's (Reuben is gonna make the lines hopefully...) 

      };
  }
  internal class Program
  {
      static void Main(string[] args)
      {
          Console.WriteLine("Hello World!");
      }
  }
}