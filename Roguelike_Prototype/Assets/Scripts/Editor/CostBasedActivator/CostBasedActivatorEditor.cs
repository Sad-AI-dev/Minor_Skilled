using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CostBasedActivator))]
public class CostBasedActivatorEditor : Editor
{
    private CostBasedActivator activator;
    private AnimationCurve curve;

    //store default values
    private int baseBudget;
    private int baseBudgetGain;
    private int baseGainRampup;

    private void OnEnable()
    {
        if (activator == null) {
            activator = target as CostBasedActivator;
        }
        CalculatePreview();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //draw animation curve
        EditorGUILayout.CurveField("Preview Curve", curve);

        //draw recalculate button
        if (GUILayout.Button("Recalculate Preview")) {
            CalculatePreview();
        }
    }

    private void CalculatePreview()
    {
        curve = new AnimationCurve();
        RecordBaseVars();

        for (int i = 0; i < activator.cyclesToSimulate; i++) {
            SimulateActivate();
            curve.AddKey(i / (float)activator.cyclesToSimulate, activator.budget); //record cycle
            activator.budget = 0;
        }
        RestoreBaseVars();
    }

    private void RecordBaseVars()
    {
        baseBudget = activator.budget;
        baseBudgetGain = activator.budgetGain;
        baseGainRampup = activator.gainRampup;
    }

    private void SimulateActivate()
    {
        activator.Activate(true);
    }

    private void RestoreBaseVars()
    {
        activator.budget = baseBudget;
        activator.budgetGain = baseBudgetGain;
        activator.gainRampup = baseGainRampup;
        //reset counters
        activator.ResetVars();
    }
}
