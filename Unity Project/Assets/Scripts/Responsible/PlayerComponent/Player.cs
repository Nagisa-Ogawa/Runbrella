using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    // プレイヤーのナンバー
    public PLAYER_NO playerNO { get; set; } = 0;
    // キャラクターのタイプ
    public string Type { get; set; } = "A";
    // プレイヤーがダウンしている時間
    public float downTime = 0;
    // タイムライン中かどうか
    public bool IsTimeLine { get; set; } = true;
    // プレイヤーのStateManager
    public PlayerStateManager playerStateManager;

    // 雨に当たっているか
    [SerializeField]
    public bool IsRain = false;
    [SerializeField]
    protected float maxVelocityY = 15f;
    // プレイヤーの速度保存領域
    public float VelocityXStorage { get; set; } = 0;
    protected PlayerSlide playerSlide;
    // アニメーター
    protected Animator animator;
    // 地面にいるか    
    [SerializeField]
    protected bool isGround = false;
    public bool IsGround { set { isGround = value; } get { return isGround; } }
    // リジッドボディ
    protected Rigidbody2D rigidBody;
    public Rigidbody2D Rigidbody { get { return rigidBody; } }


    // AnimatorのパラメーターID
    protected readonly int jumpID = Animator.StringToHash("IsGround");
    protected readonly int runID = Animator.StringToHash("Velocity");
    protected readonly int sliderID = Animator.StringToHash("IsSlider");
    protected readonly int gliderID = Animator.StringToHash("IsGlide");
    protected readonly int downID = Animator.StringToHash("IsDown");
    protected readonly int boostID = Animator.StringToHash("IsBoost");

    // プレイヤーの種類
    public GameManager.CHARTYPE charType;
    // プレイヤーの攻撃手段の種類
    public GameManager.CHARATTACKTYPE charAttackType;

    // 雨を受けているときのエフェクト
    public ParticleSystem feverEffect;
    // ブースト時のエフェクト
    public ParticleSystem boostEffect;
    // チャージ中エフェクト
    public ParticleSystem chargeingEffect;
    // 一段階チャージした際のエフェクト
    public ParticleSystem chargeSignal;
    // チャージが停止中のエフェクト
    public ParticleSystem chargePauseEffect;
    // チャージがMAXの時のエフェクト
    public ParticleSystem chargeMaxEffect;


#if UNITY_EDITOR
    // ステートの名前をデバッグ表示する変数
    [SerializeField]
    protected string stateName;
