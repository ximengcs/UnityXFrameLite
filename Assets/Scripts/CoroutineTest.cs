using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using XFrame.Tasks;

public class CoroutineTest : MonoBehaviour, ITaskBinder
{
    public bool IsDisposed
    {
        get
        {
            Debug.Log($"{this == null} {gameObject == null}");
            return gameObject == null;
        }
    }

    void Start()
    {
        //StartCoroutine(Test());
        //StartCoroutine(new EnumeratorTest());
        XTaskHelper.ExceptionHandler += Debug.LogException;
        Test().Bind(this);
    }

    public async XTask Test()
    {
        Debug.LogWarning("Test1");
        await Task.Delay(1000);
        Debug.LogWarning("Test2");
        await Task.Delay(5000);
        Debug.LogWarning("Test3");
    }
}

public class EnumeratorTest : IEnumerator
{
    private int index;

    public object Current => index;

    public bool MoveNext()
    {
        Debug.LogWarning($"EnumeratorTest {index}");
        if (index++ >= 10)
        {
            return false;
        }
        return true;
    }

    public void Reset()
    {

    }
}
