using UnityEngine;

// Використовуємо простір імен UnityEditor, щоб отримати доступ до Handles
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorCircleDrawer : MonoBehaviour
{
    [Tooltip("Основний радіус найбільшого кола.")]
    [Range(0.1f, 100f)]
    public float mainRadius = 10f;

    [Tooltip("Крок зміни радіуса для внутрішніх кіл.")]
    [Range(0.1f, 20f)]
    public float radiusStep = 1f;

    [Tooltip("Колір для малювання кіл та тексту.")]
    public Color drawingColor = Color.cyan;

    [Tooltip("Невеликий зсув для тексту мітки від кола.")]
    public float labelOffset = 0.2f;

    [Tooltip("Формат виведення числа радіуса (напр., 'F1' для 1 знаку після коми).")]
    public string numberFormat = "F1";

    // Стиль для тексту мітки
    private GUIStyle labelStyle;

    // Ініціалізація стилю (викликається при зміні скрипта або завантаженні редактора)
    void OnValidate()
    {
        // Створюємо стиль, якщо його ще немає
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = drawingColor; // Початковий колір
            labelStyle.alignment = TextAnchor.MiddleLeft; // Вирівнювання тексту
            // Можна додати інші налаштування стилю тут (розмір шрифту тощо)
            // labelStyle.fontSize = 12;
        }
        // Оновлюємо колір стилю, якщо колір в інспекторі змінився
        labelStyle.normal.textColor = drawingColor;
    }


    // Ця функція викликається редактором Unity для малювання Gizmos у вікні Scene
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR // Переконуємося, що цей код компілюється тільки в редакторі

        // Ініціалізуємо стиль, якщо він раптом не створився в OnValidate
        if (labelStyle == null)
        {
            OnValidate(); // Спробувати ініціалізувати стиль
            if (labelStyle == null) return; // Якщо все ще не вдалося, вийти
        }
        else
        {
            // Перевіряємо і оновлюємо колір, якщо він не співпадає з інспектором
            if (labelStyle.normal.textColor != drawingColor)
            {
                labelStyle.normal.textColor = drawingColor;
            }
        }


        // Перевірка на валідність значень
        if (mainRadius <= 0f || radiusStep <= 0f)
        {
            return; // Тихо виходимо, якщо значення некоректні
        }

        Vector3 center = transform.position; // Точка відліку - позиція об'єкта
        Vector3 normal = Vector3.up; // Нормаль для кола (малюємо на площині XZ)
        Vector3 right = Vector3.right; // Напрямок для розміщення мітки

        // Зберігаємо поточний колір Handles і встановлюємо наш
        Color originalColor = Handles.color;
        Handles.color = drawingColor;

        // --- Малюємо головне (найбільше) коло ---
        Handles.DrawWireDisc(center, normal, mainRadius);

        // --- Додаємо мітку для головного кола ---
        // Позиція на колі + невеликий зсув назовні
        Vector3 mainLabelPosition = center + right * (mainRadius + labelOffset);
        // Форматуємо текст радіуса
        string mainLabelText = mainRadius.ToString(numberFormat);
        // Малюємо мітку
        Handles.Label(mainLabelPosition, mainLabelText, labelStyle);


        // --- Малюємо внутрішні концентричні кола ---
        float currentRadius = mainRadius - radiusStep;
        while (currentRadius > 0.001f) // Порівнюємо з невеликим значенням, щоб уникнути проблем з точністю float
        {
            // Малюємо коло
            Handles.DrawWireDisc(center, normal, currentRadius);

            // --- Додаємо мітку для внутрішнього кола ---
            // Позиція на колі + невеликий зсув назовні
            Vector3 innerLabelPosition = center + right * (currentRadius + labelOffset);
            // Форматуємо текст радіуса
            string innerLabelText = currentRadius.ToString(numberFormat);
            // Малюємо мітку
            Handles.Label(innerLabelPosition, innerLabelText, labelStyle);

            // Зменшуємо радіус на крок для наступного кола
            currentRadius -= radiusStep;
        }

        // Повертаємо оригінальний колір Handles
        Handles.color = originalColor;

#endif // Кінець блоку UNITY_EDITOR
    }
}