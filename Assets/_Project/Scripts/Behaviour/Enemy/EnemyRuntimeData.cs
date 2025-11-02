using UnityEngine;

public class EnemyRuntimeData : MonoBehaviour
{
    private EnemyData enemyData = new();
    public EnemyData EnemyData => enemyData;

    [SerializeField] private EnemyMaterials enemyMaterials;
    public EnemyMaterials EnemyMaterials => enemyMaterials;

    [SerializeField] private Enemy_Difficult enemy_Difficult; 
    public Enemy_Difficult Enemy_Difficult => enemy_Difficult; 


    public void OnStart()
    {
        enemyData.SetDataForEnemy();
    }


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
    Easy, Med, Hard
}