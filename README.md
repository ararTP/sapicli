# sapicli
Simple Microsoft SAPI CLI. 
FOSS alternative to balabolka CLI (balcon).

# sapicli guide:

        --help, -h to display help message

        --ssml to specify the speech text uses SSML (off by default).
        
        -l[=<culture>] to list all available voices culture. eg '-l=en_US' to display american english voices or '-l' to display all. culture is type of 'https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.-ctor?view=netframework-4.8.1#system-globalization-cultureinfo-ctor(system-string)'

        -v <voiceName> to select a voice.  https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.selectvoice
        
        -i <voiceIndex> select voice by index. (see '-l' output for indexes).
        
        -r <rate> to choose speech rate.  (-10 <= integer <= 10).  https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.rate

        -a <volume> to change speech volume (0 <= integer <= 100). https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.volume

        -w <wavOutputPath> to save speech to wav. https://learn.microsoft.com/en-us/dotnet/api/system.speech.synthesis.speechsynthesizer.setoutputtowavefile

        -t <text> to set the speech text.

        -f <pathToInputFile> to set the speech text to file content.
# Examples:

```bash 
   $ sapicli -t "hello what's up" -v "Microsoft Zira Desktop"
   $ sapicli -l=en_US
   $ sapicli --ssml -f text.ssml -a 20 -r 5 -w output.wav
   $ sapicli -f text.txt -i -1 -w output.wav
```

# building instructions:
  1. Create a new VS .NET C# CLI project.
  2. Add `System.Speech` reference.
  3. Copy Program.cs code.
  4. Build.
