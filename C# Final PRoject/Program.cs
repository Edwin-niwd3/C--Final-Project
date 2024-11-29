using System;
using System.Dynamic;

namespace MyApp
{
  class Game()
  {
    private int Current_Money {get; set;}
    private bool Bad_Ending_flag {get;set;}
    private bool Battle_Ending_flag {get;set;}
    private bool Neutral_Ending_flag {get;set;}
    private bool catCheck {get; set;}
    private char playerChoice {get;set;}
    //This is going to be a 2d array
    //Turns out 2d Arrays have to be the same size for each portion, which sucks this will be an extremely dumb design...
    private string[,] Prolouge_Lines = {
      //When the string says: Ask choice <- then we will put it to the user to allow them to choose their option
      //When a NAN is present, the scene has finished, move on
      //This first part of the array will be all the first scene prolouge lines, so this would be [0][0-23]
      {"Press Any key to start!", "The night sky seems to extend indefinitly, with no end in sight", "\'ring ring\'", "William:\nHey man, how's it been?", "William:\nI'm calling because I wanted to catch up with whats been going on.", "William:\nRecently, I just got promoted at my job, and I'm making some good money!", "Oliver:\nOh yeah? Congrats. Sadly  I just got let go from my previous job as a teacher at La Verne High, I was an English Teacher", "William:\nOh damn. Really man? That sucks to hear.", "Oliver:\nYeah they said ChatGPT could do a better job than I could, and let me go", "William:\n Ah well, hopefully you can get another job soon. But for some good news, I just proposed to my girlfriend and she said yes!", "Oliver:\nHey thats amazing man! I'm glad to hear that things are looking up for you.", "Oliver:\nWell, it'sa been nice talking man, talk to you later.", "William:\n Yeah man, see ya.", "Oliver:\n...", "Oliver:\nI should eat some food...","Ask choice" ,"Oliver:\n God I wish things could have worked out for me too. Why does it have to be this way...", "Some time passes...", "Oliver:\nOh no... How am I gonna make this month's rent?!? I can't believe I still haven't found a job...", "A card slips beneath his door", "Oliver:\nWhats this? La Verne Casino.... Maybe I can make some quick cash", "You grab your essentials and makes his way to his car", "Oliver:\nIs this really a good idea? Should I really be trying this?", "Oliver:\nNah, this is a great idea! I'll become a fu**ing millionaire and NEVER have to worry about rent again!!!"},
      //The prolouge has finished, this is now Oliver at the Casino scenes, which are [1][0-23]
      {"The bright flashing lights blind you as you approach the casino", "Oliver:\nI guess this is the casino...", "Oliver:\nHey, this is La Verne Casino right?", "Bouncer:\n...", "Oliver:\nI guess it's not... maybe I should just go home...", "Bouncer:\nPass.", "Oliver:\nO-oh...ok (Should I really be doing this?)", "Attendant:\nHello there, and welcome to La Verne Casino. What would you like to play?", "Ask choice", "NAN", "NAN", "NAN", "NAN", "NAN","NAN" ,"NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN"}
      };
    private string[,] NPC_Lines = {
      {
        "You approach a fancy looking woman...", "Oliver:\nNAN"
      },
      {
        "You approach a man who looks like he's had a rough time", "Oliver:\nNAN"
      },
      {
        "You approach a man who looks like extremely angry", "Oliver:\nNAN"
      }
    };
    private string[,] Branching_Paths = {
      //0 will be the neutral ending, anything less than $1000 will lead to this ending
      {
        "Machine:\n*Click, Click, Click*", "Oliver:\nHopefully I can get by with just this..."        
      },
      //1 will be the good ending, you must have exactly $1000 in your account to get this
      {
        "Machine:\n*Click, Click, Click*", "Oliver:\nOh thank god, I think this is enough for rent... Maybe I should go get some pizza...",
      },
      //2 will be the battle ending, you pushed your luck too far and lost the 1 million, will lead to battle with bouncer
      {
        "Machine:\n*Click, Click, Click*", "Oliver:\nF*CK, I CAN'T BELIEVE I LOST THAT ALL F********CK"
      },
      //3 will be the bad ending, you won a million dollars by complete luck, will lead to bad ending
      {
        "Machine:\n*Click, Click, Click*", "Oliver:\nHELL YEAH! I CAN'T BELIEVE I'M A MILLIONAIRE NOW, WOOOOO"
      }
    };
    private string[] Battle_Ending = {
      "Oliver begins punching the machine.", "Attendant:\nSir, please refrain from harming company equipment. If you do not stop, we will have to ask you to leave.", "In a blind rage Oliver can't seem to hear the attendent and continues punching the machine","Oliver:\nF*CK, F*CK, F*CK", "Attendent:\nSECURITY!", "Bouncer:\n No pass.", "The boucer readies his stance.", "The Bouncer: 100/100hp, 20def, 50Atk\n\n\nOliver: 20/20hp, 5def, 1Atk.\n\nWhat would you like to do?", "Ask choice", "The bouncer dodges and counter-attacks.", "Oliver has ran out of HP.\n\nYou Lose."
    };
    private string[] Bad_Ending = {
      "Oliver returns home, and pays off his rent.", "Oliver:\nNow that the rents paid... what should I do now?", "Oliver:\nI have an idea...", "Attendant:\nHello, and welcome to The Store. How may we assist you?", "Oliver:\nHey, don't I know you from somewhere?", "Attendant:\nAhem, whatever could you mean?", "Oliver:\nYeah I know you, you were the girl at the casino!", "Attendant:\nUgh, yeah, I also work at the casino. Not everyone can make it big like you man, people gotta work.", "Oliver:\nSheesh, ok I get it. Anyways, I'd like 1 house and 1 car please, to go.", "Attendant:\nWell lucky you, we have a delux combo of a mansion in Beverly Hills, alongisde a Ferrari for $1,000,000.","Attendant:\nIt was originally priced at $1,000,000,000 but it has now been graciously marked down %99.9 with a $100 rebate. Are you interested?", "Oliver:\nWhoa, 99%, that's an amazing deal. Sign me up!"
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