using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LightFuse : MonoBehaviour, IBreakable, Controls.IInteractionsActions,IFixable
{
    private Lamp parentLamp;
    public FuseSettings fuseSettings;
    private bool canFix;
    private bool isFixing =false;
    private ChargingCable fixingCable;
    public Color fixingCableColour;
    private Transform targetTrans;
    public float currentTimeToFix;
    private Controls input;
    private GameObject player;
    protected AudioSource audioSource;
    [SerializeField] protected GameObject audioPlayerPrefab;
    protected AudioPlayer audioPlayer;
    [SerializeField] protected GameObject damageVFX;
    [SerializeField] private Transform cablePoint;
    private void Awake()
    {
        parentLamp = transform.parent.GetComponent<Lamp>();
        fixingCable = gameObject.GetComponent<ChargingCable>();

        //Inputs
        input = new Controls();
        input.Interactions.SetCallbacks(this);
        input.Enable();
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            Sound sound = AudioManager.instance.GetSound("DeployCableSFX");
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
        }
    }

    private void Update()
    {

        if (isFixing)
        {
            if (!parentLamp.GetIsFixed())
            {
                fixingCable.ChangeColour(fixingCableColour);
                if (currentTimeToFix <= 0)
                {
                    parentLamp.FixLamp(10f);
                    currentTimeToFix = fuseSettings.repairRate;
                }
                else
                {
                    currentTimeToFix -= Time.deltaTime;
                }
            }
            else
            {
                player.GetComponent<IFixable>().NotFixing();
                fixingCable.ChangeColour(Color.green);
                isFixing = false;
                if (audioPlayer != false)
                {

                    audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("ObjectFixed"));
                    audioPlayer.Play();
                    audioPlayer = null;
                }
            }
            
           
        }
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFixing)
            {
                targetTrans = other.gameObject.transform;

                canFix = !parentLamp.GetIsLampWorking();
           
                if (canFix)
                {
                    InGamePrompt.instance.ChangePrompt("[E] To Fix Light");
                    InGamePrompt.instance.ShowPrompt();
           
                    fixingCable.ChangeColour(fixingCableColour);
                    player = other.gameObject;

                }


            }
    
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFixing)
            {
                canFix = !parentLamp.GetIsLampWorking();
       
            }
         
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            canFix = !parentLamp.GetIsLampWorking();
            isFixing = false;
            InGamePrompt.instance.HidePrompt();
            if(targetTrans != null){

                fixingCable.StopDrawingRope();
                targetTrans = null;

                if (player != false)
                {
                    player.GetComponent<IFixable>().NotFixing();
                    player = null;
                }
                if (audioPlayer != false)
                {
                    audioPlayer.KillAudio();
                    audioPlayer = null;
                }
           

            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed&& canFix)
        {
            if (player != false)
            {
                if (player.GetComponent<IFixable>().CanFix())
                {
                    
                    isFixing = true;
                    InGamePrompt.instance.HidePrompt();
                    if (targetTrans != null)
                    {
                        fixingCable.StartDrawingRope(targetTrans);
                        audioSource.Play();
                        audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
                        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("ChargingCableSFX"));
                        audioPlayer.Play();
                    }
                }
            }
         
        }
    }
    void IBreakable.Damage(float damage,BaseEnemy interfacingEnemy)
    {
        parentLamp.DamageLamp(damage);
        if (damageVFX)
            SpawnDamgeVFX(interfacingEnemy.transform);

        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("ApplianceAttacked"));
        audioPlayer.Play();
        if (!parentLamp.GetIsLampWorking())
        {
            interfacingEnemy.GetComponent<IBreakable>().ObjectIsBroken();
        }
    }
    public void SpawnDamgeVFX(Transform attacker)
    {
        Vector3 right = attacker.right * -1.0f;

        Quaternion rotation = Quaternion.Euler(right.x, right.y, right.z);

        ObjectPoolManager.Spawn(damageVFX, cablePoint.position, rotation);

    }
    void IBreakable.ObjectIsBroken()
    {

    }

    void OnDestroy()
    {

        input.Disable();
    }

    public bool CanFix()
    {
        throw new System.NotImplementedException();
    }

    public void NotFixing()
    {
        throw new System.NotImplementedException();
    }
}
