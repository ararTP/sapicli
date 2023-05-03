using System;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Globalization;
using System.IO;

namespace SampleSynthesis
{
    class Program
    {
        static void PrintHelp() { 
            Console.WriteLine("sapicli guide:");
            Console.WriteLine("\t--help, -h to display this message\n");
            Console.WriteLine("\t-l[=<culture>] to list all available voices culture. eg '-l=en_US' to display american english voices or '-l' to display all. culture is type of https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.-ctor?view=netframework-4.8.1#system-globalization-cultureinfo-ctor(system-string) \n");
            Console.WriteLine("\t-v <voiceName> to select a voice.  https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.selectvoice \n");
            Console.WriteLine("\t-r <rate> to choose speech rate.  (-10 <= integer <= 10).  https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.rate \n");
            Console.WriteLine("\t-a <volume> to change speech volume (0 <= integer <= 100). https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.volume \n");
            Console.WriteLine("\t-w <wavOutputPath> to save speech to wav. https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.setoutputtowavefile \n");
            Console.WriteLine("\t-t <text> to set the speech text.\n"); 
            Console.WriteLine("\t-f <pathToInputFile> to set the speech text to file content.\n");
            Console.WriteLine("\t--ssml to specify the speech text uses SSML (off by default).\n");
            System.Environment.Exit(1);
        }
        static void PrintAvailVoices(SpeechSynthesizer synth)
        {
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                string AudioFormats = "";
                foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                {
                    AudioFormats += String.Format("{0}\n",
                    fmt.EncodingFormat.ToString());
                }

                Console.WriteLine(" Name:          " + info.Name);
                Console.WriteLine(" Culture:       " + info.Culture);
                Console.WriteLine(" Age:           " + info.Age);
                Console.WriteLine(" Gender:        " + info.Gender);
                Console.WriteLine(" Description:   " + info.Description);
                Console.WriteLine(" ID:            " + info.Id);
                Console.WriteLine(" Enabled:       " + voice.Enabled);
                if (info.SupportedAudioFormats.Count != 0)
                {
                    Console.WriteLine(" Audio formats: " + AudioFormats);
                }
                else
                {
                    Console.WriteLine(" No supported audio formats found");
                }

                string AdditionalInfo = "";
                foreach (string key in info.AdditionalInfo.Keys)
                {
                    AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                }

                Console.WriteLine(" Additional Info - " + AdditionalInfo);
                Console.WriteLine();
            }
            System.Environment.Exit(1);
        }
        static void PrintAvailVoices(SpeechSynthesizer synth,String culture)
        {
            foreach (InstalledVoice voice in synth.GetInstalledVoices(new CultureInfo(culture)))
            {
                VoiceInfo info = voice.VoiceInfo;
                string AudioFormats = "";
                foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                {
                    AudioFormats += String.Format("{0}\n",
                    fmt.EncodingFormat.ToString());
                }

                Console.WriteLine(" Name:          " + info.Name);
                Console.WriteLine(" Culture:       " + info.Culture);
                Console.WriteLine(" Age:           " + info.Age);
                Console.WriteLine(" Gender:        " + info.Gender);
                Console.WriteLine(" Description:   " + info.Description);
                Console.WriteLine(" ID:            " + info.Id);
                Console.WriteLine(" Enabled:       " + voice.Enabled);
                if (info.SupportedAudioFormats.Count != 0)
                {
                    Console.WriteLine(" Audio formats: " + AudioFormats);
                }
                else
                {
                    Console.WriteLine(" No supported audio formats found");
                }

                string AdditionalInfo = "";
                foreach (string key in info.AdditionalInfo.Keys)
                {
                    AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                }

                Console.WriteLine(" Additional Info - " + AdditionalInfo);
                Console.WriteLine();
            }
            System.Environment.Exit(1);
        }
        static void Err(String message,bool exit)
        {
            Console.WriteLine(message);
            if (exit)
            {
                System.Environment.Exit(-1);
            }
        }
        static void Err(String message)
        {
            Err(message, true);
        }

        static void Err(int code){
            Err(code, true);
        }

