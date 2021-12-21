using Features.Simple_Sprite_Exploder_Without_Physics.Scripts;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteExploderBehaviour))]
public class SpriteExploderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SpriteExploderBehaviour spriteExploder = (SpriteExploderBehaviour)target;

        spriteExploder.PixelPrefab = (GameObject)EditorGUILayout.ObjectField("Pixel Prefab", spriteExploder.PixelPrefab, typeof(GameObject), true);
        
        //Pixel Generation
        EditorGUILayout.BeginVertical();
        string genText;
        string genButtonText;
        if (spriteExploder.HasGeneratedSprite && spriteExploder.transform.Find("ExplosionPixels") != null)
        {
            genText = "You have generated your explosion pixels.";
            genButtonText = "Regenerate Pixels";
        }
        else
        {
            genText = "Press the button below to generate the explosion pixels.";
            genButtonText = "Generate Pixels";
        }

        if (spriteExploder.PixelPrefab == null)
            genText = "Please assign a pixel prefab to generate the explosion pixels.";

        EditorGUILayout.LabelField(genText);

        if (spriteExploder.PixelPrefab != null)
        {
            if (GUILayout.Button(genButtonText))
            {
                spriteExploder.GenerateSprite();
            }
        }
        EditorGUILayout.EndVertical();

        //Explosion Settings
        if (spriteExploder.HasGeneratedSprite && spriteExploder.transform.Find("ExplosionPixels") != null)
        {
            spriteExploder.EnableFloorCollision = EditorGUILayout.Toggle("Enable Floor Colision", spriteExploder.EnableFloorCollision);
            if (spriteExploder.EnableFloorCollision)
                spriteExploder.EnergyLostOnFloorCollision = EditorGUILayout.Slider("Energy Lost On Floor Collision", spriteExploder.EnergyLostOnFloorCollision, 0, 1);

            spriteExploder.LeanTweenType = (LeanTweenType)EditorGUILayout.EnumPopup("Move Type", spriteExploder.LeanTweenType);

            spriteExploder.ExplosionForce = EditorGUILayout.FloatField("Explosion Force", spriteExploder.ExplosionForce);
            if (spriteExploder.ExplosionForce < 0)
                spriteExploder.ExplosionForce = 0;
            spriteExploder.ExplosionRandomness = EditorGUILayout.Slider("Explosion Randomness", spriteExploder.ExplosionRandomness, 0, spriteExploder.ExplosionForce);
            if (spriteExploder.ExplosionRandomness < 0)
                spriteExploder.ExplosionRandomness = 0;
            if (spriteExploder.ExplosionRandomness > spriteExploder.ExplosionForce)
                spriteExploder.ExplosionRandomness = spriteExploder.ExplosionForce;


            spriteExploder.PixelLifespan = EditorGUILayout.FloatField("Pixel Lifespan", spriteExploder.PixelLifespan);
            if (spriteExploder.PixelLifespan < 0)
                spriteExploder.PixelLifespan = 0;
            spriteExploder.PixelLifespanRandomness = EditorGUILayout.Slider("Pixel Lifespan Randomness", spriteExploder.PixelLifespanRandomness, 0, spriteExploder.PixelLifespan);
            if (spriteExploder.PixelLifespanRandomness < 0)
                spriteExploder.PixelLifespanRandomness = 0;
            if (spriteExploder.PixelLifespanRandomness > spriteExploder.PixelLifespan)
                spriteExploder.PixelLifespanRandomness = spriteExploder.PixelLifespan;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            if (!Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }

}
