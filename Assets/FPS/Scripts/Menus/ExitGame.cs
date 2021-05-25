using UnityEngine;


namespace Shooter.Menus
{
    public class ExitGame : MonoBehaviour
    {

        public void ExitToDesktop()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }

    }

}
