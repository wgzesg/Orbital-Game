using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Damageable : MonoBehaviour
{
    [Tooltip("Multiplier to apply to the received damage")]
    public float damageMultiplier = 1f;
    [Range(0, 1)]
    [Tooltip("Multiplier to apply to self damage")]
    public float sensibilityToSelfdamage = 0.5f;
    public Material botMaterial;
    public GameObject botRoot;
    public Health health { get; private set; }

    private Material slowMaterial;
    private List<Renderer> children;
    private Coroutine UnderEffect;

    void Awake()
    {
        // find the health component either at the same level, or higher in the hierarchy
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }
        if (botMaterial)
        {
            slowMaterial = new Material(botMaterial);
            slowMaterial.color = Color.blue;
            children = new List<Renderer>(botRoot.GetComponentsInChildren<Renderer>());
            _ = children.RemoveAll(WrongMat);
        }

    }

    private bool WrongMat(Renderer rend)
    {
        return rend.sharedMaterial != botMaterial;
    }

    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {
        if(health)
        {
            var totalDamage = damage;

            // skip the crit multiplier if it's from an explosion
            if (!isExplosionDamage)
            {
                totalDamage *= damageMultiplier;
            }

            // potentially reduce damages if inflicted by self
            if (health.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfdamage;
            }

            // apply the damages
            health.TakeDamage(totalDamage, damageSource);
        }
    }

    public void InflictEffect(GameObject damageSource)
    {
        if (botRoot == null || damageSource.tag != "Player")
        {
            return;
        }
        foreach (Renderer rend in children)
        {
            rend.material = slowMaterial;
        }

        if(UnderEffect != null)
        {
            Debug.Log("it is stopped");
            StopCoroutine(UnderEffect);
        }
        UnderEffect = StartCoroutine(ChangeBack());
    }

    IEnumerator ChangeBack()
    {
        yield return new WaitForSeconds(5f);
        foreach (Renderer rend in children)
        {
            rend.material = botMaterial;
        }

    }
}
