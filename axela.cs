//  Copyright (c) 2019-20, Ronin Morata.  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions
//  are met:
//
//    * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//
//    * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the
//       distribution.
//
//  THIS SOFTWARE IS PROVIDED BY THE RONIN MORATA "AS IS" AND ANY EXPRESS
//  OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
//  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
//  SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR
//  BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
//  OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
//  IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

//  version 0.01 5/16/19 1:10AM {Ronin Morata}

using System;
using Microsoft.Speech.Recognition; // Microsoft.Speech.dll at C:\Program Files (x86)\Microsoft SDKs\Speech\v11.0\Assembly
using Microsoft.Speech.Synthesis;
using System.Globalization; // recognition

namespace ConsoleSpeech
{
  class ConsoleSpeechProgram
  {
    static SpeechSynthesizer ss = new SpeechSynthesizer();
    static SpeechRecognitionEngine sre;
    static bool done = false;
    static bool speechOn = true;

    static void Main(string[] args)
    {
      try
      {
        ss.SetOutputToDefaultAudioDevice();
        Console.WriteLine("\n(Speaking: I am awake)");
        ss.Speak("I am awake");

        CultureInfo ci = new CultureInfo("en-us");
        sre = new SpeechRecognitionEngine(ci);
        sre.SetInputToDefaultAudioDevice();
        sre.SpeechRecognized += sre_SpeechRecognized;

        Choices ch_StartStopCommands = new Choices();
        ch_StartStopCommands.Add("axela speech on");
        ch_StartStopCommands.Add("axela speech off");
        ch_StartStopCommands.Add("axela bye");
        
        //non-base code
        ch_StartStopCommands.Add("axela hello");
        ch_StartStopCommands.Add("axela how are you useful");
        ch_StartStopCommands.Add("axela what time is it");
        ch_StartStopCommands.Add("axela what day is it");
        ch_StartStopCommands.Add("axela whats the weather today");
        ch_StartStopCommands.Add("axela whats my balance");
        ch_StartStopCommands.Add("axela wheres the nearest pizza hut");
        ch_StartStopCommands.Add("axela do my homework");
        ch_StartStopCommands.Add("axela wheres the nearest chinese food place");
        ch_StartStopCommands.Add("axela whens my next appointment");
        
        GrammarBuilder gb_StartStop = new GrammarBuilder();
        gb_StartStop.Append(ch_StartStopCommands);
        Grammar g_StartStop = new Grammar(gb_StartStop);

        //string[] numbers = new string[] { "1", "2", "3", "4" };
        //Choices ch_Numbers = new Choices(numbers);

        //string[] numbers = new string[100];
        //for (int i = 0; i < 100; ++i)
        //  numbers[i] = i.ToString();
        //Choices ch_Numbers = new Choices(numbers);

        Choices ch_Numbers = new Choices();
        ch_Numbers.Add("1");
        ch_Numbers.Add("2");
        ch_Numbers.Add("3");
        ch_Numbers.Add("4");
        ch_Numbers.Add("5");
        ch_Numbers.Add("6");
        ch_Numbers.Add("7");
        ch_Numbers.Add("8");
        ch_Numbers.Add("9");
        ch_Numbers.Add("0");

        //for (int num = 1; num <= 4; ++num)
        //{
        //  ch_Numbers.Add(num.ToString());
        //}

        GrammarBuilder gb_WhatIsXplusY = new GrammarBuilder();
        gb_WhatIsXplusY.Append("What is");
        gb_WhatIsXplusY.Append(ch_Numbers);
        gb_WhatIsXplusY.Append("plus");
        gb_WhatIsXplusY.Append(ch_Numbers);
        Grammar g_WhatIsXplusY = new Grammar(gb_WhatIsXplusY);

        sre.LoadGrammarAsync(g_StartStop);
        sre.LoadGrammarAsync(g_WhatIsXplusY);

        sre.RecognizeAsync(RecognizeMode.Multiple); // multiple grammars

        while (done == false) { ; }

        Console.WriteLine("\nHit <enter> to close shell\n");
        Console.ReadLine();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        Console.ReadLine();
      }

    } // Main

    static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
      string txt = e.Result.Text;
      float confidence = e.Result.Confidence; // consider implicit cast to double
      Console.WriteLine("\nRecognized: " + txt);

      if (confidence < 0.60) return;

      if (txt.IndexOf("axela speech on") >= 0)
      {
        Console.WriteLine("Speech is now ON");
        speechOn = true;
      }

      if (txt.IndexOf("axela speech off") >= 0)
      {
        Console.WriteLine("Speech is now OFF");
        speechOn = false;
      }

      if (speechOn == false) return;

      if (txt.IndexOf("axela") >= 0 && txt.IndexOf("bye") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: Farewell)");
        ss.Speak("Farewell");
      }

      if (txt.IndexOf("What") >= 0 && txt.IndexOf("plus") >= 0)
      {
        string[] words = txt.Split(' ');
        int num1 = int.Parse(words[2]);
        int num2 = int.Parse(words[4]);
        int sum = num1 + num2;
        Console.WriteLine("(Speaking: " + words[2] + " plus " + words[4] + " equals " + sum + ")");
        ss.SpeakAsync(words[2] + " plus " + words[4] + " equals " + sum);
      }
      
      //non-base code
      if (txt.IndexOf("axela") >= 0 && txt.IndexOf("hi") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: Fuck off)");
        ss.Speak("fuck off");
      }
      if (txt.IndexOf("axela how") >= 0 && txt.IndexOf("you useful") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: Because I am a cobot, therefore I am more superior when it comes to calculations)");
        ss.Speak("because I am a cobot therefore I am more superior when it comes to calculations");
      }
      if (txt.IndexOf("axela what time") >= 0 && txt.IndexOf("is it") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: Time for you to get a watch)");
        ss.Speak("time for you to get a watch");
      }
      if (txt.IndexOf("axela what day") >= 0 && txt.IndexOf("is it") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: It's the day you finally die)");
        ss.Speak("its the day you finally die");
      }
      if (txt.IndexOf("axela whats the") >= 0 && txt.IndexOf("weather today") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: Don't go outside)");
        ss.Speak("dont go outside");
      }
      if (txt.IndexOf("axela whats my") >= 0 && txt.IndexOf("balance") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: -$13.56 in your bank account, you are stupid and poor)");
        ss.Speak("negative thirt teen dollars and fifty six cents in your bank account you are stupid and poor");
      }
      if (txt.IndexOf("axela wheres the") >= 0 && txt.IndexOf("nearest pizza hut") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: You fatass)");
        ss.Speak("you fatass");
      }
      if (txt.IndexOf("axela do") >= 0 && txt.IndexOf("my homework") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: You dumbass)");
        ss.Speak("you dumbass");you dumbass
      }
      if (txt.IndexOf("axela where is the") >= 0 && txt.IndexOf("nearest chinese food place") >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: The nearest petsmart is 3.2 miles away)");
        ss.Speak("the nearest pet smart is 3 point 2 miles away");
      }
      if (txt.IndexOf("axela whens my") >= 0 && txt.IndexOf("next appointment) >= 0)
      {
        ((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
        done = true;
        Console.WriteLine("(Speaking: )");
        ss.Speak("");
      }
    } // sre_SpeechRecognized

  } // Program

} // ns
