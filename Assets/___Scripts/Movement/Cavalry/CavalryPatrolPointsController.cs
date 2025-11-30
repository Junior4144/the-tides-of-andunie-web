using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CavalryPatrolPointsController : MonoBehaviour
{

    // create fields for the list of patrol points (transforms)
    private List<Transform> _patrolPoints;
    private int _currentPatrollingUnitsCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake(){
        _patrolPoints = GetComponentInChildren<Transform>().gameObject.GetComponentsInChildren<Transform>().ToList();
        _currentPatrollingUnitsCount = 0;
    }


    // Make the cavalry unit subscribe to this patrol point so that we'll know when it dies we can decrease the counter
    public List<Transform> SubscribeToPatrolPointSequence(GameObject Cavalry){
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
