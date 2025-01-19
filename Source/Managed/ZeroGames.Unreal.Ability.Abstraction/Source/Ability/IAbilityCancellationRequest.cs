// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityCancellationRequest
{
	AbilityActivationGroup ActivationGroup { get; }
	ActiveAbilityGuid Guid { get; }
}


