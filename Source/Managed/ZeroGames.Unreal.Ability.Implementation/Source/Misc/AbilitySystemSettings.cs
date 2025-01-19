// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public static class AbilitySystemSettings
{

	public static string AllocatorType { get; } = GConfig.GetStringOrDefault(INI, SECTION, nameof(AllocatorType), typeof(AbilitySystemAllocator).FullName!);

	private const string INI = "ZeroAbilitySystem";
	private const string SECTION = "Managed." + nameof(AbilitySystemSettings);

}


