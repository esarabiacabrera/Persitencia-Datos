using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    [SerializeField] Text UsuarioPuntaje;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int puntajeGuardado;
    private String usuarioGuardado;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadDataUsuario();
        
         
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        LoadDataUsuario();
        if(m_Points>puntajeGuardado) {

            DatosScene.Instance.mejorPuntaje=m_Points;
            SaveDataUsuario();
        }
            

        UsuarioPuntaje.text="Best Score: "+usuarioGuardado+": "+puntajeGuardado;
        
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData{
        public String nombreUsuario;
        public int mejorPuntaje;
    }

    public void SaveDataUsuario()
    {
        SaveData data = new SaveData();
        data.nombreUsuario = DatosScene.Instance.nombreUsuario;
        data.mejorPuntaje=DatosScene.Instance.mejorPuntaje;

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefileUsuario.json", json);
    }

    public void LoadDataUsuario()
    {
        string path = Application.persistentDataPath + "/savefileUsuario.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            puntajeGuardado=data.mejorPuntaje;
            usuarioGuardado=data.nombreUsuario;
            UsuarioPuntaje.text="Best Score: "+data.nombreUsuario+": "+data.mejorPuntaje;
        }
    }


}
