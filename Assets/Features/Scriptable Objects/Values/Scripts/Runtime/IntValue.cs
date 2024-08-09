using UnityEngine;

[CreateAssetMenu(fileName = nameof(IntValue), menuName = ("Custom/Scriptable Objects/Values/" + nameof(IntValue)))]
public class IntValue : BaseValue<int> { 

    // Threw this in here specifically to work with the slider in ConwaysGame
    public void SetValueWithFloat(float newValue)
    {
        this.Value = (int)newValue;
    }

    // ConwaysGame specific
    // Safety/type checks are done by TMP's InputField class, I'm only accepting integers
    public void SetValueWithString(string newValue)
    {
        if (newValue == string.Empty)
            return;

        this.Value = int.Parse(newValue);
    }
}
