﻿// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityScheduler_Proxy : IAbilityScheduler
{
	void ReceiveAbilityActivation(IAbilityActivationRequest request, ActiveAbilityGuid guid);
	void ReceiveAbilityCancellation(IAbilityCancellationRequest request);
	void ReceiveAbilityCompletion(AbilityActivationGroup group, ActiveAbilityGuid guid);
}


