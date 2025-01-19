// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityReplicationProxy
{
	void ReplicateAbilityActivation(IAbilityActivationRequest request, ActiveAbilityGuid guid);
	void ReplicateAbilityCancellation(IAbilityCancellationRequest request);
	void ReplicateAbilityCompletion(AbilityActivationGroup group, ActiveAbilityGuid id);
}


