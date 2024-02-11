using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenGenerator:MonoBehaviour{
    private float[] m_inputs;
    private float m_tokenMultiplier;
    public TokenGenerator(float[] inputs,float tokenMultiplier){
        m_inputs=inputs;
        m_tokenMultiplier=tokenMultiplier;
    }        

    public void generateToken(){
        for(int i=0; i<m_inputs.Length;i++){
            
        }
    }

    public float get(){
        return 0;
    }

    public void resetTokens(){
    }
}