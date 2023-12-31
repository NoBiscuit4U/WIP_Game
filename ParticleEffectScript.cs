using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectScript:MonoBehaviour{
    public float duration;

    private void Update(){
        duration-=Time.deltaTime;
        if(duration<0){
            Destroy(this.gameObject);
        }
    }
}
