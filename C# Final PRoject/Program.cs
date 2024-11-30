using System;
using System.Collections;
using System.Dynamic;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;

namespace MyApp
{
  class Game()
  {
    private int Current_Money {get; set;} = 500;
    private bool Bad_Ending_flag {get;set;} = false;
    private bool Battle_Ending_flag {get;set;} = false;
    private bool Neutral_Ending_flag {get;set;} = false;
    private bool catCheck {get; set;} = false;
    //This is going to be a 2d array
    //Turns out 2d Arrays have to be the same size for each portion, which sucks this will be an extremely dumb design...
    public string[,] Prolouge_Lines = {
      //When the string says: Ask choice <- then we will put it to the user to allow them to choose their option
      //When a NAN is present, the scene has finished, move on
      //This first part of the array will be all the first scene prolouge lines, so this would be [0][0-23]
      {"Press Any key to start!", "The night sky seems to extend indefinitly, with no end in sight", "\'ring ring\'", "William:\nHey man, how\'s it been?", "William:\nI\'m calling because I wanted to catch up with whats been going on.", "William:\nRecently, I just got promoted at my job, and I\'m making some good money!", "Oliver:\nOh yeah? Congrats. Sadly  I just got let go from my previous job as a teacher at La Verne High, I was an English Teacher", "William:\nOh damn. Really man? That sucks to hear.", "Oliver:\nYeah they said ChatGPT could do a better job than I could, and let me go", "William:\n Ah well, hopefully you can get another job soon. But for some good news, I just proposed to my girlfriend and she said yes!", "Oliver:\nHey thats amazing man! I\'m glad to hear that things are looking up for you.", "Oliver:\nWell, it\'s been nice talking man, talk to you later.", "William:\n Yeah man, see ya.", "Oliver:\n...", "Oliver:\nI should eat some food...","Ask choice" ,"Oliver:\nGod I wish things could have worked out for me too. Why does it have to be this way...", "Some time passes...", "Oliver:\nOh no... How am I gonna make this month\'s rent?!? I can\'t believe I still haven\'t found a job...", "A card slips beneath his door", "Oliver:\nWhats this? La Verne Casino.... Maybe I can make some quick cash", "You grab your essentials and makes his way to his car", "Oliver:\nIs this really a good idea? Should I really be trying this?", "Oliver:\nNah, this is a great idea! I\'ll become a fu**ing millionaire and NEVER have to worry about rent again!!!"},
      //The prolouge has finished, this is now Oliver at the Casino scenes, which are [1][0-23]
      {"The bright flashing lights blind you as you approach the casino", "Oliver:\nI guess this is the casino...", "Oliver:\nHey, this is La Verne Casino right?", "Bouncer:\n...", "Oliver:\nI guess it\'s not... maybe I should just go home...", "Bouncer:\nPass.", "Oliver:\nO-oh...ok (Should I really be doing this?)", "Attendant:\nHello there, and welcome to La Verne Casino. What would you like to play?", "Ask choice", "NAN", "NAN", "NAN", "NAN", "NAN","NAN" ,"NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN", "NAN"}
      };
    public string[,] NPC_Lines = {
      {
        //Lady
        "You approach a fancy looking woman...", "Oliver:\nNAN"
      },
      {
        //Divorced man
        "You approach a man who looks like he\'s had a rough time", "Oliver:\nNAN"
      },
      {
        //Angry Man
        "You approach a man who looks like extremely angry", "Oliver:\nNAN"
      }
    };
    public string[,] Branching_Paths = {
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
        "Machine:\n*Click, Click, Click*", "Oliver:\nF*CK, I CAN\'T BELIEVE I LOST THAT ALL F********CK"
      },
      //3 will be the bad ending, you won a million dollars by complete luck, will lead to bad ending
      {
        "Machine:\n*Click, Click, Click*", "Oliver:\nHELL YEAH! I CAN\'T BELIEVE I\'M A MILLIONAIRE NOW, WOOOOO"
      }
    };
    public string[] Battle_Ending = {
      "Oliver begins punching the machine.", "Attendant:\nSir, please refrain from harming company equipment. If you do not stop, we will have to ask you to leave.", "In a blind rage Oliver can't seem to hear the attendent and continues punching the machine","Oliver:\nF*CK, F*CK, F*CK", "Attendent:\nSECURITY!", "Bouncer:\n No pass.", "The boucer readies his stance.", "The Bouncer: 100/100hp, 20def, 50Atk\n\n\nOliver: 20/20hp, 5def, 1Atk.\n\nWhat would you like to do?", "Ask choice", "The bouncer dodges and counter-attacks.", "Oliver has ran out of HP.\n\nYou Lose.", "Oliver:\nF*ck, I\'ve lost all my money, what the hell am I going to do..."
    };
    public string[] Bad_Ending = {
      "Oliver returns home, and pays off his rent.", "Oliver:\nNow that the rents paid... what should I do now?", "Oliver:\nI have an idea...", "Attendant:\nHello, and welcome to The Store. How may we assist you?", "Oliver:\nHey, don\'t I know you from somewhere?", "Attendant:\nAhem, whatever could you mean?", "Oliver:\nYeah I know you, you were the girl at the casino!", "Attendant:\nUgh, yeah, I also work at the casino. Not everyone can make it big like you man, people gotta work.", "Oliver:\nSheesh, ok I get it. Anyways, I\'d like 1 house and 1 car please, to go.", "Attendant:\nWell lucky you, we have a delux combo of a mansion in Beverly Hills, alongisde a Ferrari for $1,000,000.","Attendant:\nIt was originally priced at $1,000,000,000 but it has now been graciously marked down %99.9 with a $100 rebate. Are you interested?", "Oliver:\nWhoa, 99%, that\'s an amazing deal. Sign me up!", "Attendent:\nOk, just sign here... and here... and here... and you\'re good to go, here are the keys.", "Oliver:\nSweet! Time to check this baby out.", "Six months pass...", "Cop:\n Alright bub, it\'s up for you, can\'t be late on payments for 6 months in a row.", "Oliver:\nDon\'t I know you from somewhere?... HEY LET ME GO, THIS IS MY HOUSE!", "Cop:\nNot anymore its not.", "Oliver:\nAren\'t you the cop at the casino?", "Bouncer/Cop:\nLet\'s just say you do not pass", "Oliver:\nDAMMIT", "Repo Person:\nHey we got everything loaded up already.", "Oliver:\nHuh?!? The casino attendant? You\'re also the repo person?", "Attendant/Repo:\nLook man, it\'s hard out here.", "Oliver is taken into custody for violently resisting the reposession of his car and home."
    };

