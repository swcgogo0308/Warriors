using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public Scrollbar HealthBar;
    public float Health = 10;

    public void Damage(float value)
    {
        Health -= value;
        HealthBar.size = Health / 100f;
    }
}