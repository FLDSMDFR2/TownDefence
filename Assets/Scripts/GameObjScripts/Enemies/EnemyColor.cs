using UnityEngine;

public class EnemyColor : MonoBehaviour
{
    public Color GetEnemeyColorByLevel(int level)
    {

        if (level == 0)
            level = 1;

        Random.InitState(level);

        float r = Mathf.Clamp(Random.value, 0f, 1f);
        float g = Mathf.Clamp(Random.value, 0f, 1f);
        float b = Mathf.Clamp(Random.value, 0f, 1f);

        float a = 255f ;

        return new Color(r,g,b,a);
    }
}
