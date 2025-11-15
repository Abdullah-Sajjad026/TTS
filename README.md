# Azure Text-to-Speech (TTS) Project

A .NET console application that demonstrates Text-to-Speech functionality using Microsoft Azure AI Speech Services.

## Features

- Convert text to speech using Azure's neural voices
- Output speech to audio speaker or save to WAV file
- List and browse available Azure neural voices
- Support for multiple languages and voice options
- Interactive console interface

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later
- Azure subscription with Speech Services enabled
- Azure Speech Service resource

## Azure Setup

### 1. Create Azure Speech Service Resource

1. Go to the [Azure Portal](https://portal.azure.com)
2. Click "Create a resource"
3. Search for "Speech" and select "Speech Services"
4. Click "Create"
5. Fill in the required information:
   - **Subscription**: Select your subscription
   - **Resource group**: Create new or select existing
   - **Region**: Choose a region near you (e.g., eastus, westus, westeurope)
   - **Name**: Give your resource a unique name
   - **Pricing tier**: Select Free (F0) for testing or Standard (S0) for production
6. Click "Review + create" and then "Create"

### 2. Get Your API Keys

1. Once the resource is created, navigate to it
2. Go to "Keys and Endpoint" in the left menu
3. Copy one of the keys and the region

## Installation

1. Clone this repository:
```bash
git clone <your-repo-url>
cd TTS
```

2. Navigate to the src directory:
```bash
cd src
```

3. Configure your Azure credentials:

Edit `appsettings.json` and replace the placeholder values:

```json
{
  "AzureSpeech": {
    "SubscriptionKey": "YOUR_AZURE_SPEECH_KEY",
    "Region": "YOUR_AZURE_REGION"
  }
}
```

**Alternative**: Use environment variables (recommended for production):
```bash
export AzureSpeech__SubscriptionKey="your-key-here"
export AzureSpeech__Region="your-region-here"
```

4. Restore dependencies:
```bash
dotnet restore
```

## Usage

### Run the Application

```bash
dotnet run
```

### Menu Options

The application provides an interactive menu with the following options:

1. **Synthesize speech to speaker**: Converts text to speech and plays it through your default audio device
2. **Synthesize speech to audio file**: Converts text to speech and saves it as a WAV file
3. **List available voices**: Displays all available Azure neural voices grouped by language
4. **Exit**: Closes the application

### Example Usage

```
Azure Text-to-Speech Demo
=========================

Choose an option:
1. Synthesize speech to speaker
2. Synthesize speech to audio file
3. List available voices
4. Exit

Enter your choice (1-4): 1

Enter text to synthesize: Hello, welcome to Azure Text-to-Speech!
Synthesizing speech...
Speech synthesized successfully! (48000 bytes)
```

## Configuration Options

### Voice Selection

You can change the default voice in `appsettings.json`:

```json
{
  "Speech": {
    "VoiceName": "en-US-JennyNeural",
    "Language": "en-US"
  }
}
```

### Popular Neural Voices

- **English (US)**: en-US-JennyNeural, en-US-GuyNeural, en-US-AriaNeural
- **English (UK)**: en-GB-SoniaNeural, en-GB-RyanNeural
- **Spanish**: es-ES-ElviraNeural, es-MX-DaliaNeural
- **French**: fr-FR-DeniseNeural, fr-CA-SylvieNeural
- **German**: de-DE-KatjaNeural, de-DE-ConradNeural
- **Japanese**: ja-JP-NanamiNeural, ja-JP-KeitaNeural
- **Chinese**: zh-CN-XiaoxiaoNeural, zh-CN-YunxiNeural

Use option 3 in the menu to see all available voices.

## Project Structure

```
TTS/
├── src/
│   ├── AzureTTS.csproj       # Project file with dependencies
│   ├── Program.cs             # Main application code
│   └── appsettings.json       # Configuration file
├── .gitignore
└── README.md
```

## Dependencies

- `Microsoft.CognitiveServices.Speech` (v1.40.0) - Azure Speech SDK
- `Microsoft.Extensions.Configuration` (v8.0.0) - Configuration management
- `Microsoft.Extensions.Configuration.Json` (v8.0.0) - JSON configuration support
- `Microsoft.Extensions.Configuration.EnvironmentVariables` (v8.0.0) - Environment variable support

## Security Best Practices

- Never commit your Azure subscription keys to version control
- Use environment variables for sensitive credentials in production
- Consider using Azure Key Vault for production deployments
- Rotate your keys periodically
- Use the principle of least privilege for Azure resources

## Troubleshooting

### Authentication Errors

If you see authentication errors:
- Verify your subscription key is correct
- Ensure the region matches your Azure resource region
- Check that your Azure Speech Service resource is active

### Audio Playback Issues

If speech doesn't play:
- Ensure your system has a working audio device
- Try option 2 to save to a file instead
- Check system audio settings and permissions

### Network Issues

If you encounter network errors:
- Verify internet connectivity
- Check if your firewall allows connections to Azure services
- Ensure the Azure region is accessible from your location

## Cost Considerations

Azure Speech Services pricing (as of 2024):
- **Free tier (F0)**: 5 audio hours per month, up to 0.5M characters
- **Standard tier (S0)**: Pay-as-you-go pricing

See [Azure Speech Services Pricing](https://azure.microsoft.com/pricing/details/cognitive-services/speech-services/) for current rates.

## Additional Resources

- [Azure Speech Service Documentation](https://docs.microsoft.com/azure/cognitive-services/speech-service/)
- [Speech SDK for .NET](https://docs.microsoft.com/dotnet/api/microsoft.cognitiveservices.speech)
- [Language and voice support](https://docs.microsoft.com/azure/cognitive-services/speech-service/language-support)
- [SSML (Speech Synthesis Markup Language)](https://docs.microsoft.com/azure/cognitive-services/speech-service/speech-synthesis-markup)

## License

MIT License - feel free to use this project for learning and development.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
