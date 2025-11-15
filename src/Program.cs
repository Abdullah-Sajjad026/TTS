using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;

namespace AzureTTS;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var subscriptionKey = configuration["AzureSpeech:SubscriptionKey"];
        var region = configuration["AzureSpeech:Region"];
        var voiceName = configuration["Speech:VoiceName"];

        if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(region))
        {
            Console.WriteLine("Error: Azure Speech credentials not configured.");
            Console.WriteLine("Please update appsettings.json or set environment variables:");
            Console.WriteLine("  AzureSpeech__SubscriptionKey");
            Console.WriteLine("  AzureSpeech__Region");
            return;
        }

        Console.WriteLine("Azure Text-to-Speech Demo");
        Console.WriteLine("=========================\n");

        // Create speech config
        var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
        speechConfig.SpeechSynthesisVoiceName = voiceName ?? "en-US-JennyNeural";

        // Main loop
        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Synthesize speech to speaker");
            Console.WriteLine("2. Synthesize speech to audio file");
            Console.WriteLine("3. List available voices");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice (1-4): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await SynthesizeToSpeaker(speechConfig);
                    break;
                case "2":
                    await SynthesizeToFile(speechConfig);
                    break;
                case "3":
                    await ListAvailableVoices(speechConfig);
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task SynthesizeToSpeaker(SpeechConfig speechConfig)
    {
        using var synthesizer = new SpeechSynthesizer(speechConfig);

        Console.Write("\nEnter text to synthesize: ");
        var text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("No text entered.");
            return;
        }

        Console.WriteLine("Synthesizing speech...");
        var result = await synthesizer.SpeakTextAsync(text);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            Console.WriteLine($"Speech synthesized successfully! ({result.AudioData.Length} bytes)");
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            Console.WriteLine($"Speech synthesis canceled: {cancellation.Reason}");
            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
            }
        }
    }

    static async Task SynthesizeToFile(SpeechConfig speechConfig)
    {
        Console.Write("\nEnter output filename (e.g., output.wav): ");
        var filename = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filename))
        {
            filename = "output.wav";
        }

        var audioConfig = AudioConfig.FromWavFileOutput(filename);
        using var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);

        Console.Write("Enter text to synthesize: ");
        var text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("No text entered.");
            return;
        }

        Console.WriteLine("Synthesizing speech to file...");
        var result = await synthesizer.SpeakTextAsync(text);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            Console.WriteLine($"Speech synthesized successfully to '{filename}'!");
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            Console.WriteLine($"Speech synthesis canceled: {cancellation.Reason}");
            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
            }
        }
    }

    static async Task ListAvailableVoices(SpeechConfig speechConfig)
    {
        using var synthesizer = new SpeechSynthesizer(speechConfig, null);

        Console.WriteLine("\nFetching available voices...");
        var result = await synthesizer.GetVoicesAsync();

        if (result.Reason == ResultReason.VoicesListRetrieved)
        {
            Console.WriteLine($"\nFound {result.Voices.Count} voices:");
            Console.WriteLine(new string('-', 80));

            var groupedVoices = result.Voices
                .GroupBy(v => v.Locale)
                .OrderBy(g => g.Key);

            foreach (var group in groupedVoices)
            {
                Console.WriteLine($"\n{group.Key}:");
                foreach (var voice in group.OrderBy(v => v.ShortName))
                {
                    Console.WriteLine($"  {voice.ShortName,-40} ({voice.Gender})");
                }
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve voices.");
        }
    }
}
