// Author: Lucas Vilas-Boas
// Year: 2022
// Repo: https://github.com/lucoiso/UEAzSpeech

using System.IO;
using UnrealBuildTool;

public class AzureWrapper : ModuleRules
{
	public AzureWrapper(ReadOnlyTargetRules Target) : base(Target)
	{
		Type = ModuleType.External;
		bEnableExceptions = true;

		PublicIncludePaths.AddRange(
			new[]
			{
				Path.Combine(ModuleDirectory, "include")
			}
		);
		
		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "libs", "Win",
				"Microsoft.CognitiveServices.Speech.core.lib"));
		
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.core.dll");
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.extension.audio.sys.dll");
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.extension.kws.dll");
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.extension.lu.dll");
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.extension.mas.dll");
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.extension.silk_codec.dll");
			PublicDelayLoadDLLs.Add("Microsoft.CognitiveServices.Speech.extension.codec.dll");
			
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.core.dll"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.extension.audio.sys.dll"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.extension.kws.dll"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.extension.lu.dll"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.extension.mas.dll"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.extension.silk_codec.dll"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Runtime",
				"Microsoft.CognitiveServices.Speech.extension.codec.dll"));
		}
		else if (Target.Platform == UnrealTargetPlatform.Android)
		{
			AdditionalPropertiesForReceipt.Add("AndroidPlugin", 
				Path.Combine(ModuleDirectory, "AzSpeech_UPL_Android.xml"));
			
			PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "libs", "Android",
				"libMicrosoft.CognitiveServices.Speech.core.so"));
			PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "libs", "Android",
				"libMicrosoft.CognitiveServices.Speech.extension.audio.sys.so"));
			PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "libs", "Android",
				"libMicrosoft.CognitiveServices.Speech.extension.kws.so"));
			PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "libs", "Android",
				"libMicrosoft.CognitiveServices.Speech.extension.lu.so"));
			PublicAdditionalLibraries.Add(Path.Combine(ModuleDirectory, "libs", "Android",
				"libMicrosoft.CognitiveServices.Speech.extension.silk_codec.so"));
		}
		else if (Target.Platform == UnrealTargetPlatform.Linux)
		{
			PublicDelayLoadDLLs.Add("libMicrosoft.CognitiveServices.Speech.core.so");
			PublicDelayLoadDLLs.Add("libMicrosoft.CognitiveServices.Speech.extension.audio.sys.so");
			PublicDelayLoadDLLs.Add("libMicrosoft.CognitiveServices.Speech.extension.kws.so");
			PublicDelayLoadDLLs.Add("libMicrosoft.CognitiveServices.Speech.extension.lu.so");
			PublicDelayLoadDLLs.Add("libMicrosoft.CognitiveServices.Speech.extension.mas.so");
			PublicDelayLoadDLLs.Add("libMicrosoft.CognitiveServices.Speech.extension.codec.so");
			
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Linux", "Runtime",
				"libMicrosoft.CognitiveServices.Speech.core.so"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Linux", "Runtime",
				"libMicrosoft.CognitiveServices.Speech.extension.audio.sys.so"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Linux", "Runtime",
				"libMicrosoft.CognitiveServices.Speech.extension.kws.so"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Linux", "Runtime",
				"libMicrosoft.CognitiveServices.Speech.extension.lu.so"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Linux", "Runtime",
				"libMicrosoft.CognitiveServices.Speech.extension.mas.so"));
			RuntimeDependencies.Add(Path.Combine(ModuleDirectory, "libs", "Linux", "Runtime",
				"libMicrosoft.CognitiveServices.Speech.extension.codec.so"));
		}
		
		PublicDependencyModuleNames.AddRange(new[] 
			{
				"AndroidPermission"
			});
	}
}