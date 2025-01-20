// Copyright Zero Games. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;

namespace ZeroGames.Unreal.Ability;

public interface IAbilityDefinition
{
	bool TryGetAbilityEventDefinition(AbilityEventKey key, [NotNullWhen(true)] out IAbilityEventDefinition? definition);
	
	AbilityActivationGroup ActivationGroup { get; }
	EAbilityNetPolicy NetPolicy { get; }
	IAbilityExecutor Executor { get; }
}


