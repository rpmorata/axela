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
        ch_StartStopCommands.Add("axela introduce yourself");
        
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
        ch_Numbers.Add("1");ch_Numbers.Add("2");ch_Numbers.Add("3");ch_Numbers.Add("4");ch_Numbers.Add("5");ch_Numbers.Add("6");ch_Numbers.Add("7");ch_Numbers.Add("8");ch_Numbers.Add("9");ch_Numbers.Add("10");
        ch_Numbers.Add("11");ch_Numbers.Add("12");ch_Numbers.Add("13");ch_Numbers.Add("14");ch_Numbers.Add("15");ch_Numbers.Add("16");ch_Numbers.Add("17");ch_Numbers.Add("18");ch_Numbers.Add("19");ch_Numbers.Add("20");
        ch_Numbers.Add("21");ch_Numbers.Add("22");ch_Numbers.Add("23");ch_Numbers.Add("24");ch_Numbers.Add("25");ch_Numbers.Add("26");ch_Numbers.Add("27");ch_Numbers.Add("28");ch_Numbers.Add("29");ch_Numbers.Add("30");
        ch_Numbers.Add("31");ch_Numbers.Add("32");ch_Numbers.Add("33");ch_Numbers.Add("34");ch_Numbers.Add("35");ch_Numbers.Add("36");ch_Numbers.Add("37");ch_Numbers.Add("38");ch_Numbers.Add("39");ch_Numbers.Add("40");
        ch_Numbers.Add("41");ch_Numbers.Add("42");ch_Numbers.Add("43");ch_Numbers.Add("44");ch_Numbers.Add("45");ch_Numbers.Add("46");ch_Numbers.Add("47");ch_Numbers.Add("48");ch_Numbers.Add("49");ch_Numbers.Add("50");
        ch_Numbers.Add("51");ch_Numbers.Add("52");ch_Numbers.Add("53");ch_Numbers.Add("54");ch_Numbers.Add("55");ch_Numbers.Add("56");ch_Numbers.Add("57");ch_Numbers.Add("58");ch_Numbers.Add("59");ch_Numbers.Add("60");
        ch_Numbers.Add("61");ch_Numbers.Add("62");ch_Numbers.Add("63");ch_Numbers.Add("64");ch_Numbers.Add("65");ch_Numbers.Add("66");ch_Numbers.Add("67");ch_Numbers.Add("68");ch_Numbers.Add("69");ch_Numbers.Add("70");
        ch_Numbers.Add("71");ch_Numbers.Add("72");ch_Numbers.Add("73");ch_Numbers.Add("74");ch_Numbers.Add("75");ch_Numbers.Add("76");ch_Numbers.Add("77");ch_Numbers.Add("78");ch_Numbers.Add("79");ch_Numbers.Add("80);
        ch_Numbers.Add("81");ch_Numbers.Add("82");ch_Numbers.Add("83");ch_Numbers.Add("84");ch_Numbers.Add("85");ch_Numbers.Add("86");ch_Numbers.Add("88");ch_Numbers.Add("88");ch_Numbers.Add("89");ch_Numbers.Add("90);
        ch_Numbers.Add("91");ch_Numbers.Add("92");ch_Numbers.Add("93");ch_Numbers.Add("94");ch_Numbers.Add("95");ch_Numbers.Add("96");ch_Numbers.Add("97");ch_Numbers.Add("98");ch_Numbers.Add("99");ch_Numbers.Add("100");

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
      if (txt.IndexOf("axela introduce yourself") >= 0)
      {
        Console.WriteLine("Speaking: ");
        ss.Speak("");
      }
      
      }
    } // sre_SpeechRecognized

  } // Program

} // ns
