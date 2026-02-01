using CHAVIS;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerUps : MonoBehaviour
{
    [SerializeField] SpriteRenderer puImageRenderer;
    [SerializeField] TextMeshPro puTextRenderer;
    

    private PowerUpsSO puInfo;
    public void SetUp(PowerUpsSO powerUp)
    {
        puInfo = powerUp;
        puImageRenderer.sprite = powerUp.image;
        puTextRenderer.text = powerUp.description;
    }

    public void Selectpowerup()
    {
        Debug.Log("Powerup selected");
        AudioManager.Instance?.PlayInGameSelect();
        PowerUpManager.Instance.SelectPowerUp(puInfo);
        PlayerMovement.Instance.ApplyPU(puInfo);
        Time.timeScale = 1f;
    }
    
}
