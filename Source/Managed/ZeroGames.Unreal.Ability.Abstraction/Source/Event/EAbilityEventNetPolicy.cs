// Copyright Zero Games. All Rights Reserved.

namespace ZeroGames.Unreal.Ability;

public enum EAbilityEventNetPolicy
{
	/// <summary>
	/// Ability event only triggers on authority and will be replicated to proxies.
	/// </summary>
	Replicated,
	
	/// <summary>
	/// Ability event only triggers on authority and will not be replicated to proxies.
	/// </summary>
	AuthorityOnly,
	
	/// <summary>
	/// Ability event only triggers on both authority and proxies and runs independently.
	/// </summary>
	LocalOnly,
}


