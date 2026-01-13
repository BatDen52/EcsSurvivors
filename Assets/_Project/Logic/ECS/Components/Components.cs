using UnityEngine;

public struct PlayerTag { }
public struct EnemyTag { }
public struct ProjectileTag { }
public struct CoinTag { }
public struct DeadTag { }

public struct Health
{
    public int Current;
    public int Max;
}

public struct MoveSpeed
{
    public float Value;
}

public struct Direction
{
    public Vector3 Value;
}

public struct Damage
{
    public int Amount;
}

public struct ResourceCoins
{
    public int Count;
}

public struct FireCooldown
{
    public float Current;
    public float Max;
}

public struct EnemySpawnTimer
{
    public float Current;
}

public struct TransformRef
{
    public Transform Value;
}

public struct RigidbodyRef
{
    public Rigidbody Value;
}

public struct UIHealthBar
{
    public Canvas HealthBarCanvas;
}

public struct GameState
{
    public bool IsGameOver;
}

public struct InputVector
{
    public Vector2 Value;
}

public struct CollisionEvent
{
    public GameObject Other;
}

public struct ReturnToPoolRequest { }

public struct UpdatePlayerHealthEvent
{
    public int CurrentHealth;
}

public struct UpdatePlayerCoinsEvent
{
    public int CoinCount;
}

public struct UpdateEnemyHealthEvent
{
    public int Entity;
    public int CurrentHealth;
    public int MaxHealth;
}

public struct ProjectileLifetime
{
    public float Value;
}

public struct EnemyDamageCooldown
{
    public float Current;
    public float Max;
}

public struct DestroyRequest { }

public struct SpawnPlayerRequest { }
public struct SpawnEnemyRequest { public Vector3 Position; }
public struct SpawnProjectileRequest {
    public Vector3 Position;
    public Vector3 Direction;
}
public struct SpawnCoinRequest { public Vector3 Position; }
public struct DestroyEntityRequest { public int Entity; }
