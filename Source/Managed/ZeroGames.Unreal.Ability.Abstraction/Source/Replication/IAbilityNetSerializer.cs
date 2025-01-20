// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityNetSerializer
{
	void NetSerializeAbilityActivation(IAbilityActivationRequest request, ActiveAbilityGuid guid);
	void NetSerializeAbilityCancellation(IAbilityCancellationRequest request);
	void NetSerializeAbilityCompletion(AbilityActivationGroup group, ActiveAbilityGuid guid);
	void NetSerializeAbilityEvent(AbilityEventKey key, AbilityActivationGroup group, ActiveAbilityGuid guid, IAbilityEventPayload? payload);
}


