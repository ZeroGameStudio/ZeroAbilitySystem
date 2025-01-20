// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public interface IAbilityEventDefinition
{
	AbilityEventKey Key { get; }
	EAbilityEventNetPolicy NetPolicy { get; }
	IAbilityEventExecutor Executor { get; }
}


