using UnityEngine;
using UnityEngine.UI;
namespace TestFarm
{
    public class Progressbar : MonoBehaviour
    {
        public Image porgressbar;
        public void ShowPorgress(float progress)
        {
            porgressbar.fillAmount = progress;
        }
    }
}