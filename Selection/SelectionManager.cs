using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private SelectableObject m_CurrentSelection;

    public SelectableObject CurrentSelection
    {
        get { return m_CurrentSelection; }
        set { m_CurrentSelection = value; }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                SelectableObject selectableObject = hit.collider.gameObject.GetComponent<SelectableObject>();
                if (selectableObject != null)
                {
                    if (selectableObject != m_CurrentSelection)
                    {
                        if (m_CurrentSelection != null)
                        {
                            m_CurrentSelection.OnDeselected();
                        }
                        selectableObject.OnSelected();
                        m_CurrentSelection = selectableObject;
                    }
                }

            }
        }
    }
}
