using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NOOD
{
    public class NoodyCustomCode : MonoBehaviorInstance<NoodyCustomCode>
    {
        public static Thread newThread;

        #region Look, mouse and Vector zone
        public static Vector3 ScreenPointToWorldPoint(Vector2 screenPoint)
        {
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            return cam.ScreenToWorldPoint(screenPoint);
        }

        public static Vector3 MouseToWorldPoint()
        {
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Vector3 mousePos = Input.mousePosition;
            return cam.ScreenToWorldPoint(mousePos);
        }

        public static Vector3 MouseToWorldPoint2D()
        {
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Vector3 mousePos = MouseToWorldPoint();
            Vector3 temp = new Vector3(mousePos.x, mousePos.y, 0f);
            return temp;
        }

        public static Vector2 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            return cam.WorldToScreenPoint(worldPoint);
        }

        public static void LookToMouse2D(Transform objectTransform)
        {
            Vector3 mousePosition = MouseToWorldPoint();
            LookToPoint2D(objectTransform, mousePosition);
        }

        public static void LookToPoint2D(Transform objectTransform, Vector3 targetPosition)
        {
            Vector3 lookDirection = LookDirection(objectTransform.position, targetPosition);
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            objectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        }

        public static Vector3 LookDirection(Vector3 FromPosition, Vector3 targetPosition)
        {
            return (targetPosition - FromPosition).normalized;
        }

        public static Vector3 GetPointAroundAPosition2D(Vector3 centerPosition, float degrees, float radius)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            Vector3 pos = new Vector3(x, y, centerPosition.z);
            return pos += centerPosition;
        }

        public static Vector3 GetPointAroundAPosition2D(Vector3 centerPosition, float radius)
        {
            int degrees = UnityEngine.Random.Range(0, 360);
            float radians = degrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            Vector3 pos = new Vector3(x, y, centerPosition.z);
            pos *= radius;
            return pos += centerPosition;
        }

        public static Vector3 GetPointAroundAPosition3D(Vector3 centerPosition, float degrees, float radius)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float z = Mathf.Sin(radians);
            Vector3 pos = new Vector3(x, centerPosition.y, z);
            return pos += centerPosition;
        }

        public static Vector3 GetPointAroundAPosition3D(Vector3 centerPosition, float radius)
        {
            int degrees = UnityEngine.Random.Range(0, 360);
            float radians = degrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float z = Mathf.Sin(radians);
            Vector3 pos = new Vector3(x, centerPosition.y, z);
            pos *= radius;
            return pos += centerPosition;
        }
        #endregion

        #region Background Function
        public static void RunInBackground(Action function, Queue<Action> mainThreadActions = null)
        {
            //! WebGL doesn't do multithreading

            /* Create a mainThreadQueue in main script to hold the Action and run the action in Update like below
                 if your Action do something to Unity object like set transform, set physic or contain Unity Time class 
                !Ex for mainThreadQueue:
                    Queue<Action> mainThreadQueue = new Queue<Action>()
                    
                    void Update()
                    {
                        if(mainThreadQueue.Count > 0)
                        {
                            Action action = mainThreadQueue.Dequeue();
                            action();
                        }
                    }
            */

            //! if your function has parameters, use param like this
            //! NoodyCustomCode.RunInBackGround(() => yourFunction(parameters)); 

            Thread t = new Thread(() =>
            {
                if (mainThreadActions != null)
                {
                    AddToMainThread(function, mainThreadActions);
                }
                else
                {
                    function();
                }
            });
            t.Start();
        }

        private static void AddToMainThread(Action function, Queue<Action> mainThreadActions)
        {
            mainThreadActions.Enqueue(function);
        }

        //TODO: learn Unity.Jobs and create a Function to run many complex job in multithread

        #endregion

        #region Delay Function
        public static void StartDelayFunction(Action action, float delaySecond)
        {
            GameObject delayObj = new GameObject("DelayActionGameObject");
            DelayAction delay = delayObj.AddComponent<DelayAction>();

            delay.StartDelayFunction(() =>
            {
                action?.Invoke();
            }, delaySecond);
        }
        #endregion

        #region Camera
        /// <summary>
        /// Make camera size always show all object with collider (2D and 3D)
        /// (center, size) = CalculateOrthoCamsize();
        /// </summary>
        /// <param name="_camera">Main camera</param>
        /// <param name="_buffer">Amount of padding size</param>
        /// <returns></returns>
        public static (Vector3 center, float size) CalculateOrthoCamSize(Camera _camera, float _buffer)
        {
            Bounds bound = new Bounds(); //Create bound with center Vector3.zero;

            foreach (Collider2D col in FindObjectsOfType<Collider2D>())
            {
                bound.Encapsulate(col.bounds);
            }

            foreach (Collider col in FindObjectsOfType<Collider>())
            {
                bound.Encapsulate(col.bounds);
            }

            bound.Expand(_buffer);

            float vertical = bound.size.y;
            float horizontal = bound.size.x * _camera.pixelHeight / _camera.pixelWidth;

            //Debug.Log("V: " + vertical + ", H: " + horizontal);

            float size = Mathf.Max(horizontal, vertical) * 0.5f;
            Vector3 center = bound.center + new Vector3(0f, 0f, -10f);

            return (center, size);
        }

        /// <summary>
        /// Move camera base on your input (Put this function in Update to track the input), direction = -1 for opposite direction, 1 for follow direction
        /// </summary>
        /// <param name="camera">Camera you want to move</param>
        /// <param name="direction">-1 for oposite direction, 1 for follow direction</param>
        private static Vector3 DCMousePostion = Vector3.zero;
        private static Vector3 DCDir;
        private static Vector3 tempPos;
        private static Vector3 campPos;
        public static void DragCamera(GameObject camera, int direction = 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DCMousePostion = MouseToWorldPoint2D();
            }

            if (Input.GetMouseButton(0))
            {
                if (MouseToWorldPoint2D() != DCMousePostion)
                {
                    DCDir = MouseToWorldPoint2D() - DCMousePostion;
                    camera.transform.position += direction * DCDir;
                }
            }
        }

        /// <summary>
        /// Move camera base on your input (Put this function in Update to track the input), direction = -1 for opposite direction, 1 for follow direction
        /// </summary>
        /// <param name="camera">Camera you want to move</param>
        /// <param name="direction">-1 for oposite direction, 1 for follow direction</param>
        public static void DragCamera(GameObject camera, float minX, float maxX, float minY, float maxY, int direction = 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DCMousePostion = MouseToWorldPoint2D();
            }

            if (Input.GetMouseButton(0))
            {
                if (MouseToWorldPoint2D() != DCMousePostion)
                {
                    DCDir = MouseToWorldPoint2D() - DCMousePostion;

                    tempPos = direction * DCDir;
                    campPos = camera.transform.position;

                    if (campPos.x + tempPos.x > minX && campPos.x + tempPos.x < maxX)
                    {
                        campPos.x += tempPos.x;
                    }

                    if (campPos.y + tempPos.y > minY && campPos.y + tempPos.y < maxY)
                    {
                        campPos.y += tempPos.y;
                    }

                    camera.transform.position = campPos;
                }
            }
        }

        /// <summary>
        /// Move camera base on your input (Put this function in Update to track the input), direction = -1 for opposite direction, 1 for follow direction
        /// </summary>
        /// <param name="camera">Camera you want to move</param>
        /// <param name="direction">-1 for oposite direction, 1 for follow direction</param>
        public static void DragCamera2Finger(GameObject camera, float minX, float maxX, float minY, float maxY, int direction = 1)
        {
            if (Input.touchCount >= 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector2 avergare = (touchOne.position + touchZero.position) / 2;
                if (touchOne.phase == TouchPhase.Began)
                {

                    DCMousePostion = ScreenPointToWorldPoint(avergare);

                }
                if (touchOne.phase == TouchPhase.Moved)
                {
                    if (ScreenPointToWorldPoint(avergare) != DCMousePostion)
                    {
                        DCDir = ScreenPointToWorldPoint(avergare) - DCMousePostion;

                        tempPos = direction * DCDir;
                        campPos = camera.transform.position;

                        if (campPos.x + tempPos.x > minX && campPos.x + tempPos.x < maxX)
                        {
                            campPos.x += tempPos.x;
                        }

                        if (campPos.y + tempPos.y > minY && campPos.y + tempPos.y < maxY)
                        {
                            campPos.y += tempPos.y;
                        }

                        camera.transform.position = campPos;
                    }
                }

            }

        }

        public static void SmoothCameraFollow(GameObject camera, float smoothTime, Transform targetTransform, Vector3 offset,
        float maxX, float maxY, float minX, float minY)
        {

            Vector3 temp = camera.transform.position;
            Vector3 targetPosition = targetTransform.position + offset;
            Vector3 currentSpeed = Vector3.zero;
            Vector3 smoothPosition = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref currentSpeed, smoothTime);
            if (smoothPosition.x < maxX && smoothPosition.x > minX)
            {
                temp.x = smoothPosition.x;
            }

            if (smoothPosition.y < maxY && smoothPosition.y > minY)
            {
                temp.y = smoothPosition.y;
            }

            temp.z = smoothPosition.z;
            camera.transform.position = temp;
        }

        public static void SmoothCameraFollow(GameObject camera, float smoothTime, Transform targetTransform, Vector3 offset)
        {

            Vector3 temp = camera.transform.position;
            Vector3 targetPosition = targetTransform.position + offset;
            Vector3 currentSpeed = Vector3.zero;
            Vector3 smoothPosition = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref currentSpeed, smoothTime);
            //Vector3 smoothPosition = Vector3.Lerp(temp, targetPosition, smoothTime);

            temp.x = smoothPosition.x;
            temp.y = smoothPosition.y;
            temp.z = smoothPosition.z;

            camera.transform.position = temp;
        }

        public static void LerpSmoothCameraFollow(GameObject camera, float smoothTime, Transform targetTransform, Vector3 offset)
        {

            Vector3 temp = camera.transform.position;
            Vector3 targetPosition = targetTransform.position + offset;
            Vector3 currentSpeed = Vector3.zero;
            //Vector3 smoothPosition = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref currentSpeed, smoothTime);
            Vector3 smoothPosition = Vector3.Lerp(temp, targetPosition, smoothTime * Time.fixedDeltaTime);

            temp.x = smoothPosition.x;
            temp.y = smoothPosition.y;
            temp.z = smoothPosition.z;

            camera.transform.position = temp;
        }

        public static IEnumerator ObjectShake(GameObject @object, float duration, float magnitude)
        {
            Vector3 OriginalPos = @object.transform.localPosition;
            float elapsed = 0.0f;
            float range = 1f;
            while (elapsed < duration)
            {
                float x, y;
                if (elapsed / duration * 100 < 80)
                {
                    //Starting shake
                    x = UnityEngine.Random.Range(-range, range) * magnitude;
                    y = UnityEngine.Random.Range(-range, range) * magnitude;
                }
                else
                {
                    //Ending
                    range -= Time.deltaTime * elapsed;
                    x = UnityEngine.Random.Range(-range, range) * magnitude;
                    y = UnityEngine.Random.Range(-range, range) * magnitude;
                }

                @object.transform.localPosition = new Vector3(x, y, OriginalPos.z);

                elapsed += Time.deltaTime;
                yield return null;
            }
            @object.transform.localPosition = OriginalPos;
        }
        #endregion

        #region Color
        /// <summary>
        /// <para>Return RGBA color </para>
        /// </summary>
        /// <param name="hexCode">hex code form RRGGBB or RRGGBBAA for alpha output</param>
        /// <returns>Color with RGBA form</returns>
        public static Color HexToColor(string hexCode)
        {
            _ = ColorUtility.TryParseHtmlString(hexCode, out Color color);

            return color;
        }
        //----------------------------//
        /// <summary>
        /// Return hex code with alpha of the color
        /// </summary>
        /// <param name="color">Color's form RRGGBBAA</param>
        /// <returns></returns>
        public static string ColorAToHex(Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
        //----------------------------//
        /// <summary>
        /// Return hex code without alpha of the color
        /// </summary>
        /// <param name="color">Color's form RRGGBB</param>
        /// <returns></returns>
        public static string ColorToHex(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }
        //----------------------------//
        /// <summary>
        /// reduce alpha to 0 over Time.deltaTime
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static void FadeOutImage(Image image)
        {
            FadeOutImage(image, 0, Time.deltaTime);
        }
        /// <summary>
        /// reduce alpha to 0
        /// </summary>
        /// <param name="image"></param>
        /// <param name="pauseTimePerLoop"></param>
        public static void FadeOutImage(Image image, float pauseTimePerLoop)
        {
            FadeOutImage(image, 0, pauseTimePerLoop);
        }
        /// <summary>
        /// reduce alpha to the end value
        /// </summary>
        /// <param name="image"></param>
        /// <param name="endValue">the stop value</param>
        /// <param name="pauseTimePerLoop">time between loop</param>
        public static void FadeOutImage(Image image, float endValue, float pauseTimePerLoop)
        {
            GameObject fadeOutObj = new GameObject("FadeOutObj");
            CoroutineScript coroutineScript = fadeOutObj.AddComponent<CoroutineScript>();

            coroutineScript.StartCoroutineLoop(() =>
            {
                Color color = image.color;
                color.a -= Time.deltaTime;
                image.color = color;
                if (color.a <= endValue)
                {
                    image.gameObject.SetActive(false);
                    coroutineScript.Complete();
                }
            }, pauseTimePerLoop);
        }
        //----------------------------//
        /// <summary>
        /// Fade in the image by crease color over Time.deltaTime
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static void FadeInImage(Image image)
        {
            FadeInImage(image, 1, Time.deltaTime);
        }
        /// <summary>
        /// increase alpha to 1
        /// </summary>
        /// <param name="image"></param>
        /// <param name="pauseTimePerLoop"></param>
        public static void FadeInImage(Image image, float pauseTimePerLoop)
        {
            FadeInImage(image, 1, pauseTimePerLoop);
        }
        /// <summary>
        /// increase alpha to the maxValue
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxValue"></param>
        /// <param name="pauseTimePerLoop">time between loop</param>
        public static void FadeInImage(Image image, float maxValue, float pauseTimePerLoop)
        {
            GameObject fadeInObj = new GameObject("FadeInObj");
            image.gameObject.SetActive(true);
            CoroutineScript coroutineScript = fadeInObj.AddComponent<CoroutineScript>();

            coroutineScript.StartCoroutineLoop(() =>
            {
                Color color = image.color;
                color.a += Time.deltaTime;
                image.color = color;
                if(image.color.a >= maxValue)
                {
                    coroutineScript.Complete();
                }
            }, pauseTimePerLoop);
        }
        //----------------------------//
        /// <summary>
        /// decrease alpha to 0 over Time.deltaTime
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        public static void FadeOutTextMeshUGUI(TextMeshProUGUI textMeshProUGUI)
        {
            FadeOutTextMeshUGUI(textMeshProUGUI, 0, Time.deltaTime);
        }
        /// <summary>
        /// increase alpha to 0
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="pauseTimePerLoop">time between loop</param>
        public static void FadeOutTextMeshUGUI(TextMeshProUGUI textMeshProUGUI, float pauseTimePerLoop)
        {
            FadeOutTextMeshUGUI(textMeshProUGUI, 0, pauseTimePerLoop);
        }
        /// <summary>
        /// decrease alpha to endValue
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="endValue"></param>
        /// <param name="pauseTimePerLoop">Time between loop</param>
        public static void FadeOutTextMeshUGUI(TextMeshProUGUI textMeshProUGUI, float endValue,float pauseTimePerLoop)
        {
            GameObject fadeOutObj = new GameObject("FadeOutObj");
            CoroutineScript coroutineScript = fadeOutObj.AddComponent<CoroutineScript>();

            coroutineScript.StartCoroutineLoop(() =>
            {
                Color color = textMeshProUGUI.color;
                color.a -= Time.deltaTime;
                textMeshProUGUI.color = color;
                if(textMeshProUGUI.color.a <= endValue)
                {
                    textMeshProUGUI.gameObject.SetActive(false);
                    coroutineScript.Complete();
                }
            }, pauseTimePerLoop);
        }
        //----------------------------//
        /// <summary>
        /// increase alpha to 1 over Time.deltaTime
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        public static void FadeInTextMeshUGUI(TextMeshProUGUI textMeshProUGUI)
        {
            FadeInTextMeshUGUI(textMeshProUGUI, 1, Time.deltaTime);
        }
        /// <summary>
        /// increase alpha to 1
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="pauseTimePerLoop"></param>
        public static void FadeInTextMeshUGUI(TextMeshProUGUI textMeshProUGUI, float pauseTimePerLoop)
        {
            FadeInTextMeshUGUI(textMeshProUGUI, 1, pauseTimePerLoop);
        }
        /// <summary>
        /// increase alpha to maxValue
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="maxValue"></param>
        /// <param name="pauseTimePerLoop"></param>
        public static void FadeInTextMeshUGUI(TextMeshProUGUI textMeshProUGUI, float maxValue, float pauseTimePerLoop)
        {
            textMeshProUGUI.gameObject.SetActive(true);
            GameObject fadeInObj = new GameObject("FadeInObj");
            CoroutineScript coroutineScript = fadeInObj.AddComponent<CoroutineScript>();

            coroutineScript.StartCoroutineLoop(() =>
            {
                Color color = textMeshProUGUI.color;
                color.a += Time.deltaTime;
                textMeshProUGUI.color = color;
                if(textMeshProUGUI.color.a >= maxValue)
                {
                    coroutineScript.Complete();
                }
            }, pauseTimePerLoop);
        }
        #endregion
    
        #region CoroutineFunction
        /// <summary>
        /// Create a coroutineScript for coroutine loop function
        /// </summary>
        /// <returns></returns>
        public static CoroutineScript CreateNewCoroutineObj()
        {
            GameObject fadeInObj = new GameObject("CoroutineObj");
            CoroutineScript coroutineScript = fadeInObj.AddComponent<CoroutineScript>();
            return coroutineScript;
        }
        #endregion
    }

}
