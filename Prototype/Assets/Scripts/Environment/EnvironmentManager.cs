using UnityEngine;
using System.Collections;
using Photon.Pun;

public class EnvironmentManager : MonoBehaviour
{
    // This will be triggered by 1 of the abilities
    [SerializeField] GameObject dustDusk;
    ParticleSystem[] duskDuskParticles;

    PhotonView photonView;

    [SerializeField] float team1SpawnPosX = -11;
    [SerializeField] float team2SpawnPosX = 11;

    float dustDuskDuration;
    float dustDuskDeactivationDelay = 6f;

    // This will help us position the walls and area limiters in the same pla
    public Vector2 environmentSize;

    public static EnvironmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();

        // Let the master client decide what the environment size is based on it's screen size
        // and then update the rest of the clients with this value
        if (PhotonNetwork.IsMasterClient)
        {
            environmentSize = Utils.Instance.CameraScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            photonView.RPC("SetEnvironmentSize", RpcTarget.OthersBuffered, environmentSize.x, environmentSize.y);
        }
    }

    private void Start()
    {
        // Get duration for the Dust Dusk
        dustDuskDuration = AbilityDataCache.GetDataForAbility("Dust Dusk").stats.duration;

        // Store all the particle systems from the Dusk Dusk so we can stop the and the disable the DD object
        int duskDustParticlesSize = dustDusk.transform.childCount + 1;
        duskDuskParticles = new ParticleSystem[duskDustParticlesSize];

        Debug.Log("EnvironmentManager Start Dust Dusk duskDustParticlesSize" + duskDustParticlesSize);
        Debug.Log("EnvironmentManager Start Dust Dusk ParticleSystems in children" + dustDusk.GetComponentsInChildren<ParticleSystem>().Length);

        // Cache the particlesystems from the children
        int index = 0;
        foreach (ParticleSystem pS in dustDusk.GetComponentsInChildren<ParticleSystem>())
        {
            Debug.Log("EnvironmentManager Start Dust Dusk Index " + index);
            duskDuskParticles[index] = pS;
            index++;
        }

    }

    [PunRPC]
    void SetEnvironmentSize(float x, float y)
    {
        environmentSize = new Vector2(x, y);
    }

    public void TriggerDustDusk(int duration, int teamID)
    {
        photonView.RPC("ActivateDustDusk", RpcTarget.Others, teamID);
    }

    [PunRPC]
    void ActivateDustDusk(int casterTeamID)
    {
        // We can add a fade here later
        if (Player.localTeamID != casterTeamID)
        {
            ActivateDustDusk();
            Invoke("StopDustDusk", dustDuskDuration);
        }
    }

    void ActivateDustDusk()
    {
        dustDusk.SetActive(true);
        foreach (ParticleSystem pS in duskDuskParticles)
        {
            pS.Play();
        }
    }

    void StopDustDusk()
    {
        foreach (ParticleSystem pS in duskDuskParticles)
        {
            pS.Stop();
        }

        Invoke("DeactivateDustDusk", dustDuskDeactivationDelay);
    }

    void DeactivateDustDusk()
    {
        dustDusk.SetActive(false);
    }

    public Vector3 GetPlayerSpawnPoint(int teamID)
    {
        float spawnY = Random.Range
                (Utils.Instance.CameraScreenToWorldPoint(new Vector2(0, 0)).y, Utils.Instance.CameraScreenToWorldPoint(new Vector2(0, Screen.height)).y);

        spawnY /= 2;

        if(teamID == 1)
        {
            return new Vector3(team1SpawnPosX, spawnY, 0);
        }
        else
        {
            return new Vector3(team2SpawnPosX, spawnY, 0);
        }
    }
}
