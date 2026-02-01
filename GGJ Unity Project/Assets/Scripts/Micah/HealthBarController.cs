using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(HealthBarController))]
public class HealthBarController : MonoBehaviour
{
    private VisualElement _fill1;
    private VisualElement _fill2;
    private VisualElement _shieldFill1;
    private VisualElement _shieldFill2;

    public static HealthBarController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _fill1 = root.Q<VisualElement>("HealthBarFill1");
        _fill2 = root.Q<VisualElement>("HealthBarFill2");
        _shieldFill1 = root.Q<VisualElement>("ShieldBarFill1");
        _shieldFill2 = root.Q<VisualElement>("ShieldBarFill2");
        
    }

    //i = 0 for Harley
    //i = 1 for Perry
    public void UpdateHealthBar(int index)
    {
        if (_fill1 == null || _fill2 == null) {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _fill1 = root.Q<VisualElement>("HealthBarFill1");
            _fill2 = root.Q<VisualElement>("HealthBarFill2");
        }
        float maxHp;
        float currentHp;
        float normalizedHealth = 1f;
        
        if (index == 0)
        {
            maxHp = HarleyPlayer.Instance.MaxHealth;
            currentHp = HarleyPlayer.Instance.Health;
            normalizedHealth = currentHp / maxHp;
            normalizedHealth = Mathf.Clamp01(normalizedHealth);
        
            _fill1.style.width = Length.Percent(normalizedHealth * 100);
            _fill1.style.backgroundColor = Color.Lerp(Color.red, Color.green, normalizedHealth);
        } else if (index == 1)
        {
            maxHp = PerryPlayer.Instance.MaxHealth;
            currentHp = PerryPlayer.Instance.Health;
            normalizedHealth = currentHp / maxHp;
            normalizedHealth = Mathf.Clamp01(normalizedHealth);
        
            _fill2.style.width = Length.Percent(normalizedHealth * 100);
            _fill2.style.backgroundColor = Color.Lerp(Color.red, Color.green, normalizedHealth);
        }

        
        
    }

    void Update()
    {
        /*UpdateHealthBar(0);
        UpdateHealthBar(1);
        Debug.Log(gameObject.name);
        Debug.Log(gameObject.name);*/
    }

    public void UpdateShieldBar(float normalizedShield)
    {
        normalizedShield = Mathf.Clamp01(normalizedShield);
        
        if (_shieldFill1 != null)
            _shieldFill1.style.width = Length.Percent(normalizedShield * 100);
    }
}