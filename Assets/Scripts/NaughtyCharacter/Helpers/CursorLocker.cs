using UnityEngine;

namespace NaughtyCharacter.Helpers
{
    public class CursorLocker : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
