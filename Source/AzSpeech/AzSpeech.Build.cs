// Author: Lucas Vilas-Boas
// Year: 2022
// Repo: https://github.com/lucoiso/UEAzSpeech

using System.IO;
using UnrealBuildTool;

public class AzSpeech : ModuleRules
{
	public AzSpeech(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
		CppStandard = CppStandardVersion.Cpp17;
		bEnableExceptions = true;

		PublicDefinitions.Add("WITH_OGGVORBIS");

		PrivateIncludePaths.Add(Path.Combine(ModuleDirectory, "Private"));
		PublicIncludePaths.Add(Path.Combine(ModuleDirectory, "Public"));

		PublicDependencyModuleNames.AddRange(
			new[]
			{
				"Core",
				"AzureWrapper"
			}
		);

		PrivateDependencyModuleNames.AddRange(
			new[]
			{
				"Engine",
				"CoreUObject",
        "Projects",
				"AzureWrapper",
				"AndroidPermission"
			}
		);
	}
}