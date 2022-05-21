using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Object[] objects;
    private int countDeadZones;

    [System.Serializable]
    public struct Object
    {
        public GameObject[] avaibleObjects;
    }

    public static Generator Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Generate()
    {
        DiactivateAll();

        foreach (Object m_object in objects)
        {
            int randomID = Random.Range(0, m_object.avaibleObjects.Length);
            
            if (m_object.avaibleObjects[randomID].TryGetComponent(out Door m_door))
            {
                m_door.KnockCount = Random.Range(1, 10);
                m_object.avaibleObjects[randomID].SetActive(true);
            }
            else if (m_object.avaibleObjects[randomID].TryGetComponent(out DeadZone m_deadZone))
            {
                countDeadZones++;

                if (countDeadZones > 2) { m_object.avaibleObjects[0].SetActive(true); }

                m_object.avaibleObjects[randomID].SetActive(true);
            }
        }
    }

    private void DiactivateAll()
    {
        countDeadZones = 0;

        foreach (Object m_object in objects)
        {
            foreach (GameObject m_gameObject in m_object.avaibleObjects)
            {
                m_gameObject.SetActive(false);
            }
        }
    }
}
