using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;

namespace PixelCrew.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            GameSession session = FindObjectOfType<GameSession>();
            session.LoadLastSave();

            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

