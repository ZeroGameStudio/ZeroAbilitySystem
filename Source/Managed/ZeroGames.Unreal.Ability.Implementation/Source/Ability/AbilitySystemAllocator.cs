// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public class AbilitySystemAllocator : IAbilitySystemAllocator
{

	public IAbilityActivationRequest AllocateActivationRequest(IAbilityDefinition definition)
		=> new AbilityActivationRequest(definition);

	public IAbilityCancellationRequest AllocateCancellationRequest(AbilityActivationGroup group, ActiveAbilityGuid guid = default)
		=> new AbilityCancellationRequest(group, guid);

	public IAbilityExecutionContext AllocateExecutionContext(IAbilityScheduler scheduler, IAbilityDefinition definition)
		=> new AbilityExecutionContext(scheduler, definition);
	
}


