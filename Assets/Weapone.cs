using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine.UI;



public class Weapone : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _fire;
    [SerializeField]
    private GunMode _gunMode;
    [SerializeField]
    private float _rateOfFire = 2f;
    [SerializeField]
    private bool isAutomatic;
    private float _timer = 0;

    private bool _isReadyToFire = true;
    private Animation anim = null;
    private float recoil = 0f;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private int ammo = 40;
    [SerializeField]
    private Text ammo_text;


    enum GunMode
    {
        singe,
        burst,
    }

    public void PullTheHook()
    {
        anim = GetComponent<Animation>();
        ammo_text.text = ammo.ToString();
        foreach (AnimationState state in anim)
            state.speed = 20f;

        Observable.EveryUpdate()
            .Subscribe(x => {
                _timer -= Time.deltaTime;
                if (Input.GetMouseButtonDown(0))
                {
                    if ((_timer <= 0)&&(ammo > 0))
                    {
                            Shot();
                            _timer = _rateOfFire;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if ((_timer <= 0) && (ammo > 0))
                    {
                        Shot();
                        _timer = _rateOfFire;
                    }
                }
            }).AddTo(this);

        switch (_gunMode)
        {
            case GunMode.singe:
                if (_isReadyToFire)
                {
                    Shot();
                    StartCoroutine(WaitBeforeShot());
                }
                break;

            case GunMode.burst:
                if (isAutomatic && _isReadyToFire && (_gunMode == GunMode.burst) )
                {
                    Shot();
                    StartCoroutine(WaitBeforeShot());
                }
                break;
        }
    }

    public void Reload()
    {
        ammo = 40;
        ammo_text.text = ammo.ToString();
    }
    public void Switching()
    {
        if(isAutomatic)
        {
            if(_gunMode == GunMode.singe)
                _gunMode = GunMode.burst;
            else
                _gunMode = GunMode.singe;
        }
    }

    public void Shot()
    {
        anim.Play();
        
        if (recoil >= 0f)
        {
            var Max_y_recoil = Quaternion.Euler(new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)));

            camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Max_y_recoil, Time.deltaTime * 10f);
            recoil -= Time.deltaTime;
        }
        else
        {

            var ZeroAngle = Quaternion.Euler(0, 0, 0);
            recoil = 0f;
            camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, ZeroAngle, Time.deltaTime * 3f);

        }
        var bulletOnScene = Instantiate(_bullet, _fire.transform.position, transform.rotation);
        ammo--;
        ammo_text.text = ammo.ToString();
    }

    IEnumerator WaitBeforeShot()
    {
        _isReadyToFire = false;
        yield return new WaitForSeconds(_rateOfFire);
        _isReadyToFire = true;
    }
}
