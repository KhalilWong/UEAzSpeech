// Author: Lucas Vilas-Boas
// Year: 2022
// Repo: https://github.com/lucoiso/UEAzSpeech

#include "AzSpeech/TextToVisemeAsync.h"
#include "AzSpeech.h"
#include "Async/Async.h"
#include "AzSpeech/AzSpeechHelper.h"

THIRD_PARTY_INCLUDES_START
#include <speechapi_cxx.h>
THIRD_PARTY_INCLUDES_END

using namespace Microsoft::CognitiveServices::Speech;
using namespace Microsoft::CognitiveServices::Speech::Audio;

namespace AzSpeechWrapper
{
	namespace Standard_Cpp
	{
		static std::vector<uint8_t> DoTextToVisemeWork(const std::string& TextToConvert,
		                                               const std::string& APIAccessKey,
		                                               const std::string& RegionID,
		                                               const std::string& LanguageID,
		                                               const std::string& VoiceName)
		{
			const auto& SpeechConfig = SpeechConfig::FromSubscription(APIAccessKey, RegionID);

			SpeechConfig->SetSpeechSynthesisLanguage(LanguageID);
			SpeechConfig->SetSpeechSynthesisVoiceName(VoiceName);

			std::string VID = "";
			std::string VOS = "";
			std::string SP = "";
			std::vector<uint8_t> VAudio;

			const auto& SpeechSynthesizer =
				SpeechSynthesizer::FromConfig(SpeechConfig,
				                              AudioConfig::FromStreamOutput(AudioOutputStream::CreatePullStream()));
			// Subscribes to viseme received event
			SpeechSynthesizer->VisemeReceived += [&VID, &VOS, SP](const SpeechSynthesisVisemeEventArgs& e)
			{
				// The unit of e.AudioOffset is tick (1 tick = 100 nanoseconds), divide by 10,000 to convert to milliseconds.
			}

			if (const auto& SpeechSynthesisResult =
					SpeechSynthesizer->SpeakTextAsync(TextToConvert).get();
				SpeechSynthesisResult->Reason == ResultReason::SynthesizingAudioCompleted)
			{
				UE_LOG(LogAzSpeech, Display,
				       TEXT("AzSpeech - %s: Speech Synthesis task completed"), *FString(__func__));

				return *SpeechSynthesisResult->GetAudioData().get();
			}

			UE_LOG(LogAzSpeech, Error, TEXT("AzSpeech - %s: Speech Synthesis task failed"), *FString(__func__));
			return std::vector<uint8_t>();
		}
	}

	namespace Unreal_Cpp
	{
		static void AsyncTextToViseme(const FString& TextToConvert,
		                              const FString& VoiceName,
		                              const FAzSpeechData Parameters,
		                              FTextToVisemeDelegate Delegate)
		{
			if (TextToConvert.IsEmpty() || VoiceName.IsEmpty()
				|| UAzSpeechHelper::IsAzSpeechDataEmpty(Parameters))
			{
				UE_LOG(LogAzSpeech, Error, TEXT("AzSpeech - %s: Missing parameters"), *FString(__func__));
				return;
			}

			UE_LOG(LogAzSpeech, Display, TEXT("AzSpeech - %s: Initializing task"), *FString(__func__));

			AsyncTask(ENamedThreads::AnyBackgroundThreadNormalTask, [Parameters, TextToConvert, Delegate, VoiceName]
			{
				const TFuture<std::vector<uint8_t>>& TextToVoiceAsyncWork =
					Async(EAsyncExecution::Thread,
					      [Parameters, TextToConvert, VoiceName]() -> std::vector<uint8_t>
					      {
						      const std::string& APIAccessKeyStr = TCHAR_TO_UTF8(*Parameters.APIAccessKey);
						      const std::string& RegionIDStr = TCHAR_TO_UTF8(*Parameters.RegionID);
						      const std::string& LanguageIDStr = TCHAR_TO_UTF8(*Parameters.LanguageID);
						      const std::string& NameIDStr = TCHAR_TO_UTF8(*VoiceName);
						      const std::string& ToConvertStr = TCHAR_TO_UTF8(*TextToConvert);

						      return Standard_Cpp::DoTextToVisemeWork(ToConvertStr,
						                                              APIAccessKeyStr,
						                                              RegionIDStr,
						                                              LanguageIDStr,
						                                              NameIDStr);
					      });

				TextToVoiceAsyncWork.WaitFor(FTimespan::FromSeconds(5));

				const std::vector<uint8_t>& Result = TextToVoiceAsyncWork.Get();
				const bool& bOutputValue = !Result.empty();

				TArray<uint8> OutputArr;

				for (const uint8_t i : Result)
				{
					OutputArr.Add(static_cast<uint8>(i));
				}

				AsyncTask(ENamedThreads::GameThread, [OutputArr, Delegate]
				{
					Delegate.Broadcast(OutputArr);
				});

				if (bOutputValue)
				{
					UE_LOG(LogAzSpeech, Display, TEXT("AzSpeech - AsyncTextToViseme: Result: Success"));
				}
				else
				{
					UE_LOG(LogAzSpeech, Error, TEXT("AzSpeech - AsyncTextToViseme: Result: Error"));
				}
			});
		}
	}
}

UTextToVisemeAsync* UTextToVisemeAsync::TextToVisemeAsync(const UObject* WorldContextObject,
                                                          const FString& TextToConvert,
                                                          const FString& VoiceName,
                                                          const FAzSpeechData Parameters)
{
	UTextToVisemeAsync* NewAsyncTask = NewObject<UTextToVisemeAsync>();
	NewAsyncTask->WorldContextObject = WorldContextObject;
	NewAsyncTask->TextToConvert = TextToConvert;
	NewAsyncTask->VoiceName = VoiceName;
	NewAsyncTask->Parameters = Parameters;

	return NewAsyncTask;
}

void UTextToVisemeAsync::Activate()
{
	AzSpeechWrapper::Unreal_Cpp::AsyncTextToViseme(TextToConvert, VoiceName, Parameters, TaskCompleted);
}
