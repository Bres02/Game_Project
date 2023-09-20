using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StamManager : MonoBehaviour
{
    public static PlayerController pc;
    [SerializeField] private Gradient staminaGradient;
    [SerializeField] private Gradient exhaustedGradient;
    public float startAmount, maxAmount = 100f;
    public float currentAmount;
    public float refreshPercent = 0.75f;
    [SerializeField] private Image staminaBarA;
    /*[SerializeField] private Image staminaBarB;
    [SerializeField] private Image staminaBarC;*/
    public bool canRun = true;


    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentAmount = startAmount;
    }

    //regens the stamina bar and values when the player is moving at the walkspeed
    void Update()
    {
        if(pc.MoveSpeed == pc.WalkSpeed)
        {
            if (!canRun)
            {
                staminaRegen(pc.Regen);
            }
            else
            {
                staminaRegen((pc.Regen * 1.5f));
            }
        }
        exhausted();
        getStamina();
        
    }

    //Consumes the stamina bar based on the cost variable
    public void consumeStamina(float cost)
    {
        currentAmount -= cost * Time.deltaTime;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        staminaBarA.fillAmount = currentAmount / maxAmount;
    }

    //Regens the stamina bar based on regenAmount variable
    public void staminaRegen(float regenAmount)
    {
        currentAmount += regenAmount * Time.deltaTime;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        staminaBarA.fillAmount = currentAmount / maxAmount;
        /*staminaBarB.fillAmount = staminaBarA.fillAmount;
        staminaBarC.fillAmount = staminaBarA.fillAmount;*/
    }
    
    //Used to call gradient method and update current variable for other uses
    public float getStamina()
    {
        GradientBarAmount();
        return currentAmount;
    }

    //Determine if a player can run provided they don't consume the entire stamina bar
    public void exhausted()
    {
        if (currentAmount <= 0.01f)
        {
            canRun = false;
        }
        else if (currentAmount >= maxAmount*refreshPercent)
        {
            canRun = true;
        }
    }

    //Sets the stamina bar as one of two gradients base on whether or not the player is able to use stamina
    private void GradientBarAmount()
    {
        if (canRun)
        {
            staminaBarA.color = staminaGradient.Evaluate((currentAmount / maxAmount));
        }
        else if (!canRun)
        {
            staminaBarA.color = exhaustedGradient.Evaluate((currentAmount / maxAmount));
        }
        /*staminaBarB.color = staminaBarA.color;
        staminaBarC.color = staminaBarA.color;*/
    }

}
