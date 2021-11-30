using Arcaedion.Multiplayer;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instancia { get; private set; }
    

    [SerializeField] private string _localizacaoPrefab;
    [SerializeField] private Transform[] _spawns;
    private int _jogagoresEmJogo = 0;
    private List<ControleJogadorNet> _jogadores;
    public List<ControleJogadorNet> Jogadores 
    { 
        get => _jogadores; 
        private set => _jogadores = value; 
    }

    private void Awake()
    {
        if(Instancia != null && Instancia != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        photonView.RPC("AdicionaJogador", RpcTarget.AllBuffered);
        _jogadores = new List<ControleJogadorNet>();
    }

    [PunRPC]
    private void AdicionaJogador()
    {
        _jogagoresEmJogo++;
        if(_jogagoresEmJogo == PhotonNetwork.PlayerList.Length)
        {
            CriaJogador();
        }
    }

    private void CriaJogador()
    {
        var jogadorObj = PhotonNetwork.Instantiate(_localizacaoPrefab, _spawns[UnityEngine.Random.Range(0, _spawns.Length)].position, Quaternion.identity);
        var jogagor = jogadorObj.GetComponent<ControleJogadorNet>();

        jogagor.photonView.RPC("Inicializa", RpcTarget.All, PhotonNetwork.LocalPlayer);

    }
}
