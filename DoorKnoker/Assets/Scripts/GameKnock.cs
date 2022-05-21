using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameKnock : MonoBehaviour
{
    [SerializeField] private float timeSpeed;

    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textCountKnock;
    [SerializeField] private Slider timer;
    [SerializeField] private float startTime;
    [SerializeField] private float time;

    private int score;
    private int knockCount;

    public int KnockCount { get { return knockCount; } 
        private set
        {
            knockCount = value;
            textCountKnock.text = knockCount.ToString();
        } 
    }

    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            textScore.text = score.ToString();
        }
    }

    private Camera m_camera;

    private void Start()
    {
        m_camera = Camera.main;

        StartCoroutine(SelectObject());
        StartCoroutine(Timer());
    }

    private IEnumerator SelectObject()
    {
        Generator.Instance.Generate();

        textCountKnock.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.01f);

        while (true)
        {
            if (Input.anyKeyDown)
            {
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent(out Door m_door))
                    {
                        KnockCount = m_door.KnockCount + 1;
                        StartCoroutine(KnockDoor());
                        break;
                    }
                    else if (hit.transform.TryGetComponent(out DeadZone m_deadZone))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator KnockDoor()
    {
        textCountKnock.gameObject.SetActive(true);

        while (true)
        {
            if (Input.anyKeyDown)
            {
                KnockCount--;

                if (KnockCount <= 0)
                {
                    time += time + 1 < startTime ? 1 : startTime - time;
                    Score++;

                    StartCoroutine(SelectObject());
                    break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator Timer()
    {
        timer.maxValue = startTime;

        time = startTime;

        while (true)
        {
            time -= 1 * timeSpeed;

            if (time < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            }

            timer.value = time;

            yield return new WaitForSeconds(1 * timeSpeed);
        }
    }
}
