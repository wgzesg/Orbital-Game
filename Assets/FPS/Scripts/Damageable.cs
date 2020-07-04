using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using System.Linq;

public class Damageable : MonoBehaviour
{
    [Tooltip("Multiplier to apply to the received damage")]
    public float damageMultiplier = 1f;
    [Range(0, 1)]
    [Tooltip("Multiplier to apply to self damage")]
    public float sensibilityToSelfdamage = 0.5f;
    public Material[] botMaterial;
    public GameObject botRoot;
    public Color slowColor;


    public Health health { get; private set; }

    private NavMeshAgent agent;
    private Material slowMaterial;
    private List<Renderer> children;
    private List<Material> OrignalChildren = new List<Material>();
    private Coroutine UnderEffect;

    void Awake()
    {
        // find the health component either at the same level, or higher in the hierarchy
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }
        if (botMaterial.Length != 0)
        {
            slowMaterial = new Material(botMaterial[0]);
            slowMaterial.color = slowColor;
            children = new List<Renderer>(botRoot.GetComponentsInChildren<Renderer>());
            _ = children.RemoveAll(WrongMat);
            children.ForEach(rend => OrignalChildren.Add(rend.material));
        }
        agent = GetComponentInParent<NavMeshAgent>();

    }

    private bool WrongMat(Renderer rend)
    {
        return !botMaterial.Contains(rend.sharedMaterial);
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
            Debug.Log("setting color to " + slowMaterial);
            rend.material = slowMaterial;
        }
        agent.speed /= 2;

        if (UnderEffect != null)
        {
            StopCoroutine(UnderEffect);
        }
        UnderEffect = StartCoroutine(ChangeBack());
    }

    IEnumerator ChangeBack()
    {
        yield return new WaitForSeconds(5f);
        for(int i = 0; i < children.Count; i ++)
        {
            Debug.Log("set back to " + OrignalChildren[i]);
            children[i].material = OrignalChildren[i];
        }
        agent.speed *= 2;

    }
}
