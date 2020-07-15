using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    List<Objective> m_Objectives = new List<Objective>();

    public UnityAction OnAllCompleted;

    public void OnObjectiveUpdateHandler(UnityActionUpdateObjective newUpdateInfo)
    {
        if (m_Objectives.Count == 0)
            return;

        for (int i = 0; i < m_Objectives.Count; i++)
        {
            // pass every objectives to check if they have been completed
            if (m_Objectives[i].isBlocking())
            {
                // break the loop as soon as we find one uncompleted objective
                return;
            }
        }

        // found no uncompleted objective
        if(OnAllCompleted != null)
        {
            OnAllCompleted.Invoke();
        }
    }

    public void RegisterObjective(Objective objective)
    {
        m_Objectives.Add(objective);
        objective.onUpdateObjective += OnObjectiveUpdateHandler;
    }
}
