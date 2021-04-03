using UnityEngine;

[System.Serializable]
public class CreditData 
{
    [SerializeField] private string text; //choice display text
    [SerializeField] private bool isCredit; //choice display text
    [SerializeField] private float typingTime; //choice display text

    public string CreditText { get { return text; } }
    public bool IsCredit { get { return isCredit; } }
    public float TypeTime { get { return typingTime; } }
}
