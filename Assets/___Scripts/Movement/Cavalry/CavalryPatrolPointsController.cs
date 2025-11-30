using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CavalryPatrolPointsController : MonoBehaviour
{

    private List<Transform> _patrolPoints;
    private int _currentPatrollingUnitsCount;

    void Awake(){
        InitializePatrolPoints();
    }

    private void InitializePatrolPoints()
    {
        _patrolPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            _patrolPoints.Add(child);
        }
        _currentPatrollingUnitsCount = 0;
    }


    public List<Transform> SubscribeToPatrolPointSequence(GameObject Cavalry){
        if (_patrolPoints == null)
        {
           InitializePatrolPoints();
        }
        
        var healthController = Cavalry.GetComponent<CavalryHealthController>();
        if (healthController == null)
        {
            Debug.LogWarning("Cavalry attempted to subscribe to patrol point but has no Health Controller!");
        } 
        else {
            _currentPatrollingUnitsCount++;
            Cavalry.GetComponent<CavalryHealthController>().OnDied.AddListener(() => _currentPatrollingUnitsCount--);
        }

        return _patrolPoints;
    }

    public int GetCurrentPatrollingUnits(){
        return _currentPatrollingUnitsCount;
    }
}
