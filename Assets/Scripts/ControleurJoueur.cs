using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleurJoueur : MonoBehaviour
{
    public float vitesse;
    public Text countText;
    public Text winText; // Usaremos winText para exibir as instruções
    public GameObject[] cibles; // Array para armazenar todas as "Cibles"
    private Rigidbody rb;
    private int count;
    private Vector3[] cibleInitialPositions; // Array para armazenar as posições iniciais das "Cibles"

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "Use as setas para começar a jogar";
        winText.gameObject.SetActive(true); // Garante que o texto esteja visível

        // Inicializa o array de posições iniciais das "Cibles"
        cibleInitialPositions = new Vector3[cibles.Length];
        for (int i = 0; i < cibles.Length; i++)
        {
            cibleInitialPositions[i] = cibles[i].transform.position;
        }
    }

    void FixedUpdate()
    {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 mouvement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(mouvement * vitesse);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cible"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            // Verifica se o contador é um múltiplo de 8 para reiniciar as "Cibles"
            if (count % 8 == 0)
            {
                StartCoroutine(ResetCiblesAfterDelay(1f)); // Reiniciar as "Cibles" após 1 segundo
            }
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }

    IEnumerator ResetCiblesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Desativar interação física das "Cibles" temporariamente
        foreach (GameObject cible in cibles)
        {
            Collider cibleCollider = cible.GetComponent<Collider>();
            if (cibleCollider != null)
            {
                cibleCollider.enabled = false;
            }
        }

        // Aguardar um curto período antes de reativar e reposicionar as "Cibles"
        yield return new WaitForSeconds(0.1f);

        // Ativar e reposicionar todas as "Cibles" novamente no mesmo local onde estavam
        for (int i = 0; i < cibles.Length; i++)
        {
            cibles[i].transform.position = cibleInitialPositions[i];
            cibles[i].SetActive(true);
        }

        // Reativar os colliders das "Cibles"
        foreach (GameObject cible in cibles)
        {
            Collider cibleCollider = cible.GetComponent<Collider>();
            if (cibleCollider != null)
            {
                cibleCollider.enabled = true;
            }
        }
    }

    void HideInstructions()
    {
        winText.gameObject.SetActive(false); // Esconder as instruções
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // Iniciar jogo ao pressionar seta para cima
        {
            HideInstructions();
        }
    }
}
