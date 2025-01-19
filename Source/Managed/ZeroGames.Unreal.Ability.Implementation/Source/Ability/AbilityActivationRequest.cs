// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityActivationRequest(IAbilityDefinition definition) : IAbilityActivationRequest
{
	public IAbilityDefinition Definition { get; } = definition;
}