#endif

    #region ステート変数
    private AerialState aerialState;
    private AfterSlideState afterSlideState;
    private BoostState boostState;
    private DownState downState;
    private GlideState glideState;
    private IdleState idleState;
    private RunState runState;
    private SlideState slideState;
    private AfterGoalState afterGoalState;
    #endregion
    // プレイヤーのコントローラナンバー
    public CONTROLLER_NO controllerNo { protected get; set; } = 0;


    #region ステートを変更するためのアクセサメソッド
    public void IdleStart()
    {
        playerStateManager.ChangeState(idleState);
    }
    public void RunStart()
    {
        playerStateManager.ChangeState(runState);
    }
    public void AerialStart()
    {
        playerStateManager.ChangeState(aerialState);
    }
    public void GlideStart()
    {
        playerStateManager.ChangeState(glideState);
    }
    public void SlideStart()
    {
        playerStateManager.ChangeState(slideState);
    }
    public void AfterSlideStart()
    {
        playerStateManager.ChangeState(afterSlideState);
    }
    public void BoostStart()
    {
        playerStateManager.ChangeState(boostState);
    }
    public void Down()
    {
        playerStateManager.ChangeState(downState);
    }
    public void AfterGoalStart()
    {
        playerStateManager.ChangeState(afterGoalState);
    }
    #endregion
    #region 現在のステートを確認するためのget
    public bool IsIdle { get { return playerStateManager.GetState() == (IState)idleState; } }
    public bool IsRun { get { return playerStateManager.GetState() == (IState)runState; } }
    public bool IsAerial { get { return playerStateManager.GetState() == (IState)aerialState; } }
    public bool IsGlide { get { return playerStateManager.GetState() == (IState)glideState; } }
    public bool IsSlide { get { return playerStateManager.GetState() == (IState)slideState; } }
    public bool IsAfterSlide { get { return playerStateManager.GetState() == (IState)afterSlideState; } }
    public bool IsBoost { get { return playerStateManager.GetState() == (IState)boostState; } }
    public bool IsDown { get { return playerStateManager.GetState() == (IState)downState; } }
    public bool IsAfterGoal { get { return playerStateManager.GetState() == (IState)afterGoalState; } }
    public bool IsAttack { get; /*{ return InputManager.Instance.AttackKeyIn(controllerNo); } */}

    #endregion
    #region 特定のアクションを行うか
    public bool IsJumpStart { get { return InputManager.Instance.JumpKeyIn(controllerNo); } }
    public bool IsGlideStart { get { return InputManager.Instance.StartGlidingKeyIn(controllerNo); } }
    public bool IsGlideEnd { get { return InputManager.Instance.EndGlidingKeyIn(controllerNo); } }
    public bool IsSlideStart { get { return InputManager.Instance.ActionKeyIn(controllerNo); } }
    public bool IsSlideEnd { get { return InputManager.Instance.ActionKeyIn(controllerNo); } }
    public bool IsCharge { get { return InputManager.Instance.ShotAndBoostKeyIn(controllerNo); } }
    public bool IsBoostStart { get { return InputManager.Instance.ShotAndBoostKeyOut(controllerNo); } }
    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        // 子オブジェクトを取得
        feverEffect = transform.Find("FeverEffect").GetComponent<ParticleSystem>();
        boostEffect = transform.Find("BoostEffect").GetComponent<ParticleSystem>();
        chargeingEffect = transform.Find("ChargeEffects/Charging").GetComponent<ParticleSystem>();
        chargeSignal = transform.Find("ChargeEffects/ChargeSignal").GetComponent<ParticleSystem>();
        chargePauseEffect = transform.Find("ChargeEffects/ChargePause").GetComponent<ParticleSystem>();
        chargeMaxEffect = transform.Find("ChargeEffects/ChargeMax").GetComponent<ParticleSystem>();

        // アタッチされているステートを取得
        aerialState = GetComponent<AerialState>();
        afterSlideState = GetComponent<AfterSlideState>();
        boostState = GetComponent<BoostState>();
        downState = GetComponent<DownState>();
        glideState = GetComponent<GlideState>();
        idleState = GetComponent<IdleState>();
        runState = GetComponent<RunState>();
        slideState = GetComponent<SlideState>();
        afterGoalState = GetComponent<AfterGoalState>();

        // ステートを管理するクラスを取得
        playerStateManager = new PlayerStateManager();
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSlide = GetComponent<PlayerSlide>();
        ReadTextParameter();
    }

    private void Update()
    {
        playerStateManager.Update();
        Do_AnyState();
#if UNITY_EDITOR
        if (playerStateManager.GetState() == null)
        {
            return;
        }
        // 現在のステートをInspecter上に表示
        stateName = playerStateManager.GetState().ToString();
#endif

    }

    void FixedUpdate()
    {
        // stateのDo_Fix関数を呼ぶ
        playerStateManager.FixedUpdate();
        Do_Fix_AniState();
    }

    /// <summary>
    /// textからパラメータを読み込む関数
    /// </summary>
    private void ReadTextParameter()
    {
        // 読み込むテキストの名前
        var charaATextName = "Chara_A";
        var charaBTextName = "Chara_B";
        // テキストの中のデータをセットするディクショナリー
        Dictionary<string, float> charaADictionary;
        Dictionary<string, float> charaBDictionary;
        SheetToDictionary.Instance.TextToDictionary(charaATextName, out charaADictionary);
        SheetToDictionary.Instance.TextToDictionary(charaBTextName, out charaBDictionary);
        try
        {
            // ファイル読み込み
            if (charType == GameManager.CHARTYPE.PlayerA)
            {
                downTime = charaADictionary["プレイヤーのダウンしている時間"];
            }
            else
            {
                downTime = charaBDictionary["プレイヤーがダウンしている時間"];
            }

        }
        catch
        {
            Debug.Assert(false, nameof(Player) + "でエラーが発生しました");
        }
    }

    /// <summary>
    /// アニメーターにパラメータをセット
    /// </summary>
    void SetAnimator()
    {
        // タイムライン中でなければ
        if (IsTimeLine == false)
        {
            animator.SetBool(jumpID, isGround);
            animator.SetBool(sliderID, IsSlide || IsAfterSlide);
            animator.SetFloat(runID, Mathf.Abs(rigidBody.velocity.x));
            animator.SetBool(gliderID, IsGlide);
            animator.SetBool(downID, IsDown);
            animator.SetBool(boostID, IsBoost);
        }
    }

    /// <summary>
    /// どのステートでも共通して行う処理です
    /// </summary>
    public void Do_AnyState()
    {
        Do_Rainy();
        // アニメーターにパラメータをセット
        SetAnimator();
    }

    /// <summary>
    /// どのステートでも共通して行う物理処理です
    /// </summary>
    public void Do_Fix_AniState()
    {
        RimitScreenTop();
        //RimitVelocityY();

    }

    /// <summary>
    /// 画面上部に到達した際、それ以上上にいかないようにする処理です
    /// </summary>
    void RimitScreenTop()
    {
        var ScreenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y;
        if (transform.position.y > ScreenTop)
        {
            transform.position = new Vector2(transform.position.x, ScreenTop);
            if (rigidBody.velocity.y > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            }
        }

    }
    /// <summary>
    /// 上方向への速度を制限する処理
    /// </summary>
    void RimitVelocityY()
    {
        if (rigidBody.velocity.y > maxVelocityY)
        {

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxVelocityY);
        }
    }

    /// <summary>
    /// 雨に打たれた際の処理
    /// </summary>
    void Do_Rainy()
    {
        if (IsRain)
        {
            PlayEffect(feverEffect);
        }
        else
        {
            StopEffect(feverEffect);
        }
    }


    /// <summary>
    /// エフェクトの再生処理
    /// </summary>
    /// <param name="effect"></param>
    public void PlayEffect(ParticleSystem effect)
    {
        if (effect.isPlaying)
        {
            return;
        }
        effect.Play();
    }

    /// <summary>
    /// エフェクトの停止処理
    /// </summary>
    /// <param name="effect"></param>
    public void StopEffect(ParticleSystem effect)
    {
        if (effect.isStopped)
        {
            return;
        }
        effect.Stop();
    }

}
