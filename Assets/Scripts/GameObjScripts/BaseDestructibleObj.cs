using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDestructibleObj : BaseSelectableObject
{
    [Header("Destructible Object")]
    public float MaxHealth;
    public float CurrentHealth;
    protected bool isDestroyed;

    [Header("Set Up Destructible")]
    public GameObject HealthBar;
    protected ObjectHealth healthDisplay;
    public GameObject DeathAnimation;

    public GameObject ActiveVisual;
    public GameObject DestroyedVisual;

    protected virtual void Awake()
    {
        if (HealthBar == null)
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "No Health bar attached for obj");
            return;
        }

        healthDisplay = HealthBar.GetComponent<ObjectHealth>();
    }

    protected virtual void Start()
    {
        if (CurrentHealth == -1)
        {
            CurrentHealth = MaxHealth;
        }
        DamageHandling();
    }

    protected virtual bool Die()
    {
        if (IsDestroyed)
        {
            DeathHandling();
            return true;
        }

        return false;
    }

    protected virtual void DeathHandling()
    {
        if (DeathAnimation == null)
            return;
     
        Destroy(Instantiate(DeathAnimation, transform.position, transform.rotation), 2f);
    }

    protected virtual void ToggleVisualDisplay()
    {
        if (ActiveVisual == null || DestroyedVisual == null)
            return;

        if (IsDestroyed)
        {
            ActiveVisual.SetActive(false);
            DestroyedVisual.SetActive(true);
        }
        else
        {
            ActiveVisual.SetActive(true);
            DestroyedVisual.SetActive(false);
        }
    }

    public virtual void Heal(float health)
    {
        // heal all
        if (health == -1)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth += health, 0, MaxHealth);
        }

        isDestroyed = false;
        DamageHandling();
    }

    public virtual void TakeDamage(float damage)
    {
        Mathf.Clamp(CurrentHealth -= damage, 0, MaxHealth);
        DamageHandling();
    }

    protected virtual void DamageHandling()
    {
        if (healthDisplay == null)
            return;

        if (CurrentHealth == MaxHealth)
        {
            HealthBar.SetActive(false);
        }
        else
        {
            healthDisplay.UpdateHealthBar(CurrentHealth / MaxHealth);
            HealthBar.SetActive(true);
        }
        ToggleVisualDisplay();
    }

    public bool IsDestroyed 
    { 
        get 
        {
            isDestroyed = CurrentHealth <= 0f;
            return isDestroyed; 
        } 
    }

    public override void SetData(int id)
    {
        StructureData data = StructureDataManager.Instance().GetData(Type, id);
        isDestroyed = data.IsDestroyed;
        CurrentHealth = data.CurrentHealth;

        base.SetData(id);
    }

    public override BaseData GetData()
    {
        StructureData data = new StructureData();
        data.ID = ID;
        data.Name = Name;
        data.Type = Type;
        data.IsDestroyed = isDestroyed;
        data.CurrentHealth = CurrentHealth;
        return data;
    }
}
