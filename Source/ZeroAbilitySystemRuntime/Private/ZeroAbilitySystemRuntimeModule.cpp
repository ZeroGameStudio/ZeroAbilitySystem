// Copyright Zero Games. All Rights Reserved.

#include "ZeroAbilitySystemRuntimeModule.h"

#include "Misc/Log/ZRegisterLogCategoryMacros.h"

DEFINE_LOG_CATEGORY_STATIC(LogZeroAbilitySystemScript, Log, All)

ZSHARP_REGISTER_LOG_CATEGORY(LogZeroAbilitySystemScript)

class FZeroAbilitySystemRuntimeModule : public IZeroAbilitySystemRuntimeModule
{
	// Begin IModuleInterface
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
	// End IModuleInterface
};

IMPLEMENT_MODULE(FZeroAbilitySystemRuntimeModule, ZeroAbilitySystemRuntime)

void FZeroAbilitySystemRuntimeModule::StartupModule()
{
}

void FZeroAbilitySystemRuntimeModule::ShutdownModule()
{
}


