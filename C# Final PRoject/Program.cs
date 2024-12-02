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
    private bool Thousand_Ending_flag {get;set;} = false;
    private bool catCheck {get; set;} = false;
    //This is going to be a 2d array
    //Turns out 2d Arrays have to be the same size for each portion, which sucks this will be an extremely dumb design...
    public string[,] Prolouge_Lines = {
      //When the string says: Ask choice <- then we will put it to the user to allow them to choose their option
      //When a NAN is present, the scene has finished, move on
      //This first part of the array will be all the first scene prolouge lines, so this would be [0][0-23]
      {"Press Any key to start!", "The night sky seems to extend indefinitely, with no end in sight", "\'ring ring\'", "William:\nHey man, how\'s it been?", "William:\nI\'m calling because I wanted to catch up with whats been going on.", "William:\nRecently, I just got promoted at my job, and I\'m making some good money!", "Oliver:\nOh yeah? Congrats. Sadly  I just got let go from my previous job as a teacher at La Verne High, I was an English Teacher", "William:\nOh damn. Really man? That sucks to hear.", "Oliver:\nYeah they said ChatGPT could do a better job than I could, and let me go", "William:\nAh well, hopefully you can get another job soon. But for some good news, I just proposed to my girlfriend and she said yes!", "Oliver:\nHey thats amazing man! I\'m glad to hear that things are looking up for you.", "Oliver:\nWell, it\'s been nice talking man, talk to you later.", "William:\nYeah man, see ya.", "Oliver:\n...", "Oliver:\nI should eat some food...","Ask choice" ,"Oliver:\nGod I wish things could have worked out for me too. Why does it have to be this way...", "Some time passes...", "Oliver:\nOh no... How am I gonna make this month\'s rent?!? I can\'t believe I still haven\'t found a job...", "A card slips beneath his door", "Oliver:\nWhats this? La Verne Casino.... Maybe I can make some quick cash", "You grab your essentials and makes his way to his car", "Oliver:\nIs this really a good idea? Should I really be trying this?", "Oliver:\nNah, this is a great idea! I\'ll become a fu**ing millionaire and NEVER have to worry about rent again!!!"},
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
      "Oliver begins punching the machine.", "Attendant:\nSir, please refrain from harming company equipment. If you do not stop, we will have to ask you to leave.", "In a blind rage Oliver can't seem to hear the attendent and continues punching the machine","Oliver:\nF*CK, F*CK, F*CK", "Attendent:\nSECURITY!", "Bouncer:\nNo pass.", "The boucer readies his stance.", "The Bouncer: 100/100hp, 20def, 50Atk\n\n\nOliver: 20/20hp, 5def, 1Atk.", "Ask choice", "The bouncer dodges and counter-attacks.", "Oliver has ran out of HP.\n\nYou Lose.", "Oliver:\nF*ck, I\'ve lost all my money, what the hell am I going to do..."
    };
    public string[] Bad_Ending = {
      "Oliver returns home, and pays off his rent.", "Oliver:\nNow that the rents paid... what should I do now?", "Oliver:\nI have an idea...", "Attendant:\nHello, and welcome to The Store. How may we assist you?", "Oliver:\nHey, don\'t I know you from somewhere?", "Attendant:\nAhem, whatever could you mean?", "Oliver:\nYeah I know you, you were the girl at the casino!", "Attendant:\nUgh, yeah, I also work at the casino. Not everyone can make it big like you man, people gotta work.", "Oliver:\nSheesh, ok I get it. Anyways, I\'d like 1 house and 1 car please, to go.", "Attendant:\nWell lucky you, we have a delux combo of a mansion in Beverly Hills, alongisde a Ferrari for $1,000,000.","Attendant:\nIt was originally priced at $1,000,000,000 but it has now been graciously marked down %99.9 with a $100 rebate. Are you interested?", "Oliver:\nWhoa, 99%, that\'s an amazing deal. Sign me up!", "Attendent:\nOk, just sign here... and here... and here... and you\'re good to go, here are the keys.", "Oliver:\nSweet! Time to check this baby out.", "Six months pass...", "Cop:\nAlright bub, it\'s up for you, can\'t be late on payments for 6 months in a row.", "Oliver:\nDon\'t I know you from somewhere?... HEY LET ME GO, THIS IS MY HOUSE!", "Cop:\nNot anymore its not.", "Oliver:\nAren\'t you the cop at the casino?", "Bouncer/Cop:\nLet\'s just say you do not pass", "Oliver:\nDAMMIT", "Repo Person:\nHey we got everything loaded up already.", "Oliver:\nHuh?!? The casino attendant? You\'re also the repo person?", "Attendant/Repo:\nLook man, it\'s hard out here.", "Oliver is taken into custody for violently resisting the reposession of his car and home."
    };

    public void Run()
    {
      //Start by going through the prolouge
      PlayProlouge();
      //Into the Casino
      CasinoScene();
      PlayEndings();
      DisplayThanks();
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
      Console.Clear();
      switch(gChoice)
      {
        case 'a':
            //TalkToNPC(0);
            ScarlettConvo();
            Console.Clear();
            CasinoMenu();
            
            break;
        case 'b':
            //TalkToNPC(1);
            StefanConvo();
            Console.Clear();
            CasinoMenu();
            break;
        case 'c':
            //TalkToNPC(2);
            JoeConvo();
            Console.Clear();
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
      Thousand_Ending_flag = false;
      bool MillionFlag = false;
      char PlayerChoice = 'y';
      const int maxAttempts = 5;
      int attempt = 1;
      do{
        // If it's the last attempt and the event hasn't occurred, force it
        if (attempt == maxAttempts && !Thousand_Ending_flag)
        {
          Console.WriteLine($"You won!");
          Current_Money = 1000;
          Console.WriteLine($"New bal: {Current_Money}");
          Console.WriteLine("Oliver:\nThis should be enough for rent...but I could get more...");
          Thousand_Ending_flag = true;
          attempt+=1;
          Console.WriteLine("Would you like to play again? y/n");
          PlayerChoice = Convert.ToChar(Console.ReadLine());
        }
        //You just got the Thousand, now it turns to a million :D
        else if(Thousand_Ending_flag && !MillionFlag)
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
          Console.Clear();
        }
        else if(MillionFlag)
        {
          Current_Money = 0;
          Battle_Ending_flag = true;
          Console.WriteLine("Sorry, You Lost ;)");
          Console.WriteLine($"New bal: {Current_Money}");
          Console.ReadLine();
          Console.Clear();
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
                Thousand_Ending_flag = true;
                attempt+=1;
                Console.WriteLine("Would you like to play again? y/n");
                PlayerChoice = Convert.ToChar(Console.ReadLine());
                Console.Clear();
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
                Console.Clear();
            }
        }
      }while(PlayerChoice == 'y' || PlayerChoice == 'Y');
    }

    public void PlayEndings()
    {
      if (Battle_Ending_flag)
      {
        for(int x = 0; x < 2; x++)
        {
          Console.WriteLine(Branching_Paths[2,x]);
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }
        PlayBattleEnding();
      }
      else if (Bad_Ending_flag)
      {
        for(int x = 0; x < 2; x++)
        {
          Console.WriteLine(Branching_Paths[3,x]);
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }
        PlayBadEnding();
      }
      else if(Thousand_Ending_flag)
      {
        for(int x = 0; x < 2; x++)
        {
          Console.WriteLine(Branching_Paths[1,x]);
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }
      }
      else
      {
        for(int x = 0; x < 2; x++)
        {
          Console.WriteLine(Branching_Paths[0,x]);
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }
      }
    }

    public void PlayBadEnding()
    {
      for(int x = 0; x < Bad_Ending.Length; x++)
      {
          Console.WriteLine(Bad_Ending[x]);
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
      }
    }
    public void PlayBattleEnding()
    {
      for(int x = 0; x < Battle_Ending.Length; x++)
      {
        if(Battle_Ending[x] == "Ask choice")
        {
          Console.WriteLine("What would you like to do?\na)Fight\nb)Use Item\nc)Use Special");
          Console.ReadLine();
          Console.Clear();
        }
        else
        {
          Console.WriteLine(Battle_Ending[x]);
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }
      }
    }

    public void ScarlettConvo()
    {
      char choice = ' ';
      Console.Clear();
      Console.WriteLine("Scarlett:\n**Sighs**");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Scarlett:\nWhy do men always get so bothered when you cheat on them?");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Oliver:\nWhy do you do anyways since you know they are broken hearted?");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Scarlett:\nBecause BOY, it's fun for me! So tell me what you want already?");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Oliver: \nWell, You think that you'd have any tips for me so that I could win?");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Scarlett:\nWell, that would depend.\n\na.) Would you like to know how to survive?\nb.) Would you like to know how to make it rich?\nWhat would you like to know?");
      choice = Convert.ToChar(Console.ReadLine());
      
      if (choice == 'a')
      {
        Console.Clear();
        Console.WriteLine("Greed is never the right way to go, I'd advise caution.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
          
      }
      else if (choice == 'b')
      {
        Console.Clear();
        Console.WriteLine("Fortune favours the bold, but not the stupid");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
          
      }
      else
      {
        Console.Clear();
        Console.WriteLine("UGH!!! You are wasting my time get out of here!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();

      }
        
    }

    public void StefanConvo()
    {
      char choice = ' ';
      Console.Clear();
      Console.WriteLine("Stefan:\nMy wife left me for my boss.");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Stefan: \nMy dog got hit by a car running after my wife leaving.");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Stefan:\nNow a kid wants to talk with me.\n\na.) \"You should fight with your boss.\"\nb.) \"Make your boss give you a raise for taking your wife.\"\nc.) Cheer him up? \nWhat would you like to do?");
      choice = Convert.ToChar(Console.ReadLine());

      if (choice == 'a')
      {
        Console.Clear();
        Console.WriteLine("Stefan:\nYou know what, I like advice.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("After this drink I'm going to go and give him a piece of my mind.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();

      }
      else if (choice == 'b')
      {
        Console.Clear();
        Console.WriteLine("Stefan:\nYou're right!!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Stefan:\nThe least he could do is give me a raise for stealing my wife.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Stefan:\n**Comes back after confronting his boss**\nWell, that got me fired.  Ah man!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();

      }
      else if (choice == 'c')
      {
        Console.Clear();
        Console.WriteLine("Oliver:\nCheer up buddy, it's not that bad.  There are plenty of fish in that sea.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Stefan:\nShe was the best at EVERYTHING and had a figure of a model.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Stefan:\nI could never do better I feel.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Oliver:\nhmm...That's really rough then.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Oliver:\nHey buddy, you mind if I could get your ex-wife's phone number? She sounds amazing!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Stefan:\n**cries uncontrollably as he writes down the number.**");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();


      }
      else
      {
        Console.Clear();
        Console.WriteLine("UGH!!! You are wasting my time get out of here!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();

      }
    }

    public void JoeConvo()
    {
      char choice = ' ';
      Console.Clear();
      Console.WriteLine("Joe:\nWhat the F*** do you want????");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Joe:\nThat man Stefan over there got me all bummed out because I had to fire him.");
      Console.WriteLine("\n\nPress any key to continue...");
      Console.ReadLine();
      Console.Clear();
      Console.WriteLine("Joe:\nYeah I'm his boss, so what! You have a problem with me?.\n\na.) YO!! You stole his wife and took his dog!\nb.) Buy Joe a drink.\nc.) Gamble to see who steals Stefan's wife.\nWhat would you like to know?");
      choice = Convert.ToChar(Console.ReadLine());

      if (choice == 'a')
      {
        Console.Clear();
        Console.WriteLine("Joe:\nI did him a favor! **takes another drink**");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Joe:\nA man like him shouldn't have a wife or dog for that matter. Now Get LOST!!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();

      }
      else if (choice == 'b')
      {
        Console.Clear();
        Console.WriteLine("Joe:\nWhy in the hell would you buy me a drink?");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Oliver:\nIt would be a token of good gratitude. It's not everyday you meet a Legend!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Joe:\nWell, I did steal his wife and take his dog. You have a great point. Have a drink, on me!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("**Both of you drink for awhile till you remember that you have have to make the rent**");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();

      }
      else if (choice == 'c')
      {
        int guess = 0;
        Console.Clear();
        Console.WriteLine("Oliver:\nOH, You don't deserve Scarlett, HAND HER OVER!!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Joe:\nListen to the to words coming out of this guys mouth.");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Joe:\nTell you what, if you could get the number I'm thinking of, between 1-10, Scarlett is all yours!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Oliver:\nhmm...You have a deal!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Oliver:\nI think the number is....\n\nEnter a number between 1 and 10 to guess:");
        guess = Convert.ToInt16(Console.ReadLine());
        Console.Clear();
        Random rnd = new Random();
        int rndNum = rnd.Next(1, 10);
        if (guess == rndNum)
        {
          Console.WriteLine("Joe:\nWHAT?! How in the hell did you get that?");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
          Console.WriteLine("Joe:\nMan, I was thinking of '" + rndNum + "' .  Which I didn't think you'd get.");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
          Console.WriteLine("Joe:\nFINE! Scarlett is all yours.  She was too wild for me anyways.");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
          Console.WriteLine("Oliver:\nALRIGHT, I have a new girlfriend! Now I just have to gamble to make some money. LET'S GO!! ");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }
        else
        {
          Console.WriteLine("Joe:\n**Laughs and then spits in your face**");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
          Console.WriteLine("Joe:\nYou are an idiot because you were thinking '" + guess + "', when I was thinking '" + rndNum + "'.");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
          Console.WriteLine("Joe:\nNow get out of my face before I seriously hurt you!");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
          Console.WriteLine("Oliver:\n**walks away feeling shame and even worst then before**");
          Console.WriteLine("\n\nPress any key to continue...");
          Console.ReadLine();
          Console.Clear();
        }

      }
      else
      {
        Console.Clear();
        Console.WriteLine("UGH!!! You are wasting my time get out of here!");
        Console.WriteLine("\n\nPress any key to continue...");
        Console.ReadLine();

      }
    }

    public void DisplayThanks()
    {
      Console.WriteLine(
        "We hope you’ve enjoyed playing our game. It was a lot of work, but we had a lot of fun making it. We made this game to better educate people on the realities of gambling, and how it can ruin lives. Often people in desperate situations look towards gambling as a final savior in their moments of need. In desperate times, this makes sense, but it can often be a double-edged sword. As quickly as it can save your life, it can just as quickly destroy it. This is true of any addiction, but we focused on gambling here as it is one of the most difficult addictions to combat psychologically, as money is a constant in our lives. Realistically, Oliver could have done part-time work, or create content on the internet, but both methods have drawbacks, and we understand how someone in a desperate situation may not see either of these avenues as realistic. Regardless, we hope this game goes to show how gambling can more often than not, destroy lives, instead of saving them."
        );
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