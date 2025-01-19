// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityExecutionContext
{
	ActiveAbilityGuid Guid { get; }
	IAbilityScheduler Scheduler { get; }
	IAbilityDefinition Definition { get; }
	EAbilityExecutionState ExecutionState { get; set; }
	IAbilityCancellationRequest? PendingCancellationRequest { get; set; }
}