    public void Run()
    {
      //Start by going through the prolouge
      PlayProlouge();
      //Into the Casino
      CasinoScene();
      PlayEnding();
    }
    public void PlayProlouge()
    {
      for(int j = 0; j < 24; j++)
      {
        //This is the question on if what you want to drink for breakfast
        if(Prolouge_Lines[0,j] == "Ask choice")
        {
          Console.WriteLine("What would you like to drink?");
          Console.WriteLine("a) Orange Juice\nb) Milk");//orange juice makes it the dog, milk makes it the dog.
          char dChoice = Convert.ToChar(Console.ReadLine());
          Console.Clear();
          if(dChoice == 'a')
          {
            catCheck = false;
          }
          else
          {
            catCheck = true;
          }
        }
        else if(Prolouge_Lines[0,j] == "NAN")
        {
          break;
        }
        else
        {
          Console.WriteLine(Prolouge_Lines[0,j]);
          if(j != 0)
          {
            Console.WriteLine("\n\nPress any key to continue...");
          }
          Console.ReadKey();
          Console.Clear();
        }
      }
    }
    public void CasinoScene()
    {
      for(int j = 0; j < 25; j++)
      {
        if(Prolouge_Lines[1,j] == "Ask choice")
        {
          CasinoMenu();
        }
        else if (Prolouge_Lines[1,j] == "NAN")
        {
          break;
        }
        else
        {
          Console.WriteLine(Prolouge_Lines[1,j]);
          Console.WriteLine("\n\nPress any key to continue!");
          Console.ReadLine();
          Console.Clear();
        }
      }
    }
    public void CasinoMenu()
    {
      Console.WriteLine("a)Talk to woman\nb)Talk to sad dude\nc)Talk to angry dude\nd)Play slots");
      Console.WriteLine("What would you like to do?");
      char gChoice = Convert.ToChar(Console.ReadLine());
      switch(gChoice)
      {
        case 'a':
          TalkToNPC(0);
          CasinoMenu();
          break;
        case 'b':
          TalkToNPC(1);
          CasinoMenu();
          break;
        case 'c':
          TalkToNPC(2);
          CasinoMenu();
          break;
        case 'd':
          PlaySlots();
          break;
        default:
          Console.WriteLine("Thats not a choice >:(");
          CasinoMenu();
          break;
      }
    }

