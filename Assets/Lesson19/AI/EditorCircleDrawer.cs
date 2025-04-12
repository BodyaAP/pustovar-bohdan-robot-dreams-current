using UnityEngine;

// ������������� ������ ���� UnityEditor, ��� �������� ������ �� Handles
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorCircleDrawer : MonoBehaviour
{
    [Tooltip("�������� ����� ���������� ����.")]
    [Range(0.1f, 100f)]
    public float mainRadius = 10f;

    [Tooltip("���� ���� ������ ��� �������� ��.")]
    [Range(0.1f, 20f)]
    public float radiusStep = 1f;

    [Tooltip("���� ��� ��������� �� �� ������.")]
    public Color drawingColor = Color.cyan;

    [Tooltip("��������� ���� ��� ������ ���� �� ����.")]
    public float labelOffset = 0.2f;

    [Tooltip("������ ��������� ����� ������ (����., 'F1' ��� 1 ����� ���� ����).")]
    public string numberFormat = "F1";

    // ����� ��� ������ ����
    private GUIStyle labelStyle;

    // ����������� ����� (����������� ��� ��� ������� ��� ����������� ���������)
    void OnValidate()
    {
        // ��������� �����, ���� ���� �� ����
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = drawingColor; // ���������� ����
            labelStyle.alignment = TextAnchor.MiddleLeft; // ����������� ������
            // ����� ������ ���� ������������ ����� ��� (����� ������ ����)
            // labelStyle.fontSize = 12;
        }
        // ��������� ���� �����, ���� ���� � ��������� �������
        labelStyle.normal.textColor = drawingColor;
    }


    // �� ������� ����������� ���������� Unity ��� ��������� Gizmos � ��� Scene
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR // ������������, �� ��� ��� ����������� ����� � ��������

        // ���������� �����, ���� �� ������ �� ��������� � OnValidate
        if (labelStyle == null)
        {
            OnValidate(); // ���������� ������������ �����
            if (labelStyle == null) return; // ���� ��� �� �� �������, �����
        }
        else
        {
            // ���������� � ��������� ����, ���� �� �� ������� � �����������
            if (labelStyle.normal.textColor != drawingColor)
            {
                labelStyle.normal.textColor = drawingColor;
            }
        }


        // �������� �� �������� �������
        if (mainRadius <= 0f || radiusStep <= 0f)
        {
            return; // ���� ��������, ���� �������� ���������
        }

        Vector3 center = transform.position; // ����� ����� - ������� ��'����
        Vector3 normal = Vector3.up; // ������� ��� ���� (������� �� ������ XZ)
        Vector3 right = Vector3.right; // �������� ��� ��������� ����

        // �������� �������� ���� Handles � ������������ ���
        Color originalColor = Handles.color;
        Handles.color = drawingColor;

        // --- ������� ������� (��������) ���� ---
        Handles.DrawWireDisc(center, normal, mainRadius);

        // --- ������ ���� ��� ��������� ���� ---
        // ������� �� ��� + ��������� ���� ������
        Vector3 mainLabelPosition = center + right * (mainRadius + labelOffset);
        // ��������� ����� ������
        string mainLabelText = mainRadius.ToString(numberFormat);
        // ������� ����
        Handles.Label(mainLabelPosition, mainLabelText, labelStyle);


        // --- ������� ������� ����������� ���� ---
        float currentRadius = mainRadius - radiusStep;
        while (currentRadius > 0.001f) // ��������� � ��������� ���������, ��� �������� ������� � ������� float
        {
            // ������� ����
            Handles.DrawWireDisc(center, normal, currentRadius);

            // --- ������ ���� ��� ����������� ���� ---
            // ������� �� ��� + ��������� ���� ������
            Vector3 innerLabelPosition = center + right * (currentRadius + labelOffset);
            // ��������� ����� ������
            string innerLabelText = currentRadius.ToString(numberFormat);
            // ������� ����
            Handles.Label(innerLabelPosition, innerLabelText, labelStyle);

            // �������� ����� �� ���� ��� ���������� ����
            currentRadius -= radiusStep;
        }

        // ��������� ����������� ���� Handles
        Handles.color = originalColor;

#endif // ʳ���� ����� UNITY_EDITOR
    }
}