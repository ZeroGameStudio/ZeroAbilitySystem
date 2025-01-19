// Copyright Zero Games. All Rights Reserved.

#pragma once

class IZeroAbilitySystemRuntimeModule : public IModuleInterface
{
public:
	static FORCEINLINE IZeroAbilitySystemRuntimeModule& Get()
	{
		static IZeroAbilitySystemRuntimeModule& GSingleton = FModuleManager::LoadModuleChecked<IZeroAbilitySystemRuntimeModule>("ZeroAbilitySystemRuntime");
		return GSingleton;
	}

	static FORCEINLINE bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded("ZeroAbilitySystemRuntime");
	}
};