    public void PlaySlots()
    {
      //funi meme
      if (catCheck)
      {
          Console.WriteLine("As you go to the slots, you see a Cat that just won the jackpot");
      }
      else
      {
        Console.WriteLine("As you enter the slots you see a Dog win the jackpot");
      }
      Random random = new Random();
      bool ThousandOccured = false;
      bool MillionFlag = false;
      char PlayerChoice = 'y';
      const int maxAttempts = 5;
      Neutral_Ending_flag = true;
      int attempt = 1;
      do{
        // If it's the last attempt and the event hasn't occurred, force it
        if (attempt == maxAttempts && !ThousandOccured)
        {
            Console.WriteLine($"Attempt {attempt}: Event guaranteed to happen!");
            ThousandOccured = true;
            attempt+=1;
        }
        //You just got the Thousand, now it turns to a million :D
        else if(ThousandOccured && !MillionFlag)
        {
          Current_Money = 1000000;
          MillionFlag = true;
          attempt +=1;
          Bad_Ending_flag = true;
          Console.WriteLine("You won!");
          Console.WriteLine($"New bal: {Current_Money}");
          Console.WriteLine("Oliver:\nIf I do one more I can get even more...");
          Console.WriteLine("Would you like to play again? y/n");
          PlayerChoice = Convert.ToChar(Console.ReadLine());
        }
        else if(MillionFlag)
        {
          Current_Money = 0;
          Battle_Ending_flag = true;
          Console.WriteLine("Sorry, You Lost ;)");
          Console.WriteLine($"New bal: {Current_Money}");
          break;
        }
        else
        {
            // Random chance for the event to occur
            if (random.NextDouble() < 0.3) // 30% chance
            {
                Console.WriteLine($"You won!");
                Current_Money = 1000;
                Console.WriteLine($"New bal: {Current_Money}");
                Console.WriteLine("Oliver:\nThis should be enough for rent...but I could get more...");
                ThousandOccured = true;
                attempt+=1;
                Console.WriteLine("Would you like to play again? y/n");
                PlayerChoice = Convert.ToChar(Console.ReadLine());
            }
            //You don't get the thousand
            else
            {
                Console.WriteLine($"Sorry you lost :(.");
                Current_Money -= 100;
                Console.WriteLine($"New Bal: {Current_Money}, try again?");
                Console.WriteLine("Would you like to play again? y/n");
                PlayerChoice = Convert.ToChar(Console.ReadLine());
                attempt+=1;
            }
        }
      }while(PlayerChoice == 'y' || PlayerChoice == 'Y');
    }
    public void TalkToNPC(int x)
    {
      for(int i = 0; i < 2; i++)
      {
        Console.WriteLine(NPC_Lines[x,i]);
        Console.ReadLine();
        Console.Clear();
      }
    }

  }
  internal class Program
  {
      static void Main(string[] args)
      {
          Game game = new Game();
          game.Run();
      }
  }
}