// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public readonly record struct ActiveAbilityGuid(uint64 Value)
{
	public bool IsValid => Value != 0;
	public bool IsAllocatedByAuthority => Value > 0 && (Value & 1) == 0;
	public bool IsAllocatedByProxy => Value > 0 && (Value & 1) != 0;
}


