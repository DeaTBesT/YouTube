using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textKnockCount;

    private int knockCount;

    public int KnockCount { get { return knockCount; } 
        set 
        {
            knockCount = value;
            textKnockCount.text = knockCount.ToString();
        } 
    }
}
