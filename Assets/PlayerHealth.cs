using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// player index, old health, new health, max health
    /// </summary>
    public static event Action<int, int, int, int> OnPlayerHealthChanged;
    
    /// <summary>
    /// player index, source player index, amount, is kill
    /// </summary>
    public static event Action<int, int, int, bool> OnDamageDealt;

    public static event Action<int, GameObject> OnPlayerDead;
    public static event Action<int, GameObject> OnPlayerRespawn;

    public int MaxHealth { get; private set; } = 20;
    public int CurrentHealth { get; private set; } = 20;

    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; } = 3;

    private PlayerInput _input;
    private float _respawnTime = 5f;

    private PlayerAnimator _animator;
    private PlayerMovement _movement;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<PlayerAnimator>();
        _movement = GetComponent<PlayerMovement>();
        
        OnPlayerHealthChanged?.Invoke(_input.playerIndex, MaxHealth, MaxHealth, MaxHealth);
    }

    public void TakeDamage(int damage, int sourcePlayer)
    {
        if (CurrentHealth < 0) { return; }

        int oldHealth = CurrentHealth;
        CurrentHealth -= damage;

        OnPlayerHealthChanged?.Invoke(_input.playerIndex, oldHealth, CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
            OnPlayerDead?.Invoke(_input.playerIndex, gameObject);
        }
        
        OnDamageDealt?.Invoke(_input.playerIndex, sourcePlayer, damage, CurrentHealth <= 0);
    }

    private void Die()
    {
        CurrentLives--;
        StartCoroutine(RespawnTimer());
        _animator.PlayerDead();
        _movement.PlayerDead();
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(_respawnTime);
        Respawn();
    }

    private void Respawn()
    {
        CurrentHealth = MaxHealth;
        _animator.PlayerRespawn();
        _movement.PlayerRespawn();
        OnPlayerRespawn?.Invoke(_input.playerIndex, gameObject);
    }
}
