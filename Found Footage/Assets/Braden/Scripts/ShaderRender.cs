using UnityEngine;

//[ExecuteInEditMode]
public class ShaderRender : MonoBehaviour
{
    public RenderTexture texture;
    public Material material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("yes");
        Graphics.Blit(texture, material);
        //Graphics.Blit(source, destination, material);
    }
}