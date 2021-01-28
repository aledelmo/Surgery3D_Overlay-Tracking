using System;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.Rendering;

namespace IO
{
    public class AsyncReadback : MonoBehaviour
    {
        public VideoPlayer vp;
        public string frame;

        private IEnumerator Start()
        {
            for (;;)
            {
                yield return new WaitForSeconds(1f);
                yield return new WaitForEndOfFrame();
                
                var vpTexture = vp.texture;
                var rt = RenderTexture.GetTemporary(vpTexture.width, vpTexture.height,
                    0, RenderTextureFormat.ARGB32);
                Graphics.Blit(vpTexture, rt);

                AsyncGPUReadback.Request(rt, 0, TextureFormat.ARGB32, OnCompleteReadback);
            }
        }
        
        private void OnCompleteReadback(AsyncGPUReadbackRequest request)
        {
            if (request.hasError)
            {
                Debug.Log("GPU readback error detected.");
            }
            
            var vpTexture = vp.texture;
            
            var tex = new Texture2D(vpTexture.width, vpTexture.height, TextureFormat.ARGB32, false);
            tex.LoadRawTextureData(request.GetData<uint>());
            tex.Apply();
            frame = Convert.ToBase64String(tex.EncodeToPNG());
        }
    }
}