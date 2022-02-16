using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Perceptron : MonoBehaviour
{
    [SerializeField] private TrainingSet[] ts;

    public double[] Weights { get; } = {0, 0};
    public double Bias { get; set; } = 0;
    public double TotalError { get; set; } = 0;
    [SerializeField] private SimpleGrapher sg;

    double DotProductBias(double[] v1, double[] v2)
    {
        if (v1 == null || v2 == null) return -1;
        if (v1.Length != v2.Length) return -1;
        double d = 0;
        for (int x = 0; x < v1.Length; x++)
        {
            d += v1[x] * v2[x];
        }
        d += Bias;
        return d;
    }

    double CalcOutput(int i)
    {
        double dp = DotProductBias(Weights, ts[i].input);
        if (dp > 0) return 1;
        return 0;
    }
    
    double CalcOutput(int i1, int i2)
    {
        double[] inp = new double[] {i1, i2};
        double dp = DotProductBias(Weights, inp);
        if (dp > 0) return 1;
        return 0;
    }
    
    void InitialiseWeights()
    {
        for (int i = 0; i < Weights.Length; i++)
        {
            Weights[i] = Random.Range(-1.0f, 1.0f);
        }
        Bias = Random.Range(-1.0f, 1.0f);
    }

    void Train(int epochs)
    {
        InitialiseWeights();
        for (int e = 0; e < epochs; e++)
        {
            TotalError = 0;
            for (int t = 0; t < ts.Length; t++)
            {
                UpdateWeights(t);
                Debug.Log($"W1: {Weights[0]}, W1: {Weights[1]}, B: {Bias}");
            }
            Debug.Log($"Total Error: {TotalError}");
        }
    }

    void DrawPoints()
    {
        for (int t = 0; t < ts.Length; t++)
        {
            if (ts[t].output == 0)
            {
                sg.DrawPoint((float) ts[t].input[0],(float) ts[t].input[1], Color.magenta);
            }
            else
            {
                sg.DrawPoint((float) ts[t].input[0],(float) ts[t].input[1], Color.green);
            }
        }
    }

    private void UpdateWeights(int i)
    {
        double error = ts[i].output - CalcOutput(i);
        TotalError += Mathf.Abs((float) error);
        for (int w = 0; w < Weights.Length; w++)
        {
            Weights[w] = Weights[w] + error * ts[i].input[w];
        }

        Bias += error;
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawPoints();
        Train(200);
        sg.DrawRay((float)(-(Bias/Weights[1])/(Bias/Weights[0])), (float) (-Bias/Weights[1]), Color.red );
        Debug.Log($"Test 0 0: {CalcOutput(0,0)}");
        Debug.Log($"Test 0 1: {CalcOutput(0,1)}");
        Debug.Log($"Test 1 0: {CalcOutput(1,0)}");
        Debug.Log($"Test 1 1: {CalcOutput(1,1)}");
    }
}
