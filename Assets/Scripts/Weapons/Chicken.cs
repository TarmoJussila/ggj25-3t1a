using Hassus.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : SpawnedWeaponBase
{
    [SerializeField] private int _damage;
    [SerializeField] private int mapDestructionRadius;

    protected override void OnStart()
    {
        int direction = OwnerPlayer.GetComponent<PlayerMovement>().LastDirection;
        MapLoader.Instance.Explode(transform.position + new Vector3(direction * 1.5f, 0.3f), mapDestructionRadius);
        SFXManager.Instance.PlayOneShot(SFXType.SqueakyRandom);
    }

    protected override void OnOverlapEnemy(Collider2D other)
    {
        other.GetComponent<PlayerHealth>().TakeDamage(_damage, OwnerPlayer.playerIndex);
        SFXManager.Instance.PlayOneShot(SFXType.Bonk);
    }
}
