// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilityCancellationRequest(AbilityActivationGroup activationGroup, ActiveAbilityGuid guid = default) : IAbilityCancellationRequest
{
	public AbilityActivationGroup ActivationGroup { get; } = activationGroup;
	public ActiveAbilityGuid Guid { get; } = guid;
}


