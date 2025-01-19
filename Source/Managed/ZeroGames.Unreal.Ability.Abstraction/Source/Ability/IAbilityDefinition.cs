// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityDefinition
{
	AbilityActivationGroup ActivationGroup { get; }
	EAbilityNetPolicy NetPolicy { get; }
	IAbilityExecutor Executor { get; }
}


