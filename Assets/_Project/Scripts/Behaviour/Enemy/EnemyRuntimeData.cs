using UnityEngine;

public class EnemyRuntimeData : MonoBehaviour
{
    private EnemyData enemyData = new();
    public EnemyData EnemyData => enemyData;

    [SerializeField] private EnemyMaterials enemyMaterials;
    public EnemyMaterials EnemyMaterials => enemyMaterials;

    public void OnStart()
    {
        enemyData.SetDataForEnemy();
    }

    #region Audio

    public void EnemyPunchWindSound()
    {
        var random = Random.Range(0.7f, 1f);
        SoundManager.Instance.PlaySound(SoundKey.PunchWind, 0.7f, random);
    }

    public void EnemyTrainingYellSound()
    {
        SoundManager.Instance.PlaySound(SoundKey.WarmingUpYell_01, 0.7f, 0.7f);
    }

    #endregion
}

[System.Serializable]
public class EnemyMaterials
{
    public SkinnedMeshRenderer headSkin;
    public SkinnedMeshRenderer handSkin;
    public SkinnedMeshRenderer bodySkin;
    public SkinnedMeshRenderer legSkin;
    public SkinnedMeshRenderer shoeLSkin;
    public SkinnedMeshRenderer shoeRSkin;

    public void ChangMaterialForEnemy(Material material)
    {
        if (material == null) return;

        headSkin.material = material;
        handSkin.material = material;
        bodySkin.material = material;
        legSkin.material = material;
        shoeLSkin.material = material;
        shoeRSkin.material = material;
    }
}

public enum Enemy_Difficult
{
    Easy = 0, 
    Med = 1, 
    Hard = 2
}