        static void Err(int code,bool exit)
        {
            switch (code)
            {
                case 1:
                    Console.WriteLine("Err: Please select a voice");
                    break;
                case 2:
                    Console.WriteLine("Err: Please select rate");
                    break;
                case 3:
                    Console.WriteLine("Err: Please select amplitude (Volume)");
                    break;
                case 4:
                    Console.WriteLine("Err: Invalid command args");
                    break;
                case 5:
                    Console.WriteLine("Err: Please add text");
                    break;
                case 6:
                    Console.WriteLine("Err: Please add file path");
                    break;
                case 7:
                    Console.WriteLine("Err: Please add wav output path");
                    break;
                case 8:
                    Console.WriteLine("Err: Please add text");
                    break;
                case 0:
                default:
                    Console.WriteLine("Error occured");
                    break;
                    
            }
            if (exit)
            {
                System.Environment.Exit(-1);
            }
        }
        static void Main(string[] args)
        {
            String textToSpeak="";
            bool ssml = false;
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                
                for(int i=0;i<args.Length; i++)
                {
                    String arg = args[i];
                    if (arg.StartsWith("--"))
                    {
                        String command = arg.Replace("-", String.Empty).ToLower();
                        if (command.Equals("help"))
                        {
                            PrintHelp();
                        }
                        else if (command.Equals("ssml")) ssml = true;
                    }
                    else if (arg.StartsWith("-"))
                    {
                        String command = arg.Replace("-", String.Empty);
                        switch (command)
                        {
                            case "h":
                                PrintHelp();
                                System.Environment.Exit(1);
                                break;
                            case "v":
                                if (i == args.Length - 1)
                                {
                                    Err(1);
                                }
                                String voice = args[i + 1];
                                i++;
                                try
                                {
                                    synth.SelectVoice(voice);
                                }
                                catch (Exception e)
                                {
                                    Err(e.Message);
                                }
                                break;
                            case "r":
                                if (i == args.Length - 1)
                                {
                                    Err(2);
                                }
                                int rate = 0;
                                try
                                {
                                    rate = Int32.Parse(args[i+1]);
                                    i++;
                                }
                                catch (FormatException)
                                {
                                    Err($"Err: Unable to parse '{args[i + 1]}'");
                                }

                                try
                                {
                                    synth.Rate=rate;
                                }
                                catch (Exception e)
                                {
                                    Err(e.Message);
                                }
                                break;
                            case "a":
                                if (i == args.Length - 1)
                                {
                                    Err(3, true);
                                }
                                int amplitude = 50;
                                try
                                {
                                    amplitude = Int32.Parse(args[i + 1]);
                                    i++;
                                }
                                catch (FormatException)
                                {
                                    Err($"Err: Unable to parse '{args[i + 1]}'");
                                }

                                try
                                {
                                    synth.Volume = amplitude;
                                }
                                catch (Exception e)
                                {
                                    Err(e.Message);
                                }
                                break;
                            case "w":
                                if (i == args.Length - 1)
                                {
                                    Err(7);
                                }
                                String wav = args[i + 1];
                                i++;
                                try
                                {
                                    synth.SetOutputToWaveFile(wav);
                                }
                                catch (Exception e)
                                {
                                    Err(e.Message);
                                }
                                break;
                            case String s when s.StartsWith("l"):
                                if (s.Equals("l")) PrintAvailVoices(synth);
                                else
                                {
                                    if (s[1] == '=') PrintAvailVoices(synth, s.Substring(2));
                                    else
                                    {
                                        Err(4);
                                    }
                                }
                                break;
                            case "t":
                                if (i == args.Length - 1)
                                {
                                    Err(5);
                                }
                                textToSpeak = args[i + 1];
                                i++;
                                break;
                            case "f":
                                if (i == args.Length - 1)
                                {
                                    Err(6);
                                }
                                String path = args[i + 1];
                                i++;
                                try
                                {
                                    textToSpeak = File.ReadAllText(path);
                                }catch(Exception e)
                                {
                                    Err(e.Message);
                                }
                                break;
                        }
                    }

                }
                if (ssml)
                {
                    synth.SpeakSsml(textToSpeak);
                }
                else
                {
                    synth.Speak(textToSpeak);
                }
                Console.WriteLine("Completed.");
            }
        }
    }
}
